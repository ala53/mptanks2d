using Lidgren.Network;
using Microsoft.Xna.Framework;
using MPTanks.Engine.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSB.Drm.Client.Exceptions;

namespace MPTanks.Networking.Server
{
    public class LoginManager
    {
        public Server Server { get; private set; }
        public LoginManager(Server server)
        {
            Server = server;
        }
        public void HandleConnection(NetIncomingMessage incoming)
        {
            try
            {
                //read their proclaimed info
                Guid id = new Guid(incoming.ReadBytes(16));
                string name = incoming.ReadString();
                string token = incoming.ReadString();
                string pass = incoming.ReadString();

                if (Server.Configuration.Password != null)
                {
                    if (pass != Server.Configuration.Password)
                    {
                        DenyConnection(incoming, "Password incorrect");
                        return;
                    }
                }
                //Check that they aren't on the server
                if (Server.Players.FirstOrDefault(a => a.Player.UniqueId == id) != null)
                {
                    DenyConnection(incoming, "Already connected to server");
                    return;
                }

                if (Server.Configuration.Offline)
                {
                    //Directly connect, regardless of whether they are who they say they are
                    if (Server.Players.Count < Server.Configuration.MaxPlayers) 
                        ApproveConnection(incoming, new WebInterface.WebPlayerInfoResponse
                        {
                            Id = id,
                            Username = name,
                            Premium = true
                        });
                    else DenyConnection(incoming, "Too many players");
                }
                else
                {
                    //Defer and check
                    Task.Run(async () =>
                    {
                        try
                        {
                            var info = await ZSB.DrmClient.Multiplayer.ValidateServerTokenAsync(token);
                            //Check that they own the game
                            if (!info.Owns(Common.StaticSettings.MPTanksProductId))
                            {
                                DenyConnection(incoming, "Game not owned");
                                return;
                            }
                            //Validate the info
                            if (info.UniqueId != id || info.Username.ToLower().Trim() != name.ToLower().Trim())
                            {
                                DenyConnection(incoming, "Information mismatch. Log back in.");
                                return;
                            }
                            //Approve the connection
                            ApproveConnection(incoming, new WebInterface.WebPlayerInfoResponse
                            {
                                Premium = info.Owns(Common.StaticSettings.MPTanksPremiumProductId),
                                Id = id,
                                Username = name
                            });
                        }
                        catch (MultiplayerAuthTokenInvalidException)
                        {
                            DenyConnection(incoming, "Auth token invalid");
                            return;
                        }
                        catch
                        {
                            if (GlobalSettings.Trace) throw;
                            DenyConnection(incoming, "Unknown error");
                            return;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                if (GlobalSettings.Debug) throw;
                DenyConnection(incoming, "Invalid connection");
                Server.Logger.Error("[SERVER] [LOGIN] Connection Approval", ex);
                return;
            }
        }
        private void ApproveConnection(NetIncomingMessage msg, WebInterface.WebPlayerInfoResponse playerInfo = null)
        {
            msg.SenderConnection.Approve();
            Server.Logger.Info($"[SERVER] [LOGIN] {playerInfo?.Username ?? ""} joined");
            //To be removed: we should defer until we check the login info
            Server.Connections.Accept(msg.SenderConnection, playerInfo);
        }

        private void DenyConnection(NetIncomingMessage msg, string reason = "Server terminated connection")
        {
            msg.SenderConnection.Deny(reason);
        }

        public void ProcessMessage(NetIncomingMessage message)
        {

        }
    }
}

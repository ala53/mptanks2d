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
            var connection = incoming.SenderConnection;
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
                        DenyConnection(connection, "Password incorrect");
                        return;
                    }
                }
                //Check that they aren't on the server
                if (Server.Players.FirstOrDefault(a => a.Player.UniqueId == id) != null)
                {
                    DenyConnection(connection, "Already connected to server");
                    return;
                }

                if (Server.Configuration.Offline)
                {
                    //Directly connect, regardless of whether they are who they say they are
                    if (Server.Players.Count < Server.Configuration.MaxPlayers)
                        ApproveConnection(connection, new WebInterface.WebPlayerInfoResponse
                        {
                            Id = id,
                            Username = name,
                            Premium = true
                        });
                    else DenyConnection(connection, "Too many players");
                }
                else
                {
                    if (token == "OFFLINE")
                    {
                        DenyConnection(connection, "You're currently in offline mode.");
                        return;
                    }
                    //Defer and check
                    Task.Run(async () =>
                    {
                        try
                        {
                            var info = await ZSB.DrmClient.Multiplayer.ValidateServerTokenAsync(token);
                            //Check that they own the game
                            if (!info.Owns(Common.StaticSettings.MPTanksProductId))
                            {
                                DenyConnection(connection, "Game not owned");
                                return;
                            }
                            //Validate the info
                            if (info.UniqueId != id || info.Username.ToLower().Trim() != name.ToLower().Trim())
                            {
                                DenyConnection(connection, "Information mismatch. Log back in.");
                                return;
                            }
                            //Approve the connection
                            ApproveConnection(connection, new WebInterface.WebPlayerInfoResponse
                            {
                                Premium = info.Owns(Common.StaticSettings.MPTanksPremiumProductId),
                                Id = id,
                                Username = name
                            });
                        }
                        catch (MultiplayerAuthTokenInvalidException)
                        {
                            DenyConnection(connection, "Auth token invalid");
                            return;
                        }
                        catch (UnableToAccessAccountServerException)
                        {
                            Server.Logger.Info($"{name} kicked from game: unable to access ZSB account servers.");
                            DenyConnection(connection, "Server is unable to access the ZSB account server");
                        }
                        catch (Exception ex)
                        {
                            Server.Logger.Error("[SERVER] [LOGIN] Connection handling error", ex);
                            if (GlobalSettings.Trace) throw;
                            DenyConnection(connection, "Unknown server error");
                            return;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                if (GlobalSettings.Debug) throw;
                DenyConnection(connection, "Invalid connection");
                Server.Logger.Error("[SERVER] [LOGIN] Connection Approval Failure", ex);
                return;
            }
        }
        private void ApproveConnection(NetConnection conn, WebInterface.WebPlayerInfoResponse playerInfo = null)
        {
            conn.Approve();
            Server.Logger.Info($"[SERVER] [LOGIN] {playerInfo?.Username ?? ""} joined");
            //To be removed: we should defer until we check the login info
            Server.Connections.Accept(conn, playerInfo);
        }

        private void DenyConnection(NetConnection conn, string reason = "Server terminated connection")
        {
            conn.Deny(reason);
        }

        public void ProcessMessage(NetIncomingMessage message)
        {

        }
    }
}

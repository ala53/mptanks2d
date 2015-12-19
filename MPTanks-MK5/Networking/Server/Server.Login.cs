using Lidgren.Network;
using Microsoft.Xna.Framework;
using MPTanks.Engine.Settings;
using MPTanks.Networking.Common;
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
                int verMajor = incoming.ReadInt32(), verMinor = incoming.ReadInt32();

                if (StaticSettings.VersionMajor != verMajor ||
                    (StaticSettings.VersionMajor == verMajor && StaticSettings.VersionMinor > verMinor))
                {
                    DenyConnection(connection, "Client version mismatch. Server version is " +
                        $"{StaticSettings.VersionMajor}.{StaticSettings.VersionMinor}. Client is {verMajor}.{verMinor}.");
                    return;
                }

                if (Server.Configuration.Password != null)
                {
                    if (pass != Server.Configuration.Password)
                    {
                        DenyConnection(connection, "The password you entered was incorrect.");
                        return;
                    }
                }
                //Check that they aren't on the server
                if (Server.Players.FirstOrDefault(a => a.Player.UniqueId == id) != null)
                {
                    DenyConnection(connection, "You're already connected to the server.");
                    return;
                }

                if (Server.Players.Count >= Server.Configuration.MaxPlayers)
                {
                    DenyConnection(connection, "There are too many players on the server.");
                    return;
                }

                if (Server.Configuration.Offline)
                {
                    //Directly connect, regardless of whether they are who they say they are
                    ApproveConnection(connection, new WebInterface.WebPlayerInfoResponse
                    {
                        Id = id,
                        Username = name,
                        Premium = true
                    });
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
                                DenyConnection(connection, "You don't own MP Tanks.");
                                return;
                            }
                            //Validate the info
                            if (info.UniqueId != id || info.Username.ToLower().Trim() != name.ToLower().Trim())
                            {
                                DenyConnection(connection, "Information mismatch. Please log back in.");
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
                            DenyConnection(connection, "The authentication token the client sent was invalid.");
                            return;
                        }
                        catch (UnableToAccessAccountServerException)
                        {
                            Server.Logger.Info($"{name} kicked from game: unable to access ZSB account servers.");
                            DenyConnection(connection, "The server isn't able to access the ZSB account server.");
                            return;
                        }
                        catch (Exception ex)
                        {
                            Server.Logger.Error("[SERVER] [LOGIN] Connection handling error", ex);
                            if (GlobalSettings.Trace) throw;
                            DenyConnection(connection, "An unknown server error occurred.");
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
            //Send a mod listing
            var modList = Modding.ModDatabase.LoadedModulesList.Select(a => new
            {
                Name = a.Name,
                Major = a.Version.Major,
                MinMinor = a.Version.Minor,
                Whitelisted = a.UsesWhitelist
            });

            var msg = Server.NetworkServer.CreateMessage();
            //Write mod list
            msg.Write(modList.Count());
            foreach (var mod in modList)
            {
                msg.Write(mod.Name);
                msg.Write(mod.Major);
                msg.Write(mod.MinMinor);
                msg.Write(mod.Whitelisted);
                msg.WritePadBits();
            }

            conn.Approve();
            Server.Logger.Info($"[SERVER] [LOGIN] {playerInfo?.Username ?? ""} joined");
            //To be removed: we should defer until we check the login info
            Server.Connections.Accept(conn, playerInfo);
        }

        private void DenyConnection(NetConnection conn, string reason = "Server terminated connection")
        {
            conn?.Deny(reason);
        }

        public void ProcessMessage(NetIncomingMessage message)
        {

        }
    }
}

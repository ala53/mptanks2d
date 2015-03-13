using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using PlayerIO.GameLibrary;
using System.Drawing;

namespace DrawPad {
	//Player class. each player that join the game will have these attributes.
	public class Player : BasePlayer {
	}

	[RoomType("DrawPad")]
	public class GameCode : Game<Player> {
		// This method is called when an instance of your the game is created
		public override void GameStarted() {
			// anything you write to the Console will show up in the 
			// output window of the development server
			Console.WriteLine("Game is started");
		}

		// This method is called when the last player leaves the room, and it's closed down.
		public override void GameClosed() {
			Console.WriteLine("RoomId: " + RoomId);
		}

		// This method is called whenever a player joins the game
		public override void UserJoined(Player player) {
			
			//Send info about all already connected users to the newly joined users chat
			Message m = Message.Create("ChatInit");
			m.Add(player.Id);

			foreach(Player p in Players) {
				m.Add(p.Id, p.ConnectUserId);
			}

			player.Send(m);

			//Informs other users chats that a new user just joined.
			Broadcast("ChatJoin", player.Id, player.ConnectUserId);
		}

		// This method is called when a player leaves the game
		public override void UserLeft(Player player) {
			//Tell the chat that the player left.
			Broadcast("ChatLeft", player.Id);
		}

		// This method is called when a player sends a message into the server code
		public override void GotMessage(Player player, Message message) {
			switch(message.Type) {
				case "start": {
						Broadcast("start", player.Id, player.ConnectUserId, message.GetInt(0), message.GetInt(1));
						break;
					}
				case "stop": {
						Broadcast("stop", player.Id);
						break;
					}
				case "move": {
						Broadcast("move", player.Id, message.GetInt(0), message.GetInt(1));
						break;
					}
				case "ChatMessage":{
						Broadcast("ChatMessage", player.Id, message.GetString(0));
						break;
					}
			}
		}
	}
}
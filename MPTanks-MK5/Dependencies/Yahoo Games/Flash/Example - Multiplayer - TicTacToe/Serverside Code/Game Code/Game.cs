using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using PlayerIO.GameLibrary;
using System.Drawing;

namespace TicTacToe {
	//Player class. each player that join the game will have these attributes.
	public class Player : BasePlayer {
		public Boolean IsReady = true;
	}

	public class Tile {
		public String Type;
	}

	[RoomType("TicTacToe")]
	public class GameCode : Game<Player> {
		private Player player1;
		private Player player2;
		private Player hasTurn;
		private int fieldsUsed = 0;
		private Tile[,] field = {
            {new Tile(),new Tile(),new Tile()},
            {new Tile(),new Tile(),new Tile()},
            {new Tile(),new Tile(),new Tile()}
        };
		private String Blank = "blank";
		private String Circle = "circle";
		private String Cross = "cross";		
		
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

			joinGame(player);


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
			
		//	Console.WriteLine("User left the chat " + player.Id);
			
			if(player == player1) {
				player1 = null;
			} else if(player == player2) {
				player2 = null;
			} else
				return;

			Broadcast("left", player1 != null ? player1.ConnectUserId : "", player2 != null ? player2.ConnectUserId : "");

		}

		// This method is called when a player sends a message into the server code
		public override void GotMessage(Player player, Message message) {
			switch(message.Type) {
				case "click": {
						int x = message.GetInt(0);
						int y = message.GetInt(1);
						if(x >= 0 && x <= 2 && y >= 0 && y <= 2) {
							if(field[x, y].Type == Blank) {

								fieldsUsed++;

								String type = player == player1 ? Cross : Circle;
								field[x, y].Type = type;

								if(hasWon(type, x, y)) {
									Broadcast("win", player1 == hasTurn ? 0 : 1, hasTurn.ConnectUserId);

								} else if(fieldsUsed == 9) {
									Broadcast("tie");
								}

								hasTurn = hasTurn == player1 ? player2 : player1;
								Broadcast("place", x, y, type, player1 == hasTurn ? 0 : 1);
							}
						}
						break;
					}
				case "reset": {
						if(player1 != null && player2 != null) {
							player.IsReady = true;
							if(player1.IsReady && player2.IsReady) {
								resetGame(null);
							}
						}
						break;
					}
				case "join": {
						joinGame(player);
						break;
					}

				case "ChatMessage": {
						Broadcast("ChatMessage", player.Id, message.GetString(0));
						break;
					}
			}
		}

		private void joinGame(Player user) {
			if(player1 == null) {
				user.Send("init", 0, user.ConnectUserId);
				player1 = user;
				hasTurn = user;
			} else if(player2 == null) {
				user.Send("init", 1, user.ConnectUserId);
				player2 = user;
				hasTurn = user;
			} else {
				//Send current game state to spectators
				user.Send("spectator", player1.ConnectUserId, player2.ConnectUserId,
					field[0, 0].Type, field[1, 0].Type, field[2, 0].Type,
					field[0, 1].Type, field[1, 1].Type, field[2, 1].Type,
					field[0, 2].Type, field[1, 2].Type, field[2, 2].Type
				);
				return;
			}
			if(player1 != null && player2 != null) {
				Broadcast("join", player1.ConnectUserId, player2.ConnectUserId);
				user.Send("join", player1.ConnectUserId, player2.ConnectUserId);
				resetGame(user);
			}
		}

		private Boolean hasWon(String type, int x, int y) {
			//Pretty hardcoded
			return (isEqual(type, x - 2, y) + isEqual(type, x - 1, y) + isEqual(type, x + 1, y) + isEqual(type, x + 2, y)) == 2 ||
				   (isEqual(type, x, y - 2) + isEqual(type, x, y - 1) + isEqual(type, x, y + 1) + isEqual(type, x, y + 2)) == 2 ||
				   (isEqual(type, x - 2, y - 2) + isEqual(type, x - 1, y - 1) + isEqual(type, x + 1, y + 1) + isEqual(type, x + 2, y + 2)) == 2 ||
				   (isEqual(type, x - 2, y + 2) + isEqual(type, x - 1, y + 1) + isEqual(type, x + 1, y - 1) + isEqual(type, x + 2, y - 2)) == 2;
		}


		private int isEqual(String type, int x, int y) {
			if(x >= 0 && x <= 2 && y >= 0 && y <= 2) {
				return field[x, y].Type == type ? 1 : 0;
			}
			return 0;
		}

		private void resetGame(Player user) {

			fieldsUsed = 0;
			for(int a = 0; a < 3; a++) {
				for(int b = 0; b < 3; b++) {
					field[a, b].Type = Blank;
				}
			}

			player1.IsReady = false;
			player2.IsReady = false;
			if(user != null)
				user.Send("reset", player1 == hasTurn ? 0 : 1);
			Broadcast("reset", player1 == hasTurn ? 0 : 1);
		}
	}
}
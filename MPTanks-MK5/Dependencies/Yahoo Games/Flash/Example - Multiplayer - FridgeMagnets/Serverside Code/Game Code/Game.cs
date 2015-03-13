using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using PlayerIO.GameLibrary;
using System.Drawing;

namespace FridgeMagnets {
	//Player class. each player that join the game will have these attributes.
	public class Player : BasePlayer {
		public int X;
		public int Y;
		public Player() {
			X = 0; //Player mouseX
			Y = 0; //Player mouseY
		}
	}

	//Letter class. Each letter on the screen is represented by an instance of this class.
	public class Letter {
		public int X;
		public int Y;
		public Letter(int x, int y) {
			this.X = x;
			this.Y = y;
		}
	}

	[RoomType("FridgeMagnets")]
	public class GameCode : Game<Player> {
		//Create array to store our letters
		private Letter[] letters = new Letter[230];
		// This method is called when an instance of your the game is created
		public override void GameStarted() {
			// anything you write to the Console will show up in the 
			// output window of the development server
			Console.WriteLine("Game is started");

			// Create 230 letters
			for(int a = 0; a < letters.Length; a++) {
				letters[a] = new Letter(-1, -1);
			}

			// add a timer that sends out an update every 100th millisecond
			AddTimer(delegate {
				//Create update message
				Message update = Message.Create("update");

				//Add mouse cordinates for each player to the message
				foreach(Player p in Players) {
					update.Add(p.Id, p.X, p.Y);
				}

				//Broadcast message to all players
				Broadcast(update);
			}, 100);

		}

		// This method is called when the last player leaves the room, and it's closed down.
		public override void GameClosed() {
			Console.WriteLine("RoomId: " + RoomId);
		}

		// This method is called whenever a player joins the game
		public override void UserJoined(Player player) {
			// Create init message for the joining player
			Message m = Message.Create("init");

			// Tell player their own id
			m.Add(player.Id);

			//Add the current position of all letters to the init message
			for(int a = 0; a < letters.Length; a++) {
				Letter l = letters[a];
				m.Add(l.X, l.Y);
			}

			// Send init message to player
			player.Send(m);
		}

		// This method is called when a player leaves the game
		public override void UserLeft(Player player) {
			Console.WriteLine("Player " + player.Id + " left the room");

			//Inform all other players that user left.
			Broadcast("left", player.Id);
		}

		// This method is called when a player sends a message into the server code
		public override void GotMessage(Player player, Message message) {
			//Switch on message type
			switch(message.Type) {
				case "move": {
						//Move letter in internal representation
						Letter l = letters[message.GetInteger(0)];
						l.X = message.GetInteger(1);
						l.Y = message.GetInteger(2);

						//inform all players that the letter have been moved
						Broadcast("move", message.GetInteger(0), l.X, l.Y);
						break;
					}
				case "mouse": {
						//Set player mouse information
						player.X = message.GetInteger(0);
						player.Y = message.GetInteger(1);
						break;
					}
				case "activate": {
						Broadcast("activate", player.Id, message.GetInteger(0));
						break;
					}
			}
		}
	}
}

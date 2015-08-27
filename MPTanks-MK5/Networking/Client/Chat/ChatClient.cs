using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Client.Chat
{
    public class ChatClient
    {
        public int NumberOfMessagesToStore { get; private set; }

        public ChatMessage[] Messages { get; private set; }

        public Client Client { get; private set; }

        public ChatClient(Client client, int numMessagesToStore = 20)
        {
            Messages = new ChatMessage[numMessagesToStore];
            for (var i = 0; i < numMessagesToStore; i++)
                Messages[i] = new ChatMessage
                {
                    Client = client,
                    Message = "",
                    SentTime = DateTime.MinValue
                };

            Client = client;
        }

        public void AddMessage(ChatMessage message)
        {
            for (var i = 1; i < Messages.Length; i++)
                Messages[i - 1] = Messages[i];

            Messages[Messages.Length - 1] = message;
            message.Client = Client;
            message.SentTime = DateTime.Now;
        }
    }
}

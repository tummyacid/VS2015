using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Examples.Echo
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("xx");
        private static Socket ircClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnMessage += BotOnMessageReceived;
            //Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnInlineQuery += BotOnInlineQueryReceived;
            Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            //Bot.OnReceiveError += BotOnReceiveError;

            var me = Bot.GetMeAsync().Result;

            Console.Title = me.Username;

            var upd = Bot.GetUpdatesAsync().Result;
            
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
        {
            if (ircClient.Connected)
            {
                ircClient.Send(Encoding.ASCII.GetBytes("PRIVMSG #nogchat :" + e.CallbackQuery.Data + "!\r\n"));
                ircClient.Send(Encoding.ASCII.GetBytes("QUIT \r\n"));

                await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "what a fag", false, string.Empty, 1);

                InlineKeyboardMarkup emtpyKey = new InlineKeyboardMarkup();
                emtpyKey.InlineKeyboard = new[]
{
                    new [] // first row
                    {
                        new InlineKeyboardButton()
                    }
};


                //await Bot.EditInlineMessageReplyMarkupAsync(e.CallbackQuery.InlineMessageId, emtpyKey);
                
                ircClient.Close();
            }
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Debugger.Break();
        }

        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received choosen inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
            
                String getUsers = "PRIVMSG #nogchat ur a fag\r\n";

            ircClient.Send(Encoding.ASCII.GetBytes(getUsers));

            ircClient.Close();
        }

        private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            InlineQueryResult[] results = {
                new InlineQueryResultLocation
                {
                    Id = "1",
                    Latitude = 40.7058316f, // displayed result
                    Longitude = -74.2581888f,
                    Title = "New York",
                    InputMessageContent = new InputLocationMessageContent // message if result is selected
                    {
                        Latitude = 40.7058316f,
                        Longitude = -74.2581888f,
                    }
                },

                new InlineQueryResultLocation
                {
                    Id = "2",
                    Longitude = 52.507629f, // displayed result
                    Latitude = 13.1449577f,
                    Title = "Berlin",
                    InputMessageContent = new InputLocationMessageContent // message if result is selected
                    {
                        Longitude = 52.507629f,
                        Latitude = 13.1449577f
                    }
                }
            };

            await Bot.AnswerInlineQueryAsync(inlineQueryEventArgs.InlineQuery.Id, results, isPersonal: true, cacheTime: 0);
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.TextMessage) return;

            if (message.Text.Contains("ender")) // send inline keyboard
            {
                var reply = "Kill all the humans!";

                await Bot.SendTextMessageAsync(message.Chat.Id, reply,
                    replyMarkup: new ReplyKeyboardHide());
            }
            if (message.Text.StartsWith("/irc"))
            {
                await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

                String setNick = "NICK nogHead\r\n";
                String setUser = "USER nogHead 0 * :nogHead \r\n";
                String setChan = "JOIN #nogchat\r\n";
                String getUsers = "NAMES #nogchat\r\n";

                ircClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ircClient.Connect("irc.freenode.org", 6667);

                byte[] inbound = new byte[1024];

                ircClient.Receive(inbound);
                Console.WriteLine(ASCIIEncoding.ASCII.GetString(inbound));

                ircClient.Send(Encoding.ASCII.GetBytes(setNick));
                ircClient.Send(Encoding.ASCII.GetBytes(setUser));
                ircClient.Receive(inbound);
                Console.WriteLine(ASCIIEncoding.ASCII.GetString(inbound));

                
                ircClient.Send(Encoding.ASCII.GetBytes(setChan));

                ircClient.Receive(inbound);
                Console.WriteLine(ASCIIEncoding.ASCII.GetString(inbound));

                ircClient.Send(Encoding.ASCII.GetBytes(setChan));
                ircClient.Receive(inbound);
                Console.WriteLine(ASCIIEncoding.ASCII.GetString(inbound));
                ircClient.Send(Encoding.ASCII.GetBytes(getUsers));
                while (!ASCIIEncoding.ASCII.GetString(inbound).Contains("353"))
                {
                    ircClient.Receive(inbound);
                }
                String nameList = ASCIIEncoding.ASCII.GetString(inbound);
                nameList = nameList.Substring(nameList.IndexOf("353"));
                while (!ASCIIEncoding.ASCII.GetString(inbound).Contains("366"))
                {
                    ircClient.Receive(inbound);
                    nameList = nameList + ASCIIEncoding.ASCII.GetString(inbound);
                }
                nameList = nameList.Substring(0, nameList.IndexOf("366"));
                Console.WriteLine(nameList.Split(':')[1]);

                List<string> names = new List<string>();

                List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
                foreach (string s in nameList.Split(':')[1].Split())
                {
                    InlineKeyboardButton btn = new InlineKeyboardButton(s);
                    btn.CallbackData = s + " is a fag";
                    buttons.Add(btn);
                }

                var keyboard = new InlineKeyboardMarkup(buttons.ToArray());

                await Bot.SendTextMessageAsync(message.Chat.Id, "who is a fag?",
                    replyMarkup: keyboard);
                

            }
        }


    }
}
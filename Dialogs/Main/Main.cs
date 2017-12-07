﻿using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public partial class Main
    {
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            Func<string, Task> YouFoundABug = new Func<string, Task>(async (bug) =>
            {
                await context.PostAsync("吼～崩潰啦，都是你害的><");
                await Task.Delay(2000);
                await context.PostAsync("為了報復你，我要叫你幫我debug哇哈哈哈OwO");
                await Task.Delay(3000);
                await context.PostAsync(bug);
            });
            try
            {
                var message = await argument;
                //LastUserMessageData lastUserMessageData = null;
                if (message.Text == null)
                {
                    await context.PostAsync("吼～都這樣，不說點話嗎？><");
                }
                else if (message.Text.Length > 250)
                {
                    await context.PostAsync("???");
                }
                else if (await Posts.UrlReactor.ReactAsync(context, argument, message)) { }
                else
                {
                    var messageText = ConvertMessageText(message.Text);
                    var messageRepeatCount = GetRepeatCount(message.From.Id, messageText);
                    if (await Gossiper.ReactAsync(context, argument, messageText)) { }
                    else switch (messageText)
                        {
                            case "我要說甚麼":
                                {
                                    var commands = Constants.Commands.ListCommands();
                                    await context.PostAsync($"你可以說說看：{commands[Rand.Next(commands.Count)]}");
                                }
                                break;
                            case Constants.Commands.C1:
                                {
                                    var content = string.Join("<br/>", ganTalkBoard.GetBoard().Select(v => $"{v.Item2}人說了：{v.Item1}"));
                                    if (string.IsNullOrEmpty(content)) content = "目前沒有資料TwT";
                                    await context.PostAsync("\\幹話排行榜/ <(_ _)><br/>" + content);
                                }
                                break;
                            case Constants.Commands.C2:
                                {
                                    switch (messageRepeatCount)
                                    {
                                        case 0: await context.PostAsync("不好說，這真的不好說"); break;
                                        case 1: await context.PostAsync("走遠了......"); break;
                                        case 2: await context.PostAsync("剩下的就不要再問了"); break;
                                        case 3: await context.PostAsync("就跟你說不要再問了！"); break;
                                        case 4: await context.PostAsync("就說不要再問了你是沒聽到嗎？"); break;
                                        case 5: await context.PostAsync("好啦好啦，我說我說"); break;
                                        case 6: await context.PostAsync("好啦我真的要說了，但是要幫我保密哦"); break;
                                        case 7: await context.PostAsync("真的要幫我保密哦！(勾小拇指"); break;
                                        default:
                                            {
                                                switch ((messageRepeatCount - 8) % 10)
                                                {
                                                    case 0: await context.PostAsync("就是呢......"); break;
                                                    case 1: await context.PostAsync("我覺得......"); break;
                                                    case 2: await context.PostAsync("那個Spec......"); break;
                                                    case 3: await context.PostAsync("應該要一開始就寫清楚，而且......"); break;
                                                    case 4: await context.PostAsync("不要一直改啦，這樣......"); break;
                                                    case 5: await context.PostAsync("壞透了，真的壞透了！><"); break;
                                                    case 6: await context.PostAsync("生氣氣啦><"); break;
                                                    case 7: await context.PostAsync("沒了，你還要我說甚麼？"); break;
                                                    case 8: await context.PostAsync("好啦其實助教人也滿好的，也很厲害，甚麼問題都可以很快回答得出來～"); break;
                                                    case 9: await context.PostAsync("而且作業也是很好玩、可以學到很多東西！只是呢......"); break;
                                                    default: await YouFoundABug($"messageRepeatCount: {messageRepeatCount}"); break;
                                                }
                                            }
                                            break;
                                    }
                                }
                                break;
                            case Constants.Commands.C3:
                                {
                                    await context.PostAsync($"↓我知道你的資訊有這麼多↓<br/>Id: {message.From.Id}<br/>" + $"Name: {message.From.Name}<br/>" + $"Properties: {message.From.Properties}");
                                }
                                break;
                            case Constants.Commands.C4:
                                {
                                    switch (messageRepeatCount)
                                    {
                                        case 0: await context.PostAsync("這是秘密>///<"); break;
                                        case 1: await context.PostAsync("不要再問了，這是秘密！"); break;
                                        case 2: await context.PostAsync("就說這是秘密了，再問下去我崩潰給你看哦><"); break;
                                        case 3: throw new NotImplementedException();
                                    }
                                }
                                break;
                            default:
                                {
                                    //await context.PostAsync("\u4f60\u8aaa\u4e86\u300c"/*你說了「*/ + message.Text + "\u300d"/*」*/);
                                    string msg = message.Text;
                                    ganTalkBoard.Update(message.From.Id, msg);
                                    switch ((int)(Rand.NextDouble() * 6))
                                    {
                                        case 0: break;
                                        case 1: msg = "你說了「" + msg + "」"; break;
                                        case 2: msg = "好啦，" + msg; break;
                                        case 3: msg = msg + " XDD"; break;
                                        case 4: msg = msg + " www"; break;
                                        case 5: msg = msg + " ^_^"; break;
                                    }
                                    await context.PostAsync(msg);
                                }
                                break;
                        }
                    SetLastUserMessage(message.From.Id, messageText);
                }
            }
            catch (Exception error)
            {
                await YouFoundABug(error.ToString());
            }
            context.Wait(MessageReceivedAsync);
        }
    }
    public partial class Main
    {
        [Serializable]
        class GanTalkBoard
        {
            public const int BoardSize = 10;
            Dictionary<string, HashSet<string>> data = new Dictionary<string, HashSet<string>>();
            List<Tuple<int,string>> board = new List<Tuple<int,string>>();
            private void UpdateBoard(string msg,int cnt)
            {
                bool found = false;
                for (int i = 0; i < board.Count; i++) if (board[i].Item2 == msg)
                    {
                        board[i] = new Tuple<int,string>(-cnt,msg);
                        found = true;
                        break;
                    }
                if (!found) board.Add(new Tuple<int, string>(-cnt, msg));
                board.Sort();
                if (board.Count > BoardSize) board.RemoveRange(BoardSize, board.Count - BoardSize);
            }
            public void Update(string userId, string msg)
            {
                if (!data.ContainsKey(msg)) data.Add(msg, new HashSet<string>());
                if (data[msg].Add(userId)) UpdateBoard(msg, data[msg].Count);
            }
            public List<Tuple<string,int>>GetBoard()
            {
                return board.Select(v => new Tuple<string, int>(v.Item2, -v.Item1)).ToList();
            }
        }
        // Azure page: https://portal.azure.com/#blade/WebsitesExtension/BotsIFrameBlade/id/%2Fsubscriptions%2Fed3b27fa-21db-4e94-8061-2d654c6b87d5%2FresourceGroups%2Ffsps60312botservicetest%2Fproviders%2FMicrosoft.Web%2Fsites%2Ffsps60312botservicetest
        // Unicode convert: https://www.ifreesite.com/unicode-ascii-ansi.htm
        static Random Rand = new Random();
        [Serializable]
        class LastUserMessageData
        {
            public string message;
            public int repeat;
        }
        Dictionary<string, LastUserMessageData> LastUserMessage = new Dictionary<string, LastUserMessageData>();
        GanTalkBoard ganTalkBoard = new Main.GanTalkBoard();
        int GetRepeatCount(string userId,string message)
        {
            var lastUserMessageData = GetLastUserMessage(userId);
            if (lastUserMessageData == null || lastUserMessageData.message != message) return 0;
            else return lastUserMessageData.repeat;
        }
        LastUserMessageData GetLastUserMessage(string userId)
        {
            if (LastUserMessage.ContainsKey(userId)) return LastUserMessage[userId];
            return null;
        }
        string RemoveDuplicatedSpaces(string msg)
        {
            string ans = "";
            for (int i = 0; i < msg.Length; i++) if (!(i > 0 && msg[i] == ' ' && msg[i - 1] == ' ')) ans += msg[i];
            return ans;
        }
        string ConvertMessageText(string msg)
        {
            msg = msg.Replace("？", "?").Replace("什麼","甚麼").Replace('喔','哦').Replace('\t',' ').TrimEnd(new char[]{ '?',' '}).TrimStart(' ');
            msg = RemoveDuplicatedSpaces(msg);
            return Mapping.Mapper.Map(msg);
        }
        void SetLastUserMessage(string userId, string message)
        {
            System.Diagnostics.Trace.WriteLine($"userId: {userId}");
            System.Diagnostics.Trace.WriteLine($"message: {message}");
            if (message == null) return;
            if (message.Length > 100) message = message.Remove(100);
            if (!LastUserMessage.ContainsKey(userId)) LastUserMessage.Add(userId, new LastUserMessageData { message = message, repeat = 1 });
            else if (LastUserMessage[userId].message != message) LastUserMessage[userId] = new LastUserMessageData { message = message, repeat = 1 };
            else LastUserMessage[userId].repeat++;
        }
    }
}
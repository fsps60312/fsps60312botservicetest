using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

using System.Collections.Generic;

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
                    switch (messageText)
                    {
                        case "code風景區":
                        case "Code風景區": await context.PostAsync("很棒的名字，不覺得嗎？XD<br/>然後，我的英文名字是「code scenic」哦，Google看看！<br/>總之，像欣賞風景一樣快樂的探索程式之美吧！");break;
                        case "傳一則貼文的網址(?)": await context.PostAsync("吼～不是真的要你說這句話啦！<br/>是你要傳一則貼文的網址給我～><"); break;
                        case "說話": await context.PostAsync("話"); break;
                        case "借我錢":await context.PostAsync("我沒錢><");break;
                        case "我要說甚麼":
                            {
                                var commands = Constants.Commands.ListCommands();
                                await context.PostAsync($"你可以說說看：{commands[Rand.Next(commands.Count)]}");
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
                                switch ((int)(Rand.NextDouble() * 6))
                                {
                                    case 0: break;
                                    case 1: msg = "你說了「" + msg + "」"; break;
                                    case 2: msg = "好啦，"+ msg; break;
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
        string ConvertMessageText(string msg)
        {
            msg = msg.Replace("？", "?").Replace("什麼","甚麼").TrimEnd('?');
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
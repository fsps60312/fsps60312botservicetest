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
                await context.PostAsync("\u543c\uff5e\u5d29\u6f70\u5566\uff0c\u90fd\u662f\u4f60\u5bb3\u7684><"/*吼～崩潰啦，都是你害的><*/);
                await Task.Delay(2000);
                await context.PostAsync("\u70ba\u4e86\u5831\u5fa9\u4f60\uff0c\u6211\u8981\u53eb\u4f60\u5e6b\u6211debug\u54c7\u54c8\u54c8\u54c8OwO"/*為了報復你，我要叫你幫我debug哇哈哈哈OwO*/);
                await Task.Delay(3000);
                await context.PostAsync(bug);
            });
            try
            {
                var message = await argument;
                //LastUserMessageData lastUserMessageData = null;
                if (message.Text == null)
                {
                    await context.PostAsync("\u543c\uff5e\u90fd\u9019\u6a23\uff0c\u4e0d\u8aaa\u9ede\u8a71\u55ce\uff1f><"/*吼～都這樣，不說點話嗎？><*/);
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
                        case Constants.Commands.C1:
                            {
                                await context.PostAsync($"你可以試著說說看這些話：<br/>{string.Join("<br/>", Constants.Commands.ListCommands())}");
                            }
                            break;
                        case Constants.Commands.C2:
                            {
                                switch (messageRepeatCount)
                                {
                                    case 0: await context.PostAsync("\u4e0d\u597d\u8aaa\uff0c\u9019\u771f\u7684\u4e0d\u597d\u8aaa"/*不好說，這真的不好說*/); break;
                                    case 1: await context.PostAsync("\u8d70\u9060\u4e86......"/*走遠了......*/); break;
                                    case 2: await context.PostAsync("\u5269\u4e0b\u7684\u5c31\u4e0d\u8981\u518d\u554f\u4e86"/*剩下的就不要再問了*/); break;
                                    case 3: await context.PostAsync("\u5c31\u8ddf\u4f60\u8aaa\u4e0d\u8981\u518d\u554f\u4e86\uff01"/*就跟你說不要再問了！*/); break;
                                    case 4: await context.PostAsync("\u5c31\u8aaa\u4e0d\u8981\u518d\u554f\u4e86\u4f60\u662f\u6c92\u807d\u5230\u55ce\uff1f"/*就說不要再問了你是沒聽到嗎？*/); break;
                                    case 5: await context.PostAsync("\u597d\u5566\u597d\u5566\uff0c\u6211\u8aaa\u6211\u8aaa"/*好啦好啦，我說我說*/); break;
                                    case 6: await context.PostAsync("\u597d\u5566\u6211\u771f\u7684\u8981\u8aaa\u4e86\uff0c\u4f46\u662f\u8981\u5e6b\u6211\u4fdd\u5bc6\u54e6"/*好啦我真的要說了，但是要幫我保密哦*/); break;
                                    case 7: await context.PostAsync("\u771f\u7684\u8981\u5e6b\u6211\u4fdd\u5bc6\u54e6\uff01(\u52fe\u5c0f\u62c7\u6307"/*真的要幫我保密哦！(勾小拇指*/); break;
                                    default:
                                        {
                                            switch ((messageRepeatCount - 8) % 10)
                                            {
                                                case 0: await context.PostAsync("\u5c31\u662f\u5462......"/*就是呢......*/); break;
                                                case 1: await context.PostAsync("\u6211\u89ba\u5f97......"/*我覺得......*/); break;
                                                case 2: await context.PostAsync("\u90a3\u500bSpec......"/*那個Spec......*/); break;
                                                case 3: await context.PostAsync("\u61c9\u8a72\u8981\u4e00\u958b\u59cb\u5c31\u5beb\u6e05\u695a\uff0c\u800c\u4e14......"/*應該要一開始就寫清楚，而且......*/); break;
                                                case 4: await context.PostAsync("\u4e0d\u8981\u4e00\u76f4\u6539\u5566\uff0c\u9019\u6a23......"/*不要一直改啦，這樣......*/); break;
                                                case 5: await context.PostAsync("\u58de\u900f\u4e86\uff0c\u771f\u7684\u58de\u900f\u4e86\uff01><"/*壞透了，真的壞透了！><*/); break;
                                                case 6: await context.PostAsync("\u751f\u6c23\u6c23\u5566><"/*生氣氣啦><*/); break;
                                                case 7: await context.PostAsync("\u6c92\u4e86\uff0c\u4f60\u9084\u8981\u6211\u8aaa\u751a\u9ebc\uff1f"/*沒了，你還要我說甚麼？*/); break;
                                                case 8: await context.PostAsync("\u597d\u5566\u5176\u5be6\u52a9\u6559\u4eba\u4e5f\u6eff\u597d\u7684\uff0c\u4e5f\u5f88\u53b2\u5bb3\uff0c\u751a\u9ebc\u554f\u984c\u90fd\u53ef\u4ee5\u5f88\u5feb\u56de\u7b54\u5f97\u51fa\u4f86\uff5e"/*好啦其實助教人也滿好的，也很厲害，甚麼問題都可以很快回答得出來～*/); break;
                                                case 9: await context.PostAsync("\u800c\u4e14\u4f5c\u696d\u4e5f\u662f\u5f88\u597d\u73a9\u3001\u53ef\u4ee5\u5b78\u5230\u5f88\u591a\u6771\u897f\uff01\u53ea\u662f\u5462......"/*而且作業也是很好玩、可以學到很多東西！只是呢......*/); break;
                                                default: await YouFoundABug($"messageRepeatCount: {messageRepeatCount}"); break;
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        case Constants.Commands.C3:
                            {
                                await context.PostAsync(/*↓我知道你的資訊有這麼多↓*/$"\u2193\u6211\u77e5\u9053\u4f60\u7684\u8cc7\u8a0a\u6709\u9019\u9ebc\u591a\u2193<br/>Id: {message.From.Id}<br/>" + $"Name: {message.From.Name}<br/>" + $"Properties: {message.From.Properties}");
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
                                    case 1: msg = "\u4f60\u8aaa\u4e86\u300c"/*你說了「*/ + msg + "\u300d"/*」*/; break;
                                    case 2: msg = "\u597d\u5566\uff0c"/*好啦，*/+ msg; break;
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
            msg = msg.Replace("？", "?").Replace("什麼","甚麼");
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
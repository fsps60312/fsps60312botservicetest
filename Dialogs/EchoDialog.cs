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
    public partial class EchoDialog : IDialog<object>
    {
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            Func<string, Task> YouFoundABug = new Func<string, Task>(async (bug) =>
                 {
                     await context.PostAsync("\u543c\uff5e\u88ab\u4f60\u627e\u5230bug\u60f9\u5566><"/*�q��Q�A���bug�S��><*/);
                     await context.PostAsync("\u70ba\u4e86\u5831\u5fa9\u4f60\uff0c\u6211\u8981\u53eb\u4f60\u5e6b\u6211debug\u54c8\u54c8\u54c8OwO"/*���F���_�A�A�ڭn�s�A����debug������OwO*/);
                     await context.PostAsync(bug);
                 });
            try
            {
                var message = await argument;
                var lastUserMessageData = GetLastUserMessage(message.From.Id);
                switch (message.Text)
                {
                    case "SP�U�Ы��ˡH":
                        {
                            if (lastUserMessageData == null) await context.PostAsync("\u4e0d\u597d\u8aaa\uff0c\u9019\u771f\u7684\u4e0d\u597d\u8aaa"/*���n���A�o�u�����n��*/);
                            else
                            {
                                switch (lastUserMessageData.repeat)
                                {
                                    case 1: await context.PostAsync("\u8d70\u9060\u4e86......"/*�����F......*/); break;
                                    case 2: await context.PostAsync("\u5269\u4e0b\u7684\u5c31\u4e0d\u8981\u518d\u554f\u4e86"/*�ѤU���N���n�A�ݤF*/); break;
                                    case 3: await context.PostAsync("\u5c31\u8ddf\u4f60\u8aaa\u4e0d\u8981\u518d\u554f\u4e86\uff01"/*�N��A�����n�A�ݤF�I*/); break;
                                    case 4: await context.PostAsync("\u5c31\u8aaa\u4e0d\u8981\u518d\u554f\u4e86\u4f60\u662f\u6c92\u807d\u5230\u55ce\uff1f"/*�N�����n�A�ݤF�A�O�Sť��ܡH*/); break;
                                    case 5: await context.PostAsync("\u597d\u5566\u597d\u5566\uff0c\u6211\u8aaa\u6211\u8aaa"/*�n�զn�աA�ڻ��ڻ�*/); break;
                                    case 6: await context.PostAsync("\u597d\u5566\u6211\u771f\u7684\u8981\u8aaa\u4e86\uff0c\u4f46\u662f\u8981\u5e6b\u6211\u4fdd\u5bc6\u54e6"/*�n�էگu���n���F�A���O�n���ګO�K�@*/); break;
                                    case 7: await context.PostAsync("\u771f\u7684\u8981\u5e6b\u6211\u4fdd\u5bc6\u54e6\uff01(\u52fe\u5c0f\u62c7\u6307"/*�u���n���ګO�K�@�I(�Ĥp���*/); break;
                                    default:
                                        {
                                            switch ((lastUserMessageData.repeat - 8) % 9)
                                            {
                                                case 0: await context.PostAsync("\u5c31\u662f\u5462......"/*�N�O�O......*/); break;
                                                case 1: await context.PostAsync("\u6211\u89ba\u5f97......"/*��ı�o......*/); break;
                                                case 2: await context.PostAsync("\u90a3\u500bSpec......"/*����Spec......*/); break;
                                                case 3: await context.PostAsync("\u61c9\u8a72\u8981\u4e00\u958b\u59cb\u5c31\u5beb\u6e05\u695a\uff0c\u800c\u4e14......"/*���ӭn�@�}�l�N�g�M���A�ӥB......*/); break;
                                                case 4: await context.PostAsync("\u4e0d\u8981\u4e00\u76f4\u6539\u5566\uff0c\u9019\u6a23......"/*���n�@����աA�o��......*/); break;
                                                case 5: await context.PostAsync("\u58de\u900f\u4e86\uff0c\u771f\u7684\u58de\u900f\u4e86\uff01><"/*�a�z�F�A�u���a�z�F�I><*/); break;
                                                case 6: await context.PostAsync("\u751f\u6c23\u6c23\u5566><"/*�ͮ���><*/); break;
                                                case 7: await context.PostAsync("\u6c92\u4e86\uff0c\u4f60\u9084\u8981\u6211\u8aaa\u751a\u9ebc\uff1f"/*�S�F�A�A�٭n�ڻ��ƻ�H*/); break;
                                                case 8: await context.PostAsync("\u597d\u5566\u5176\u5be6SP\u4f5c\u696d\u4e5f\u662f\u6eff\u597d\u73a9\u7684\uff0c\u53ea\u662f\u5462......"/*�n�ը��SP�@�~�]�O���n�����A�u�O�O......*/); break;
                                                default:await YouFoundABug($"lastUserMessageData: {JsonConvert.SerializeObject(lastUserMessageData)}");break;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    case "https://www.facebook.com/CodingSimplifyLife/posts/1995730270697235":
                        {
                            throw new NotImplementedException();
                        }
                    case "What you know about me?":
                        {
                            await context.PostAsync($"\u2193These are what I know about you\u2193<br/>Id: {message.From.Id}<br/>" + $"Name: {message.From.Name}<br/>" + $"Properties: {message.From.Properties}");
                        }
                        break;
                    default:
                        {
                            await context.PostAsync("\u4f60\u8aaa\u4e86\u300c"/*�A���F�u*/ + message.Text + "\u300d"/*�v*/);
                        }
                        break;
                }
                //StringBuilder sb = new StringBuilder();
                //foreach (var v in context.UserData as System.Collections.IEnumerable) sb.Append(v);
                SetLastUserMessage(message.From.Id, message.Text);
            }
            catch(Exception error)
            {
                await YouFoundABug(error.ToString());
            }
            context.Wait(MessageReceivedAsync);
        }
    }
    public partial class EchoDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }
        // Azure page: https://portal.azure.com/#blade/WebsitesExtension/BotsIFrameBlade/id/%2Fsubscriptions%2Fed3b27fa-21db-4e94-8061-2d654c6b87d5%2FresourceGroups%2Ffsps60312botservicetest%2Fproviders%2FMicrosoft.Web%2Fsites%2Ffsps60312botservicetest
        // Unicode convert: https://www.ifreesite.com/unicode-ascii-ansi.htm
        Random Rand = new Random();
        class LastUserMessageData
        {
            public string message;
            public int repeat;
        }
        Dictionary<string, LastUserMessageData> LastUserMessage = new Dictionary<string, LastUserMessageData>();
        LastUserMessageData GetLastUserMessage(string userId)
        {
            if (LastUserMessage.ContainsKey(userId)) return LastUserMessage[userId];
            return null;
        }
        void SetLastUserMessage(string userId, string message)
        {
            if (message.Length > 100) message = message.Remove(100);
            if (!LastUserMessage.ContainsKey(userId)) LastUserMessage[userId] = new LastUserMessageData { message = message, repeat = 1 };
            else LastUserMessage[userId].repeat++;
        }
    }
}
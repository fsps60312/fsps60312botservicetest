using System;
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
    public partial class Main : MyDialog<object>
    {
        async Task ResumeAfterFinalJudge(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            if (message == null) return;
            context.Wait(MessageReceivedAsync);//TODO
        }
        async Task ResumeAfterGossiper(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            if (message == null) return;
            if (IsContextCompleted(message)) context.Wait(MessageReceivedAsync);
            else await context.Forward(new FinalJudge(), ResumeAfterFinalJudge, message);
        }
        async Task ResumeAfterUrlReactor(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            if (message == null) return;
            if (IsContextCompleted(message)) context.Wait(MessageReceivedAsync);
            else await context.Forward(new Gossiper(), ResumeAfterGossiper, message);
        }
        async Task ResumeAfterBasicJudge(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            if (message == null) return;
            if (IsContextCompleted(message)) context.Wait(MessageReceivedAsync);
            else await context.Forward(new Posts.UrlReactor(), ResumeAfterUrlReactor, message);
        }
        protected override async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            //Set(message, Constants.ConvertedMessageText, ConvertMessageText(message.Text));
            await context.Forward(new BasicJudge(), ResumeAfterBasicJudge, message);
        }
    }
    public partial class Main
    {
        public static bool IsContextCompleted(IMessageActivity message)
        {
            Newtonsoft.Json.Linq.JToken value;
            return message.From.Properties.TryGetValue(Constants.IsContextCompleted, out value);
        }
        public static void MarkContextCompleted(IMessageActivity message)
        {
            if (!IsContextCompleted(message)) message.From.Properties.Add(Constants.IsContextCompleted, default(Newtonsoft.Json.Linq.JToken));
        }
        public static string GetConvertedMessageText(IMessageActivity message)
        {
            //return Get(message, Constants.ConvertedMessageText);
            return ConvertMessageText(message.Text);
        }
        private static string Get(IMessageActivity message, string name)
        {
            return (string)message.From.Properties.GetValue(name);
        }
        private static void Set(IMessageActivity message, string name, string value)
        {
            message.From.Properties.Add(name, value);
        }
        public static string ConvertMessageText(string msg)
        {
            msg = msg.Replace("？", "?").Replace("什麼", "甚麼").Replace("神麼", "甚麼").Replace('喔', '哦').Replace('\t', ' ').TrimEnd(new char[] { '?', ' ' }).TrimStart(' ');
            msg = RemoveDuplicatedSpaces(msg);
            return Mapping.Mapper.Map(msg);
        }
        public static string RemoveDuplicatedSpaces(string msg)
        {
            string ans = "";
            for (int i = 0; i < msg.Length; i++) if (!(i > 0 && msg[i] == ' ' && msg[i - 1] == ' ')) ans += msg[i];
            return ans;
        }
    }
}

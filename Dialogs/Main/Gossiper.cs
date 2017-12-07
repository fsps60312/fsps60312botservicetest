using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class Gossiper
    {
        private static Gossiper Instance = new Gossiper();
        public static async Task<bool> ReactAsync(IDialogContext context, IAwaitable<IMessageActivity> argument, string message)
        {
            return await Instance.Main(context, argument, message);
        }
        Dictionary<string, string> gossips = new Dictionary<string, string>
        {
            { "code風景區", "很棒的名字，不覺得嗎？XD<br/>然後，我的英文名字是「code scenic」哦，Google看看！<br/>總之，像欣賞風景一樣快樂的探索程式之美吧！"},
            //{"Code風景區" ,"很棒的名字，不覺得嗎？XD<br/>然後，我的英文名字是「Code Scenic」哦，Google看看！<br/>總之，像欣賞風景一樣快樂的探索程式之美吧！"},
            { "傳一則貼文的網址(?)","吼～不是真的要你說這句話啦！<br/>是你要傳一則貼文的網址給我～><" },
            {"說話","話" },
            {"借我錢","我沒錢><" },
            {"不要","好吧，你壞壞 :p" },
            {"對","沒錯，就是這樣！😎" },
            {"bot","嘿，什麼事？ ^_^" },
            {"qq","好啦，乖（拍拍" },
            {"你好雷哦","你也很雷，別五十步笑百步www" },
            {"hi","恩？" },
            {"在嗎","不在～（不知道你要幹嘛怎麼決定我要不要在呢？XD）" }
        };
        Dictionary<string, string> mappings = new Dictionary<string, string>
        {
            { "chat bot","bot" },
            { "chatbot","bot" },
        };
        private string MapMessage(string message)
        {
            message = message.ToLower();
            while (mappings.ContainsKey(message)) message = mappings[message];
            return message;
        }
        private async Task<bool> Main(IDialogContext context, IAwaitable<IMessageActivity> argument, string message)
        {
            message = MapMessage(message);
            if (gossips.ContainsKey(message))
            {
                await context.PostAsync(gossips[message]);
                return true;
            }
            else return false;
        }
    }
}
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
    public static class Constants
    {
        [Serializable]
        public static class Commands
        {
            public static List<string> ListCommands()
            {
                var info = typeof(Commands).GetFields();
                return info.Select((f) => (string)f.GetValue(null)).ToList();
            }
            public const string
                C1 = "幹話排行榜",
                C2 = "SP助教怎麼樣",
                C3 = "你對我了解多少",
                C4 = "你是誰",
                Curl = "傳一則貼文的網址(?)";
        }
        public static string IsContextWaited = "IsContextWaited";
        public static string IsContextCompleted = "IsContextCompleted";
        public static string ConvertedMessageText = "ConvertedMessageText";
    }
}
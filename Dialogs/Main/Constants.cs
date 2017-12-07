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
                C1 = "\u6211\u8981\u8aaa\u751a\u9ebc?"/*我要說甚麼?*/,
                C2 = "SP\u52a9\u6559\u600e\u9ebc\u6a23?"/*SP助教怎麼樣?*/,
                C3 = "\u4f60\u5c0d\u6211\u4e86\u89e3\u591a\u5c11?"/*你對我了解多少?*/,
                C4 = "你是誰?",
                Curl= "\u6216\u8005\u50b3\u4e00\u5247\u8cbc\u6587\u7684\u7db2\u5740(?)"/*或者傳一則貼文的網址(?)*/;
        }
    }
}
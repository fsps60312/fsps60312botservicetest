using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

using System.Collections.Generic;
using AdaptiveCards;

namespace Microsoft.Bot.Sample.SimpleEchoBot.Mapping
{
    [Serializable]
    public static class Mapper
    {
        static Dictionary<string, string> mapping = new Dictionary<string, string>
        {
            {"借我錢錢","借我錢" },
            {"我要說啥?","我要說甚麼?" },
            {"我要說啥","我要說甚麼?" },
            {"我要說甚麼","我要說甚麼?" },
            {"help","我要說甚麼?" },
            {"我要怎麼說點話?","我要說甚麼?" },
            {"我應該要說甚麼?","我要說甚麼?" },
            {"SP 助教怎麼樣?","SP助教怎麼樣?" }
        };
        public static string Map(string message)
        {
            if (mapping.ContainsKey(message)) message = mapping[message];
            return message;
        }
    }
}
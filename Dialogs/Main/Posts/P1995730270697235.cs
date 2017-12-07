using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

using System.Collections.Generic;
using AdaptiveCards;

namespace Microsoft.Bot.Sample.SimpleEchoBot.Posts
{
    [Serializable]
    public class P1995730270697235
    {
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument,IMessageActivity message)
        {
            await context.PostAsync("\u60f3\u8981\u5c0d\u7b54\u6848\u662f\u5427\uff1fXD<br/>\u597d\uff0c\u4f86\uff01\u8acb\u8f38\u5165\u60a8\u7684\u7b54\u6848\uff5e"/*想要對答案是吧？XD<br/>好，來！請輸入您的答案～*/);
            //await context.PostAsync("");
            var msg = context.MakeMessage();
            //new Microsoft.Bot.Connector.Attachment(Microsoft.Bot.Connector.ActionTypes.ShowImage, "https://lh3.googleusercontent.com/cd3ESRhwidl-flcOj_rF6nqX6NShAiH8S2T5gafsR_RxymqNGxReTiwxmjtnoDYDML2h4ISp49Frmg=w1626-h1620-no");
            msg.Attachments.Add(new Attachment
            {
                ContentUrl = "https://lh3.googleusercontent.com/cd3ESRhwidl-flcOj_rF6nqX6NShAiH8S2T5gafsR_RxymqNGxReTiwxmjtnoDYDML2h4ISp49Frmg=w1626-h1620-no",
                ContentType = "image/png",
                Name = "圖示"
            });
            await context.PostAsync(msg);
            AdaptiveCard card = new AdaptiveCard();
            card.Body.Add(new TextBlock
            {
                Text = "\u60f3\u8981\u5c0d\u7b54\u6848\u662f\u5427\uff1fXD<br/>\u597d\uff0c\u4f86\uff01\u8acb\u8f38\u5165\u60a8\u7684\u7b54\u6848\uff5e"/*想要對答案是吧？XD<br/>好，來！請輸入您的答案～*/,
                Size = TextSize.Medium,
                Weight = TextWeight.Normal
            });
            //context.Activity.
            //card.Actions.Add(new SuggestedActions
            //{
                
            //})
            throw new NotImplementedException();
        }
    }
}
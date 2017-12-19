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
using IronPython;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class MathHelper : MyDialog<IMessageActivity>
    {
        async Task ResumeAfterPython(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            context.Done(message);
        }
        async Task ResumeAfterVectorNormalizer(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            if (message == null) context.Done(message);
            else await context.Forward(new Python(), ResumeAfterPython, message);
        }
        protected override async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            await context.Forward(new VectorNormalizer(), ResumeAfterVectorNormalizer, message);
        }
    }
}
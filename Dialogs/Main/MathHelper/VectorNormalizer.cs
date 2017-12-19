﻿using System;
using System.IO;
using System.Security;
using System.Security.Policy;
using System.Security.Permissions;
using System.Runtime.Remoting;
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
using System.Threading;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class VectorNormalizer : MyDialog<IMessageActivity>
    {
        protected override async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            if (message.Text.StartsWith("標準化"))
            {
                await context.PostAsync($"正在標準化...{message.Text.Substring(5)}");
                var values = new List<double>(
                    await Task.WhenAll(message.Text.Substring(5).TrimStart('<').TrimEnd('>').Split(',')
                    .Select(async s => await Python.ExecutePython(Python.ProcessPythonCode(s, true)))
                    .Select(async s=>double.Parse(await s)))
                    );
                await context.PostAsync($"<{string.Join(", ", values)}>的標準化：");
                double length = 0;
                values.ForEach(v => length += v * v);
                length = Math.Sqrt(length);
                await context.PostAsync($"倍數：{length}<br/>標準化結果：<{string.Join(", ", values.Select(v=>v/length))}>");
                message = null;
            }
            context.Done(message);
        }
    }
}
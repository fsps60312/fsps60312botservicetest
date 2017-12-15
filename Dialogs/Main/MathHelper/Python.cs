using System;
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
    namespace UntrustedCode
    {
        public class PythonExecutor
        {
            public static string Execute(string pythonCode)
            {
                var engine = IronPython.Hosting.Python.CreateEngine();
                //var scope = engine.CreateScope();
                //var scriptSource = engine.CreateScriptSourceFromString(pythonCode);
                //await context.PostAsync("計算中......");
                object result = engine.Execute(pythonCode);
                //await context.PostAsync("計算完成");
                return JsonConvert.SerializeObject(result);
            }
        }
    }
    [Serializable]
    public class Python : MyDialog<IMessageActivity>
    {
        [Serializable]
        class Sandboxer : MarshalByRefObject
        {
            const string pathToUntrusted = @"Sandbox";
            const string untrustedAssembly = "Microsoft.Bot.Sample.SimpleEchoBot.UntrustedCode";
            const string untrustedClass = "Microsoft.Bot.Sample.SimpleEchoBot.UntrustedCode.PythonExecutor";
            const string entryPoint = "Execute";
            //private Object[] parameters = { 45 };
            public static string ExecutePython(string pythonCode)
            {
                //Setting the AppDomainSetup. It is very important to set the ApplicationBase to a folder   
                //other than the one in which the sandboxer resides.  
                AppDomainSetup adSetup = new AppDomainSetup();
                var fullPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + pathToUntrusted;
                if (!System.IO.Directory.Exists(fullPath)) System.IO.Directory.CreateDirectory(fullPath);
                adSetup.ApplicationBase = fullPath;

                //Setting the permissions for the AppDomain. We give the permission to execute and to   
                //read/discover the location where the untrusted code is loaded.  
                PermissionSet permSet = new PermissionSet(PermissionState.None);
                permSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));

                //We want the sandboxer assembly's strong name, so that we can add it to the full trust list.  
                StrongName fullTrustAssembly = typeof(Sandboxer).Assembly.Evidence.GetHostEvidence<StrongName>();

                //Now we have everything we need to create the AppDomain, so let's create it.  
                AppDomain newDomain = AppDomain.CreateDomain("Sandbox", null, adSetup, permSet, fullTrustAssembly);

                //Use CreateInstanceFrom to load an instance of the Sandboxer class into the  
                //new AppDomain.   
                ObjectHandle handle = Activator.CreateInstanceFrom(
                    newDomain, typeof(Sandboxer).Assembly.ManifestModule.FullyQualifiedName,
                    typeof(Sandboxer).FullName
                    );
                //Unwrap the new domain instance into a reference in this domain and use it to execute the   
                //untrusted code.  
                Sandboxer newDomainInstance = (Sandboxer)handle.Unwrap();
                return newDomainInstance.ExecuteUntrustedPythonCode(untrustedAssembly, untrustedClass, entryPoint, pythonCode);
            }
            public string ExecuteUntrustedPythonCode(string assemblyName, string typeName, string entryPoint, string pythonCode)
            {
                //Load the MethodInfo for a method in the new Assembly. This might be a method you know, or   
                //you can use Assembly.EntryPoint to get to the main function in an executable.  
                MethodInfo target = Assembly.Load(assemblyName).GetType(typeName).GetMethod(entryPoint);
                try
                {
                    //Now invoke the method.  
                    return (string)target.Invoke(null, new Object[1] { pythonCode });
                }
                catch (Exception ex)
                {
                    // When we print informations from a SecurityException extra information can be printed if we are   
                    //calling it with a full-trust stack.  
                    (new PermissionSet(PermissionState.Unrestricted)).Assert();
                    var ret= $"SecurityException caught:\n{ex}";
                    CodeAccessPermission.RevertAssert();
                    return ret;
                }
            }
        }
        protected override async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            if (message.Text.StartsWith("幫我算"))
            {
                var pythonCode = message.Text.Substring(3);
                await context.PostAsync($"計算中......{pythonCode}");
                //await context.PostAsync(Sandboxer.ExecutePython(pythonCode));
                try
                {
                    string answer = null;
                    bool completed = false;
                    var startTime = DateTime.Now;
                    Thread thread = new Thread(() =>
                    {
                        answer = UntrustedCode.PythonExecutor.Execute(pythonCode);
                        completed = true;
                    });
                    thread.IsBackground = true;
                    thread.Start();
                    while((DateTime.Now-startTime).TotalSeconds<3)
                    {
                        await Task.Delay(100);
                        if (completed) break;
                    }
                    if(!completed)
                    {
                        thread.Abort();
                        await context.PostAsync("計算超時，已中斷");
                    }
                    //var tokenSource = new CancellationTokenSource();
                    //tokenSource.CancelAfter(1000);
                    //await Task.Run(async () => await context.PostAsync(UntrustedCode.PythonExecutor.Execute(pythonCode)), tokenSource.Token);   //Execute a long running process
                    //await Task.Run(async () => await context.PostAsync(UntrustedCode.PythonExecutor.Execute(pythonCode)), new System.Threading.CancellationTokenSource(1000).Token);
                }
                catch(Exception error)
                {
                    await context.PostAsync($"計算時發生問題：<br/>{error}");
                }
                message = null;
            }
            context.Done(message);
        }
    }
}
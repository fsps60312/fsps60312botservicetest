using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

using System.Collections.Generic;
using AdaptiveCards;
using System.Linq;

namespace Microsoft.Bot.Sample.SimpleEchoBot.Posts
{
    [Serializable]
    public class P1995730270697235
    {
        [Serializable]
        class Graph
        {
            int N, M;
            List<HashSet<int>> ET;
            bool RemoveExtraNodesForPlanarIdentification()
            {
                for (int i = 0; i < N; i++)
                {
                    if (ET[i].Contains(i)) ET[i].Remove(i);
                    if (ET[i].Count == 1)
                    {
                        int a = i, b = ET[i].ElementAt(0);
                        ET[a].Remove(b);
                        ET[b].Remove(a);
                        return true;
                    }
                    else if (ET[i].Count == 2)
                    {
                        int a = ET[i].ElementAt(0), b = i, c = ET[i].ElementAt(1);
                        ET[a].Remove(b);
                        ET[b].Remove(a);
                        ET[b].Remove(c);
                        ET[c].Remove(b);
                        ET[a].Add(c);
                        ET[c].Add(a);
                        return true;
                    }
                }
                return false;
            }
            bool IsK55(List<int> s)
            {
                foreach (int a in s) foreach (int b in s) if (a < b && !ET[a].Contains(b)) return false;
                return true;
            }
            bool IsK33(List<int> aa, List<int> bb)
            {
                foreach (int a in aa) foreach (int b in bb) if (!ET[a].Contains(b)) return false;
                return true;
            }
            int __builtin_popcount(int v)
            {
                unchecked
                {
                    v = (v & (int)0x55555555) + ((v & (int)0xaaaaaaaa) >> 1);
                    v = (v & (int)0x33333333) + ((v & (int)0xcccccccc) >> 2);
                    v = (v & (int)0x0f0f0f0f) + ((v & (int)0xf0f0f0f0) >> 4);
                    v = (v & (int)0x00ff00ff) + ((v & (int)0xff00ff00) >> 8);
                    v = (v & (int)0x0000ffff) + ((v & (int)0xffff0000) >> 16);
                }
                return v;
            }
            bool IsK33(List<int> s)
            {
                for (int _ = 0; _ < (1 << 6); _++) if (__builtin_popcount(_) == 3)
                    {
                        List<int>[] ss = new List<int>[2] { new List<int>(), new List<int>() };
                        for (int i = 0; i < 6; i++) ss[(_ >> i) & 1].Add(s[i]);
                        if (IsK33(ss[0], ss[1])) return true;
                    }
                return false;
            }
            List<int> Trans(int s)
            {
                List<int> vs = new List<int>();
                for (int i = 0; i < N; i++) if ((s & (1 << i)) > 0) vs.Add(i);
                return vs;
            }
            bool IsK55(int s) { return IsK55(Trans(s)); }
            bool IsK33(int s) { return IsK33(Trans(s)); }
            public bool IsPlanar()
            {
                while (RemoveExtraNodesForPlanarIdentification()) ;
                for (int s = 0; s < (1 << N); s++)
                {
                    //		printf("s=%d, popcount=%d\n",s,__builtin_popcount(s));
                    if (__builtin_popcount(s) == 5 && IsK55(s)) return false;
                    if (__builtin_popcount(s) == 6 && IsK33(s)) return false;
                }
                return true;
            }
            List<int> colors;
            public List<int> Colors { get { return colors; } }
            bool IsColorsValid()
            {
                for (int a = 0; a < N; a++) foreach (int b in ET[a]) if (colors[a] == colors[b]) return false;
                return true;
            }
            public bool CanThreeColored()
            {
                int s_max = 1;
                for (int i = 0; i < N; i++) s_max *= 3;
                for(int s=0;s<s_max;s++)
                {
                    colors = new List<int>();
                    int ss = s;
                    for (int i = 0; i < N; i++, ss /= 3) colors.Add(ss % 3);
                    if (IsColorsValid()) return true;
                }
                return false;
            }
            bool IsSubclique(List<int>s)
            {
                foreach (int a in s) foreach (int b in s) if (a < b && !ET[a].Contains(b)) return false;
                return true;
            }
            public List<int>GetMaxCliqueForPlanar()
            {
                for (int a = 0; a < N; a++) for (int b = 0; b < N; b++) for (int c = 0; c < N; c++) for (int d = 0; d < N; d++) if (IsSubclique(new List<int> { a, b, c, d })) return new List<int> { a, b, c, d };
                for (int a = 0; a < N; a++) for (int b = 0; b < N; b++) for (int c = 0; c < N; c++) if (IsSubclique(new List<int> { a, b, c })) return new List<int> { a, b, c };
                for (int a = 0; a < N; a++) for (int b = 0; b < N; b++) if (IsSubclique(new List<int> { a, b })) return new List<int> { a, b };
                for (int a = 0; a < N; a++) if (IsSubclique(new List<int> { a })) return new List<int> { a };
                return new List<int>();
            }
            public Graph(int _N,int _M,List<HashSet<int>>_ET)
            {
                N = _N;M = _M;ET = _ET;
            }
        }
        int N, M, EdgeRemain;
        Dictionary<string, HashSet<string>> ET = new Dictionary<string, HashSet<string>>();
        Graph BuildGraph()
        {
            Dictionary<string, int> idx = new Dictionary<string, int>();
            int i = 0;
            foreach (var p in ET) idx[p.Key] = i++;
            return new Graph(N, M, ET.Select(v1 => new HashSet<int>(v1.Value.Select(v2 => idx[v2]))).ToList());
        }
        bool IsPlanar() { return BuildGraph().IsPlanar(); }
        Dictionary<string, int> Colors = null;
        bool CanThreeColored() { return BuildGraph().CanThreeColored(); }
        List<string> GetMaxCliqueForPlanar() { return BuildGraph().GetMaxCliqueForPlanar().Select(v => ET.ElementAt(v).Key).ToList(); }
        async Task SendImage(IDialogContext context,string url)
        {
            //await context.PostAsync("");
            var msg = context.MakeMessage();
            //new Microsoft.Bot.Connector.Attachment(Microsoft.Bot.Connector.ActionTypes.ShowImage, "https://lh3.googleusercontent.com/cd3ESRhwidl-flcOj_rF6nqX6NShAiH8S2T5gafsR_RxymqNGxReTiwxmjtnoDYDML2h4ISp49Frmg=w1626-h1620-no");
            var contentType = "image/png";
            if (url.ToLower().EndsWith(".jpg")) contentType = "image/jpeg";// Not "image/jpg"
            msg.Attachments.Add(new Attachment
            {
                ContentUrl = url,
                ContentType = contentType,
                Name = "圖示"
            });
            await context.PostAsync(msg);
            //AdaptiveCard card = new AdaptiveCard();
            //card.Body.Add(new TextBlock
            //{
            //    Text = "\u60f3\u8981\u5c0d\u7b54\u6848\u662f\u5427\uff1fXD<br/>\u597d\uff0c\u4f86\uff01\u8acb\u8f38\u5165\u60a8\u7684\u7b54\u6848\uff5e"/*想要對答案是吧？XD<br/>好，來！請輸入您的答案～*/,
            //    Size = TextSize.Medium,
            //    Weight = TextWeight.Normal
            //});
            //context.Activity.
            //card.Actions.Add(new SuggestedActions
            //{

            //})
            //throw new NotImplementedException();
        }
        public async Task Stage4(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            switch (message.Text.ToLower())
            {
                case "quit": await context.PostAsync("已退出"); break;
                case "這跟code有甚麼關係？":
                    {
                        var problemUrl = "https://ada-judge.csie.org/#!/problem/12";
                        var statusImage = "https://3.bp.blogspot.com/-odKLIepgfnk/Wi9w_GEsWHI/AAAAAAAAKDc/3_kGJIhSN_8PVUTIr23nIg7mBQH4-bRSACLcBGAs/s1600/Screenshot%2B%2528502%2529.png";
                        var rankingImage = "https://3.bp.blogspot.com/-xk_nfzqenUs/Wi9w_Pr2e2I/AAAAAAAAKDY/1VrYC2EUQUk1pikdKHZombO3FW5AaaxxwCLcBGAs/s1600/Screenshot%2B%2528503%2529.png";
                        var myDisprove = "https://2.bp.blogspot.com/-pBbGdM_p87w/Wi9yHDfetSI/AAAAAAAAKDo/GpQffboxOqY6FWd3rpo7zTXemOARDfaEgCPcBGAYYCw/s1600/DSC_0047.JPG";
                        await context.PostAsync($"題目網址：{problemUrl}");
                        await SendImage(context, statusImage);
                        await context.PostAsync("其實呢這要從台大資工大二必修課作業開始講起，這堂課叫做ADA，第三次作業中有一題線上作業叫做「Metropolitan」");
                        await Task.Delay(7000);
                        await SendImage(context, rankingImage);
                        await context.PostAsync("行健同學一時想不出好的解法，只好直接假設了「若平面圖無法3著色則最大團大小等於4」這個性質是好的，然後照這個想法寫了一份code，傳上去就AC了XDD 還在速度上得到第1名！<(\\_ \\_)>");
                        await Task.Delay(10000);
                        await context.PostAsync("於是小莫就想要幫他驗證這個性質是不是好的。殊不知，就不小心找到反例了呢！>///<");
                        await Task.Delay(6000);
                        await context.PostAsync("這是小莫當初找到的反例哦：");
                        await SendImage(context, myDisprove);
                        await Task.Delay(3000);
                        await context.PostAsync("有沒有和你找到的反例一樣呢？ ;)");
                        await context.PostAsync("-----The End-----");
                        break;
                    }
                default:
                    await context.PostAsync($"請輸入「這跟code有甚麼關係？」，您輸入的是「{message.Text}」");
                    context.Wait(Stage4);
                    return;
            }
            ReleaseSemaphore();
        }
        public async Task Stage3(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            switch (message.Text.ToLower())
            {
                case "quit": await context.PostAsync("已退出"); break;
                case "重新輸入":
                    {
                        ET.Clear();
                        EdgeRemain = M;
                        await context.PostAsync($"請重新輸入您的M={M}條邊：");
                        context.Wait(Stage3);
                        return;
                    }
                default:
                    {
                        var data = message.Text.Split(' ').Where((v) => !string.IsNullOrWhiteSpace(v)).ToList();
                        if (data.Count % 2 == 1)
                        {
                            await context.PostAsync($"您輸入了奇數 ({data.Count}) 個點，但正常來講不管幾條邊都會有偶數個點耶（每條邊2個點），要不要再檢查看看您的輸入呢？><（輸入「重新輸入」來重新輸入這M={M}條邊）");
                            context.Wait(Stage3);
                            return;
                        }
                        if (data.Count / 2 > EdgeRemain)
                        {
                            await context.PostAsync($"您輸入太多邊了，之前您已經輸入了{M - EdgeRemain}條邊，因此只剩{EdgeRemain}條邊可以輸入哦！（輸入「重新輸入」來重新輸入這M={M}條邊）");
                            context.Wait(Stage3);
                            return;
                        }
                        EdgeRemain -= data.Count / 2;
                        for (int i = 0; i < data.Count; i += 2)
                        {
                            if (!ET.ContainsKey(data[i])) ET.Add(data[i], new HashSet<string>());
                            if (!ET.ContainsKey(data[i + 1])) ET.Add(data[i + 1], new HashSet<string>());
                            ET[data[i]].Add(data[i + 1]);
                            ET[data[i + 1]].Add(data[i]);
                        }
                        if(ET.Count>N)
                        {
                            await context.PostAsync($"您目前輸入的邊已經包含了{ET.Count}個點，但您一開始說總共有N={N}個點，是不是哪裡出錯了呢？請重新輸入這M={M}條邊吧～");
                            ET.Clear();
                            EdgeRemain = M;
                            await context.PostAsync($"請重新輸入您的M={M}條邊：");
                            context.Wait(Stage3);
                            return;
                        }
                        for (int i = 0; i < data.Count; i += 2) await context.PostAsync($"{data[i]} ←→ {data[i + 1]}");
                        if (EdgeRemain > 0)
                        {
                            await context.PostAsync($"您這次輸入了{data.Count / 2}條邊，請繼續輸入剩下的{EdgeRemain}條邊：");
                            context.Wait(Stage3);
                            return;
                        }
                        else
                        {
                            await context.PostAsync($"您這次輸入了{data.Count / 2}條邊，輸入完成！");
                            if (ET.Count == N) await context.PostAsync($"您的輸入包含了{ET.Count}個點，和N一樣！");
                            else await context.PostAsync($"警告！您的輸入包含了{ET.Count}個點，可是N={N}，不一樣！");
                            await context.PostAsync("驗證您的答案中，請稍後......");
                            await context.PostAsync("驗證是否為平面圖......");
                            if(!IsPlanar())
                            {
                                await context.PostAsync("答案錯誤！您的反例不是平面圖哦，請再檢查～ ^\\_^");
                                break;
                            }
                            await context.PostAsync("驗證是否無法3著色......");
                            if(CanThreeColored())
                            {
                                await context.PostAsync("答案錯誤！您的反例其實可以3著色哦～");
                                foreach (var p in Colors) await context.PostAsync($"「{p.Key}」塗上「{new string[3] { "紅色", "藍色", "綠色" }[p.Value]}」");
                                await context.PostAsync("啪搭～就是這樣～");
                                break;
                            }
                            await context.PostAsync("真的是一個無法3著色的平面圖耶！正在計算最大團大小，看看是不是真的<4......");
                            var clique = GetMaxCliqueForPlanar();
                            {
                                StringBuilder sb = new StringBuilder("其中一個最大團：");
                                foreach (var v in clique) sb.Append($" {v}");
                                await context.PostAsync(sb.ToString());
                            }
                            if(clique.Count<4)
                            {
                                await context.PostAsync("AC！答對了！恭喜您！ ^\\_^");
                                await context.PostAsync("現在來問問看這跟code有甚麼關係吧！請輸入：「這跟code有甚麼關係？」");
                                context.Wait(Stage4);
                                return;
                            }
                            else
                            {
                                await context.PostAsync($"WA！答錯了！您的反例最大團大小是{clique.Count}哦～再接再勵，加油吧！");
                                break;
                            }
                        }
                    }
            }
            ReleaseSemaphore();
        }
        public async Task Stage2(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            switch(message.Text.ToLower())
            {
                case "quit": await context.PostAsync("已退出"); break;
                default:
                    {
                        var data = message.Text.Split(' ').Where(v => { int tmp; return int.TryParse(v, out tmp); }).ToList();
                        if (data.Count != 2)
                        {
                            await context.PostAsync($"請輸入2個數字，您輸入了{data.Count}個");
                            context.Wait(Stage2);
                            return;
                        }
                        else
                        {
                            var nums = data.Select((v) => int.Parse(v)).ToList();
                            N = nums[0]; M = nums[1];
                            await context.PostAsync($"您輸入了N={N}，M={M}");
                            if (N > 10 || M > 45)
                            {
                                if (N > 10 && M <= 45) await context.PostAsync("N太大了，N的上限是10哦！");
                                else if (N <= 10 && M > 45) await context.PostAsync("M太大了，M的上限是45哦！");
                                else await context.PostAsync("N和M都太大了，N的上限是10、M的上限是45哦！");
                                context.Wait(Stage2);
                                return;
                            }
                            else
                            {

                                await context.PostAsync($"現在請輸入M={M}條邊，每條邊由兩個點表示，例如：「A B」代表有一條邊從A連到B（也是從B連到A），「A B B C」代表有兩條邊AB和BC，現在，請輸入您反例中的M條邊：（輸入「重新輸入」來重新輸入這M={M}條邊）");
                                EdgeRemain = M;
                                context.Wait(Stage3);
                                return;
                            }
                        }
                    }
            }
            ReleaseSemaphore();
        }
        public async Task Stage1(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            switch (message.Text.ToLower())
            {
                case "quit": await context.PostAsync("已退出"); break;
                case "prove":
                    {
                        await context.PostAsync("恭喜你～");
                        await Task.Delay(5000);
                        await context.PostAsync("答錯了！！");
                        await context.PostAsync("請再想想吧～XD");
                    }
                    break;
                case "disprove":
                    {
                        await context.PostAsync("咦，想必您是想到反例囉？請給我看看您想到甚麼反例吧～");
                        await context.PostAsync("請輸入兩個數字N和M，N是反例中的點數，M是反例中的邊數，例如：「2 3」代表反例中有2個點、3條邊。現在，請照順序輸入您的N和M：");
                        context.Wait(Stage2);
                        return;
                    }
                default:
                    {
                        await context.PostAsync($"請輸入「prove」或「disprove」，您輸入的是「{message.Text}」");
                        context.Wait(Stage1);
                        return;
                    }
            }
            ReleaseSemaphore();
        }
        void ReleaseSemaphore() { lock (semaphore) semaphore.Release(); }
        System.Threading.SemaphoreSlim semaphore = new System.Threading.SemaphoreSlim(0);
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument,IMessageActivity message)
        {
            await context.PostAsync("想要對答案是吧？XD<br/>好，來！請輸入您的答案～<br/>任何時候輸入「quit」可以退出");
            await context.PostAsync("請問您要prove還是disprove呢？請輸入「prove」或「disprove」");
            context.Wait(Stage1);
            await semaphore.WaitAsync();
            await context.PostAsync("歡迎再傳訊息給我哦！>w<");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandLine;
using PuppeteerSharp;

namespace Pup
{
    class Program
    {
        public class Options
        {
            [Option('u', "url", Required = true, HelpText = "要访问的网址")]
            public string Url { get; set; }

            [Option('w', "width", Required = false, HelpText = "设置PDF宽度")]
            public int Width { get; set; } = 1920;

            [Option('h', "height", Required = false, HelpText = "设置PDF高度")]
            public int Height { get; set; } = 1080;

            [Option("until", Required = false, HelpText = "设置页面完成事件DOMContentLoaded，Load(default)，Networkidle2，Networkidle0")]
            public string WaitUntil { get; set; } = "Load";

            [Option("timeout", Required = false, HelpText = "设置超时时间")] public int Timeout { get; set; } = 3000;

            [Option('o', "output", Required = true, HelpText = "输出文件名.pdf")] public string OutPutFile { get; set; }
        }

        public static Task PrintError(IEnumerable<Error> errors)
        {
            return Task.CompletedTask;
        }

        private static async Task DownloadBrowserAsync()
        {
            var browserFetcher = new BrowserFetcher();
            browserFetcher.DownloadProgressChanged += new MyHandler().DownloadProgressChangedHandler;
            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultRevision);
        }

        private static async Task<Browser> CreateBrowser(Options option)
        {
            return await Puppeteer.LaunchAsync(new LaunchOptions()
            {
                Headless = true,
                DefaultViewport = new ViewPortOptions()
                {
                    Height = option.Height,
                    Width = option.Width,
                },
                IgnoreHTTPSErrors = true,
            });
        }

        private static async Task Run(Options option)
        {
            await DownloadBrowserAsync();
            var browser = await CreateBrowser(option);
            await using var page = await browser.NewPageAsync();
            await page.GoToAsync(option.Url, new NavigationOptions()
            {
                Timeout = option.Timeout,
                WaitUntil = new[] { Enum.Parse<WaitUntilNavigation>(option.WaitUntil) }
            });

            await page.PdfAsync(option.OutPutFile, new PdfOptions()
            {
                Height = 1080,
                Width = 1920,
                PrintBackground = true,
                PreferCSSPageSize = true,
            });
        }

        static async Task Main(string[] args)
        {
            var options = Parser.Default.ParseArguments<Options>(args);
            await options.MapResult(
                Run,
                PrintError);
        }
    }
}

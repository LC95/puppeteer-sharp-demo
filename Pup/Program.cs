using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace Pup
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var browserFetcher = new BrowserFetcher();
            //await browserFetcher.DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions()
            {
                Headless = true,
                DefaultViewport = new ViewPortOptions()
                {
                    Height = 1080,
                    Width = 1920,
                },
                IgnoreHTTPSErrors = true,

            });
            await using (var page = await browser.NewPageAsync())
            {
                await page.GoToAsync("https://www.sogou.com", new NavigationOptions()
                {
                    Timeout = 30000,
                    WaitUntil = new[] { WaitUntilNavigation.Load }
                });
                //await page.GoToAsync("https://www.sogou.com", 30000, new []
                //{
                //    WaitUntilNavigation.Networkidle0//不再有网络连接时触发
                //    //WaitUntilNavigation.DOMContentLoaded//页面的DOMContentLoaded触发时
                //    //WaitUntilNavigation.Networkidle2,//只有两个网络连接时触发
                //    //WaitUntilNavigation.Load//页面Load页面触发时
                //}).ConfigureAwait(false);
                //await page.SetViewportAsync(new ViewPortOptions()
                //{
                //    Height = 1920,
                //    Width = 1920,
                //});
                await page.PdfAsync($"{Guid.NewGuid()}.pdf", new PdfOptions()
                {
                    Height = 1080,
                    Width = 1920,
                    PrintBackground = true,
                    PreferCSSPageSize = true,
                });
            }

        }
    }
}

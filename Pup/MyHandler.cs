using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Pup
{
    public class MyHandler
    {
        private int _flush = 1;

        public void DownloadProgressChangedHandler(object sender, DownloadProgressChangedEventArgs arg)
        {
            if (_flush++ % 100 == 0)
            {
                var percent = (float) arg.BytesReceived / (float) arg.TotalBytesToReceive;
                Console.WriteLine(
                    $"正在下载chromium! Total:{arg.TotalBytesToReceive}B Received:{arg.BytesReceived }B percent:{percent * 100:F}");
            }
        }
    }
}

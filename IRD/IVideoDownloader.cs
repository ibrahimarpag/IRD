using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRD
{
    public interface IVideoDownloader
    {
         Task DownloadVideoToGalleryAsync(string videoUrl, string fileName);
    }
    public class VideoDownloaderAndroid : IVideoDownloader
    {
        public async Task DownloadVideoToGalleryAsync(string videoUrl, string fileName)
        {
            videoUrl = videoUrl.Replace("\"", "");
#if ANDROID
await IRD.Platforms.Android.VideoDownloaderAndroid.DownloadVideoToGalleryAsync(videoUrl,fileName);
#endif
        }
    }
}

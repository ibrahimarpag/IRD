using Android.Content;
using Android.Provider;
using IRD.Platforms.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Dependency(typeof(VideoDownloaderAndroid))]
namespace IRD.Platforms.Android
{
    public static class VideoDownloaderAndroid
    {
        public static async Task DownloadVideoToGalleryAsync(string videoUrl, string fileName)
        {
            using (var httpClient = new HttpClient())
            {
                // Videoyu byte olarak indir
                var videoBytes = await httpClient.GetByteArrayAsync(videoUrl);

                // Android için dosyayı genel depolama alanına kaydet
                var contentValues = new ContentValues();
                contentValues.Put(MediaStore.MediaColumns.DisplayName, fileName);
                contentValues.Put(MediaStore.MediaColumns.MimeType, "video/mp4");
                contentValues.Put(MediaStore.Video.Media.InterfaceConsts.DateAdded, (int)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds);
                contentValues.Put(MediaStore.Video.Media.InterfaceConsts.DateModified, (int)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds);

                // ExternalContentUri ile dosyayı kaydetmek için URI al
                var uri = Platform.AppContext.ContentResolver.Insert(MediaStore.Video.Media.ExternalContentUri, contentValues);

                // Elde edilen URI'ya videoyu yaz
                using (var stream = Platform.AppContext.ContentResolver.OpenOutputStream(uri))
                {
                    stream.Write(videoBytes, 0, videoBytes.Length);
                }

                // Galeri uygulamalarının dosyayı taraması için medya taramasını tetikle
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                mediaScanIntent.SetData(uri);
                Platform.AppContext.SendBroadcast(mediaScanIntent);
            }
        }
    }
}

using System;

namespace IRD
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            CounterBtn.IsEnabled = false;
            loader.IsRunning = true;
            loader.IsVisible = true;
            await Task.Delay(100);
            try
            {
                var videoUrl = await GetVideoUrl();
                var fileName = "downloadedVideo.mp4";
                VideoDownloaderAndroid v = new VideoDownloaderAndroid();
                await v.DownloadVideoToGalleryAsync(videoUrl, fileName);
                await DisplayAlert("Başarılı", "Video galeriye indirildi.", "Tamam");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", ex.Message, "Tamam");
            }
            loader.IsRunning = false;
            loader.IsVisible = false;
            CounterBtn.IsEnabled = true;
        }
        public async Task<string> GetVideoUrl()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.webisyerim.com.tr/insta/get-reels-video-url?pageUrl=" + entryLink.Text);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return await response.Content.ReadAsStringAsync();
        }

        private async void CliekUrl(object sender, TappedEventArgs e)
        {
            if (sender is Grid g)
            {
                if (!string.IsNullOrEmpty(g.AutomationId))
                {
                    try
                    {
                        await Browser.OpenAsync(g.AutomationId, BrowserLaunchMode.SystemPreferred);
                    }
                    catch (Exception ex)
                    {
                        // Hata yönetimi
                        Console.WriteLine($"URL açılamadı: {ex.Message}");
                    }
                }
            }
        }
    }

}

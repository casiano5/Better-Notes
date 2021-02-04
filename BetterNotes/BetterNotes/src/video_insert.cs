using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YoutubeExtractor;

namespace BetterNotes{
    class video_insert{
        public static void download_video(string link){
            IEnumerable<VideoInfo> video_info = DownloadUrlResolver.GetDownloadUrls(link);
            VideoInfo video = video_info.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360); //This will download at 360p, thats really low but YT may not have a 720p available so that could break it if it was higher, maybe fix?
            if (video.RequiresDecryption) {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }
            var videoDownloader = new VideoDownloader(video, Path.Combine("C:\Users\Public\Downloads", video.Title + video.VideoExtension)); //Temporary download location, needs to be program install path whenever that happens, or needs to be linked with a prop file.
            videoDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage); //writing percentage complete to console, will eventually map to GUI element
            videoDownloader.Execute();
        }
        public static void get_link(){
            string video_link;
            //Search for youtube video, send link to above
            download_video(video_link);
        }
    }
}
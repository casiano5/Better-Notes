using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YoutubeExplode;

namespace BetterNotes{
    class VideoInsert {
        public static List<string> GetVideosFromSearchTerm(string searchTerm) {
            var task = GetVideosFromSearchTermAsync(searchTerm);
            List<string> videoLinks = task.Result;
            return videoLinks;
        }
        private async static Task<List<string>> GetVideosFromSearchTermAsync(string searchTerm) {
            List<string> videoLinks = new List<string>();
            var youtube = new YoutubeClient();
            var videos = await youtube.Search.GetVideosAsync(searchTerm, 1, 1);
            foreach (var video in videos) videoLinks.Add(video.Url);
            return videoLinks;
        }
    }
}
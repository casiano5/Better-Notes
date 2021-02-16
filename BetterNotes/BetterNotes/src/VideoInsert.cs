using System;
using System.Collections.Generic;
using System.Net;
using HtmlAgilityPack;

namespace BetterNotes{
    class VideoInsert {
        //TODO: Function to return embed for videos to insert into notes. (OR maybe url????????)
        public static List<string> getVideosFromSearchTerm(string searchTerm) {
            const string videoSearchUrl = "https://www.youtube.com/results?search_query=";
            searchTerm = WebUtility.UrlEncode(searchTerm);
            return getVideosFromUrl(videoSearchUrl + searchTerm);
        }
        public static List<string> getVideosFromUrl(string url) {
            //TODO: Implement
            return new List<string>();
        }
    }
}
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace BetterNotes{
    public static class ImageInsert{
        //TODO: Function to download images and save them in the correct path, return the path of the saved image (path given based on notes requesting insert)
        public static List<string> GetImagesFromSearchTerm(string searchTerm) {
            const string imageSearchUrl = "https://www.google.com/search?site=&tbm=isch&q=";
            searchTerm = WebUtility.UrlEncode(searchTerm);
            return GetImagesFromUrl(imageSearchUrl + searchTerm);
        }
        public static List<string> GetImagesFromUrl(string url) {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument imageDocument = web.Load(url);
            List<string> links = imageDocument.DocumentNode.SelectNodes("//img").Select(b => b.Attributes["src"].Value).ToList();
            List<string> filteredLinks = new List<string>();
            foreach (string link in links) if (link.Contains("https://")) filteredLinks.Add(link);
            return filteredLinks;
        }
    }
}
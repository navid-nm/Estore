using Estore.Models;
using System;
using System.Collections.Generic;

namespace Estore.Views
{
    public class ViewFunctions
    {
        public List<string> GetImageUrls(Item item)
        {
            if (item != null)
            {
                if (item.ImageUrls != null)
                {
                    List<string> urls = new List<string>();
                    foreach (string url in item.ImageUrls)
                    {
                        urls.Add("/" + url[url.IndexOf("img")..].Replace("\\", "/"));
                    }
                    return urls;
                }
            }
            return new List<string> { "/img/generic/invertedplaceholder.png" };
        }

        public string NormaliseName(string name, int amount)
        {
            if (name.Length > amount)
            {
                name = name.Substring(0, Math.Min(amount, name.Length)) + "...";
            }
            return name;
        }
    }
}

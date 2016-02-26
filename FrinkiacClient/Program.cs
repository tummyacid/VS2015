using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ImageMagick;
using System.Collections.Specialized;
using System.Xml.Linq;

namespace FrinkiacClient
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string searchQuery = System.Windows.Forms.Clipboard.GetText();
            using (MagickImageCollection collection = new MagickImageCollection())
            using (WebClient wc = new WebClient())
            {
                List<Search> target = JsonConvert.DeserializeObject<List<Search>>(wc.DownloadString("https://frinkiac.com/api/search?q=" + searchQuery.Replace(" ", "%20")));
                CaptionQuery cq = JsonConvert.DeserializeObject<CaptionQuery>(wc.DownloadString("https://frinkiac.com/api/caption?e=" + target[0].Episode + "&t=" + target[0].Timestamp));
                foreach (Nearby frame in cq.Nearby)
                {
                    collection.Add(new MagickImage(wc.DownloadData("https://frinkiac.com/img/" + frame.Episode + "/" + frame.Timestamp + ".jpg"), new MagickReadSettings()));
                    collection[collection.Count - 1].AnimationDelay = 20;
                }

                // Optionally reduce colors
                QuantizeSettings settings = new QuantizeSettings();
                settings.Colors = 256;
                collection.Quantize(settings);

                // Optionally optimize the images (images should have the same size).
                collection.Optimize();

                //Upload gif to imgur
                string clientID = "key";
                wc.Headers.Add("Authorization", "Client-ID " + clientID);
                NameValueCollection values = new NameValueCollection
                    {
                        { "image", Convert.ToBase64String(collection.ToByteArray(MagickFormat.Gif)) }
                    };

                XDocument reply = XDocument.Load(new MemoryStream(wc.UploadValues("https://api.imgur.com/3/upload.xml", values)));
                System.Console.WriteLine(reply.Root.Element("link"));
                System.Windows.Forms.Clipboard.SetText(reply.Root.Element("link").Value + " <--- " + searchQuery);

            }
            
        }
    }
}

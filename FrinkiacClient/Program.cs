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
            using (MagickImageCollection collection = new MagickImageCollection())  //Very few cartoons are broadcast live it's a terrible strain on the animators wrist.
            using (WebClient wc = new WebClient())                                  //The internet, eh?
            {
                List<Search> target = JsonConvert.DeserializeObject<List<Search>>(wc.DownloadString("https://frinkiac.com/api/search?q=" + searchQuery.Replace(" ", "%20")));
                CaptionQuery cq = JsonConvert.DeserializeObject<CaptionQuery>(wc.DownloadString("https://frinkiac.com/api/caption?e=" + target[0].Episode + "&t=" + target[0].Timestamp));

                foreach (Nearby frame in cq.Nearby)
                {   //request each frame in this CaptionQuery and add to our MagickImageCollection for the animation
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
                wc.Headers.Add("Authorization", "Client-ID " +  System.Configuration.ConfigurationManager.AppSettings["imgurClientID"].ToString());
                NameValueCollection values = new NameValueCollection
                    {
                        { "image", Convert.ToBase64String(collection.ToByteArray(MagickFormat.Gif)) }
                    };
                //Parse the xml reply
                XDocument reply = XDocument.Load(new MemoryStream(wc.UploadValues("https://api.imgur.com/3/upload.xml", values)));
                //Give up the goods
                System.Console.WriteLine(reply.Root.Element("link"));
                System.Windows.Forms.Clipboard.SetText(reply.Root.Element("link").Value + " <--- " + searchQuery);

            }
            
        }
    }
}

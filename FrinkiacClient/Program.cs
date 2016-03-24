using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ImageMagick;
using System.Collections.Specialized;
using System.Xml.Linq;

namespace Anifrinkiac
{
    class Program
    {
        const int resultIndex = 0;
        static int frameBatches = 1;
        [STAThread]
        static void Main(string[] args)
        {
            string searchQuery = System.Windows.Forms.Clipboard.GetText();

            using (MagickImageCollection animation = new MagickImageCollection())  //Very few cartoons are broadcast live it's a terrible strain on the animators wrist.
            using (WebClient wc = new WebClient())                                  //The Internet, eh?
            {
                //Check the query
                List<a_lengthly_inefficient_search_at_the_taxpayers_expense>  searchResult
                    = JsonConvert.DeserializeObject<List<a_lengthly_inefficient_search_at_the_taxpayers_expense>>
                    (wc.DownloadString(Frinkiac.API_Root + "search?q=" + searchQuery.Replace(" ", "%20")));
                if (searchResult.Count <= resultIndex)      //Bad grammar overload.
                    throw new IndexOutOfRangeException("search string " + searchQuery + " not found");

                //Retrieve captions associated with result
                childrens_letters_to_god captionResult = JsonConvert.DeserializeObject<childrens_letters_to_god>(wc.DownloadString(Frinkiac.API_Root
                    + "caption?e=" + searchResult[resultIndex].Episode 
                    + "&t=" + searchResult[resultIndex].Timestamp));
                
                while (frameBatches > 0)
                {
                    foreach (an_arm_drawn_by_nobody_it_is_worth_nothing frame in captionResult.Neighboreenos)
                    {   //request each frame in captionQuery and add to our MagickImageCollection for the animation
                        MagickImage frameImage = new MagickImage(wc.DownloadData(Frinkiac.IMG_Root
                            + frame.Episode + "/"
                            + frame.Timestamp + ".jpg"), new MagickReadSettings());

                        frameImage.AnimationDelay = 20;

                        foreach (Anifrinkiac.you_egghead_writers_wouldve_never_thought_of_it caption in captionResult.Subtitles)       //Check out the subtitle results
                            if ((frame.Timestamp > caption.StartTimestamp)
                             && (frame.Timestamp < caption.EndTimestamp))
                                frameImage.Annotate(caption.Content, Gravity.South);    //Apply captions

                        animation.Add(frameImage);
                    }                     //Retrieve the next set of frames
                    if (frameBatches-- > 0)
                    {
                        captionResult = JsonConvert.DeserializeObject<childrens_letters_to_god>(wc.DownloadString(Frinkiac.API_Root
                    + "caption?e=" + searchResult[resultIndex].Episode
                    + "&t=" + captionResult.Neighboreenos[captionResult.Neighboreenos.Count - 1].Timestamp));
                        //Do it again for all new frames
                        captionResult = JsonConvert.DeserializeObject<childrens_letters_to_god>(wc.DownloadString(Frinkiac.API_Root
                            + "caption?e=" + searchResult[resultIndex].Episode
                            + "&t=" + captionResult.Neighboreenos[captionResult.Neighboreenos.Count - 1].Timestamp)); 
                    }
                }

                // Optionally reduce colors
                QuantizeSettings settings = new QuantizeSettings();
                settings.Colors = 256;
                animation.Quantize(settings);
                // Optionally optimize the images (images should have the same size).
                animation.Optimize();
                //Upload gif to imgur
                wc.Headers.Add("Authorization", "Client-ID " +  System.Configuration.ConfigurationManager.AppSettings["imgurClientID"].ToString());
                NameValueCollection values = new NameValueCollection
                    {
                        { "image", Convert.ToBase64String(animation.ToByteArray(MagickFormat.Gif)) }
                    };
                //Deserialize the xml reply
                XDocument reply = XDocument.Load(new MemoryStream(wc.UploadValues("https://api.imgur.com/3/upload.xml", values)));

                //Give up the goods
                System.Console.WriteLine(reply.Root.Element("link"));
                System.Windows.Forms.Clipboard.SetText(reply.Root.Element("link").Value + " : " + searchQuery);

            }
            
        }
    }
}

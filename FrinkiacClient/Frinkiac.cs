using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrinkiacClient
{
    class Frinkiac
    {
    }

    public class Search
    {
        [JsonProperty("Id")]
        public String Id;
        [JsonProperty("Episode")]
        public String Episode;
        [JsonProperty("Timestamp")]
        public String Timestamp;
        [JsonProperty("Filename")]
        public String Filename;
    }

    public class Frame
    {
        public int Id { get; set; }
        public string Episode { get; set; }
        public int Timestamp { get; set; }
        public string Filename { get; set; }
    }

    public class Subtitle
    {
        public int Id { get; set; }
        public string Episode { get; set; }
        public int StartTimestamp { get; set; }
        public int EndTimestamp { get; set; }
        public string Content { get; set; }
    }

    public class Nearby
    {
        public int Id { get; set; }
        public string Episode { get; set; }
        public int Timestamp { get; set; }
        public string Filename { get; set; }
    }

    public class CaptionQuery
    {
        public Frame Frame { get; set; }
        public List<Subtitle> Subtitles { get; set; }
        public List<Nearby> Nearby { get; set; }
    }
}

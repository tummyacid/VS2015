using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Anifrinkiac
{
    class Frinkiac
    {
        public static string API_Root = "https://frinkiac.com/api/";
        public static string IMG_Root = "https://frinkiac.com/img/";
    }

    public class a_lengthly_inefficient_search_at_the_taxpayers_expense

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

    public class an_arm_drawn_by_nobody_it_is_worth_nothing
    {
        public int Id { get; set; }
        public string Episode { get; set; }
        public int Timestamp { get; set; }
        public string Filename { get; set; }
    }

    public class you_egghead_writers_wouldve_never_thought_of_it
    {
        public int Id { get; set; }
        public string Episode { get; set; }
        public int StartTimestamp { get; set; }
        public int EndTimestamp { get; set; }
        public string Content { get; set; }
    }
    
    public class childrens_letters_to_god
    {
        public an_arm_drawn_by_nobody_it_is_worth_nothing Cell { get; set; }
        public List<you_egghead_writers_wouldve_never_thought_of_it> Subtitles { get; set; }
        [JsonProperty("Nearby")]
        public List<an_arm_drawn_by_nobody_it_is_worth_nothing> Neighboreenos { get; set; }
    }
}

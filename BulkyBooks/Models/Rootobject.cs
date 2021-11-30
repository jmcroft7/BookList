using Newtonsoft.Json;

namespace BookList.Models
{
    [Serializable()]
    public class Rootobject
    {
        [JsonProperty("data")]
        public Data data { get; set; }
    }

    public class Data
    {
        [JsonProperty("_id")]
        public string _id { get; set; }

        [JsonProperty("content")]
        public string content { get; set; }

        [JsonProperty("character")]
        public Character character { get; set; }

        [JsonProperty("__v")]
        public int __v { get; set; }
    }

    public class Character
    {
        [JsonProperty("_id")]
        public string _id { get; set; }

        [JsonProperty("firstname")]
        public string firstname { get; set; }

        [JsonProperty("lastname")]
        public string lastname { get; set; }

        [JsonProperty("__v")]
        public int __v { get; set; }
    }

}
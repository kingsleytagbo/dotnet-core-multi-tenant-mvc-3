using System;
namespace KT.Core.Mvc.Models
{
    public class wp_postmeta
    {
        public Int64 meta_id { get; set; }
        public Int64 post_id { get; set; }
        public string meta_key { get; set; }
        public string meta_value { get; set; }
    }
}

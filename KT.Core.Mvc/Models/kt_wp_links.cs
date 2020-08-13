using System;
namespace KT.Core.Mvc.Models
{
    public class kt_wp_links
    {
        public UInt64 link_id { get; set; }
        public string link_url { get; set; }
        public string link_name { get; set; }
        public string link_image { get; set; }
        public string link_target { get; set; }
        public string link_description { get; set; }
        public string link_visible { get; set; }
        public UInt64 link_owner { get; set; }
        public Int32 link_rating { get; set; }
        public DateTime link_updated { get; set; }
        public string link_rel { get; set; }
        public string link_notes { get; set; }
        public string link_rss { get; set; }
    }
}

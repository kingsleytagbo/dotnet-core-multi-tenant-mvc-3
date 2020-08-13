using System;
namespace KT.Core.Mvc.Models
{
    public class kt_wp_term_relationships
    {
        public UInt64 term_id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public UInt64 term_group { get; set; }
    }
}

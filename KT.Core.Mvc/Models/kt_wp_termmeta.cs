using System;
namespace KT.Core.Mvc.Models
{
    public class kt_wp_termmeta
    {
        public UInt64 meta_id { get; set; }
        public UInt64 term_id { get; set; }
        public string meta_key { get; set; }
        public string meta_value { get; set; }
    }
}

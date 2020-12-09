using System;
namespace KT.Core.Mvc.Models
{
    public class wp_terms
    {
        public Int64 term_id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public Int64 term_group { get; set; }
    }
}

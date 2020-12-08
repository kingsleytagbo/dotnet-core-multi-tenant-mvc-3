using System;
namespace KT.Core.Mvc.Models
{
    public class wp_term_relationships
    {
        public UInt64 object_id { get; set; }
        public UInt64 term_taxonomy_id { get; set; }
        public Int32 term_order { get; set; }
    }
}

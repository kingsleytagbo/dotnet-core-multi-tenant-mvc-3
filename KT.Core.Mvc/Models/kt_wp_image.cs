using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KT.Core.Mvc.Models
{
    [Table("kt_wp_image")]
    public class kt_wp_image
    {
        [Key]
        public UInt64 ID { get; set; }
        public string url { get; set; }
        public string path { get; set; }

        public string content { get; set; }
        public UInt64 site_id { get; set; }

    }
}

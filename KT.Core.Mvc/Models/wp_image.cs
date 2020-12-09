using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KT.Core.Mvc.Models
{
    [Table("wp_image")]
    public class wp_image
    {
        [Key]
        public Int64 ID { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public byte[] content { get; set; }
        public Int64 site_id { get; set; }

    }
}

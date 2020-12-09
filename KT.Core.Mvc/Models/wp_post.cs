using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KT.Core.Mvc.Models
{
    [Table("wp_post")]
    public class wp_post
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 post_author { get; set; }
        public DateTime post_date { get; set; }
        public DateTime post_date_gmt { get; set; }
        public string post_content { get; set; }
        public string post_title { get; set; }
        public string post_excerpt { get; set; }
        public string post_status { get; set; }
        public string comment_status { get; set; }
        public string ping_status { get; set; }
        public string post_password { get; set; }
        public string post_name { get; set; }
        public string to_ping { get; set; }
        public string pinged { get; set; }
        public DateTime post_modified { get; set; }
        public DateTime post_modified_gmt { get; set; }
        public string post_content_filtered { get; set; }
        public Int64 post_parent { get; set; }
        public string guid { get; set; }
        public string post_type { get; set; }
        public string post_mime_type { get; set; }
        public string post_category { get; set; }
        public Int64 comment_count { get; set; }
        public Int64 site_id { get; set; }
    }
}

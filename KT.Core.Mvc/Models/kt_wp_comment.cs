﻿using System;
namespace KT.Core.Mvc.Models
{
    public class wp_comment
    {
        public Int64 comment_ID { get; set; }
        public Int64 comment_post_ID { get; set; }
        public string comment_author { get; set; }
        public string comment_author_email { get; set; }
        public string comment_author_url { get; set; }
        public string comment_author_IP { get; set; }
        public DateTime comment_date { get; set; }
        public DateTime comment_date_gmt { get; set; }
        public string comment_content { get; set; }
        public int comment_karma { get; set; }
        public string comment_approved { get; set; }
        public string comment_agent { get; set; }
        public string comment_type { get; set; }
        public Int64 comment_parent { get; set; }
        public Int64 user_id { get; set; }
    }
}

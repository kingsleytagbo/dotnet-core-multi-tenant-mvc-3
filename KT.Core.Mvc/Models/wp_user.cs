using System;
namespace KT.Core.Mvc.Models
{
    public class wp_user
    {
        public UInt64 ID { get; set; }
        public string user_login { get; set; }
        public string user_pass { get; set; }
        public string user_nicename { get; set; }
        public string user_email { get; set; }
        public string user_url { get; set; }
        public DateTime user_registered { get; set; }
        public string user_activation_key { get; set; }
        public int user_status { get; set; }
        public string display_name { get; set; }

        public string last_name { get; set; }
        public string first_name { get; set; }
    }
}

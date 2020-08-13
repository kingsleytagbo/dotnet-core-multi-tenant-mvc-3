using System;
namespace KT.Core.Mvc.Models
{

    public class kt_wp_usermeta
    {
        public UInt64 umeta_id { get; set; }
        public UInt64 user_id { get; set; }
        public string meta_key { get; set; }
        public string meta_value { get; set; }
    }
}

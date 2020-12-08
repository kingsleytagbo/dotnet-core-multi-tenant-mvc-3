using System;
namespace KT.Core.Mvc.Models
{
    public class wp_options
    {
        public UInt64 option_id { get; set; }
        public string option_name { get; set; }
        public string option_value { get; set; }
        public string autoload { get; set; }
    }
}

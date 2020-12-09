using System;
namespace KT.Core.Mvc.Models
{
    public class Tenant
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string PrivateKey { get; set; }
        public string Template { get; set; }
        public string Host { get; set; }
        public string ConnectionString { get; set; }
        public string ApiUrl { get; set; }
    }
}

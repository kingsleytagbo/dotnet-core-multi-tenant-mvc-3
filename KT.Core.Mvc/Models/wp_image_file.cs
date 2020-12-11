using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KT.Core.Mvc.Models
{
    public class wp_image_file: wp_image
    {
        public IFormFile FormFile { get; set; }
    }
}

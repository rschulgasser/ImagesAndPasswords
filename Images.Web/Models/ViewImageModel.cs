using Images.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Images.Web.Models
{
    public class ViewImageModel
    {
        public Image Image { get; set; }
        public string Password { get; set; }
  
        public bool CorrectPassword { get; set; }
    }
}

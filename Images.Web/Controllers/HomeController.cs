using Images.Data;
using Images.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Images.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ImageUploads; Integrated Security=true;";

        private readonly IWebHostEnvironment _webHostEnvironment;
        
        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(Image image, IFormFile imageFile)
        {
            ImagesRepository Im = new(_connectionString);

            string fileName = $"{Guid.NewGuid()}-{imageFile.FileName}";

            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            using var fs = new FileStream(filePath, FileMode.CreateNew);
            imageFile.CopyTo(fs);
            image.ImageLocation = fileName;
            image.Id = Im.AddImage(image);

            return View(image);
        }
        public IActionResult ViewImage(string password, int id)

        {
            ImagesRepository Im = new(_connectionString);
            Image image = Im.GetImageById(id);

         
                List<int> listOfIds = HttpContext.Session.Get<List<int>>("IdList");
                if (listOfIds == null)
                {
                    listOfIds = new List<int>();
                }
            if (password == image.Password)
            {
                listOfIds.Add(id);
               
            }
          

            HttpContext.Session.Set("IdList", listOfIds);

            ViewImageModel vm = new();
           
           List<int> list = HttpContext.Session.Get<List<int>>("IdList");
           
            
            vm.CorrectPassword = false;
            if (list.Contains(id))
            {
                vm.CorrectPassword = true;
                Im.AddVeiw(id);
            }
          
           
            vm.Image = image;
            vm.Password = password;
            return View(vm);
        }
      


    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}

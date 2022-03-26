using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    //[Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly NewBookAlertConfig _newBookAlertConfigs;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IOptions<NewBookAlertConfig> newBookAlertConfigs)
        {
            _logger = logger;
            _configuration = configuration;

            #region using IOptions Method 
            _newBookAlertConfigs = newBookAlertConfigs.Value;
            #endregion
        }

        #region [ViewData] is Attribute
        #endregion
        [ViewData]
        public string myPropertyData { get; set; }

        [ViewData]
        public string Title { get; set; }

        [ViewData]
        public BookModel bookModel { get; set; }

        [Route("~/")]
        #region we can use below method for route but if change name of Home Controller Or Index Action then we do change in below code it's solution is below [Route("[controller/action]")] You can also use controller level
        //[Route("home/index")]
        #endregion
        //[Route("[controller/action]")]
        public IActionResult Index()
        {
            #region Start ViewBag Concept
            ViewBag.MyName = "Shah's Store";
            ViewBag.MyName = "Mehta's Store";

            ViewBag.Data = new
            {
                Id = 1,
                Name = "Tirth",
                City = "Ahm",
            };
            #endregion

            #region passing dynmic data using ViewBag
            dynamic myData = new ExpandoObject();
            myData.Id = 1;
            myData.Name = "Shah Tirth";
            myData.Mobileno = "9429088222";
            ViewBag.myData = myData;
            #endregion

            #region passing data type of BookModel using ViewBag
            ViewBag.Type = new BookModel()
            {
                Id = 6,
                Title = "Data Sturature Book",
                Author = "Sunil Patel"
            };
            #endregion End ViewBag Concept

            #region Start ViewData
            ViewData["Name"] = "Shah Tirth";
            ViewData["Books"] = new BookModel()
            {
                Id = 1,
                Author = "Jay Panchal",
                Title = "C++ Book",
            };

            #region
            myPropertyData = "Hello I am Tirth";
            #endregion

            #region Set Home Page Title
            Title = "Home Page Title From Controller";
            #endregion

            #region ViewDataAttribute For Multiple Item in BookModel
            bookModel = new BookModel()
            {
                Id = 1,
                Title = "Java Book",
                Author = "Mayank Raval"
            };
            #endregion

            #endregion

            //var appNameResult = _configuration["AppName"];
            //var key1 = _configuration["infoObj:key1"];
            //var key2 = _configuration["infoObj:key2"];
            //var key3 = _configuration["infoObj:key3:key3obj1"];

            var result = _configuration.GetValue<bool>("DisplayBookAlert");
            var result1 = _configuration["DisplayBookAlert"];


            var DisplayBookAlert = _configuration.GetValue<bool>("NewBookAlert:DisplayBookAlert");
            var BookName = _configuration.GetValue<string>("NewBookAlert:BookName");

            //Get NewBookAlert from NewBookAlert
            //var newBookAlertDataType = _configuration.GetSection("NewBookAlert").GetValue<bool>("DisplayBookAlert");
            //var newBookAlert = _configuration.GetSection("NewBookAlert");
            //var DisplayBookAlertSection = newBookAlert.GetValue<bool>("DisplayBookAlert");
            //var BookNameSection = newBookAlert.GetValue<string>("BookName");

            //var newBookAlertModel = new NewBookAlertConfig();
            //_configuration.Bind("NewBookAlert", newBookAlertModel);
            //bool isDisplay = newBookAlertModel.DisplayBookAlert;

            bool isDisplay = _newBookAlertConfigs.DisplayBookAlert;
            return View();
        }
        public IActionResult Privacy(int id, string name)
        {
            return View();
        }
        //[Route("about-us/{id?}/test/{name?}")]
        //[Route("about-us")]
        public IActionResult AboutUs(int id, string name)
        {
            #region Set Home Page Title
            Title = "About us Page Title From Controller";
            #endregion
            return View();
        }
        //if you have same name multiple route then use Order Attribute... 
        //HttpGet("about-us",Name = "about-us",Order =1)]
        //public IActionResult AboutUs()
        //{
        //    return View();
        //}

        //use name if we change in Route("contact-us") but we do not need in application where Name is called 
        //[Route("contact-us",Name = "contact-us")]
        public IActionResult ContactUs()
        {
            return View();
        }
        [Route("our-products/{name:alpha}")]
        public ViewResult Products(string name)
        {
            return View();
        }
    }
}

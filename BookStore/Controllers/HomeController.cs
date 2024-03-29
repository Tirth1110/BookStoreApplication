﻿using BookStore.Models;
using BookStore.Repository;
using BookStore.Services;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMessageRepository _messageRepository;
        private readonly NewBookAlertConfig _newBookAlertConfigs;
        private readonly NewBookAlertConfig _thirdPartyBookAlertConfigs;
        private readonly IUserServices _userServices;
        private readonly IEmailServices _emailServices;


        public HomeController(ILogger<HomeController> logger,
            IConfiguration configuration,
            IOptions<NewBookAlertConfig> newBookAlertConfigs,
            IOptionsSnapshot<NewBookAlertConfig> newBookAlertConfigsIOptionSnapshot,
            IMessageRepository messageRepository,
            IUserServices userServices,
            IEmailServices emailServices)
        {
            _logger = logger;
            _configuration = configuration;

            #region using IOptions Method 
            _newBookAlertConfigs = newBookAlertConfigs.Value;
            #endregion

            #region using IOptionsSnapshot Method 
            _newBookAlertConfigs = newBookAlertConfigsIOptionSnapshot.Value;
            _thirdPartyBookAlertConfigs = newBookAlertConfigsIOptionSnapshot.Get("ThirdPartyBook");
            #endregion

            #region message Repository
            _messageRepository = messageRepository;
            #endregion

            #region Get User Id Using UserServices
            _userServices = userServices;
            #endregion

            #region Assign _emailServices
            _emailServices = emailServices;
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
        public async Task<ViewResult> Index()
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

            #region Using ViewData Attribute
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

            var appNameResult = _configuration["AppName"];
            var key1 = _configuration["infoObj:key1"];
            var key2 = _configuration["infoObj:key2"];
            var key3 = _configuration["infoObj:key3:key3obj1"];

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

            #region using IOptions Method 
            bool isDisplay = _newBookAlertConfigs.DisplayBookAlert;
            #endregion


            #region using IOptionsSnapshot Method 
            bool isDisplayIOptionSnapshot = _newBookAlertConfigs.DisplayBookAlert;
            bool isDisplay1IOptionSnapshot = _thirdPartyBookAlertConfigs.DisplayBookAlert;
            #endregion

            #region message Repository
            var value = _messageRepository.GetName();
            #endregion

            #region Get User Id as String & isUserLogged as Boolean(Bool)
            var userId = _userServices.GetUserId();
            var isUserLogged = _userServices.IsAuthencated();
            #endregion


            #region  Send Email Using HTML Template
            //UserEmailOptions options = new UserEmailOptions
            //{
            //    ToEmails = new List<string>() { "tirthshah111099@gmail.com" },
            //    PlaceHolders = new List<KeyValuePair<string, string>>()
            //    {
            //        new KeyValuePair<string, string>("{{UserName}}", "Shah Tirth"),
            //        new KeyValuePair<string, string>("{{instagramUrl}}", "htttps://www.instagram.com/shah__tirth")
            //    }
            //};
            //await _emailServices.SendTestEmail(options);
            #endregion

            return View();
        }
        public IActionResult Privacy(int id, string name)
        {
            return View();
        }
        //[Route("~/about-us/{id?}/test/{name?}")]
        [Route("about-us")]
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
        [Authorize(Roles = "Admin")]
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

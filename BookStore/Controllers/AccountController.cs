using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [Route("singup")]
        public async Task<ViewResult> Singup()
        {
            return View();
        }

        [Route("singup")]
        [HttpPost]
        public async Task<IActionResult> Singup(SingUpUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.CreateUserAsync(userModel);
                if (!result.Succeeded)
                {
                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }
                    return View();
                }
                ModelState.Clear();
            }
            return View();
        }
        [Route("singin")]
        public async Task<ViewResult> Singin()
        {
            return View();
        }
        [Route("singin")]
        [HttpPost]
        public async Task<IActionResult> Singin(SignInModel signInModel,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.PasswordSingInAsync(signInModel);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        //return previous page URL Like (AddNewBook)
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("index", "home");
                }
                ModelState.AddModelError("", "Invalid User Name Password");
            }
            return View(signInModel);
        }
        [Route("signout")]
        public async Task<IActionResult> SignOut()
        {
            await _accountRepository.SingOutAsync();
            return RedirectToAction("index", "home");
        }
    }
}

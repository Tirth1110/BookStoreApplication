using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Authorization;
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
                    return View(userModel);
                }
                ModelState.Clear();
                return RedirectToAction("ConfirmEmail", new { email = userModel.Email });
            }
            return View();
        }
        [Route("singin")]
        public IActionResult Singin()
        {
            return View();
        }
        [Route("singin")]
        [HttpPost]
        public async Task<IActionResult> Singin(SignInModel signInModel, string returnUrl)
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
                //ModelState.AddModelError("", "Invalid User Name Password");
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Login is Not Allowed");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid User Name Or Password");
                }
            }
            return View(signInModel);
        }
        [Route("signout")]
        public async Task<IActionResult> SignOut()
        {
            await _accountRepository.SingOutAsync();
            return RedirectToAction("index", "home");
        }
        [Route("change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [Route("change-password")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.ChangePassword(changePasswordModel);
                if (result.Succeeded)
                {
                    ViewBag.IsSuccessed = true;
                    ModelState.Clear();
                    return View();
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(changePasswordModel);
        }
        [HttpGet("confirm-email")]
        public async Task<ViewResult> ConfirmEmail(string userId, string token, string email)
        {
            EmailConfirmModel emailConfirmModel = new EmailConfirmModel
            {
                Email = email
            };

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var result = await _accountRepository.ConfirmEmailAsync(userId, token);
                if (result.Succeeded)
                {
                    ViewBag.IsSucceeded = true;
                    emailConfirmModel.EmailVerify = true;
                }
            }
            return View(emailConfirmModel);
        }
        [HttpPost("confirm-email")]
        public async Task<ViewResult> ConfirmEmail(EmailConfirmModel emailConfirmModel)
        {
            var user = await _accountRepository.GetUserByEmailIdAsync(emailConfirmModel.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    emailConfirmModel.EmailVerify = true;
                    return View(emailConfirmModel);
                }
                await _accountRepository.GenerateEmailConfirmationTokenAsync(user);
                emailConfirmModel.EmailSent = true;
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("", "Somethings Went Wrong");
            }
            return View(emailConfirmModel);
        }
        [AllowAnonymous, HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [AllowAnonymous, HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordModel forgetPasswordModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountRepository.GetUserByEmailIdAsync(forgetPasswordModel.Email);
                if (user != null)
                {
                    await _accountRepository.GenerateEmailForgetPasswordTokenAsync(user);
                }
                ModelState.Clear();
                forgetPasswordModel.EmailSent = true;
            }
            return View(forgetPasswordModel);
        }
        [AllowAnonymous, HttpGet("reset-password")]
        public IActionResult ResetPassword(string userId, string token)
        {
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel()
            {
                Token = token,
                UserId = userId
            };
            return View(resetPasswordModel);
        }
        [AllowAnonymous, HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (ModelState.IsValid)
            {
                resetPasswordModel.Token = resetPasswordModel.Token.Replace(' ', '+');
                var result = await _accountRepository.ResetPasswordAsync(resetPasswordModel);
                if (result.Succeeded)
                {
                    ModelState.Clear();
                    resetPasswordModel.IsSuuccess = true;
                    return View(resetPasswordModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(resetPasswordModel);
        }
    }
}

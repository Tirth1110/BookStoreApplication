using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserServices _userServices;
        private readonly IEmailServices _emailServices;
        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserServices userServices,
            IEmailServices emailServices,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userServices = userServices;
            _emailServices = emailServices;
            _configuration = configuration;
        }
        public async Task<IdentityResult> CreateUserAsync(SingUpUserModel userModel)
        {
            var applicationUser = new ApplicationUser()
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                DateofBirth = userModel.DateofBirth,
                Email = userModel.Email,
                UserName = userModel.Email,
            };
            var result = await _userManager.CreateAsync(applicationUser, userModel.Password);
            if (result.Succeeded)
            {
                await GenerateEmailConfirmationTokenAsync(applicationUser);
            }
            return result;
        }
        public async Task<ApplicationUser> GetUserByEmailIdAsync(string emailId)
        {
            return await _userManager.FindByEmailAsync(emailId);
        }
        public async Task GenerateEmailConfirmationTokenAsync(ApplicationUser applicationUser)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
            if (!string.IsNullOrEmpty(token))
            {
                await SendEmailForConfirmtion(applicationUser, token);
            }
        }
        public async Task GenerateEmailForgetPasswordTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendEmailForForgetPassword(user, token);
            }
        }
        public async Task<SignInResult> PasswordSingInAsync(SignInModel signInModel)
        {
            var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, signInModel.RememberMe, false);
            return result;
        }
        public async Task SingOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<IdentityResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            var userId = _userServices.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.ChangePasswordAsync(user, changePasswordModel.CurrentPassword, changePasswordModel.NewPassword);
        }
        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(userId), token);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel resetPasswordModel)
        {
            return await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(resetPasswordModel.UserId), resetPasswordModel.Token, resetPasswordModel.NewPassword);
        }

        private async Task SendEmailForConfirmtion(ApplicationUser applicationUser, string token)
        {
            var hostName = _configuration.GetSection("Application:AppDomain").Value;
            var emailConfirmation = _configuration.GetSection("Application:EmailConfirmation").Value;

            #region Send Email For Confirmation
            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { applicationUser.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", applicationUser.FirstName +" "+applicationUser.LastName),
                    new KeyValuePair<string, string>("{{TokenLink}}",string.Format(hostName + emailConfirmation ,applicationUser.Id,token))
                }
            };
            await _emailServices.SendEmailForConfirmation(options);
            #endregion
        }
        private async Task SendEmailForForgetPassword(ApplicationUser applicationUser, string token)
        {
            var hostName = _configuration.GetSection("Application:AppDomain").Value;
            var forgetPasswordLink = _configuration.GetSection("Application:ForgetPassword").Value;

            #region Send Email For ForgetPassword
            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { applicationUser.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", applicationUser.FirstName +" "+applicationUser.LastName),
                    new KeyValuePair<string, string>("{{TokenLink}}",string.Format(hostName + forgetPasswordLink ,applicationUser.Id,token))
                }
            };
            await _emailServices.SendEmailForForgetPassword(options);
            #endregion
        }
    }
}

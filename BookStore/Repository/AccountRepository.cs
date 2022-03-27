using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Identity;
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

        public AccountRepository(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IUserServices userServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userServices = userServices;
        }
        public async Task<IdentityResult> CreateUserAsync(SingUpUserModel userModel)
        {
            var user = new ApplicationUser()
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                DateofBirth = userModel.DateofBirth,
                Email = userModel.Email,
                UserName = userModel.Email,
            };
            var result = await _userManager.CreateAsync(user, userModel.Password);
            return result;
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
        public async Task <IdentityResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            var userId = _userServices.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
           return await _userManager.ChangePasswordAsync(user, changePasswordModel.CurrentPassword, changePasswordModel.NewPassword);
        }
    }
}

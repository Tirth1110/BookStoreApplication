﻿using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BookStore.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(SingUpUserModel userModel);
        Task<SignInResult> PasswordSingInAsync(SignInModel signInModel);
        Task SingOutAsync();
        Task<IdentityResult> ChangePassword(ChangePasswordModel changePasswordModel);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
        Task GenerateEmailConfirmationTokenAsync(ApplicationUser applicationUser);
        Task<ApplicationUser> GetUserByEmailIdAsync(string emailId);
        Task GenerateEmailForgetPasswordTokenAsync(ApplicationUser user); Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel resetPasswordModel);
    }
}
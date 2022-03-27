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
    }
}
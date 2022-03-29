﻿using BookStore.Models;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public interface IEmailServices
    {
        Task SendTestEmail(UserEmailOptions userEmailOptions);
    }
}
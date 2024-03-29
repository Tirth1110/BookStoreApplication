﻿using BookStore.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class EmailServices : IEmailServices
    {
        private const string templatePath = @"EmailTemplate/{0}.html";
        private readonly SMTPConfigModel _smtpConfig;
        public async Task SendTestEmail(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subjects = UpdatePlaceHolders("This Is Demo Message from Book Store Application", userEmailOptions.PlaceHolders);
            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("EmailTemplate"), userEmailOptions.PlaceHolders);
            await SendEmail(userEmailOptions);
        }
        public async Task SendEmailForConfirmation(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subjects = UpdatePlaceHolders("Hello {{UserName}}, Confirm Your Email here...", userEmailOptions.PlaceHolders);
            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("EmailConfirm"), userEmailOptions.PlaceHolders);
            await SendEmail(userEmailOptions);
        }
        public async Task SendEmailForForgetPassword(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subjects = UpdatePlaceHolders("Hello {{UserName}}, Forget Your Password here...", userEmailOptions.PlaceHolders);
            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("ForgetPassword"), userEmailOptions.PlaceHolders);
            await SendEmail(userEmailOptions);
        }
        public EmailServices(IOptions<SMTPConfigModel> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }
        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            MailMessage mail = new MailMessage()
            {
                Body = userEmailOptions.Body,
                Subject = userEmailOptions.Subjects,
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHtml,
            };
            foreach (var toEmail in userEmailOptions.ToEmails)
            {
                mail.To.Add(toEmail);
            }

            SmtpClient smtpClients = new SmtpClient();
            smtpClients.Credentials = new NetworkCredential(_smtpConfig.SenderAddress, _smtpConfig.Password);
            smtpClients.Host = "smtp.gmail.com";
            smtpClients.Port = 587;
            smtpClients.EnableSsl = true;
            smtpClients.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClients.UseDefaultCredentials = false;
            await smtpClients.SendMailAsync(mail);
        }
        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }
        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeHolder in keyValuePairs)
                {
                    if (text.Contains(placeHolder.Key))
                    {
                        text = text.Replace(placeHolder.Key, placeHolder.Value);
                    }
                }
            }
            return text;
        }
    }
}

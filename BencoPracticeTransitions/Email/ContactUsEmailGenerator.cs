using System;
using BencoPracticeTransitions.Infrastructure.Email;
using BencoPracticeTransitions.ViewModels.Home;
using Microsoft.Extensions.Options;
using RazorLight;

namespace BencoPracticeTransitions.Email
{
    public class ContactUsEmailGenerator : IGenerateEmail
    {

        private readonly IRazorLightEngine _razorLightEngine;
        private readonly BencoEmailMessageSettings _emailMessageSettings;
        private const string EmailTemplateName = "Email.Templates.ContactUsNotification.cshtml";

        public ContactUsEmailGenerator(IRazorLightEngine razorLightEngine, IOptions<BencoEmailMessageSettings> emailMessageSettings)
        {
            _razorLightEngine = razorLightEngine;
            _emailMessageSettings = emailMessageSettings.Value;
        }


        public EmailRequest GenerateEmail(object model)
        {
            if (!(model is ContactUsModel contactUsModel))
            {
                throw new ArgumentException($"{nameof(model)} must be of type {nameof(ContactUsModel)}", nameof(model));
            }

            var template = _razorLightEngine.CompileRenderAsync(EmailTemplateName, contactUsModel).Result;

            return new EmailRequest
            {
                To = _emailMessageSettings.To,
                From = contactUsModel.EmailAddress,
                Cc = _emailMessageSettings.Cc,
                Bcc = _emailMessageSettings.Bcc,
                ReplyTo = _emailMessageSettings.ReplyTo,
                Subject = "Practice Transitions : Contact Us",
                Body = template,
                IsBodyHtml = true
            };
        }



        public bool CanGenerateEmailFromModel(object model)
        {
            return model is ContactUsModel;
        }
    }
}

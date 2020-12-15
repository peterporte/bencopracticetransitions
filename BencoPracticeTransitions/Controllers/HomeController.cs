using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BencoPracticeTransitions.Email;
using BencoPracticeTransitions.Framework.Enums;
using BencoPracticeTransitions.Framework.Extensions;
using BencoPracticeTransitions.Infrastructure.Email;
using BencoPracticeTransitions.Models;
using BencoPracticeTransitions.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using reCAPTCHA.AspNetCore;

namespace BencoPracticeTransitions.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEnumerable<IGenerateEmail> _emailGenerators;
        private readonly ISendEmail _sendEmail;
        private readonly IRecaptchaService _reCaptchaService;

        public HomeController(IEnumerable<IGenerateEmail> emailGenerators, ISendEmail sendEmail, IRecaptchaService reCaptchaService)
        {
            _emailGenerators = emailGenerators;
            _sendEmail = sendEmail;
            _reCaptchaService = reCaptchaService;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View(nameof(Index));
        }

        [HttpPost]
        public IActionResult Index(PracticeOptionsModel practiceOptionsModel)
        {
            if (!ModelState.IsValid)
            {
                return View(practiceOptionsModel);
            }

            switch (practiceOptionsModel.SelectedPracticeOption)
            {
                case "sell_a_practice":
                    return RedirectToAction("Sell", "Practice");
				case "buy_a_practice":
                    return RedirectToAction("Buy", "Practice");
                case "post_job_opening":
                    return RedirectToAction("Create", "JobListing");
                case "look_for_job":
                    return RedirectToAction("Inquire", "JobListing");
                default:
                    return View(practiceOptionsModel);
            }
        }


        private async Task ValidateReCaptcha()
        {
            var reCaptcha = await _reCaptchaService.Validate(Request, false);

            if (!reCaptcha.success)
            {
                ModelState.AddModelError("reCaptchaError", "There was an error validating the reCaptcha. Please check the box and try again.");
            }
        }



        [HttpGet]
        public IActionResult ContactUs()
        {
            return View(nameof(ContactUs));
        }



        [HttpPost]
        public async Task<IActionResult> ContactUs(ContactUsModel model)
        {
            await ValidateReCaptcha();

            if (!ModelState.IsValid)
            {
                this.CreateAlert(AlertTypeEnum.Warning, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
                return View(nameof(ContactUs));
            }

            GenerateAndSendEmail(model);
            return RedirectToAction(nameof(ThankYouController.Index), "ThankYou");
        }


        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(nameof(Error), new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private void GenerateAndSendEmail(object model)
        {
            foreach (var emailGenerator in _emailGenerators.Where(e => e.CanGenerateEmailFromModel(model)))
            {
                _sendEmail.Send(emailGenerator.GenerateEmail(model));
            }
        }




    }
}

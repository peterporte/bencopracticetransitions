using System;
using BencoPracticeTransitions.Email;
using BencoPracticeTransitions.Infrastructure.Email;
using BencoPracticeTransitions.ViewModels.Practice;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BencoPracticeTransitions.Framework.Enums;
using BencoPracticeTransitions.Framework.Extensions;
using BencoPracticeTransitions.Framework.Helper;
using BencoPracticeTransitions.ViewModels.Home;
using Microsoft.AspNetCore.Mvc.Rendering;
using reCAPTCHA.AspNetCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BencoPracticeTransitions.Controllers
{
    public class PracticeController : Controller
    {
        private readonly IEnumerable<IGenerateEmail> _emailGenerators;
        private readonly ISendEmail _sendEmail;
        private readonly IRecaptchaService _reCaptchaService;

        public PracticeController(IEnumerable<IGenerateEmail> emailGenerators, ISendEmail sendEmail,
            IRecaptchaService reCaptchaService)
        {
            _emailGenerators = emailGenerators;
            _sendEmail = sendEmail;
            _reCaptchaService = reCaptchaService;
        }

        public IActionResult Index()
        {
            return View(nameof(Index));
        }

        public IActionResult Sell()
        {
            ViewBag.CollectionsAmount = CollectionsAmountOptions();
            return View(nameof(Sell));
        }

        [HttpPost]
        public IActionResult Index(PracticeOptionsModel practiceOptionsModel)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Index), practiceOptionsModel);
            }

            switch (practiceOptionsModel.SelectedPracticeOption)
            {
                case "sell_a_practice":
                    return RedirectToAction(nameof(Sell), "Practice");

                case "buy_a_practice":
                    return RedirectToAction(nameof(Buy), "Practice");                

                default: return View(nameof(Index), practiceOptionsModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Sell(SellPracticeModel model)
        {
            await ValidateReCaptcha();

            if (!ModelState.IsValid)
            {
                this.CreateAlert(AlertTypeEnum.Warning, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
                ViewBag.CollectionsAmount = CollectionsAmountOptions();
                return View(nameof(Sell), model);
            }

            GenerateAndSendEmail(model);

            return RedirectToAction(nameof(ThankYouController.Index), "ThankYou");
        }

        public IActionResult Buy()
        {
            ViewBag.CollectionsAmount = CollectionsAmountOptions();
            return View(nameof(Buy));
        }

        [HttpPost]
        public async Task<IActionResult> Buy(BuyPracticeModel model)
        {
            await ValidateReCaptcha();

            Validate(model);
            if (!ModelState.IsValid)
            {
                this.CreateAlert(AlertTypeEnum.Warning, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
                ViewBag.CollectionsAmount = CollectionsAmountOptions();
                return View(nameof(Buy), model);
            }

            GenerateAndSendEmail(model);

            return RedirectToAction(nameof(ThankYouController.Index), "ThankYou");
        }

        [NonAction]
        private void Validate(BuyPracticeModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.MinPurchaseAmount >= model.MaxPurchaseAmount)
            {
                ModelState.AddModelError(nameof(model.MinPurchaseAmount),
                    $"{DisplayNameHelper.GetDisplayName(model.GetType(), nameof(model.MinPurchaseAmount))} must be less than {DisplayNameHelper.GetDisplayName(model.GetType(), nameof(model.MaxPurchaseAmount))}");
            }
        }

        private void GenerateAndSendEmail(object model)
        {
            foreach (var emailGenerator in _emailGenerators.Where(e => e.CanGenerateEmailFromModel(model)))
            {
                _sendEmail.Send(emailGenerator.GenerateEmail(model));
            }
        }

        private static SelectList CollectionsAmountOptions()
        {
            var collectionsAmountValues = new[]
                {"$0 - $500,000", "$500,000 - $1,000,000", "$1,500,000 - $2,000,000", "$2,000,000 +"};
            return new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem {Text = "", Value = null},
                    new SelectListItem {Text = collectionsAmountValues[0], Value = collectionsAmountValues[0]},
                    new SelectListItem {Text = collectionsAmountValues[1], Value = collectionsAmountValues[1]},
                    new SelectListItem {Text = collectionsAmountValues[2], Value = collectionsAmountValues[2]},
                    new SelectListItem {Text = collectionsAmountValues[3], Value = collectionsAmountValues[3]}
                }, "Value", "Text", null);
        }

        private async Task ValidateReCaptcha()
        {
            var reCaptcha = await _reCaptchaService.Validate(Request, false);

            if (!reCaptcha.success)
            {
                ModelState.AddModelError("reCaptchaError",
                    "There was an error validating the reCaptcha. Please check the box and try again.");
            }
        }
    }
}
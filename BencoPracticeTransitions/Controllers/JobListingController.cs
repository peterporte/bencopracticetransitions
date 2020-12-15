using BencoPracticeTransitions.Email;
using BencoPracticeTransitions.Framework.Extensions;
using BencoPracticeTransitions.Infrastructure.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BencoPracticeTransitions.Framework.Enums;
using BencoPracticeTransitions.Framework.Helper;
using BencoPracticeTransitions.ViewModels.Home;
using BencoPracticeTransitions.ViewModels.JobListing;
using reCAPTCHA.AspNetCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BencoPracticeTransitions.Controllers
{
    public class JobListingController : Controller
    {
        private readonly IEnumerable<IGenerateEmail> _emailGenerators;
        private readonly ISendEmail _sendEmail;
        private readonly IValidateResume _resumeValidator;
        private readonly IRecaptchaService _recaptchaService;

        public JobListingController(IEnumerable<IGenerateEmail> emailGenerators, ISendEmail sendEmail, IValidateResume resumeValidator, IRecaptchaService recaptchaService)
        {
            _emailGenerators = emailGenerators;
            _sendEmail = sendEmail;
            _resumeValidator = resumeValidator;
            _recaptchaService = recaptchaService;
        }

        public IActionResult Index()
        {
            var model = new PracticeOptionsModel();

            return View(nameof(Index), model);
        }

        public IActionResult Create()
        {
            // Initialize Job House with the days of the week. Note dow % 7 is used to move Sunday to the end of the list
            var model = new CreateModel
            {
                JobHours = new List<CreateJobHourModel>(
                    Enumerable.Range(1, 7) // 1,7 start the week on Monday. To start the week on Sunday use 0,6
                        .Select(dow => new CreateJobHourModel
                        {
                            Day = Enum.GetName(typeof(DayOfWeek), dow % 7).ToString(),
                            Hours = null,
                            Checked = false
                        })
                )
            };

            return View(nameof(Create), model);
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
                case "post_job_opening":
                    return RedirectToAction(nameof(Create), "JobListing");

                case "look_for_job":
                    return RedirectToAction(nameof(Inquire), "JobListing");

                default: return View(nameof(Index), practiceOptionsModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            await ValidateReCaptcha();

            Validate(model);

            if (!ModelState.IsValid)
            {
                this.CreateAlert(AlertTypeEnum.Warning, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
                return View(nameof(Create), model);
            }

            GenerateAndSendEmail(model);
            return RedirectToAction(nameof(ThankYouController.Index), "ThankYou");
        }

        public IActionResult Inquire()
        {
            var model = new InquireModel
            {
                Availability = new List<InquireAvailabilityModel>(
                    Enumerable.Range(1, 7)
                        .Select(dow => new InquireAvailabilityModel
                        {
                            Day = Enum.GetName(typeof(DayOfWeek), dow % 7).ToString(),
                            Hours = null,
                            Checked = false
                        })
                )
            };
            return View(nameof(Inquire), model);
        }

        [HttpPost]
        public async Task<IActionResult> Inquire(InquireModel model, IFormFile resumeUploader)
        {
            await ValidateReCaptcha();

            Validate(model, resumeUploader);

            if (!ModelState.IsValid)
            {
                this.CreateAlert(AlertTypeEnum.Warning, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
                return View(nameof(Inquire), model);
            }

            GenerateAndSendEmail(model);
            return RedirectToAction(nameof(ThankYouController.Index), "ThankYou");
        }


        private void Validate(InquireModel model, IFormFile uploadedResume)
        {
            if (_resumeValidator.UploadedResumeIsValid(uploadedResume, out var errorMessage))
            {
                model.AttachResume(uploadedResume);
            }
            else
            {
                ModelState.AddModelError("Resume", errorMessage);
            }

            if (!model.Availability.Any(l => l.Checked))
            {
                ModelState.AddModelError(nameof(model.Availability), "At least one value should be selected.");
            }
            else if (model.Availability.Any(l => l.Checked && string.IsNullOrWhiteSpace(l.Hours)))
            {
                ModelState.AddModelError(nameof(model.Availability), $"{DisplayNameHelper.GetDisplayName(model.GetType(), nameof(model.Availability))} is required.");
            }

            if (model.JobType == JobTypeEnum.Doctor.Description() && string.IsNullOrWhiteSpace(model.Specialty))
            {
                ModelState.AddModelError(nameof(model.Specialty), $"{DisplayNameHelper.GetDisplayName(model.GetType(), nameof(model.Specialty))} is required.");
            }

            if (model.WorkExperience == WorkExperienceEnum.Other.ToString() && string.IsNullOrWhiteSpace(model.WorkExperienceOther))
            {
                ModelState.AddModelError(nameof(model.WorkExperienceOther), $"{DisplayNameHelper.GetDisplayName(model.GetType(), nameof(model.WorkExperienceOther))} is required.");
            }
        }

        private void Validate(CreateModel model)
        {
            if (!model.JobHours.Any(j => j.Checked))
            {
                ModelState.AddModelError(nameof(model.JobHours), "At least one value should be selected.");
            }
            else if (model.JobHours.Any(m => m.Checked && string.IsNullOrWhiteSpace(m.Hours)))
            {
                ModelState.AddModelError(nameof(model.JobHours), $"{DisplayNameHelper.GetDisplayName(model.GetType(), nameof(model.JobHours))} is required.");
            }

            if (model.JobType == JobTypeEnum.Doctor.Description() && string.IsNullOrWhiteSpace(model.Specialty))
            {
                ModelState.AddModelError(nameof(CreateModel.Specialty), "Specialty is required for doctor.");
            }
        }
        
        private async Task ValidateReCaptcha()
        {
            var reCaptcha = await _recaptchaService.Validate(Request, false);

            if (!reCaptcha.success)
            {
                ModelState.AddModelError("reCaptchaError", "There was an error validating the reCaptcha. Please check the box and try again.");
            }
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
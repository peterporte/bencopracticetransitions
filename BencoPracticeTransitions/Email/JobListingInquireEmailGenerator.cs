using BencoPracticeTransitions.Framework.Helper;
using BencoPracticeTransitions.Framework.Utility;
using BencoPracticeTransitions.Infrastructure.Email;
using BencoPracticeTransitions.ViewModels.JobListing;
using Microsoft.Extensions.Options;
using RazorLight;
using System;
using System.Collections.Generic;

namespace BencoPracticeTransitions.Email
{
    public class JobListingInquireEmailGenerator : IGenerateEmail
    {
        private readonly IRazorLightEngine _razorLightEngine;
        private readonly IGenerateEmailAttachment _emailAttachmentGenerator;
        private readonly BencoEmailMessageSettings _emailMessageSettings;
        private const string EmailTemplateName = "Email.Templates.JobListingInquireNotification.cshtml";

        public JobListingInquireEmailGenerator(IRazorLightEngine razorLightEngine, IGenerateEmailAttachment emailAttachmentGenerator, IOptions<BencoEmailMessageSettings> emailMessageSettings)
        {
            _razorLightEngine = razorLightEngine;
            _emailAttachmentGenerator = emailAttachmentGenerator;
            _emailMessageSettings = emailMessageSettings.Value;
        }

        public EmailRequest GenerateEmail(object model)
        {
            if (!(model is InquireModel inquireModel))
            {
                throw new ArgumentException(nameof(model));
            }
            
            var template =
                _razorLightEngine.CompileRenderAsync(EmailTemplateName, model).Result;

            return new EmailRequest
            {
                To = _emailMessageSettings.To,
                From = _emailMessageSettings.From,
                Cc = _emailMessageSettings.Cc,
                Bcc = _emailMessageSettings.Bcc,
                ReplyTo = _emailMessageSettings.ReplyTo,
                Subject = "Practice Transitions : Job Seeker",
                Body = template,
                IsBodyHtml = true,
                Attachments = GenerateEmailAttachments(inquireModel)
            };
        }
  
        public bool CanGenerateEmailFromModel(object jobModel)
        {
            return jobModel is InquireModel;
        }

        private IEnumerable<EmailAttachment> GenerateEmailAttachments(InquireModel model)
        {
            var attachments =
                new List<EmailAttachment>
                {
                    GenerateCsvAttachmentForFields(model, "Practice Transitions.csv")
                };

            if (model.Resume != null)
            {
                attachments.Add(model.Resume);
            }

            return attachments;
        }

        private EmailAttachment GenerateCsvAttachmentForFields(InquireModel model, string filename)
        {
            var type = model.GetType();
            var columns = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Type", "Job Hunter"),
                new KeyValuePair<string, string>("Benco Customer Number", string.Empty),
                new KeyValuePair<string, string>("Owner or Associate or Student Resident Associate", string.Empty),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.FirstName)), model.FirstName),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.LastName)), model.LastName),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.ContactNumber)), model.ContactNumber),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.ContactEmail)), model.ContactEmail),
                new KeyValuePair<string, string>("TR Name", string.Empty),
                new KeyValuePair<string, string>("Emailed Date", string.Empty),
                new KeyValuePair<string, string>("Partner Referred", string.Empty),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.HowDidYouHearAboutUs)), EnumHelper.HowDidYouHearAboutUsEnumNameToDescription(model.HowDidYouHearAboutUs)),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.HowDidYouHearAboutUsDetail)), model.HowDidYouHearAboutUsDetail),
                new KeyValuePair<string, string>("Type/Practice(Ops, Collections)", string.Empty),
                new KeyValuePair<string, string>("Date Partner was contacted (cc yourself on email and keep on file)", string.Empty),
                new KeyValuePair<string, string>("Initial Contact Date", DateTime.Now.ToShortDateString()),
                new KeyValuePair<string, string>("Recent Contact Date", string.Empty),
                new KeyValuePair<string, string>("Next Contact Date", string.Empty),
                new KeyValuePair<string, string>("Comment/Next Steps", string.Empty),
                new KeyValuePair<string, string>("Interested State(s)", string.Empty),
                new KeyValuePair<string, string>("Eval Started", string.Empty),
                new KeyValuePair<string, string>("Eval Completed", string.Empty),
                new KeyValuePair<string, string>("Listing Signed", string.Empty),
                new KeyValuePair<string, string>("Listing $", string.Empty),
                new KeyValuePair<string, string>("Closing Date", string.Empty),
                new KeyValuePair<string, string>("Merch Rev Saved or Increased", string.Empty),
                new KeyValuePair<string, string>("Partner Check Received", string.Empty),
                new KeyValuePair<string, string>("Non-Partner Trans", string.Empty),
                new KeyValuePair<string, string>("Bank / Approved/ $ Amount", string.Empty),
                new KeyValuePair<string, string>("Type Of Practice", string.Empty),
                new KeyValuePair<string, string>("Years Practicing", string.Empty),
                new KeyValuePair<string, string>("Years to Ownership", string.Empty),
                new KeyValuePair<string, string>("Grad Date/Age", string.Empty),
                new KeyValuePair<string, string>("Placement Date", string.Empty),
                new KeyValuePair<string, string>("Placed in Benco Office Y/N", string.Empty),
                new KeyValuePair<string, string>("Benco Merch Rev Increase", string.Empty),
                new KeyValuePair<string, string>("Did Placement in Non-Benco Office Result in Merch Rev Y/N", string.Empty),
                new KeyValuePair<string, string>("# of Evaluations", string.Empty),
                new KeyValuePair<string, string>("# of Listings", string.Empty),
                new KeyValuePair<string, string>("# of Closings", string.Empty),
                new KeyValuePair<string, string>("Partner Revenue to Benco", string.Empty),
                new KeyValuePair<string, string>("# of Contacts", string.Empty),
                new KeyValuePair<string, string>("# of Non-Partner Trans", string.Empty),
                new KeyValuePair<string, string>("Practice Name", string.Empty),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.JobLocation)), model.JobLocation),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.Address)), model.Address),
                new KeyValuePair<string, string>("Address 2", string.Empty),
                new KeyValuePair<string, string>("City", string.Empty),
                new KeyValuePair<string, string>("State", string.Empty),
                new KeyValuePair<string, string>("Zip Code", string.Empty),
                new KeyValuePair<string, string>("Asking Price", string.Empty),
                new KeyValuePair<string, string>("Appraisal Needed?", string.Empty),
                new KeyValuePair<string, string>("Min Purchase Amount", string.Empty),
                new KeyValuePair<string, string>("Max Purchase Amount", string.Empty),
                new KeyValuePair<string, string>("Min Collections Amount", string.Empty),
                new KeyValuePair<string, string>("Max Collections Amount", string.Empty),
                new KeyValuePair<string, string>("Working Operatories", string.Empty),
                new KeyValuePair<string, string>("Expandable Operatories", string.Empty),
                new KeyValuePair<string, string>("Doctor Stay On?", string.Empty),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.JobType)), model.JobType),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.Specialty)), model.Specialty),
                new KeyValuePair<string, string>("Job Requirements", string.Empty),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.WorkExperience)), model.WorkExperienceOther ?? model.WorkExperience),
            };
            columns.AddRange(CsvColumnUtility.CreateEmptyDayOfWeekColumns("Schedule"));
            foreach (var availability in model.Availability)
            {
                columns.Add(new KeyValuePair<string, string>($"{availability.Day} Available", $" {availability.Hours ?? string.Empty}"));
            }
            columns.AddRange(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.LinkedInAccount)), model.LinkedInAccount),
                new KeyValuePair<string, string>("Real Estate Option", string.Empty),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.AdditionalNotes)),model.AdditionalNotes)
            });

            return _emailAttachmentGenerator.GenerateCsvEmailAttachment(columns, filename);
        }
    }
}

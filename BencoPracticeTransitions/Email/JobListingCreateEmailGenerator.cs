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
    public class JobListingCreateEmailGenerator : IGenerateEmail
    {
        private readonly IRazorLightEngine _razorLightEngine;
        private readonly IGenerateEmailAttachment _emailEmailAttachmentGenerator;
        private readonly BencoEmailMessageSettings _emailMessageSettings;
        private const string EmailTemplateName = "Email.Templates.JobListingCreateNotification.cshtml";

        public JobListingCreateEmailGenerator(IRazorLightEngine razorLightEngine, IGenerateEmailAttachment emailEmailAttachmentGenerator, IOptions<BencoEmailMessageSettings> emailMessageSettings)
        {
            _razorLightEngine = razorLightEngine;
            _emailEmailAttachmentGenerator = emailEmailAttachmentGenerator;
            _emailMessageSettings = emailMessageSettings.Value;
        }

        public EmailRequest GenerateEmail(object model)
        {
            if (!(model is CreateModel createModel))
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
                Subject = "Practice Transitions : Job Post",
                Body = template,
                IsBodyHtml = true,
                Attachments = GenerateEmailAttachments(createModel)
            };
        }

        public bool CanGenerateEmailFromModel(object jobModel)
        {
            return jobModel is CreateModel;
        }

        private IEnumerable<EmailAttachment> GenerateEmailAttachments(CreateModel model)
        {
            var attachments =
                new List<EmailAttachment> {GenerateCsvAttachmentForFields(model, "Practice Transitions.csv")};

            //Any further attachments need to be assembled here

            return attachments;
        }

        private EmailAttachment GenerateCsvAttachmentForFields(CreateModel model, string filename)
        {
            var type = model.GetType();
            var columns = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Type", "Job Poster"),
                new KeyValuePair<string, string>("Benco Customer Number", string.Empty),
                new KeyValuePair<string, string>("Owner or Associate or Student Resident Associate", string.Empty),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.ContactFirstName)), model.ContactFirstName),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.ContactLastName)), model.ContactLastName),
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
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.PracticeName)), model.PracticeName),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.PracticeLocation)), model.PracticeLocation),
                new KeyValuePair<string, string>("Address 1", string.Empty),
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
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.JobRequirements)), model.JobRequirements),
                new KeyValuePair<string, string>("Work Experience", string.Empty)
            };
            foreach (var jobHour in model.JobHours)
            {
                columns.Add(new KeyValuePair<string, string>($"{jobHour.Day} Schedule", $" {jobHour.Hours ?? string.Empty}"));
            }
            columns.AddRange(CsvColumnUtility.CreateEmptyDayOfWeekColumns("Available"));
            columns.AddRange(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.LinkedInAccount)), model.LinkedInAccount),
                new KeyValuePair<string, string>("Real Estate Option", string.Empty),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.AdditionalNotes)), model.AdditionalNotes)
            });

            return _emailEmailAttachmentGenerator.GenerateCsvEmailAttachment(columns, filename);
        }
    }
}

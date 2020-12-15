using BencoPracticeTransitions.Infrastructure.Email;
using RazorLight;
using System;
using System.Collections.Generic;
using BencoPracticeTransitions.Framework.Enums;
using BencoPracticeTransitions.Framework.Extensions;
using BencoPracticeTransitions.Framework.Helper;
using BencoPracticeTransitions.Framework.Utility;
using Microsoft.Extensions.Options;
using SellPracticeModel = BencoPracticeTransitions.ViewModels.Practice.SellPracticeModel;

namespace BencoPracticeTransitions.Email
{
    public class PracticeSellEmailGenerator : IGenerateEmail
    {
        private readonly IRazorLightEngine _razorLightEngine;
        private readonly IGenerateEmailAttachment _emailAttachmentGenerator;
        private const string EmailTemplateName = "Email.Templates.PracticeSellNotification.cshtml";
        private readonly BencoEmailMessageSettings _emailMessageSettings;

        public PracticeSellEmailGenerator(IRazorLightEngine razorLightEngine, IGenerateEmailAttachment emailAttachmentGenerator, IOptions<BencoEmailMessageSettings> emailMessageSettings)
        {
            _razorLightEngine = razorLightEngine;
            _emailAttachmentGenerator = emailAttachmentGenerator;
            _emailMessageSettings = emailMessageSettings.Value;
        }

        public EmailRequest GenerateEmail(object model)
        {
            if (!(model is SellPracticeModel sellPracticeModel))
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
                Subject = "Practice Transitions : Practice Seller",
                Body = template,
                IsBodyHtml = true,
                Attachments = GenerateEmailAttachments(sellPracticeModel)
            };
        }

        public bool CanGenerateEmailFromModel(object sellPracticeModel)
        {
            return sellPracticeModel is SellPracticeModel;
        }

        private IEnumerable<EmailAttachment> GenerateEmailAttachments(SellPracticeModel model)
        {
            var attachments =
                new List<EmailAttachment>
                {
                    GenerateCsvAttachmentForFields(model, "Practice Transitions.csv")
                };

            return attachments;
        }

        private EmailAttachment GenerateCsvAttachmentForFields(SellPracticeModel model, string filename)
        {
            string minCollectionsAmount;
            string maxCollectionsAmount;
            switch (model.CollectionsAmount)
            {
                case "$0 - $500,000":
                    minCollectionsAmount = "$0";
                    maxCollectionsAmount = "$500,000";
                    break;
                case "$500,000 - $1,000,000":
                    minCollectionsAmount = "$500,000";
                    maxCollectionsAmount = "$1,000,000";
                    break;
                case "$1,500,000 - $2,000,000":
                    minCollectionsAmount = "$1,500,000";
                    maxCollectionsAmount = "$2,000,000";
                    break;
                case "$2,000,000 +":
                    minCollectionsAmount = "$2,000,000";
                    maxCollectionsAmount = "$2,000,000 +";
                    break;
                default:
                    minCollectionsAmount = string.Empty;
                    maxCollectionsAmount = string.Empty;
                    break;
            }
            var type = model.GetType();
            var columns = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Type", "Seller"),
                new KeyValuePair<string, string>("Benco Customer Number", string.Empty),
                new KeyValuePair<string, string>("Owner or Associate or Student Resident Associate", string.Empty),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.ContactFirstName)), model.ContactFirstName),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.ContactLastName)), model.ContactLastName),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.ContactPhone)), model.ContactPhone),
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
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.PracticeType)), model.PracticeType),
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
                new KeyValuePair<string, string>("Location", string.Empty),
                new KeyValuePair<string, string>("Address 1", string.Empty),
                new KeyValuePair<string, string>("Address 2", string.Empty),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.City)), model.City),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.State)), model.State),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.ZipCode)), model.ZipCode),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.AskingPrice)), $"{model.AskingPrice:c}"),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.IsAppraisalNeeded)), model.IsAppraisalNeeded != null && (bool)model.IsAppraisalNeeded ? "Yes" : "No"),
                new KeyValuePair<string, string>("Min Purchase Amount", string.Empty),
                new KeyValuePair<string, string>("Max Purchase Amount", string.Empty),
                new KeyValuePair<string, string>("Min Collections Amount", minCollectionsAmount),
                new KeyValuePair<string, string>("Max Collections Amount", maxCollectionsAmount),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.WorkingOperatoryCount)), model.WorkingOperatoryCount.ToString()),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.ExpandableOperatoryCount)), model.ExpandableOperatoryCount.ToString()),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.IsDoctorWillingToStayOnAfterTransition)), model.IsDoctorWillingToStayOnAfterTransition != null && (bool) model.IsDoctorWillingToStayOnAfterTransition ? "Yes" : "No"),
                new KeyValuePair<string, string>("Job Type", string.Empty),
                new KeyValuePair<string, string>("Specialty", string.Empty),
                new KeyValuePair<string, string>("Job Requirements", string.Empty),
                new KeyValuePair<string, string>("Work Experience", string.Empty)
            };
            columns.AddRange(CsvColumnUtility.CreateEmptyDayOfWeekColumns("Schedule"));
            columns.AddRange(CsvColumnUtility.CreateEmptyDayOfWeekColumns("Available"));
            columns.AddRange(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Linked In Account", string.Empty),
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.RealEstateOption)), model.RealEstateOption == RealEstateOptionForSellEnum.RentLease.ToString() ? RealEstateOptionForSellEnum.RentLease.Description() : model.RealEstateOption), // TODO What is this testing for. Shouldn't it just be printing the value?
                new KeyValuePair<string, string>(DisplayNameHelper.GetColumnName(type, nameof(model.AdditionalNotes)), model.AdditionalNotes)
            });

            return _emailAttachmentGenerator.GenerateCsvEmailAttachment(columns, filename);
        }
    }
}

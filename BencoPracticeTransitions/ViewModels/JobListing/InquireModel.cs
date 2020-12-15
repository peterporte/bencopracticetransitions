using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BencoPracticeTransitions.Framework.CustomAttributes;
using BencoPracticeTransitions.Framework.Helper;
using BencoPracticeTransitions.Infrastructure.Email;

namespace BencoPracticeTransitions.ViewModels.JobListing
{
    public class InquireModel
    {
        private List<InquireAvailabilityModel> _jobHours;

        [DisplayName("First Name")]
        [ColumnName("First")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [ColumnName("Last")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [DisplayName("Address")]
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [DisplayName("Contact Number")]
        [ColumnName("Preferred Phone")]
        [RegularExpression("^\\s*([+]?1[-]?\\s*)?\\(?(\\d{3})\\)?[-\\s]?(\\d{3})[-\\s]?(\\d{4})\\s*$", ErrorMessage = "Invalid Contact Number.")]
        [Required(ErrorMessage = "Contact Number is required.")]
        public string ContactNumber { get; set; }

        [DisplayName("Contact Email")]
        [ColumnName("Email")]
        [Required(ErrorMessage = "Contact Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Contact Email.")]
        public string ContactEmail { get; set; }

        [DisplayName("I am a")]
        [ColumnName("Work Experience")]
        [Required(ErrorMessage = "\"I am a\" is required.")]
        public string WorkExperience { get; set; }

        [DisplayName("I am a (other)")]
        public string WorkExperienceOther { get; set; }

        [DisplayName("What type of job are you looking for?")]
        [ColumnName("Job Type")]
        [Required(ErrorMessage = "Job Type is required.")]
        public string JobType { get; set; }

        [DisplayName("Job Location")]
        [Required(ErrorMessage = "Job Location is required.")]
        [ColumnName("Location")]
        public string JobLocation { get; set; }

        [DisplayName("Availability")]
        [Required(ErrorMessage = "At least one value should be selected.")]
        [ColumnName("Availability")]
        public List<InquireAvailabilityModel> Availability
        {
            get => _jobHours ?? (_jobHours = new List<InquireAvailabilityModel>());
            set => _jobHours = value;
        }

        [DisplayName("Doctor's Specialty")]
        [ColumnName("Specialty")]
        public string Specialty { get; set; }

        public EmailAttachment Resume { get; set; }

        [DisplayName("LinkedIn Profile URL")]
        [ColumnName("LinkedIn Account")]
        [RegularExpression(@"(http(s)?://)?(www\.)?linkedin.com/in/(.+)(/)?", ErrorMessage =
            "Invalid LinkedIn Profile URL")]
        public string LinkedInAccount { get; set; }

        [DisplayName("Additional Notes")]
        [StringLength(1_000_000, ErrorMessage = "Additional notes should not exceed 1,000,000 characters.")]
        [ColumnName("Additional Notes")]
        public string AdditionalNotes { get; set; }

        [DisplayName("How did you hear about us?")]
        [StringLength(100)]
        [ColumnName("Referral Type")]
        public string HowDidYouHearAboutUs { get; set; }


        [DisplayName("<set in js based on value in HowDidYouHearAboutUs>")]
        [StringLength(100)]
        [ColumnName("Referral Detail")]
        public string HowDidYouHearAboutUsDetail { get; set; }



        [DisplayName("<not a displayed value>")]
        [ColumnName("Referred By")]
        public string ReferredBy => HowDidYouHearAboutUsDetail == null
            ? EnumHelper.HowDidYouHearAboutUsEnumNameToDescription(HowDidYouHearAboutUs)
            : $"{EnumHelper.HowDidYouHearAboutUsEnumNameToDescription(HowDidYouHearAboutUs)} / {HowDidYouHearAboutUsDetail}";
    }
}

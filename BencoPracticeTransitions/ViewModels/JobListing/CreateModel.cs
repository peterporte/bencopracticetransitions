using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BencoPracticeTransitions.Framework.CustomAttributes;
using BencoPracticeTransitions.Framework.Helper;

namespace BencoPracticeTransitions.ViewModels.JobListing
{
    public class CreateModel
    {
        private List<CreateJobHourModel> _jobHours;


        [DisplayName("Practice Name")]
        [Required(ErrorMessage = "Practice Name is required.")]
        [ColumnName("Practice Name")]
        public string PracticeName { get; set; }

        [DisplayName("Practice Location")]
        [Required(ErrorMessage = "Practice Location is required.")]
        [ColumnName("Location")]
        public string PracticeLocation { get; set; }

        [DisplayName("Contact First Name")]
        [ColumnName("First")]
        [Required(ErrorMessage = "Contact First Name is required.")]
        public string ContactFirstName { get; set; }

        [DisplayName("Contact Last Name")]
        [ColumnName("Last")]
        [Required(ErrorMessage = "Contact Last Name is required.")]
        public string ContactLastName { get; set; }

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

        [DisplayName("Job Type")]
        [Required(ErrorMessage = "Job Type is required.")]
        [ColumnName("Job Type")]
        public string JobType { get; set; }

        [DisplayName("Job Hours")]
        [Required(ErrorMessage = "At least one value should be selected.")]
        [ColumnName("Job Hours")]
        public List<CreateJobHourModel> JobHours
        {
            get => _jobHours ?? (_jobHours = new List<CreateJobHourModel>());
            set => _jobHours = value;
        }

        [DisplayName("Job Requirements")]
        [Required(ErrorMessage = "Job Requirements is required.")]
        [ColumnName("Job Requirements")]
        public string JobRequirements { get; set; }

        [DisplayName("Doctor's Specialty")]
        [ColumnName("Specialty")]
        public string Specialty { get; set; }

        [DisplayName("LinkedIn Profile URL")]
        [ColumnName("LinkedIn Account")]
        [RegularExpression(@"(http(s)?://)?(www\.)?linkedin\.com/in/(.+)(/)?", ErrorMessage =
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

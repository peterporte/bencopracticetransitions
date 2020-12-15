using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BencoPracticeTransitions.Framework.CustomAttributes;
using BencoPracticeTransitions.Framework.Helper;

namespace BencoPracticeTransitions.ViewModels.Practice
{
    public class SellPracticeModel
    {
        [DisplayName("Practice Name")]
        [Required(ErrorMessage = "Practice Name is required.")]
        [ColumnName("Practice Name")]
        public string PracticeName { get; set; }

        [DisplayName("City")]
        [Required(ErrorMessage = "Practice City is required.")]
        [ColumnName("City")]
        public string City { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = "Practice State is required.")]
        [ColumnName("State")]
        public string State { get; set; }

        [DisplayName("Zip Code")]
        [RegularExpression(@"^\s*\d{5}(-\d{4})?\s*$", ErrorMessage = "Invalid Zip Code.")]
        [Required(ErrorMessage = "Practice Zip Code is required.")]
        [ColumnName("Zip Code")]
        public string ZipCode { get; set; }

        [DisplayName("Practice Type")]
        [ColumnName("Type Of Practice")]
        [Required(ErrorMessage = "Practice Type is required.")]
        public string PracticeType { get; set; }

        [DisplayName("Contact First Name")]
        [ColumnName("First")]
        [Required(ErrorMessage = "Contact First Name is required.")]
        public string ContactFirstName { get; set; }

        [DisplayName("Contact Last Name")]
        [ColumnName("Last")]
        [Required(ErrorMessage = "Contact Last Name is required.")]
        public string ContactLastName { get; set; }

        [DisplayName("Contact Phone Number")]
        [ColumnName("Preferred Phone")]
        [RegularExpression("^\\s*([+]?1[-]?\\s*)?\\(?(\\d{3})\\)?[-\\s]?(\\d{3})[-\\s]?(\\d{4})\\s*$", ErrorMessage = "Invalid Contact Number.")]
        [Required(ErrorMessage = "Contact Phone Number is required.")]
        public string ContactPhone { get; set; }

        [DisplayName("Contact Email")]
        [ColumnName("Email")]
        [Required(ErrorMessage = "Contact Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Contact Email.")]
        public string ContactEmail { get; set; }

        [DisplayName("Asking Price")]
        [Required(ErrorMessage = "Asking Price is required.")]
        [Range(typeof(decimal), "0", "10000000", ErrorMessage = "Enter amount between 0 to 10,000,000 only.")]
        [ColumnName("Asking Price")]
        public decimal? AskingPrice { get; set; }

        [DisplayName("Appraisal Needed?")]
        [Required(ErrorMessage = "Please indicate if an appraisal is needed.")]
        [ColumnName("Appraisal Needed?")]
        public bool? IsAppraisalNeeded { get; set; }

        [DisplayName("Amount of Collections")]
        [Required(ErrorMessage = "Amount of Collections is required.")]
        public string CollectionsAmount { get; set; }

        [DisplayName("Real Estate Option (I currently)")]
        [ColumnName("Real Estate Option")]
        [Required(ErrorMessage = "Real Estate Option is required.")]
        public string RealEstateOption { get; set; }

        [DisplayName("Number of Working Operatories")]
        [Required(ErrorMessage = "Number of Working Operatories is required.")]
        [Range(0, 1000, ErrorMessage = "Enter amount between 0 to 1000 only.")]
        [ColumnName("Working Operatories")]
        public int? WorkingOperatoryCount { get; set; }

        [DisplayName("Number of Available Operatories for Expansion")]
        [Range(0, 1000, ErrorMessage = "Enter amount between 0 to 1000 only.")]
        [ColumnName("Expandable Operatories")]
        public int? ExpandableOperatoryCount { get; set; }
        
        [DisplayName("Is the doctor willing to stay on post-transition?")]
        [Required(ErrorMessage = "Please indicate if the doctor is willing to stay on post-transition.")]
        [ColumnName("Doctor Stay On?")]
        public bool? IsDoctorWillingToStayOnAfterTransition { get; set; }

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

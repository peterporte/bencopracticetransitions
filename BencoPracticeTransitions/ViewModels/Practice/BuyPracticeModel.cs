using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BencoPracticeTransitions.Framework.CustomAttributes;
using BencoPracticeTransitions.Framework.Helper;

namespace BencoPracticeTransitions.ViewModels.Practice
{
    public class BuyPracticeModel
    {
        [DisplayName("Contact First Name")]
        [ColumnName("First")]
        [Required(ErrorMessage = "Contact First Name is required.")]
        public string ContactFirstName { get; set; }


        [DisplayName("Contact Last Name")]
        [ColumnName("Last")]
        [Required(ErrorMessage="Contact Last Name is required.")]
        public string ContactLastName { get; set; }


        [DisplayName("Contact Phone Number")]
        [ColumnName("Preferred Phone")]
        [RegularExpression("^\\s*([+]?1[-]?\\s*)?\\(?(\\d{3})\\)?[-\\s]?(\\d{3})[-\\s]?(\\d{4})\\s*$", ErrorMessage = "Invalid Contact Number.")]
        [Required(ErrorMessage = "Contact Phone Number is required.")]
        public string ContactPhoneNumber { get; set; }


        [DisplayName("Contact Email")]
        [ColumnName("Email")]
        [EmailAddress(ErrorMessage = "Invalid Contact Email.")]
        [Required(ErrorMessage="Contact Email is required.")]
        public string ContactEmail { get; set; }


        [DisplayName("Practice Type")]
        [ColumnName("Type Of Practice")]
        [Required(ErrorMessage = "Practice Type is required.")]
        public string PracticeType { get; set; }


        [DisplayName("Purchase Location")]
        [Required(ErrorMessage="Purchase Location is required.")]
        [ColumnName("Location")]
        public string PurchaseLocation { get; set; }


        [DisplayName("Amount of Collections interested in")]
        [Required(ErrorMessage="Amount of Collections is required.")]
        public string CollectionsAmount { get; set; }


        [DisplayName("Real Estate Option (I am looking to)")]
        [ColumnName("Real Estate Option")]
        [Required(ErrorMessage = "Real Estate Option is required.")]
        public string RealEstateOption { get; set; }


        [DisplayName("Minimum Practice Purchase Amount")]
        [ColumnName("Min Purchase Amount")]
        [Required(ErrorMessage = "Minimum Purchase Amount is required.")]
        [Range(typeof(decimal), "0", "10000000", ErrorMessage = "Enter amount between 0 to 10,000,000.")]
        public decimal? MinPurchaseAmount { get; set; }


        [DisplayName("Maximum Practice Purchase Amount")]
        [ColumnName("Max Purchase Amount")]
        [Required(ErrorMessage = "Maximum Purchase Amount is required.")]
        [Range(typeof(decimal), "0", "10000000", ErrorMessage = "Enter amount between 0 to 10,000,000.")]
        public decimal? MaxPurchaseAmount { get; set; }


        [DisplayName("Number of working operatories seeking")]
        [Required(ErrorMessage = "Number of working operatories seeking is required.")]
        [Range(1, 1_000, ErrorMessage = "Enter amount between 1 to 1,000 only.")]
        [ColumnName("Working operatories")]
        public int? MinOperatoryCount { get; set; }


        [DisplayName("Would you like the doctor to stay on post-transition?")]
        [Required(ErrorMessage="Please select if Yes or No")]
        [ColumnName("Doctor Stay On?")]
        public bool? IsDoctorWillingToStayOnAfterTransition { get; set; }


        [DisplayName("Additional Notes")]
        [StringLength(1_000_000, ErrorMessage = "Additional notes should not exceed 1,000,000 characters.")]
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

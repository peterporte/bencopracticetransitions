using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BencoPracticeTransitions.Framework.CustomAttributes;
using BencoPracticeTransitions.Framework.Helper;

namespace BencoPracticeTransitions.ViewModels.Home
{
    public class ContactUsModel
    {
        [DisplayName("First Name")]
        [ColumnName("First")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }


        [DisplayName("Last Name")]
        [ColumnName("Last")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }


        [DisplayName("Phone Number")]
        [ColumnName("Preferred Phone")]
        [RegularExpression("^\\s*([+]?1[-]?\\s*)?\\(?(\\d{3})\\)?[-\\s]?(\\d{3})[-\\s]?(\\d{4})\\s*$", ErrorMessage = "Invalid Phone Number.")]
        [Required(ErrorMessage = " Phone Number is required.")]
        public string PhoneNumber { get; set; }


        [DisplayName("Email Address")]
        [ColumnName("Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [Required(ErrorMessage = "Email Address is required.")]
        public string EmailAddress { get; set; }


        [DisplayName("Message")]
        [Required(ErrorMessage = "Message is required.")]
        [StringLength(1_000_000, ErrorMessage = "Message should not exceed 1,000,000 characters.")]
        public string Message { get; set; }

        [DisplayName("How did you hear about us?")]
        [StringLength(100)]
        [ColumnName("<does not have column in spreadsheet>")]
        public string HowDidYouHearAboutUs { get; set; }


        [DisplayName("<set in js based on value in HowDidYouHearAboutUs>")]
        [StringLength(100)]
        public string HowDidYouHearAboutUsDetail { get; set; }


        [DisplayName("<not a displayed value>")]
        [ColumnName("Referred By")]
        public string ReferredBy => HowDidYouHearAboutUsDetail == null
            ? EnumHelper.HowDidYouHearAboutUsEnumNameToDescription(HowDidYouHearAboutUs)
            : $"{EnumHelper.HowDidYouHearAboutUsEnumNameToDescription(HowDidYouHearAboutUs)} / {HowDidYouHearAboutUsDetail}";
    }
}

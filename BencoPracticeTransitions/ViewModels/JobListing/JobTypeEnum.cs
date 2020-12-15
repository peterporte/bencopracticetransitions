using System.ComponentModel;

namespace BencoPracticeTransitions.ViewModels.JobListing
{
    public enum JobTypeEnum
    {
        [Description("Doctor")]
        Doctor,
        [Description("Hygienist")]
        Hygienist,
        [Description("Dental Assistant")]
        DentalAssistant
    }
}
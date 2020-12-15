using System.ComponentModel;

namespace BencoPracticeTransitions.ViewModels.JobListing
{
    public enum WorkExperienceEnum
    {
        [Description("Student")]
        Student,
        [Description("Resident")]
        Resident,
        [Description("Experienced Dentist")]
        ExperiencedDentist,
        [Description("Practicing Dentist")]
        PracticingDentist,
        [Description("Hygienist")]
        Hygienist,
        [Description("Dental Assistant")]
        DentalAssistant,
        [Description("Other")]
        Other
    }
}

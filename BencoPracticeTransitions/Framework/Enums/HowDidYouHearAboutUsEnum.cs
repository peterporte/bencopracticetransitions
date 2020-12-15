using System.ComponentModel;

namespace BencoPracticeTransitions.Framework.Enums
{
    public enum HowDidYouHearAboutUsEnum
    {
        [Description("Internet search")]
        InternetSearch,
        [Description("Colleague")]
        Colleague,
        [Description("Practice Transitions Team member")]
        PracticeTransitionsTeamMember,
        [Description("Podcast")]
        Podcast,
        [Description("Benco Territory Representative")]
        BencoTerritoryRepresentative,
        [Description("Benco Website")]
        BencoWebsite,
        [Description("Benco Customer Service")]
        BencoCustomerService,
        [Description("Other")]
        Other
    }
}
using Benco.Framework.UI.Tests.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BencoPracticeTransitions.UI.Tests.Framework.Pages
{
    class Pages
    {
        public static void SetBrowser(WebDriver.Browser browser)
        {
            WebDriver.BrowserType = browser;
        }

        private static PracticeTransistionsHomePage _practiceTransistionsHomePage;
        public static PracticeTransistionsHomePage PracticeTransistionsHomePage => _practiceTransistionsHomePage ?? (_practiceTransistionsHomePage = new PracticeTransistionsHomePage());

        private static ContactUsPage _contactUsPage;
        public static ContactUsPage ContactUsPage => _contactUsPage ?? (_contactUsPage = new ContactUsPage());

        private static PracticeBuyPage _practiceBuyPage;
        public static PracticeBuyPage PracticeBuyPage => _practiceBuyPage ?? (_practiceBuyPage = new PracticeBuyPage());

        private static PracticeSellPage _practiceSellPage;
        public static PracticeSellPage PracticeSellPage => _practiceSellPage ?? (_practiceSellPage = new PracticeSellPage());

        private static JobListingCreatePage _jobListingCreatePage;
        public static JobListingCreatePage JobListingCreatePage => _jobListingCreatePage ?? (_jobListingCreatePage = new JobListingCreatePage());

        private static JobListingInquirePage _jobListingInquirePage;
        public static JobListingInquirePage JobListingInquirePage => _jobListingInquirePage ?? (_jobListingInquirePage = new JobListingInquirePage());

    }
}

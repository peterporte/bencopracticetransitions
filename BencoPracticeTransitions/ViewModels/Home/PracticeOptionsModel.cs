using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BencoPracticeTransitions.ViewModels.Home
{
    public class PracticeOptionsModel
    {
        [Required(ErrorMessage = "Practice Options is required")]
        public string SelectedPracticeOption { get; set; }

        private Dictionary<string, string> _practiceOptions;

        public Dictionary<string, string> PracticeOptions
        {
            get => _practiceOptions ?? (_practiceOptions = new Dictionary<string, string>());
            set => _practiceOptions = value;
        }
    }
}

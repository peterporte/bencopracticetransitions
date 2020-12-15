using System;
using System.Collections.Generic;
using BencoPracticeTransitions.Framework.Enums;

namespace BencoPracticeTransitions.ViewModels.Shared
{
    public class AlertViewModel
    {
        public AlertTypeEnum AlertType { get; set; }
        public List<string> Messages { get; set; }
        public bool IsInternalServerError { get; set; }
        public bool CanCloseAlert { get; set; }

        public string AlertTypeStyleClass
        {
            get
            {
                switch (AlertType)
                {
                    case AlertTypeEnum.Success:
                        return "alert-success";
                    case AlertTypeEnum.Error:
                        return "alert-danger";
                    case AlertTypeEnum.Info:
                        return "alert-info";
                    case AlertTypeEnum.Warning:
                        return "alert-warning";
                    case AlertTypeEnum.Unauthorized:
                        return "alert-danger";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public string AlertDismissableClass => CanCloseAlert ? "alert-dismissable" : null;
    }
}

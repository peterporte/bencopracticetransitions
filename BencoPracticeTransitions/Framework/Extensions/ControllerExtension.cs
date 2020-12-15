using System.Collections.Generic;
using BencoPracticeTransitions.Framework.Enums;
using BencoPracticeTransitions.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BencoPracticeTransitions.Framework.Extensions
{
    public static class ControllerExtension
    {
        public static void CreateAlert(this Controller controller, AlertTypeEnum alertType, string message, bool isInternalServerError = false, bool canCloseAlert = true)
        {
            controller.CreateAlert(alertType, new List<string> { message }, isInternalServerError, canCloseAlert);
        }

        public static void CreateAlert(this Controller controller, AlertTypeEnum alertType, List<string> messages, bool isInternalServerError = false, bool canCloseAlert = true)
        {
            var alertViewModel = new AlertViewModel
            {
                AlertType = alertType,
                IsInternalServerError = isInternalServerError,
                Messages = messages,
                CanCloseAlert = canCloseAlert
            };
            controller.TempData["alerts"] = alertViewModel;
        }
    }
}

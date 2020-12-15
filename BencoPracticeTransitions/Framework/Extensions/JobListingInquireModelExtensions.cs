using BencoPracticeTransitions.Infrastructure.Email;
using Microsoft.AspNetCore.Http;
using System.IO;
using BencoPracticeTransitions.ViewModels.JobListing;

namespace BencoPracticeTransitions.Framework.Extensions
{
    public static class JobListingInquireModelExtensions
    {
        public static void AttachResume(this InquireModel model, IFormFile uploadedFile)
        {
            if (!string.IsNullOrEmpty(uploadedFile?.FileName) && uploadedFile.Length > 0)
            {
                model.Resume = new EmailAttachment
                {
                    Data = uploadedFile.OpenReadStream(),
                    Filename = $"Resume{Path.GetExtension(uploadedFile.FileName)}"
                };
            }
        }
    }
}

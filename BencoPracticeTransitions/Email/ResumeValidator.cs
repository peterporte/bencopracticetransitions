using Microsoft.AspNetCore.Http;
using System.IO;

namespace BencoPracticeTransitions.Email
{
    public class ResumeValidator : IValidateResume
    {
        private const int FileSizeLimit = 2100000;
        public bool UploadedResumeIsValid(IFormFile file, out string errorMessage)
        {
            if (!string.IsNullOrEmpty(file?.FileName) && file.Length > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName).Replace(".", string.Empty);
                if (!IsFileExtensionAllowedAsEmailAttachment(fileExtension))
                {
                    errorMessage = "Invalid file format. Only Word and PDF files are allowed.";
                    return false;
                }
                if (file.Length > FileSizeLimit)
                {
                    errorMessage = "File has exceeded the file size limit of 2MB.";
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
        }

        public bool IsFileExtensionAllowedAsEmailAttachment(string fileExtension)
        {
            switch (fileExtension)
            {
                case "doc": return true;
                case "docx": return true;
                case "pdf": return true;
                default: return false;
            }
        }
    }
}

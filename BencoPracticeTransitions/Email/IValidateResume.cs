using Microsoft.AspNetCore.Http;

namespace BencoPracticeTransitions.Email
{
    public interface IValidateResume
    {
        bool UploadedResumeIsValid(IFormFile file, out string errorMessage);
        bool IsFileExtensionAllowedAsEmailAttachment(string fileExtension);
    }
}

using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace Presentation.Utilities
{
    public class FileValidator
    {
        byte[] dictionary_png = new byte[] { 255, 216 };
        byte[] dictionary_pdf = new byte[] { 37, 80, 68, 70, 45 }; 
        public bool MagicChecker(IFormFile file)
        {
            bool isJpg = false, isPdf = false, isJpgE = false, isPdfE = false;
            using (Stream myFileForCheckingType = file.OpenReadStream())
            {
                byte[] toBeVerified = new byte[dictionary_png.Length];
                myFileForCheckingType.Read(toBeVerified, 0, dictionary_png.Length);
                isJpg = dictionary_png.SequenceEqual(toBeVerified);
            }

            if (Path.GetExtension(file.FileName) == ".jpg")
            {
                isJpgE = true;
            }

            using (Stream myFileForCheckingType = file.OpenReadStream())
            {
                byte[] toBeVerified = new byte[dictionary_pdf.Length];
                myFileForCheckingType.Read(toBeVerified, 0, dictionary_pdf.Length);
                isPdf = dictionary_pdf.SequenceEqual(toBeVerified);
            }

            if (Path.GetExtension(file.FileName) == ".pdf")
            {
                isPdfE = true;
            }

            return ((isJpg && isJpgE) || (isPdf && isPdfE));
        }
    }
}
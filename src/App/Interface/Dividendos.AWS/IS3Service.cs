using System.Threading.Tasks;

namespace Dividendos.AWS
{
    public interface IS3Service
    {
        Task<string> PutImage(byte[] file, string fileName);
        Task<string> PutPDF(byte[] file, string fileName);
        string GenerateURL(string key);
        //Task<string> GetXml(string key);
    }
}

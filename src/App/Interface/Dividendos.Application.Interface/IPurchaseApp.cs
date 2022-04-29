using Dividendos.API.Model.Request.Purchase;
using Dividendos.API.Model.Response.Common;
using Dividendos.API.Model.Response.Purchase;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Dividendos.Application.Interface
{
    public interface IPurchaseApp
    {
        ResultResponseObject<Subscribe> SubscribeDetails(string userID);

        void ReceiveInformation(string token, dynamic value);

        void GetPurchaseDetailsFromIapHub(string idPurchase);

        ResultResponseObject<Subscribe> SubscribeDetailsV2(string userID);

        ResultResponseObject<List<Dividendos.API.Model.Response.Purchase.ProductVM>> GetProducts();
        void SubscribePartnerFile(string path, string fileName);
        void SubscribePartnerFileNotDefined(string path, string fileName, bool checkAll = false);
        void SubscribePartnerActivated(string path, string fileName);
        void GetEmailByIdentifier(string path, string fileName);
        void SendPushPartner();
        void SubscribePartnerFileV2(string path, string fileName, bool checkAll = false);
        void CheckPilantra(string path, string fileName, bool checkAll = false);
        void SendPushPilantra(string path, string fileName);
        ResultResponseObject<List<ProductVM>> GetProducts(DeviceType deviceType, string appVersion);
        ResultResponseObject<List<ProductVM>> GetProductsV4(DeviceType deviceType, string appVersion);
        ResultResponseObject<List<ProductVM>> GetProductsV5(DeviceType deviceType, string appVersion);
        ResultResponseObject<Subscribe> GetSubscribeDetailsByEmail(string email);
        ResultResponseObject<Subscribe> AddNewLicence(AddSubscription addSubscription);
        ResultResponseObject<Subscribe> UpdateLicence(EditSubscription editSubscription);
    }
}

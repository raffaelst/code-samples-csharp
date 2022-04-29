using System;
using System.Collections.Generic;
using RestSharp;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace IO.Swagger.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface ICryptoTransactionApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoTransactionBuycryptoGuidCryptoPortfolioPost (Guid? guidCryptoPortfolio, CryptoAddVM body, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolioSub"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoTransactionCryptotransactionBuyDetailsGuidCryptoPortfolioSubGet (Guid? guidCryptoPortfolioSub, string startDate, string endDate, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolioSub"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoTransactionCryptotransactionSellDetailsGuidCryptoPortfolioSubGet (Guid? guidCryptoPortfolioSub, string startDate, string endDate, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoTransactionEditbuytransactionGuidCryptoPortfolioPut (Guid? guidCryptoPortfolio, CryptoEditVM body, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoTransactionEditselltransactionGuidCryptoPortfolioPut (Guid? guidCryptoPortfolio, CryptoEditVM body, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="guidCryptoCurrency"></param>
        /// <param name="transactionType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoTransactionGuidCryptoPortfolioCryptotransactionitemGuidCryptoCurrencyTransactionTypeGet (Guid? guidCryptoPortfolio, Guid? guidCryptoCurrency, int? transactionType, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="guidCryptoTransactionItem"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoTransactionGuidCryptoPortfolioCryptotransactionitemGuidCryptoTransactionItemDelete (Guid? guidCryptoPortfolio, Guid? guidCryptoTransactionItem, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoTransactionSellcryptoGuidCryptoPortfolioPost (Guid? guidCryptoPortfolio, CryptoAddVM body, string apiVersion);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class CryptoTransactionApi : ICryptoTransactionApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoTransactionApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public CryptoTransactionApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoTransactionApi"/> class.
        /// </summary>
        /// <returns></returns>
        public CryptoTransactionApi(String basePath)
        {
            this.ApiClient = new ApiClient(basePath);
        }
    
        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public void SetBasePath(String basePath)
        {
            this.ApiClient.BasePath = basePath;
        }
    
        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public String GetBasePath(String basePath)
        {
            return this.ApiClient.BasePath;
        }
    
        /// <summary>
        /// Gets or sets the API client.
        /// </summary>
        /// <value>An instance of the ApiClient</value>
        public ApiClient ApiClient {get; set;}
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoTransactionBuycryptoGuidCryptoPortfolioPost (Guid? guidCryptoPortfolio, CryptoAddVM body, string apiVersion)
        {
            // verify the required parameter 'guidCryptoPortfolio' is set
            if (guidCryptoPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidCryptoPortfolio' when calling CryptoTransactionBuycryptoGuidCryptoPortfolioPost");
    
            var path = "/CryptoTransaction/buycrypto/{guidCryptoPortfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidCryptoPortfolio" + "}", ApiClient.ParameterToString(guidCryptoPortfolio));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        PostBody = ApiClient.Serialize(body); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.Post, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoTransactionBuycryptoGuidCryptoPortfolioPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoTransactionBuycryptoGuidCryptoPortfolioPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolioSub"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoTransactionCryptotransactionBuyDetailsGuidCryptoPortfolioSubGet (Guid? guidCryptoPortfolioSub, string startDate, string endDate, string apiVersion)
        {
            // verify the required parameter 'guidCryptoPortfolioSub' is set
            if (guidCryptoPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidCryptoPortfolioSub' when calling CryptoTransactionCryptotransactionBuyDetailsGuidCryptoPortfolioSubGet");
    
            var path = "/CryptoTransaction/cryptotransactionBuyDetails/{guidCryptoPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidCryptoPortfolioSub" + "}", ApiClient.ParameterToString(guidCryptoPortfolioSub));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (startDate != null) queryParams.Add("startDate", ApiClient.ParameterToString(startDate)); // query parameter
 if (endDate != null) queryParams.Add("endDate", ApiClient.ParameterToString(endDate)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoTransactionCryptotransactionBuyDetailsGuidCryptoPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoTransactionCryptotransactionBuyDetailsGuidCryptoPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolioSub"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoTransactionCryptotransactionSellDetailsGuidCryptoPortfolioSubGet (Guid? guidCryptoPortfolioSub, string startDate, string endDate, string apiVersion)
        {
            // verify the required parameter 'guidCryptoPortfolioSub' is set
            if (guidCryptoPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidCryptoPortfolioSub' when calling CryptoTransactionCryptotransactionSellDetailsGuidCryptoPortfolioSubGet");
    
            var path = "/CryptoTransaction/cryptotransactionSellDetails/{guidCryptoPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidCryptoPortfolioSub" + "}", ApiClient.ParameterToString(guidCryptoPortfolioSub));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (startDate != null) queryParams.Add("startDate", ApiClient.ParameterToString(startDate)); // query parameter
 if (endDate != null) queryParams.Add("endDate", ApiClient.ParameterToString(endDate)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoTransactionCryptotransactionSellDetailsGuidCryptoPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoTransactionCryptotransactionSellDetailsGuidCryptoPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoTransactionEditbuytransactionGuidCryptoPortfolioPut (Guid? guidCryptoPortfolio, CryptoEditVM body, string apiVersion)
        {
            // verify the required parameter 'guidCryptoPortfolio' is set
            if (guidCryptoPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidCryptoPortfolio' when calling CryptoTransactionEditbuytransactionGuidCryptoPortfolioPut");
    
            var path = "/CryptoTransaction/editbuytransaction/{guidCryptoPortfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidCryptoPortfolio" + "}", ApiClient.ParameterToString(guidCryptoPortfolio));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        PostBody = ApiClient.Serialize(body); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoTransactionEditbuytransactionGuidCryptoPortfolioPut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoTransactionEditbuytransactionGuidCryptoPortfolioPut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoTransactionEditselltransactionGuidCryptoPortfolioPut (Guid? guidCryptoPortfolio, CryptoEditVM body, string apiVersion)
        {
            // verify the required parameter 'guidCryptoPortfolio' is set
            if (guidCryptoPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidCryptoPortfolio' when calling CryptoTransactionEditselltransactionGuidCryptoPortfolioPut");
    
            var path = "/CryptoTransaction/editselltransaction/{guidCryptoPortfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidCryptoPortfolio" + "}", ApiClient.ParameterToString(guidCryptoPortfolio));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        PostBody = ApiClient.Serialize(body); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoTransactionEditselltransactionGuidCryptoPortfolioPut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoTransactionEditselltransactionGuidCryptoPortfolioPut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="guidCryptoCurrency"></param>
        /// <param name="transactionType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoTransactionGuidCryptoPortfolioCryptotransactionitemGuidCryptoCurrencyTransactionTypeGet (Guid? guidCryptoPortfolio, Guid? guidCryptoCurrency, int? transactionType, string apiVersion)
        {
            // verify the required parameter 'guidCryptoPortfolio' is set
            if (guidCryptoPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidCryptoPortfolio' when calling CryptoTransactionGuidCryptoPortfolioCryptotransactionitemGuidCryptoCurrencyTransactionTypeGet");
            // verify the required parameter 'guidCryptoCurrency' is set
            if (guidCryptoCurrency == null) throw new ApiException(400, "Missing required parameter 'guidCryptoCurrency' when calling CryptoTransactionGuidCryptoPortfolioCryptotransactionitemGuidCryptoCurrencyTransactionTypeGet");
            // verify the required parameter 'transactionType' is set
            if (transactionType == null) throw new ApiException(400, "Missing required parameter 'transactionType' when calling CryptoTransactionGuidCryptoPortfolioCryptotransactionitemGuidCryptoCurrencyTransactionTypeGet");
    
            var path = "/CryptoTransaction/{guidCryptoPortfolio}/cryptotransactionitem/{guidCryptoCurrency}/{transactionType}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidCryptoPortfolio" + "}", ApiClient.ParameterToString(guidCryptoPortfolio));
path = path.Replace("{" + "guidCryptoCurrency" + "}", ApiClient.ParameterToString(guidCryptoCurrency));
path = path.Replace("{" + "transactionType" + "}", ApiClient.ParameterToString(transactionType));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoTransactionGuidCryptoPortfolioCryptotransactionitemGuidCryptoCurrencyTransactionTypeGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoTransactionGuidCryptoPortfolioCryptotransactionitemGuidCryptoCurrencyTransactionTypeGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="guidCryptoTransactionItem"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoTransactionGuidCryptoPortfolioCryptotransactionitemGuidCryptoTransactionItemDelete (Guid? guidCryptoPortfolio, Guid? guidCryptoTransactionItem, string apiVersion)
        {
            // verify the required parameter 'guidCryptoPortfolio' is set
            if (guidCryptoPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidCryptoPortfolio' when calling CryptoTransactionGuidCryptoPortfolioCryptotransactionitemGuidCryptoTransactionItemDelete");
            // verify the required parameter 'guidCryptoTransactionItem' is set
            if (guidCryptoTransactionItem == null) throw new ApiException(400, "Missing required parameter 'guidCryptoTransactionItem' when calling CryptoTransactionGuidCryptoPortfolioCryptotransactionitemGuidCryptoTransactionItemDelete");
    
            var path = "/CryptoTransaction/{guidCryptoPortfolio}/cryptotransactionitem/{guidCryptoTransactionItem}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidCryptoPortfolio" + "}", ApiClient.ParameterToString(guidCryptoPortfolio));
path = path.Replace("{" + "guidCryptoTransactionItem" + "}", ApiClient.ParameterToString(guidCryptoTransactionItem));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoTransactionGuidCryptoPortfolioCryptotransactionitemGuidCryptoTransactionItemDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoTransactionGuidCryptoPortfolioCryptotransactionitemGuidCryptoTransactionItemDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoTransactionSellcryptoGuidCryptoPortfolioPost (Guid? guidCryptoPortfolio, CryptoAddVM body, string apiVersion)
        {
            // verify the required parameter 'guidCryptoPortfolio' is set
            if (guidCryptoPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidCryptoPortfolio' when calling CryptoTransactionSellcryptoGuidCryptoPortfolioPost");
    
            var path = "/CryptoTransaction/sellcrypto/{guidCryptoPortfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidCryptoPortfolio" + "}", ApiClient.ParameterToString(guidCryptoPortfolio));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        PostBody = ApiClient.Serialize(body); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.Post, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoTransactionSellcryptoGuidCryptoPortfolioPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoTransactionSellcryptoGuidCryptoPortfolioPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}

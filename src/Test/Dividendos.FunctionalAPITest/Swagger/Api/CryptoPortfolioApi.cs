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
    public interface ICryptoPortfolioApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoPortfolioCalculateperformancePut (string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoPortfolioCryptoportfoliostatementGuidCryptoPortfolioSubGet (Guid? guidCryptoPortfolioSub, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoPortfolioCryptoportfolioviewGet (string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoSubPortfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoPortfolioCryptosubportfolioGuidCryptoSubPortfolioDelete (Guid? guidCryptoSubPortfolio, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="guidCryptoCurrency"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoPortfolioGuidCryptoPortfolioCryptoCurrencystatementGuidCryptoCurrencyGet (Guid? guidCryptoPortfolio, Guid? guidCryptoCurrency, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="guidCryptoSubportfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoPortfolioGuidCryptoPortfolioCryptosubportfolioGuidCryptoSubportfolioGet (Guid? guidCryptoPortfolio, Guid? guidCryptoSubportfolio, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoPortfolioGuidCryptoPortfolioCryptosubportfolioPost (Guid? guidCryptoPortfolio, CryptoSubportfolioVM body, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoPortfolioGuidCryptoPortfolioDelete (Guid? guidCryptoPortfolio, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoPortfolioGuidCryptoPortfolioGet (Guid? guidCryptoPortfolio, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoPortfolioGuidCryptoPortfolioUpdatenamePut (Guid? guidCryptoPortfolio, CryptoPortfolioEditVM body, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoSubportfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoPortfolioGuidCryptoSubportfolioSubportfolioPut (Guid? guidCryptoSubportfolio, CryptoSubportfolioVM body, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void CryptoPortfolioManualportfolioPost (CryptoPortolioRequest body, string apiVersion);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class CryptoPortfolioApi : ICryptoPortfolioApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoPortfolioApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public CryptoPortfolioApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoPortfolioApi"/> class.
        /// </summary>
        /// <returns></returns>
        public CryptoPortfolioApi(String basePath)
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
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoPortfolioCalculateperformancePut (string apiVersion)
        {
    
            var path = "/CryptoPortfolio/calculateperformance";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioCalculateperformancePut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioCalculateperformancePut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoPortfolioCryptoportfoliostatementGuidCryptoPortfolioSubGet (Guid? guidCryptoPortfolioSub, string apiVersion)
        {
            // verify the required parameter 'guidCryptoPortfolioSub' is set
            if (guidCryptoPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidCryptoPortfolioSub' when calling CryptoPortfolioCryptoportfoliostatementGuidCryptoPortfolioSubGet");
    
            var path = "/CryptoPortfolio/cryptoportfoliostatement/{guidCryptoPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidCryptoPortfolioSub" + "}", ApiClient.ParameterToString(guidCryptoPortfolioSub));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioCryptoportfoliostatementGuidCryptoPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioCryptoportfoliostatementGuidCryptoPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoPortfolioCryptoportfolioviewGet (string apiVersion)
        {
    
            var path = "/CryptoPortfolio/cryptoportfolioview";
            path = path.Replace("{format}", "json");
                
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
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioCryptoportfolioviewGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioCryptoportfolioviewGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoSubPortfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoPortfolioCryptosubportfolioGuidCryptoSubPortfolioDelete (Guid? guidCryptoSubPortfolio, string apiVersion)
        {
            // verify the required parameter 'guidCryptoSubPortfolio' is set
            if (guidCryptoSubPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidCryptoSubPortfolio' when calling CryptoPortfolioCryptosubportfolioGuidCryptoSubPortfolioDelete");
    
            var path = "/CryptoPortfolio/cryptosubportfolio/{guidCryptoSubPortfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidCryptoSubPortfolio" + "}", ApiClient.ParameterToString(guidCryptoSubPortfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioCryptosubportfolioGuidCryptoSubPortfolioDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioCryptosubportfolioGuidCryptoSubPortfolioDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="guidCryptoCurrency"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoPortfolioGuidCryptoPortfolioCryptoCurrencystatementGuidCryptoCurrencyGet (Guid? guidCryptoPortfolio, Guid? guidCryptoCurrency, string apiVersion)
        {
            // verify the required parameter 'guidCryptoPortfolio' is set
            if (guidCryptoPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidCryptoPortfolio' when calling CryptoPortfolioGuidCryptoPortfolioCryptoCurrencystatementGuidCryptoCurrencyGet");
            // verify the required parameter 'guidCryptoCurrency' is set
            if (guidCryptoCurrency == null) throw new ApiException(400, "Missing required parameter 'guidCryptoCurrency' when calling CryptoPortfolioGuidCryptoPortfolioCryptoCurrencystatementGuidCryptoCurrencyGet");
    
            var path = "/CryptoPortfolio/{guidCryptoPortfolio}/cryptoCurrencystatement/{guidCryptoCurrency}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidCryptoPortfolio" + "}", ApiClient.ParameterToString(guidCryptoPortfolio));
path = path.Replace("{" + "guidCryptoCurrency" + "}", ApiClient.ParameterToString(guidCryptoCurrency));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioGuidCryptoPortfolioCryptoCurrencystatementGuidCryptoCurrencyGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioGuidCryptoPortfolioCryptoCurrencystatementGuidCryptoCurrencyGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="guidCryptoSubportfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoPortfolioGuidCryptoPortfolioCryptosubportfolioGuidCryptoSubportfolioGet (Guid? guidCryptoPortfolio, Guid? guidCryptoSubportfolio, string apiVersion)
        {
            // verify the required parameter 'guidCryptoPortfolio' is set
            if (guidCryptoPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidCryptoPortfolio' when calling CryptoPortfolioGuidCryptoPortfolioCryptosubportfolioGuidCryptoSubportfolioGet");
            // verify the required parameter 'guidCryptoSubportfolio' is set
            if (guidCryptoSubportfolio == null) throw new ApiException(400, "Missing required parameter 'guidCryptoSubportfolio' when calling CryptoPortfolioGuidCryptoPortfolioCryptosubportfolioGuidCryptoSubportfolioGet");
    
            var path = "/CryptoPortfolio/{guidCryptoPortfolio}/cryptosubportfolio/{guidCryptoSubportfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidCryptoPortfolio" + "}", ApiClient.ParameterToString(guidCryptoPortfolio));
path = path.Replace("{" + "guidCryptoSubportfolio" + "}", ApiClient.ParameterToString(guidCryptoSubportfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioGuidCryptoPortfolioCryptosubportfolioGuidCryptoSubportfolioGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioGuidCryptoPortfolioCryptosubportfolioGuidCryptoSubportfolioGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoPortfolioGuidCryptoPortfolioCryptosubportfolioPost (Guid? guidCryptoPortfolio, CryptoSubportfolioVM body, string apiVersion)
        {
            // verify the required parameter 'guidCryptoPortfolio' is set
            if (guidCryptoPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidCryptoPortfolio' when calling CryptoPortfolioGuidCryptoPortfolioCryptosubportfolioPost");
    
            var path = "/CryptoPortfolio/{guidCryptoPortfolio}/cryptosubportfolio";
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
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioGuidCryptoPortfolioCryptosubportfolioPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioGuidCryptoPortfolioCryptosubportfolioPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoPortfolioGuidCryptoPortfolioDelete (Guid? guidCryptoPortfolio, string apiVersion)
        {
            // verify the required parameter 'guidCryptoPortfolio' is set
            if (guidCryptoPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidCryptoPortfolio' when calling CryptoPortfolioGuidCryptoPortfolioDelete");
    
            var path = "/CryptoPortfolio/{guidCryptoPortfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidCryptoPortfolio" + "}", ApiClient.ParameterToString(guidCryptoPortfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioGuidCryptoPortfolioDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioGuidCryptoPortfolioDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoPortfolioGuidCryptoPortfolioGet (Guid? guidCryptoPortfolio, string apiVersion)
        {
            // verify the required parameter 'guidCryptoPortfolio' is set
            if (guidCryptoPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidCryptoPortfolio' when calling CryptoPortfolioGuidCryptoPortfolioGet");
    
            var path = "/CryptoPortfolio/{guidCryptoPortfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidCryptoPortfolio" + "}", ApiClient.ParameterToString(guidCryptoPortfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioGuidCryptoPortfolioGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioGuidCryptoPortfolioGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoPortfolioGuidCryptoPortfolioUpdatenamePut (Guid? guidCryptoPortfolio, CryptoPortfolioEditVM body, string apiVersion)
        {
            // verify the required parameter 'guidCryptoPortfolio' is set
            if (guidCryptoPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidCryptoPortfolio' when calling CryptoPortfolioGuidCryptoPortfolioUpdatenamePut");
    
            var path = "/CryptoPortfolio/{guidCryptoPortfolio}/updatename";
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
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioGuidCryptoPortfolioUpdatenamePut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioGuidCryptoPortfolioUpdatenamePut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidCryptoSubportfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoPortfolioGuidCryptoSubportfolioSubportfolioPut (Guid? guidCryptoSubportfolio, CryptoSubportfolioVM body, string apiVersion)
        {
            // verify the required parameter 'guidCryptoSubportfolio' is set
            if (guidCryptoSubportfolio == null) throw new ApiException(400, "Missing required parameter 'guidCryptoSubportfolio' when calling CryptoPortfolioGuidCryptoSubportfolioSubportfolioPut");
    
            var path = "/CryptoPortfolio/{guidCryptoSubportfolio}/subportfolio";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidCryptoSubportfolio" + "}", ApiClient.ParameterToString(guidCryptoSubportfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioGuidCryptoSubportfolioSubportfolioPut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioGuidCryptoSubportfolioSubportfolioPut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void CryptoPortfolioManualportfolioPost (CryptoPortolioRequest body, string apiVersion)
        {
    
            var path = "/CryptoPortfolio/manualportfolio";
            path = path.Replace("{format}", "json");
                
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
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioManualportfolioPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CryptoPortfolioManualportfolioPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}

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
    public interface IFollowStockApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void FollowStockAlertsPost (FollowStockAlertVM body, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="followStockAlertGuid"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void FollowStockFollowStockAlertGuidAlertsDelete (Guid? followStockAlertGuid, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="followStockGuid"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void FollowStockFollowStockGuidAlertsGet (Guid? followStockGuid, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="followStockGuid"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void FollowStockFollowStockGuidDelete (Guid? followStockGuid, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void FollowStockGet (string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="idStockOrCrypto"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void FollowStockIdStockOrCryptoGet (string idStockOrCrypto, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void FollowStockPost (FollowStockVM body, string apiVersion);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class FollowStockApi : IFollowStockApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FollowStockApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public FollowStockApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="FollowStockApi"/> class.
        /// </summary>
        /// <returns></returns>
        public FollowStockApi(String basePath)
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
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void FollowStockAlertsPost (FollowStockAlertVM body, string apiVersion)
        {
    
            var path = "/FollowStock/alerts";
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
                throw new ApiException ((int)response.StatusCode, "Error calling FollowStockAlertsPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling FollowStockAlertsPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="followStockAlertGuid"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void FollowStockFollowStockAlertGuidAlertsDelete (Guid? followStockAlertGuid, string apiVersion)
        {
            // verify the required parameter 'followStockAlertGuid' is set
            if (followStockAlertGuid == null) throw new ApiException(400, "Missing required parameter 'followStockAlertGuid' when calling FollowStockFollowStockAlertGuidAlertsDelete");
    
            var path = "/FollowStock/{followStockAlertGuid}/alerts";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "followStockAlertGuid" + "}", ApiClient.ParameterToString(followStockAlertGuid));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling FollowStockFollowStockAlertGuidAlertsDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling FollowStockFollowStockAlertGuidAlertsDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="followStockGuid"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void FollowStockFollowStockGuidAlertsGet (Guid? followStockGuid, string apiVersion)
        {
            // verify the required parameter 'followStockGuid' is set
            if (followStockGuid == null) throw new ApiException(400, "Missing required parameter 'followStockGuid' when calling FollowStockFollowStockGuidAlertsGet");
    
            var path = "/FollowStock/{followStockGuid}/alerts";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "followStockGuid" + "}", ApiClient.ParameterToString(followStockGuid));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling FollowStockFollowStockGuidAlertsGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling FollowStockFollowStockGuidAlertsGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="followStockGuid"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void FollowStockFollowStockGuidDelete (Guid? followStockGuid, string apiVersion)
        {
            // verify the required parameter 'followStockGuid' is set
            if (followStockGuid == null) throw new ApiException(400, "Missing required parameter 'followStockGuid' when calling FollowStockFollowStockGuidDelete");
    
            var path = "/FollowStock/{followStockGuid}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "followStockGuid" + "}", ApiClient.ParameterToString(followStockGuid));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling FollowStockFollowStockGuidDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling FollowStockFollowStockGuidDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void FollowStockGet (string apiVersion)
        {
    
            var path = "/FollowStock";
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
                throw new ApiException ((int)response.StatusCode, "Error calling FollowStockGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling FollowStockGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="idStockOrCrypto"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void FollowStockIdStockOrCryptoGet (string idStockOrCrypto, string apiVersion)
        {
            // verify the required parameter 'idStockOrCrypto' is set
            if (idStockOrCrypto == null) throw new ApiException(400, "Missing required parameter 'idStockOrCrypto' when calling FollowStockIdStockOrCryptoGet");
    
            var path = "/FollowStock/{idStockOrCrypto}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "idStockOrCrypto" + "}", ApiClient.ParameterToString(idStockOrCrypto));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling FollowStockIdStockOrCryptoGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling FollowStockIdStockOrCryptoGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void FollowStockPost (FollowStockVM body, string apiVersion)
        {
    
            var path = "/FollowStock";
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
                throw new ApiException ((int)response.StatusCode, "Error calling FollowStockPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling FollowStockPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}

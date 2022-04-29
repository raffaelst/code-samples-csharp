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
    public interface INotificationApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void NotificationBatchDeleteDelete (string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="notificationHistoricalGuid"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void NotificationDelete (string notificationHistoricalGuid, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void NotificationGet (string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void NotificationPushForListPost (PushContentListVM body, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void NotificationPushPost (PushContentVM body, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void NotificationTestPost (string apiVersion);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class NotificationApi : INotificationApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public NotificationApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationApi"/> class.
        /// </summary>
        /// <returns></returns>
        public NotificationApi(String basePath)
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
        public void NotificationBatchDeleteDelete (string apiVersion)
        {
    
            var path = "/Notification/batch-delete";
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
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationBatchDeleteDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationBatchDeleteDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="notificationHistoricalGuid"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void NotificationDelete (string notificationHistoricalGuid, string apiVersion)
        {
    
            var path = "/Notification";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (notificationHistoricalGuid != null) queryParams.Add("notificationHistoricalGuid", ApiClient.ParameterToString(notificationHistoricalGuid)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void NotificationGet (string apiVersion)
        {
    
            var path = "/Notification";
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
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void NotificationPushForListPost (PushContentListVM body, string apiVersion)
        {
    
            var path = "/Notification/push-for-list";
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
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationPushForListPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationPushForListPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void NotificationPushPost (PushContentVM body, string apiVersion)
        {
    
            var path = "/Notification/push";
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
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationPushPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationPushPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void NotificationTestPost (string apiVersion)
        {
    
            var path = "/Notification/test";
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
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.Post, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationTestPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling NotificationTestPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}

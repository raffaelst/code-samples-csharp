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
    public interface ITaxesApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="user"></param>
        /// <param name="xAuthToken"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void TaxesUserMailPost (string user, string xAuthToken, string title, string body, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="user"></param>
        /// <param name="xAuthToken"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void TaxesUserPushPost (string user, string xAuthToken, string title, string text, string apiVersion);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class TaxesApi : ITaxesApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaxesApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public TaxesApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="TaxesApi"/> class.
        /// </summary>
        /// <returns></returns>
        public TaxesApi(String basePath)
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
        /// <param name="user"></param>
        /// <param name="xAuthToken"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void TaxesUserMailPost (string user, string xAuthToken, string title, string body, string apiVersion)
        {
            // verify the required parameter 'user' is set
            if (user == null) throw new ApiException(400, "Missing required parameter 'user' when calling TaxesUserMailPost");
    
            var path = "/Taxes/{user}/mail";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "user" + "}", ApiClient.ParameterToString(user));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (title != null) queryParams.Add("title", ApiClient.ParameterToString(title)); // query parameter
 if (body != null) queryParams.Add("body", ApiClient.ParameterToString(body)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
             if (xAuthToken != null) headerParams.Add("X-Auth-Token", ApiClient.ParameterToString(xAuthToken)); // header parameter
            
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.Post, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling TaxesUserMailPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling TaxesUserMailPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="user"></param>
        /// <param name="xAuthToken"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void TaxesUserPushPost (string user, string xAuthToken, string title, string text, string apiVersion)
        {
            // verify the required parameter 'user' is set
            if (user == null) throw new ApiException(400, "Missing required parameter 'user' when calling TaxesUserPushPost");
    
            var path = "/Taxes/{user}/push";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "user" + "}", ApiClient.ParameterToString(user));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (title != null) queryParams.Add("title", ApiClient.ParameterToString(title)); // query parameter
 if (text != null) queryParams.Add("text", ApiClient.ParameterToString(text)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
             if (xAuthToken != null) headerParams.Add("X-Auth-Token", ApiClient.ParameterToString(xAuthToken)); // header parameter
            
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.Post, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling TaxesUserPushPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling TaxesUserPushPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}

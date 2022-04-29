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
    public interface IHealthCheckApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="xAuthToken"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void HealthCheckGet (string xAuthToken, string apiVersion);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class HealthCheckApi : IHealthCheckApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheckApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public HealthCheckApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheckApi"/> class.
        /// </summary>
        /// <returns></returns>
        public HealthCheckApi(String basePath)
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
        /// <param name="xAuthToken"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void HealthCheckGet (string xAuthToken, string apiVersion)
        {
    
            var path = "/HealthCheck";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
             if (xAuthToken != null) headerParams.Add("X-Auth-Token", ApiClient.ParameterToString(xAuthToken)); // header parameter
            
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling HealthCheckGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling HealthCheckGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}

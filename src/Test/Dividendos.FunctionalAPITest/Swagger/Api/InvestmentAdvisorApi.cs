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
    public interface IInvestmentAdvisorApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void InvestmentAdvisorFreeRecommendationsGet (string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="investmentAdvisorFreeRecommendationID"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void InvestmentAdvisorFreeRecommendationsInvestmentAdvisorFreeRecommendationIDDelete (long? investmentAdvisorFreeRecommendationID, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void InvestmentAdvisorFreeRecommendationsPost (FreeRecommendationRequest body, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void InvestmentAdvisorVideoPost (InvestmentAdvisorVideoRequest body, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="onlyLastVideo"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void InvestmentAdvisorVideosGet (bool? onlyLastVideo, string apiVersion);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class InvestmentAdvisorApi : IInvestmentAdvisorApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvestmentAdvisorApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public InvestmentAdvisorApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="InvestmentAdvisorApi"/> class.
        /// </summary>
        /// <returns></returns>
        public InvestmentAdvisorApi(String basePath)
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
        public void InvestmentAdvisorFreeRecommendationsGet (string apiVersion)
        {
    
            var path = "/InvestmentAdvisor/free-recommendations";
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
                throw new ApiException ((int)response.StatusCode, "Error calling InvestmentAdvisorFreeRecommendationsGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling InvestmentAdvisorFreeRecommendationsGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="investmentAdvisorFreeRecommendationID"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void InvestmentAdvisorFreeRecommendationsInvestmentAdvisorFreeRecommendationIDDelete (long? investmentAdvisorFreeRecommendationID, string apiVersion)
        {
            // verify the required parameter 'investmentAdvisorFreeRecommendationID' is set
            if (investmentAdvisorFreeRecommendationID == null) throw new ApiException(400, "Missing required parameter 'investmentAdvisorFreeRecommendationID' when calling InvestmentAdvisorFreeRecommendationsInvestmentAdvisorFreeRecommendationIDDelete");
    
            var path = "/InvestmentAdvisor/free-recommendations/{investmentAdvisorFreeRecommendationID}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "investmentAdvisorFreeRecommendationID" + "}", ApiClient.ParameterToString(investmentAdvisorFreeRecommendationID));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling InvestmentAdvisorFreeRecommendationsInvestmentAdvisorFreeRecommendationIDDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling InvestmentAdvisorFreeRecommendationsInvestmentAdvisorFreeRecommendationIDDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void InvestmentAdvisorFreeRecommendationsPost (FreeRecommendationRequest body, string apiVersion)
        {
    
            var path = "/InvestmentAdvisor/free-recommendations";
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
                throw new ApiException ((int)response.StatusCode, "Error calling InvestmentAdvisorFreeRecommendationsPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling InvestmentAdvisorFreeRecommendationsPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void InvestmentAdvisorVideoPost (InvestmentAdvisorVideoRequest body, string apiVersion)
        {
    
            var path = "/InvestmentAdvisor/video";
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
                throw new ApiException ((int)response.StatusCode, "Error calling InvestmentAdvisorVideoPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling InvestmentAdvisorVideoPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="onlyLastVideo"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void InvestmentAdvisorVideosGet (bool? onlyLastVideo, string apiVersion)
        {
    
            var path = "/InvestmentAdvisor/videos";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (onlyLastVideo != null) queryParams.Add("onlyLastVideo", ApiClient.ParameterToString(onlyLastVideo)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling InvestmentAdvisorVideosGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling InvestmentAdvisorVideosGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}

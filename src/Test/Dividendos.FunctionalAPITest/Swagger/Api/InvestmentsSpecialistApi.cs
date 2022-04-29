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
    public interface IInvestmentsSpecialistApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void InvestmentsSpecialistGet (string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="suggestedPortfolioID"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void InvestmentsSpecialistPortfolioSuggestedPortfolioIDGet (string suggestedPortfolioID, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="specialistID"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void InvestmentsSpecialistSpecialistIDPortfolioGet (string specialistID, string apiVersion);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class InvestmentsSpecialistApi : IInvestmentsSpecialistApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvestmentsSpecialistApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public InvestmentsSpecialistApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="InvestmentsSpecialistApi"/> class.
        /// </summary>
        /// <returns></returns>
        public InvestmentsSpecialistApi(String basePath)
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
        public void InvestmentsSpecialistGet (string apiVersion)
        {
    
            var path = "/InvestmentsSpecialist";
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
                throw new ApiException ((int)response.StatusCode, "Error calling InvestmentsSpecialistGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling InvestmentsSpecialistGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="suggestedPortfolioID"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void InvestmentsSpecialistPortfolioSuggestedPortfolioIDGet (string suggestedPortfolioID, string apiVersion)
        {
            // verify the required parameter 'suggestedPortfolioID' is set
            if (suggestedPortfolioID == null) throw new ApiException(400, "Missing required parameter 'suggestedPortfolioID' when calling InvestmentsSpecialistPortfolioSuggestedPortfolioIDGet");
    
            var path = "/InvestmentsSpecialist/portfolio/{suggestedPortfolioID}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "suggestedPortfolioID" + "}", ApiClient.ParameterToString(suggestedPortfolioID));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling InvestmentsSpecialistPortfolioSuggestedPortfolioIDGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling InvestmentsSpecialistPortfolioSuggestedPortfolioIDGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="specialistID"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void InvestmentsSpecialistSpecialistIDPortfolioGet (string specialistID, string apiVersion)
        {
            // verify the required parameter 'specialistID' is set
            if (specialistID == null) throw new ApiException(400, "Missing required parameter 'specialistID' when calling InvestmentsSpecialistSpecialistIDPortfolioGet");
    
            var path = "/InvestmentsSpecialist/{specialistID}/portfolio";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "specialistID" + "}", ApiClient.ParameterToString(specialistID));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling InvestmentsSpecialistSpecialistIDPortfolioGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling InvestmentsSpecialistSpecialistIDPortfolioGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}

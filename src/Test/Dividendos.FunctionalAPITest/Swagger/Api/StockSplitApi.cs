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
    public interface IStockSplitApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="stockGuid"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void StockSplitByStockGet (Guid? stockGuid, DateTime? startDate, DateTime? endDate, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void StockSplitGet (string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidOperation"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void StockSplitGuidOperationApplystocksplitPut (Guid? guidOperation, OperationEditAvgPriceVM body, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidOperation"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void StockSplitGuidOperationDiscardstocksplitPut (Guid? guidOperation, string apiVersion);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class StockSplitApi : IStockSplitApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StockSplitApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public StockSplitApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="StockSplitApi"/> class.
        /// </summary>
        /// <returns></returns>
        public StockSplitApi(String basePath)
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
        /// <param name="stockGuid"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void StockSplitByStockGet (Guid? stockGuid, DateTime? startDate, DateTime? endDate, string apiVersion)
        {
    
            var path = "/StockSplit/byStock";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (stockGuid != null) queryParams.Add("stockGuid", ApiClient.ParameterToString(stockGuid)); // query parameter
 if (startDate != null) queryParams.Add("startDate", ApiClient.ParameterToString(startDate)); // query parameter
 if (endDate != null) queryParams.Add("endDate", ApiClient.ParameterToString(endDate)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StockSplitByStockGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StockSplitByStockGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void StockSplitGet (string apiVersion)
        {
    
            var path = "/StockSplit";
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
                throw new ApiException ((int)response.StatusCode, "Error calling StockSplitGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StockSplitGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidOperation"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void StockSplitGuidOperationApplystocksplitPut (Guid? guidOperation, OperationEditAvgPriceVM body, string apiVersion)
        {
            // verify the required parameter 'guidOperation' is set
            if (guidOperation == null) throw new ApiException(400, "Missing required parameter 'guidOperation' when calling StockSplitGuidOperationApplystocksplitPut");
    
            var path = "/StockSplit/{guidOperation}/applystocksplit";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidOperation" + "}", ApiClient.ParameterToString(guidOperation));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling StockSplitGuidOperationApplystocksplitPut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StockSplitGuidOperationApplystocksplitPut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidOperation"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void StockSplitGuidOperationDiscardstocksplitPut (Guid? guidOperation, string apiVersion)
        {
            // verify the required parameter 'guidOperation' is set
            if (guidOperation == null) throw new ApiException(400, "Missing required parameter 'guidOperation' when calling StockSplitGuidOperationDiscardstocksplitPut");
    
            var path = "/StockSplit/{guidOperation}/discardstocksplit";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidOperation" + "}", ApiClient.ParameterToString(guidOperation));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling StockSplitGuidOperationDiscardstocksplitPut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StockSplitGuidOperationDiscardstocksplitPut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}

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
    public interface IOperationApi
    {
        /// <summary>
        /// Add Buy Operation 
        /// </summary>
        /// <param name="guidPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void OperationBuyoperationGuidPortfolioPost (Guid? guidPortfolio, OperationAddVM body, string apiVersion);
        /// <summary>
        /// Edit Buy Operation 
        /// </summary>
        /// <param name="guidPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void OperationEditbuyoperationGuidPortfolioPut (Guid? guidPortfolio, OperationEditVM body, string apiVersion);
        /// <summary>
        /// Edit Sell Operation 
        /// </summary>
        /// <param name="guidPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void OperationEditselloperationGuidPortfolioPut (Guid? guidPortfolio, OperationEditVM body, string apiVersion);
        /// <summary>
        /// Edit Sell Operation Item 
        /// </summary>
        /// <param name="guidOperationItem"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void OperationEditselloperationitemGuidOperationItemPut (Guid? guidOperationItem, OperationItemEditVM body, string apiVersion);
        /// <summary>
        /// Edit Average Price 
        /// </summary>
        /// <param name="guidOperation"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void OperationGuidOperationOperationPut (Guid? guidOperation, OperationEditAvgPriceVM body, string apiVersion);
        /// <summary>
        /// Inactive Operation 
        /// </summary>
        /// <param name="guidPortfolio"></param>
        /// <param name="idOperationItem"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void OperationGuidPortfolioOperationitemIdOperationItemDelete (Guid? guidPortfolio, long? idOperationItem, string apiVersion);
        /// <summary>
        /// Get Operation Items 
        /// </summary>
        /// <param name="guidPortfolio"></param>
        /// <param name="idStock"></param>
        /// <param name="idOperationType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void OperationGuidPortfolioOperationitemIdStockIdOperationTypeGet (Guid? guidPortfolio, long? idStock, int? idOperationType, string apiVersion);
        /// <summary>
        /// Get Operation Buy Items 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void OperationOperationBuyDetailsGuidPortfolioSubGet (Guid? guidPortfolioSub, string startDate, string endDate, string apiVersion);
        /// <summary>
        /// Get Operation Sell Items 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void OperationOperationSellDetailsGuidPortfolioSubGet (Guid? guidPortfolioSub, string startDate, string endDate, string apiVersion);
        /// <summary>
        /// Inactive Operation Item 
        /// </summary>
        /// <param name="guidOperationItem"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void OperationOperationitemGuidOperationItemDelete (Guid? guidOperationItem, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void OperationOperationsummaryGet (string apiVersion);
        /// <summary>
        /// Add Sell Operation 
        /// </summary>
        /// <param name="guidPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void OperationSelloperationGuidPortfolioPost (Guid? guidPortfolio, OperationAddVM body, string apiVersion);
        /// <summary>
        /// Add Sell Operation 
        /// </summary>
        /// <param name="guidPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void OperationSelloperationitemGuidPortfolioPost (Guid? guidPortfolio, OperationItemAddVM body, string apiVersion);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class OperationApi : IOperationApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public OperationApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationApi"/> class.
        /// </summary>
        /// <returns></returns>
        public OperationApi(String basePath)
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
        /// Add Buy Operation 
        /// </summary>
        /// <param name="guidPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void OperationBuyoperationGuidPortfolioPost (Guid? guidPortfolio, OperationAddVM body, string apiVersion)
        {
            // verify the required parameter 'guidPortfolio' is set
            if (guidPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidPortfolio' when calling OperationBuyoperationGuidPortfolioPost");
    
            var path = "/Operation/buyoperation/{guidPortfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolio" + "}", ApiClient.ParameterToString(guidPortfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OperationBuyoperationGuidPortfolioPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OperationBuyoperationGuidPortfolioPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Edit Buy Operation 
        /// </summary>
        /// <param name="guidPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void OperationEditbuyoperationGuidPortfolioPut (Guid? guidPortfolio, OperationEditVM body, string apiVersion)
        {
            // verify the required parameter 'guidPortfolio' is set
            if (guidPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidPortfolio' when calling OperationEditbuyoperationGuidPortfolioPut");
    
            var path = "/Operation/editbuyoperation/{guidPortfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolio" + "}", ApiClient.ParameterToString(guidPortfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OperationEditbuyoperationGuidPortfolioPut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OperationEditbuyoperationGuidPortfolioPut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Edit Sell Operation 
        /// </summary>
        /// <param name="guidPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void OperationEditselloperationGuidPortfolioPut (Guid? guidPortfolio, OperationEditVM body, string apiVersion)
        {
            // verify the required parameter 'guidPortfolio' is set
            if (guidPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidPortfolio' when calling OperationEditselloperationGuidPortfolioPut");
    
            var path = "/Operation/editselloperation/{guidPortfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolio" + "}", ApiClient.ParameterToString(guidPortfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OperationEditselloperationGuidPortfolioPut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OperationEditselloperationGuidPortfolioPut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Edit Sell Operation Item 
        /// </summary>
        /// <param name="guidOperationItem"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void OperationEditselloperationitemGuidOperationItemPut (Guid? guidOperationItem, OperationItemEditVM body, string apiVersion)
        {
            // verify the required parameter 'guidOperationItem' is set
            if (guidOperationItem == null) throw new ApiException(400, "Missing required parameter 'guidOperationItem' when calling OperationEditselloperationitemGuidOperationItemPut");
    
            var path = "/Operation/editselloperationitem/{guidOperationItem}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidOperationItem" + "}", ApiClient.ParameterToString(guidOperationItem));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OperationEditselloperationitemGuidOperationItemPut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OperationEditselloperationitemGuidOperationItemPut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Edit Average Price 
        /// </summary>
        /// <param name="guidOperation"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void OperationGuidOperationOperationPut (Guid? guidOperation, OperationEditAvgPriceVM body, string apiVersion)
        {
            // verify the required parameter 'guidOperation' is set
            if (guidOperation == null) throw new ApiException(400, "Missing required parameter 'guidOperation' when calling OperationGuidOperationOperationPut");
    
            var path = "/Operation/{guidOperation}/operation";
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
                throw new ApiException ((int)response.StatusCode, "Error calling OperationGuidOperationOperationPut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OperationGuidOperationOperationPut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Inactive Operation 
        /// </summary>
        /// <param name="guidPortfolio"></param>
        /// <param name="idOperationItem"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void OperationGuidPortfolioOperationitemIdOperationItemDelete (Guid? guidPortfolio, long? idOperationItem, string apiVersion)
        {
            // verify the required parameter 'guidPortfolio' is set
            if (guidPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidPortfolio' when calling OperationGuidPortfolioOperationitemIdOperationItemDelete");
            // verify the required parameter 'idOperationItem' is set
            if (idOperationItem == null) throw new ApiException(400, "Missing required parameter 'idOperationItem' when calling OperationGuidPortfolioOperationitemIdOperationItemDelete");
    
            var path = "/Operation/{guidPortfolio}/operationitem/{idOperationItem}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolio" + "}", ApiClient.ParameterToString(guidPortfolio));
path = path.Replace("{" + "idOperationItem" + "}", ApiClient.ParameterToString(idOperationItem));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OperationGuidPortfolioOperationitemIdOperationItemDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OperationGuidPortfolioOperationitemIdOperationItemDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get Operation Items 
        /// </summary>
        /// <param name="guidPortfolio"></param>
        /// <param name="idStock"></param>
        /// <param name="idOperationType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void OperationGuidPortfolioOperationitemIdStockIdOperationTypeGet (Guid? guidPortfolio, long? idStock, int? idOperationType, string apiVersion)
        {
            // verify the required parameter 'guidPortfolio' is set
            if (guidPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidPortfolio' when calling OperationGuidPortfolioOperationitemIdStockIdOperationTypeGet");
            // verify the required parameter 'idStock' is set
            if (idStock == null) throw new ApiException(400, "Missing required parameter 'idStock' when calling OperationGuidPortfolioOperationitemIdStockIdOperationTypeGet");
            // verify the required parameter 'idOperationType' is set
            if (idOperationType == null) throw new ApiException(400, "Missing required parameter 'idOperationType' when calling OperationGuidPortfolioOperationitemIdStockIdOperationTypeGet");
    
            var path = "/Operation/{guidPortfolio}/operationitem/{idStock}/{idOperationType}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolio" + "}", ApiClient.ParameterToString(guidPortfolio));
path = path.Replace("{" + "idStock" + "}", ApiClient.ParameterToString(idStock));
path = path.Replace("{" + "idOperationType" + "}", ApiClient.ParameterToString(idOperationType));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OperationGuidPortfolioOperationitemIdStockIdOperationTypeGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OperationGuidPortfolioOperationitemIdStockIdOperationTypeGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get Operation Buy Items 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void OperationOperationBuyDetailsGuidPortfolioSubGet (Guid? guidPortfolioSub, string startDate, string endDate, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling OperationOperationBuyDetailsGuidPortfolioSubGet");
    
            var path = "/Operation/operationBuyDetails/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OperationOperationBuyDetailsGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OperationOperationBuyDetailsGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get Operation Sell Items 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void OperationOperationSellDetailsGuidPortfolioSubGet (Guid? guidPortfolioSub, string startDate, string endDate, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling OperationOperationSellDetailsGuidPortfolioSubGet");
    
            var path = "/Operation/operationSellDetails/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OperationOperationSellDetailsGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OperationOperationSellDetailsGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Inactive Operation Item 
        /// </summary>
        /// <param name="guidOperationItem"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void OperationOperationitemGuidOperationItemDelete (Guid? guidOperationItem, string apiVersion)
        {
            // verify the required parameter 'guidOperationItem' is set
            if (guidOperationItem == null) throw new ApiException(400, "Missing required parameter 'guidOperationItem' when calling OperationOperationitemGuidOperationItemDelete");
    
            var path = "/Operation/operationitem/{guidOperationItem}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidOperationItem" + "}", ApiClient.ParameterToString(guidOperationItem));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OperationOperationitemGuidOperationItemDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OperationOperationitemGuidOperationItemDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void OperationOperationsummaryGet (string apiVersion)
        {
    
            var path = "/Operation/operationsummary";
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
                throw new ApiException ((int)response.StatusCode, "Error calling OperationOperationsummaryGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OperationOperationsummaryGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Add Sell Operation 
        /// </summary>
        /// <param name="guidPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void OperationSelloperationGuidPortfolioPost (Guid? guidPortfolio, OperationAddVM body, string apiVersion)
        {
            // verify the required parameter 'guidPortfolio' is set
            if (guidPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidPortfolio' when calling OperationSelloperationGuidPortfolioPost");
    
            var path = "/Operation/selloperation/{guidPortfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolio" + "}", ApiClient.ParameterToString(guidPortfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OperationSelloperationGuidPortfolioPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OperationSelloperationGuidPortfolioPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Add Sell Operation 
        /// </summary>
        /// <param name="guidPortfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void OperationSelloperationitemGuidPortfolioPost (Guid? guidPortfolio, OperationItemAddVM body, string apiVersion)
        {
            // verify the required parameter 'guidPortfolio' is set
            if (guidPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidPortfolio' when calling OperationSelloperationitemGuidPortfolioPost");
    
            var path = "/Operation/selloperationitem/{guidPortfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolio" + "}", ApiClient.ParameterToString(guidPortfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OperationSelloperationitemGuidPortfolioPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OperationSelloperationitemGuidPortfolioPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}

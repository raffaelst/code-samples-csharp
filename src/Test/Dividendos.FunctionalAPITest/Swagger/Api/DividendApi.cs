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
    public interface IDividendApi
    {
        /// <summary>
        /// Get Dividend Details 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void DividendDividenddetailsGuidPortfolioSubGet (Guid? guidPortfolioSub, string startDate, string endDate, string apiVersion);
        /// <summary>
        /// Get Dividend List 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="idStock"></param>
        /// <param name="idStockType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void DividendDividendlistGuidPortfolioSubGet (Guid? guidPortfolioSub, int? year, int? month, long? idStock, int? idStockType, string apiVersion);
        /// <summary>
        /// Get Dividend Yield List 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="idStock"></param>
        /// <param name="idStockType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void DividendDividendyieldlistGuidPortfolioSubGet (Guid? guidPortfolioSub, string startDate, string endDate, long? idStock, int? idStockType, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void DividendGet (string apiVersion);
        /// <summary>
        /// Delete a Dividend 
        /// </summary>
        /// <param name="idDividend"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void DividendIdDividendDelete (long? idDividend, string apiVersion);
        /// <summary>
        /// Edit Dividend 
        /// </summary>
        /// <param name="idDividend"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void DividendIdDividendPut (long? idDividend, DividendEditVM body, string apiVersion);
        /// <summary>
        /// Create a new Dividend 
        /// </summary>
        /// <param name="idStock"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void DividendIdStockPost (long? idStock, DividendAddVM body, string apiVersion);
        /// <summary>
        /// Get Ranking Div Yield 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="idStock"></param>
        /// <param name="idStockType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void DividendRankingdividendyieldGuidPortfolioSubGet (Guid? guidPortfolioSub, int? year, int? month, long? idStock, int? idStockType, string apiVersion);
        /// <summary>
        /// Restore Dividends 
        /// </summary>
        /// <param name="guidportfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void DividendRestoredividendsGuidportfolioPut (Guid? guidportfolio, string apiVersion);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class DividendApi : IDividendApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DividendApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public DividendApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="DividendApi"/> class.
        /// </summary>
        /// <returns></returns>
        public DividendApi(String basePath)
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
        /// Get Dividend Details 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void DividendDividenddetailsGuidPortfolioSubGet (Guid? guidPortfolioSub, string startDate, string endDate, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling DividendDividenddetailsGuidPortfolioSubGet");
    
            var path = "/Dividend/dividenddetails/{guidPortfolioSub}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DividendDividenddetailsGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendDividenddetailsGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get Dividend List 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="idStock"></param>
        /// <param name="idStockType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void DividendDividendlistGuidPortfolioSubGet (Guid? guidPortfolioSub, int? year, int? month, long? idStock, int? idStockType, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling DividendDividendlistGuidPortfolioSubGet");
    
            var path = "/Dividend/dividendlist/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (year != null) queryParams.Add("year", ApiClient.ParameterToString(year)); // query parameter
 if (month != null) queryParams.Add("month", ApiClient.ParameterToString(month)); // query parameter
 if (idStock != null) queryParams.Add("idStock", ApiClient.ParameterToString(idStock)); // query parameter
 if (idStockType != null) queryParams.Add("idStockType", ApiClient.ParameterToString(idStockType)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendDividendlistGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendDividendlistGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get Dividend Yield List 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="idStock"></param>
        /// <param name="idStockType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void DividendDividendyieldlistGuidPortfolioSubGet (Guid? guidPortfolioSub, string startDate, string endDate, long? idStock, int? idStockType, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling DividendDividendyieldlistGuidPortfolioSubGet");
    
            var path = "/Dividend/dividendyieldlist/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (startDate != null) queryParams.Add("startDate", ApiClient.ParameterToString(startDate)); // query parameter
 if (endDate != null) queryParams.Add("endDate", ApiClient.ParameterToString(endDate)); // query parameter
 if (idStock != null) queryParams.Add("idStock", ApiClient.ParameterToString(idStock)); // query parameter
 if (idStockType != null) queryParams.Add("idStockType", ApiClient.ParameterToString(idStockType)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendDividendyieldlistGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendDividendyieldlistGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void DividendGet (string apiVersion)
        {
    
            var path = "/Dividend";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DividendGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete a Dividend 
        /// </summary>
        /// <param name="idDividend"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void DividendIdDividendDelete (long? idDividend, string apiVersion)
        {
            // verify the required parameter 'idDividend' is set
            if (idDividend == null) throw new ApiException(400, "Missing required parameter 'idDividend' when calling DividendIdDividendDelete");
    
            var path = "/Dividend/{idDividend}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "idDividend" + "}", ApiClient.ParameterToString(idDividend));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DividendIdDividendDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendIdDividendDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Edit Dividend 
        /// </summary>
        /// <param name="idDividend"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void DividendIdDividendPut (long? idDividend, DividendEditVM body, string apiVersion)
        {
            // verify the required parameter 'idDividend' is set
            if (idDividend == null) throw new ApiException(400, "Missing required parameter 'idDividend' when calling DividendIdDividendPut");
    
            var path = "/Dividend/{idDividend}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "idDividend" + "}", ApiClient.ParameterToString(idDividend));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DividendIdDividendPut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendIdDividendPut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Create a new Dividend 
        /// </summary>
        /// <param name="idStock"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void DividendIdStockPost (long? idStock, DividendAddVM body, string apiVersion)
        {
            // verify the required parameter 'idStock' is set
            if (idStock == null) throw new ApiException(400, "Missing required parameter 'idStock' when calling DividendIdStockPost");
    
            var path = "/Dividend/{idStock}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "idStock" + "}", ApiClient.ParameterToString(idStock));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DividendIdStockPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendIdStockPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get Ranking Div Yield 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="idStock"></param>
        /// <param name="idStockType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void DividendRankingdividendyieldGuidPortfolioSubGet (Guid? guidPortfolioSub, int? year, int? month, long? idStock, int? idStockType, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling DividendRankingdividendyieldGuidPortfolioSubGet");
    
            var path = "/Dividend/rankingdividendyield/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (year != null) queryParams.Add("year", ApiClient.ParameterToString(year)); // query parameter
 if (month != null) queryParams.Add("month", ApiClient.ParameterToString(month)); // query parameter
 if (idStock != null) queryParams.Add("idStock", ApiClient.ParameterToString(idStock)); // query parameter
 if (idStockType != null) queryParams.Add("idStockType", ApiClient.ParameterToString(idStockType)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendRankingdividendyieldGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendRankingdividendyieldGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Restore Dividends 
        /// </summary>
        /// <param name="guidportfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void DividendRestoredividendsGuidportfolioPut (Guid? guidportfolio, string apiVersion)
        {
            // verify the required parameter 'guidportfolio' is set
            if (guidportfolio == null) throw new ApiException(400, "Missing required parameter 'guidportfolio' when calling DividendRestoredividendsGuidportfolioPut");
    
            var path = "/Dividend/restoredividends/{guidportfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidportfolio" + "}", ApiClient.ParameterToString(guidportfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DividendRestoredividendsGuidportfolioPut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendRestoredividendsGuidportfolioPut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}

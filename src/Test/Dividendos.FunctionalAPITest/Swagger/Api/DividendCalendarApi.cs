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
    public interface IDividendCalendarApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void DividendCalendarDatacomGet (string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="year"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void DividendCalendarGet (int? year, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="year"></param>
        /// <param name="mouth"></param>
        /// <param name="countryType"></param>
        /// <param name="dividendCalendarType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void DividendCalendarListGet (int? year, int? mouth, CountryType countryType, DividendCalendarType dividendCalendarType, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="year"></param>
        /// <param name="mouth"></param>
        /// <param name="countryType"></param>
        /// <param name="dividendCalendarType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void DividendCalendarListOnlyMyStocksGet (int? year, int? mouth, CountryType countryType, DividendCalendarType dividendCalendarType, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="year"></param>
        /// <param name="mouth"></param>
        /// <param name="countryType"></param>
        /// <param name="dividendCalendarType"></param>
        /// <param name="stockTypes"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void DividendCalendarListStockTypeGet (int? year, int? mouth, CountryType countryType, DividendCalendarType dividendCalendarType, List<StockType> stockTypes, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="dividendCalendarType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void DividendCalendarListSymbolDetailGet (string symbol, DateTime? startDate, DateTime? endDate, DividendCalendarType dividendCalendarType, string apiVersion);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class DividendCalendarApi : IDividendCalendarApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DividendCalendarApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public DividendCalendarApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="DividendCalendarApi"/> class.
        /// </summary>
        /// <returns></returns>
        public DividendCalendarApi(String basePath)
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
        public void DividendCalendarDatacomGet (string apiVersion)
        {
    
            var path = "/DividendCalendar/datacom";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DividendCalendarDatacomGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendCalendarDatacomGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="year"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void DividendCalendarGet (int? year, string apiVersion)
        {
    
            var path = "/DividendCalendar";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (year != null) queryParams.Add("year", ApiClient.ParameterToString(year)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendCalendarGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendCalendarGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="year"></param>
        /// <param name="mouth"></param>
        /// <param name="countryType"></param>
        /// <param name="dividendCalendarType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void DividendCalendarListGet (int? year, int? mouth, CountryType countryType, DividendCalendarType dividendCalendarType, string apiVersion)
        {
    
            var path = "/DividendCalendar/list";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (year != null) queryParams.Add("year", ApiClient.ParameterToString(year)); // query parameter
 if (mouth != null) queryParams.Add("mouth", ApiClient.ParameterToString(mouth)); // query parameter
 if (countryType != null) queryParams.Add("countryType", ApiClient.ParameterToString(countryType)); // query parameter
 if (dividendCalendarType != null) queryParams.Add("dividendCalendarType", ApiClient.ParameterToString(dividendCalendarType)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendCalendarListGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendCalendarListGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="year"></param>
        /// <param name="mouth"></param>
        /// <param name="countryType"></param>
        /// <param name="dividendCalendarType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void DividendCalendarListOnlyMyStocksGet (int? year, int? mouth, CountryType countryType, DividendCalendarType dividendCalendarType, string apiVersion)
        {
    
            var path = "/DividendCalendar/list-only-my-stocks";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (year != null) queryParams.Add("year", ApiClient.ParameterToString(year)); // query parameter
 if (mouth != null) queryParams.Add("mouth", ApiClient.ParameterToString(mouth)); // query parameter
 if (countryType != null) queryParams.Add("countryType", ApiClient.ParameterToString(countryType)); // query parameter
 if (dividendCalendarType != null) queryParams.Add("dividendCalendarType", ApiClient.ParameterToString(dividendCalendarType)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendCalendarListOnlyMyStocksGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendCalendarListOnlyMyStocksGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="year"></param>
        /// <param name="mouth"></param>
        /// <param name="countryType"></param>
        /// <param name="dividendCalendarType"></param>
        /// <param name="stockTypes"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void DividendCalendarListStockTypeGet (int? year, int? mouth, CountryType countryType, DividendCalendarType dividendCalendarType, List<StockType> stockTypes, string apiVersion)
        {
    
            var path = "/DividendCalendar/list-stock-type";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (year != null) queryParams.Add("year", ApiClient.ParameterToString(year)); // query parameter
 if (mouth != null) queryParams.Add("mouth", ApiClient.ParameterToString(mouth)); // query parameter
 if (countryType != null) queryParams.Add("countryType", ApiClient.ParameterToString(countryType)); // query parameter
 if (dividendCalendarType != null) queryParams.Add("dividendCalendarType", ApiClient.ParameterToString(dividendCalendarType)); // query parameter
 if (stockTypes != null) queryParams.Add("stockTypes", ApiClient.ParameterToString(stockTypes)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendCalendarListStockTypeGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendCalendarListStockTypeGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="dividendCalendarType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void DividendCalendarListSymbolDetailGet (string symbol, DateTime? startDate, DateTime? endDate, DividendCalendarType dividendCalendarType, string apiVersion)
        {
            // verify the required parameter 'symbol' is set
            if (symbol == null) throw new ApiException(400, "Missing required parameter 'symbol' when calling DividendCalendarListSymbolDetailGet");
    
            var path = "/DividendCalendar/list/{symbol}/detail";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "symbol" + "}", ApiClient.ParameterToString(symbol));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (startDate != null) queryParams.Add("startDate", ApiClient.ParameterToString(startDate)); // query parameter
 if (endDate != null) queryParams.Add("endDate", ApiClient.ParameterToString(endDate)); // query parameter
 if (dividendCalendarType != null) queryParams.Add("dividendCalendarType", ApiClient.ParameterToString(dividendCalendarType)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendCalendarListSymbolDetailGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DividendCalendarListSymbolDetailGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}

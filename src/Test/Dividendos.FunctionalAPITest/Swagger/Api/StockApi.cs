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
    public interface IStockApi
    {
        /// <summary>
        /// Get stock by symbol 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void StockBysymbolSymbolGet (string symbol, string apiVersion);
        /// <summary>
        /// Get stock by symbol, by country 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="idcountry"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void StockBysymbolcountrySymbolIdcountryGet (string symbol, int? idcountry, string apiVersion);
        /// <summary>
        /// Get all stock by user&#x27;s portfolios 
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void StockGet (string apiVersion);
        /// <summary>
        /// Import St Inv Stocks 
        /// </summary>
        /// <param name="idstocktype"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void StockImportstinvstocksIdstocktypeGet (int? idstocktype, string apiVersion);
        /// <summary>
        /// Import Usa Stocks 
        /// </summary>
        /// <param name="idstocktype"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void StockImportusastocksIdstocktypeGet (int? idstocktype, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="makertMoversType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void StockMarketMoversGet (MakertMoversType makertMoversType, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="countryType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void StockMilkingCowsGet (CountryType countryType, string apiVersion);
        /// <summary>
        /// Sync Stock Price 
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void StockSyncstockpriceGet (string apiVersion);
        /// <summary>
        /// Sync Stock Price 
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void StockSyncusastockpriceGet (string apiVersion);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class StockApi : IStockApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StockApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public StockApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="StockApi"/> class.
        /// </summary>
        /// <returns></returns>
        public StockApi(String basePath)
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
        /// Get stock by symbol 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void StockBysymbolSymbolGet (string symbol, string apiVersion)
        {
            // verify the required parameter 'symbol' is set
            if (symbol == null) throw new ApiException(400, "Missing required parameter 'symbol' when calling StockBysymbolSymbolGet");
    
            var path = "/Stock/bysymbol/{symbol}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "symbol" + "}", ApiClient.ParameterToString(symbol));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling StockBysymbolSymbolGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StockBysymbolSymbolGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get stock by symbol, by country 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="idcountry"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void StockBysymbolcountrySymbolIdcountryGet (string symbol, int? idcountry, string apiVersion)
        {
            // verify the required parameter 'symbol' is set
            if (symbol == null) throw new ApiException(400, "Missing required parameter 'symbol' when calling StockBysymbolcountrySymbolIdcountryGet");
            // verify the required parameter 'idcountry' is set
            if (idcountry == null) throw new ApiException(400, "Missing required parameter 'idcountry' when calling StockBysymbolcountrySymbolIdcountryGet");
    
            var path = "/Stock/bysymbolcountry/{symbol}/{idcountry}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "symbol" + "}", ApiClient.ParameterToString(symbol));
path = path.Replace("{" + "idcountry" + "}", ApiClient.ParameterToString(idcountry));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling StockBysymbolcountrySymbolIdcountryGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StockBysymbolcountrySymbolIdcountryGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get all stock by user&#x27;s portfolios 
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void StockGet (string apiVersion)
        {
    
            var path = "/Stock";
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
                throw new ApiException ((int)response.StatusCode, "Error calling StockGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StockGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Import St Inv Stocks 
        /// </summary>
        /// <param name="idstocktype"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void StockImportstinvstocksIdstocktypeGet (int? idstocktype, string apiVersion)
        {
            // verify the required parameter 'idstocktype' is set
            if (idstocktype == null) throw new ApiException(400, "Missing required parameter 'idstocktype' when calling StockImportstinvstocksIdstocktypeGet");
    
            var path = "/Stock/importstinvstocks/{idstocktype}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "idstocktype" + "}", ApiClient.ParameterToString(idstocktype));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling StockImportstinvstocksIdstocktypeGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StockImportstinvstocksIdstocktypeGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Import Usa Stocks 
        /// </summary>
        /// <param name="idstocktype"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void StockImportusastocksIdstocktypeGet (int? idstocktype, string apiVersion)
        {
            // verify the required parameter 'idstocktype' is set
            if (idstocktype == null) throw new ApiException(400, "Missing required parameter 'idstocktype' when calling StockImportusastocksIdstocktypeGet");
    
            var path = "/Stock/importusastocks/{idstocktype}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "idstocktype" + "}", ApiClient.ParameterToString(idstocktype));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling StockImportusastocksIdstocktypeGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StockImportusastocksIdstocktypeGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="makertMoversType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void StockMarketMoversGet (MakertMoversType makertMoversType, string apiVersion)
        {
    
            var path = "/Stock/market-movers";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (makertMoversType != null) queryParams.Add("makertMoversType", ApiClient.ParameterToString(makertMoversType)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StockMarketMoversGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StockMarketMoversGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="countryType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void StockMilkingCowsGet (CountryType countryType, string apiVersion)
        {
    
            var path = "/Stock/milking-cows";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (countryType != null) queryParams.Add("countryType", ApiClient.ParameterToString(countryType)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StockMilkingCowsGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StockMilkingCowsGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Sync Stock Price 
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void StockSyncstockpriceGet (string apiVersion)
        {
    
            var path = "/Stock/syncstockprice";
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
                throw new ApiException ((int)response.StatusCode, "Error calling StockSyncstockpriceGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StockSyncstockpriceGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Sync Stock Price 
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void StockSyncusastockpriceGet (string apiVersion)
        {
    
            var path = "/Stock/syncusastockprice";
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
                throw new ApiException ((int)response.StatusCode, "Error calling StockSyncusastockpriceGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StockSyncusastockpriceGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}

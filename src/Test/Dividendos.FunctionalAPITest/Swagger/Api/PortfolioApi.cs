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
    public interface IPortfolioApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="dateRangeType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioAssetsprogresschartGuidPortfolioSubGet (Guid? guidPortfolioSub, DateRangeType dateRangeType, string startDate, string endDate, string apiVersion);
        /// <summary>
        /// Calculate Portfolio Performance by User 
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioCalculateperformancePut (string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="stockType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioChartSegmentAllocationGuidPortfolioSubStockTypeGet (Guid? guidPortfolioSub, StockType stockType, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="calculateType"></param>
        /// <param name="periodType"></param>
        /// <param name="amountSeries"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioChartcomparativeGuidPortfolioSubGet (Guid? guidPortfolioSub, int? calculateType, int? periodType, int? amountSeries, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioChartcomparativecontainerGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="year"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioChartdividendGuidPortfolioSubByyearYearGet (Guid? guidPortfolioSub, int? year, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioChartdividendGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="year"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioChartdividendscrollGuidPortfolioSubByyearYearGet (Guid? guidPortfolioSub, int? year, string apiVersion);
        /// <summary>
        /// Get Chart Dividends Scroll 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="year"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioChartdividendstackedGuidPortfolioSubByyearYearGet (Guid? guidPortfolioSub, int? year, string apiVersion);
        /// <summary>
        /// Get Chart Performance Stock 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioChartperformancestockGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion);
        /// <summary>
        /// Get Chart Sector Allocation 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioChartsectorallocationGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion);
        /// <summary>
        /// Get Chart Segment Allocation 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioChartsegmentallocationGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion);
        /// <summary>
        /// Get Chart Stock Allocation 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioChartstockallocationGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion);
        /// <summary>
        /// Get Tree Map Chart 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioChartstocktreemapGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion);
        /// <summary>
        /// Get Chart Stock Type 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioChartstocktypeGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion);
        /// <summary>
        /// Get Chart Subsector Allocation 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioChartsubsectorallocationGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion);
        /// <summary>
        /// Get Tree Map Chart 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="sunburstType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioChartsunburstGuidPortfolioSubGet (Guid? guidPortfolioSub, SunburstType sunburstType, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioDividendDetailsGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioDividendspermonthchartGuidPortfolioSubGet (Guid? guidPortfolioSub, string startDate, string endDate, string apiVersion);
        /// <summary>
        /// Force manual import from CEI (Without queue) 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="password"></param>
        /// <param name="idUser"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioForceUpdatePut (string identifier, string password, string idUser, string apiVersion);
        /// <summary>
        /// Get Portfolios 
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioGet (string apiVersion);
        /// <summary>
        /// Get stock statement by idstock 
        /// </summary>
        /// <param name="guidPortfolio"></param>
        /// <param name="idStock"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioGuidPortfolioStockstatementIdStockGet (Guid? guidPortfolio, long? idStock, string apiVersion);
        /// <summary>
        /// Delete a portfolio 
        /// </summary>
        /// <param name="idportfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioIdportfolioDelete (Guid? idportfolio, string apiVersion);
        /// <summary>
        /// Get details of specific portfolio 
        /// </summary>
        /// <param name="idportfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioIdportfolioGet (Guid? idportfolio, string apiVersion);
        /// <summary>
        /// Get the list of subportfolios by portfolio 
        /// </summary>
        /// <param name="idportfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioIdportfolioSubportfolioGet (Guid? idportfolio, string apiVersion);
        /// <summary>
        /// Delete a SubPortfolio 
        /// </summary>
        /// <param name="idportfolio"></param>
        /// <param name="idsubportfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioIdportfolioSubportfolioIdsubportfolioDelete (Guid? idportfolio, Guid? idsubportfolio, string apiVersion);
        /// <summary>
        /// Get details of specific subportfolio 
        /// </summary>
        /// <param name="idportfolio"></param>
        /// <param name="idsubportfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioIdportfolioSubportfolioIdsubportfolioGet (Guid? idportfolio, Guid? idsubportfolio, string apiVersion);
        /// <summary>
        /// Create a new SubPortfolio 
        /// </summary>
        /// <param name="idportfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioIdportfolioSubportfolioPost (Guid? idportfolio, SubPortfolioVM body, string apiVersion);
        /// <summary>
        /// Update Portfolio Name 
        /// </summary>
        /// <param name="idportfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioIdportfolioUpdatenamePut (Guid? idportfolio, PortfolioEditVM body, string apiVersion);
        /// <summary>
        /// Edit a SubPortfolio 
        /// </summary>
        /// <param name="idsubportfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioIdsubportfolioSubportfolioPut (Guid? idsubportfolio, SubPortfolioVM body, string apiVersion);
        /// <summary>
        /// Create a new Manual Portfolio 
        /// </summary>
        /// <param name="portfolioName"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioManualportfolioPortfolioNamePost (string portfolioName, string apiVersion);
        /// <summary>
        /// Create a new Manual Portfolio 
        /// </summary>
        /// <param name="portfolioName"></param>
        /// <param name="idCountry"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioManualportfoliocountryPortfolioNameIdCountryPost (string portfolioName, int? idCountry, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioOperationSellDetailsGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioPortfoliostatementGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="stockType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioPortfoliostatementGuidPortfolioSubStockTypeGet (Guid? guidPortfolioSub, StockType stockType, string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioPortfolioszeropriceGet (string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioPortfolioviewGet (string apiVersion);
        /// <summary>
        /// Add a new principal portfolio. (Import CEI) 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioPost (PortfolioAddVM body, string apiVersion);
        /// <summary>
        /// Update (if necessary) a portfolio using information from CEI 
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioPut (string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioRunperformancePut (string apiVersion);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        void PortfolioSyncceiPut (string apiVersion);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class PortfolioApi : IPortfolioApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PortfolioApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public PortfolioApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="PortfolioApi"/> class.
        /// </summary>
        /// <returns></returns>
        public PortfolioApi(String basePath)
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
        /// <param name="guidPortfolioSub"></param>
        /// <param name="dateRangeType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioAssetsprogresschartGuidPortfolioSubGet (Guid? guidPortfolioSub, DateRangeType dateRangeType, string startDate, string endDate, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioAssetsprogresschartGuidPortfolioSubGet");
    
            var path = "/Portfolio/assetsprogresschart/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (dateRangeType != null) queryParams.Add("dateRangeType", ApiClient.ParameterToString(dateRangeType)); // query parameter
 if (startDate != null) queryParams.Add("startDate", ApiClient.ParameterToString(startDate)); // query parameter
 if (endDate != null) queryParams.Add("endDate", ApiClient.ParameterToString(endDate)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioAssetsprogresschartGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioAssetsprogresschartGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Calculate Portfolio Performance by User 
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioCalculateperformancePut (string apiVersion)
        {
    
            var path = "/Portfolio/calculateperformance";
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
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioCalculateperformancePut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioCalculateperformancePut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="stockType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioChartSegmentAllocationGuidPortfolioSubStockTypeGet (Guid? guidPortfolioSub, StockType stockType, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioChartSegmentAllocationGuidPortfolioSubStockTypeGet");
            // verify the required parameter 'stockType' is set
            if (stockType == null) throw new ApiException(400, "Missing required parameter 'stockType' when calling PortfolioChartSegmentAllocationGuidPortfolioSubStockTypeGet");
    
            var path = "/Portfolio/chart-segment-allocation/{guidPortfolioSub}/{stockType}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
path = path.Replace("{" + "stockType" + "}", ApiClient.ParameterToString(stockType));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartSegmentAllocationGuidPortfolioSubStockTypeGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartSegmentAllocationGuidPortfolioSubStockTypeGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="calculateType"></param>
        /// <param name="periodType"></param>
        /// <param name="amountSeries"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioChartcomparativeGuidPortfolioSubGet (Guid? guidPortfolioSub, int? calculateType, int? periodType, int? amountSeries, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioChartcomparativeGuidPortfolioSubGet");
            // verify the required parameter 'calculateType' is set
            if (calculateType == null) throw new ApiException(400, "Missing required parameter 'calculateType' when calling PortfolioChartcomparativeGuidPortfolioSubGet");
            // verify the required parameter 'periodType' is set
            if (periodType == null) throw new ApiException(400, "Missing required parameter 'periodType' when calling PortfolioChartcomparativeGuidPortfolioSubGet");
            // verify the required parameter 'amountSeries' is set
            if (amountSeries == null) throw new ApiException(400, "Missing required parameter 'amountSeries' when calling PortfolioChartcomparativeGuidPortfolioSubGet");
    
            var path = "/Portfolio/chartcomparative/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (calculateType != null) queryParams.Add("calculateType", ApiClient.ParameterToString(calculateType)); // query parameter
 if (periodType != null) queryParams.Add("periodType", ApiClient.ParameterToString(periodType)); // query parameter
 if (amountSeries != null) queryParams.Add("amountSeries", ApiClient.ParameterToString(amountSeries)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartcomparativeGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartcomparativeGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioChartcomparativecontainerGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioChartcomparativecontainerGuidPortfolioSubGet");
    
            var path = "/Portfolio/chartcomparativecontainer/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartcomparativecontainerGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartcomparativecontainerGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="year"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioChartdividendGuidPortfolioSubByyearYearGet (Guid? guidPortfolioSub, int? year, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioChartdividendGuidPortfolioSubByyearYearGet");
            // verify the required parameter 'year' is set
            if (year == null) throw new ApiException(400, "Missing required parameter 'year' when calling PortfolioChartdividendGuidPortfolioSubByyearYearGet");
    
            var path = "/Portfolio/chartdividend/{guidPortfolioSub}/byyear/{year}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
path = path.Replace("{" + "year" + "}", ApiClient.ParameterToString(year));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartdividendGuidPortfolioSubByyearYearGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartdividendGuidPortfolioSubByyearYearGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioChartdividendGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioChartdividendGuidPortfolioSubGet");
    
            var path = "/Portfolio/chartdividend/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartdividendGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartdividendGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="year"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioChartdividendscrollGuidPortfolioSubByyearYearGet (Guid? guidPortfolioSub, int? year, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioChartdividendscrollGuidPortfolioSubByyearYearGet");
            // verify the required parameter 'year' is set
            if (year == null) throw new ApiException(400, "Missing required parameter 'year' when calling PortfolioChartdividendscrollGuidPortfolioSubByyearYearGet");
    
            var path = "/Portfolio/chartdividendscroll/{guidPortfolioSub}/byyear/{year}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
path = path.Replace("{" + "year" + "}", ApiClient.ParameterToString(year));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartdividendscrollGuidPortfolioSubByyearYearGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartdividendscrollGuidPortfolioSubByyearYearGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get Chart Dividends Scroll 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="year"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioChartdividendstackedGuidPortfolioSubByyearYearGet (Guid? guidPortfolioSub, int? year, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioChartdividendstackedGuidPortfolioSubByyearYearGet");
            // verify the required parameter 'year' is set
            if (year == null) throw new ApiException(400, "Missing required parameter 'year' when calling PortfolioChartdividendstackedGuidPortfolioSubByyearYearGet");
    
            var path = "/Portfolio/chartdividendstacked/{guidPortfolioSub}/byyear/{year}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
path = path.Replace("{" + "year" + "}", ApiClient.ParameterToString(year));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartdividendstackedGuidPortfolioSubByyearYearGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartdividendstackedGuidPortfolioSubByyearYearGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get Chart Performance Stock 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioChartperformancestockGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioChartperformancestockGuidPortfolioSubGet");
    
            var path = "/Portfolio/chartperformancestock/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartperformancestockGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartperformancestockGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get Chart Sector Allocation 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioChartsectorallocationGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioChartsectorallocationGuidPortfolioSubGet");
    
            var path = "/Portfolio/chartsectorallocation/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartsectorallocationGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartsectorallocationGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get Chart Segment Allocation 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioChartsegmentallocationGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioChartsegmentallocationGuidPortfolioSubGet");
    
            var path = "/Portfolio/chartsegmentallocation/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartsegmentallocationGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartsegmentallocationGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get Chart Stock Allocation 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioChartstockallocationGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioChartstockallocationGuidPortfolioSubGet");
    
            var path = "/Portfolio/chartstockallocation/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartstockallocationGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartstockallocationGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get Tree Map Chart 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioChartstocktreemapGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioChartstocktreemapGuidPortfolioSubGet");
    
            var path = "/Portfolio/chartstocktreemap/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartstocktreemapGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartstocktreemapGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get Chart Stock Type 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioChartstocktypeGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioChartstocktypeGuidPortfolioSubGet");
    
            var path = "/Portfolio/chartstocktype/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartstocktypeGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartstocktypeGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get Chart Subsector Allocation 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioChartsubsectorallocationGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioChartsubsectorallocationGuidPortfolioSubGet");
    
            var path = "/Portfolio/chartsubsectorallocation/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartsubsectorallocationGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartsubsectorallocationGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get Tree Map Chart 
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="sunburstType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioChartsunburstGuidPortfolioSubGet (Guid? guidPortfolioSub, SunburstType sunburstType, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioChartsunburstGuidPortfolioSubGet");
    
            var path = "/Portfolio/chartsunburst/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (sunburstType != null) queryParams.Add("sunburstType", ApiClient.ParameterToString(sunburstType)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.GET, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartsunburstGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioChartsunburstGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioDividendDetailsGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioDividendDetailsGuidPortfolioSubGet");
    
            var path = "/Portfolio/dividendDetails/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioDividendDetailsGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioDividendDetailsGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioDividendspermonthchartGuidPortfolioSubGet (Guid? guidPortfolioSub, string startDate, string endDate, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioDividendspermonthchartGuidPortfolioSubGet");
    
            var path = "/Portfolio/dividendspermonthchart/{guidPortfolioSub}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioDividendspermonthchartGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioDividendspermonthchartGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Force manual import from CEI (Without queue) 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="password"></param>
        /// <param name="idUser"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioForceUpdatePut (string identifier, string password, string idUser, string apiVersion)
        {
    
            var path = "/Portfolio/force-update";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String PostBody = null;
    
             if (identifier != null) queryParams.Add("identifier", ApiClient.ParameterToString(identifier)); // query parameter
 if (password != null) queryParams.Add("password", ApiClient.ParameterToString(password)); // query parameter
 if (idUser != null) queryParams.Add("idUser", ApiClient.ParameterToString(idUser)); // query parameter
 if (apiVersion != null) queryParams.Add("api-version", ApiClient.ParameterToString(apiVersion)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "Bearer" };
    
            // make the HTTP request
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioForceUpdatePut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioForceUpdatePut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get Portfolios 
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioGet (string apiVersion)
        {
    
            var path = "/Portfolio";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get stock statement by idstock 
        /// </summary>
        /// <param name="guidPortfolio"></param>
        /// <param name="idStock"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioGuidPortfolioStockstatementIdStockGet (Guid? guidPortfolio, long? idStock, string apiVersion)
        {
            // verify the required parameter 'guidPortfolio' is set
            if (guidPortfolio == null) throw new ApiException(400, "Missing required parameter 'guidPortfolio' when calling PortfolioGuidPortfolioStockstatementIdStockGet");
            // verify the required parameter 'idStock' is set
            if (idStock == null) throw new ApiException(400, "Missing required parameter 'idStock' when calling PortfolioGuidPortfolioStockstatementIdStockGet");
    
            var path = "/Portfolio/{guidPortfolio}/stockstatement/{idStock}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolio" + "}", ApiClient.ParameterToString(guidPortfolio));
path = path.Replace("{" + "idStock" + "}", ApiClient.ParameterToString(idStock));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioGuidPortfolioStockstatementIdStockGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioGuidPortfolioStockstatementIdStockGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete a portfolio 
        /// </summary>
        /// <param name="idportfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioIdportfolioDelete (Guid? idportfolio, string apiVersion)
        {
            // verify the required parameter 'idportfolio' is set
            if (idportfolio == null) throw new ApiException(400, "Missing required parameter 'idportfolio' when calling PortfolioIdportfolioDelete");
    
            var path = "/Portfolio/{idportfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "idportfolio" + "}", ApiClient.ParameterToString(idportfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioIdportfolioDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioIdportfolioDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get details of specific portfolio 
        /// </summary>
        /// <param name="idportfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioIdportfolioGet (Guid? idportfolio, string apiVersion)
        {
            // verify the required parameter 'idportfolio' is set
            if (idportfolio == null) throw new ApiException(400, "Missing required parameter 'idportfolio' when calling PortfolioIdportfolioGet");
    
            var path = "/Portfolio/{idportfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "idportfolio" + "}", ApiClient.ParameterToString(idportfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioIdportfolioGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioIdportfolioGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get the list of subportfolios by portfolio 
        /// </summary>
        /// <param name="idportfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioIdportfolioSubportfolioGet (Guid? idportfolio, string apiVersion)
        {
            // verify the required parameter 'idportfolio' is set
            if (idportfolio == null) throw new ApiException(400, "Missing required parameter 'idportfolio' when calling PortfolioIdportfolioSubportfolioGet");
    
            var path = "/Portfolio/{idportfolio}/subportfolio";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "idportfolio" + "}", ApiClient.ParameterToString(idportfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioIdportfolioSubportfolioGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioIdportfolioSubportfolioGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete a SubPortfolio 
        /// </summary>
        /// <param name="idportfolio"></param>
        /// <param name="idsubportfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioIdportfolioSubportfolioIdsubportfolioDelete (Guid? idportfolio, Guid? idsubportfolio, string apiVersion)
        {
            // verify the required parameter 'idportfolio' is set
            if (idportfolio == null) throw new ApiException(400, "Missing required parameter 'idportfolio' when calling PortfolioIdportfolioSubportfolioIdsubportfolioDelete");
            // verify the required parameter 'idsubportfolio' is set
            if (idsubportfolio == null) throw new ApiException(400, "Missing required parameter 'idsubportfolio' when calling PortfolioIdportfolioSubportfolioIdsubportfolioDelete");
    
            var path = "/Portfolio/{idportfolio}/subportfolio/{idsubportfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "idportfolio" + "}", ApiClient.ParameterToString(idportfolio));
path = path.Replace("{" + "idsubportfolio" + "}", ApiClient.ParameterToString(idsubportfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioIdportfolioSubportfolioIdsubportfolioDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioIdportfolioSubportfolioIdsubportfolioDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get details of specific subportfolio 
        /// </summary>
        /// <param name="idportfolio"></param>
        /// <param name="idsubportfolio"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioIdportfolioSubportfolioIdsubportfolioGet (Guid? idportfolio, Guid? idsubportfolio, string apiVersion)
        {
            // verify the required parameter 'idportfolio' is set
            if (idportfolio == null) throw new ApiException(400, "Missing required parameter 'idportfolio' when calling PortfolioIdportfolioSubportfolioIdsubportfolioGet");
            // verify the required parameter 'idsubportfolio' is set
            if (idsubportfolio == null) throw new ApiException(400, "Missing required parameter 'idsubportfolio' when calling PortfolioIdportfolioSubportfolioIdsubportfolioGet");
    
            var path = "/Portfolio/{idportfolio}/subportfolio/{idsubportfolio}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "idportfolio" + "}", ApiClient.ParameterToString(idportfolio));
path = path.Replace("{" + "idsubportfolio" + "}", ApiClient.ParameterToString(idsubportfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioIdportfolioSubportfolioIdsubportfolioGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioIdportfolioSubportfolioIdsubportfolioGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Create a new SubPortfolio 
        /// </summary>
        /// <param name="idportfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioIdportfolioSubportfolioPost (Guid? idportfolio, SubPortfolioVM body, string apiVersion)
        {
            // verify the required parameter 'idportfolio' is set
            if (idportfolio == null) throw new ApiException(400, "Missing required parameter 'idportfolio' when calling PortfolioIdportfolioSubportfolioPost");
    
            var path = "/Portfolio/{idportfolio}/subportfolio";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "idportfolio" + "}", ApiClient.ParameterToString(idportfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioIdportfolioSubportfolioPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioIdportfolioSubportfolioPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Update Portfolio Name 
        /// </summary>
        /// <param name="idportfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioIdportfolioUpdatenamePut (Guid? idportfolio, PortfolioEditVM body, string apiVersion)
        {
            // verify the required parameter 'idportfolio' is set
            if (idportfolio == null) throw new ApiException(400, "Missing required parameter 'idportfolio' when calling PortfolioIdportfolioUpdatenamePut");
    
            var path = "/Portfolio/{idportfolio}/updatename";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "idportfolio" + "}", ApiClient.ParameterToString(idportfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioIdportfolioUpdatenamePut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioIdportfolioUpdatenamePut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Edit a SubPortfolio 
        /// </summary>
        /// <param name="idsubportfolio"></param>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioIdsubportfolioSubportfolioPut (Guid? idsubportfolio, SubPortfolioVM body, string apiVersion)
        {
            // verify the required parameter 'idsubportfolio' is set
            if (idsubportfolio == null) throw new ApiException(400, "Missing required parameter 'idsubportfolio' when calling PortfolioIdsubportfolioSubportfolioPut");
    
            var path = "/Portfolio/{idsubportfolio}/subportfolio";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "idsubportfolio" + "}", ApiClient.ParameterToString(idsubportfolio));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioIdsubportfolioSubportfolioPut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioIdsubportfolioSubportfolioPut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Create a new Manual Portfolio 
        /// </summary>
        /// <param name="portfolioName"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioManualportfolioPortfolioNamePost (string portfolioName, string apiVersion)
        {
            // verify the required parameter 'portfolioName' is set
            if (portfolioName == null) throw new ApiException(400, "Missing required parameter 'portfolioName' when calling PortfolioManualportfolioPortfolioNamePost");
    
            var path = "/Portfolio/manualportfolio/{portfolioName}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "portfolioName" + "}", ApiClient.ParameterToString(portfolioName));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioManualportfolioPortfolioNamePost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioManualportfolioPortfolioNamePost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Create a new Manual Portfolio 
        /// </summary>
        /// <param name="portfolioName"></param>
        /// <param name="idCountry"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioManualportfoliocountryPortfolioNameIdCountryPost (string portfolioName, int? idCountry, string apiVersion)
        {
            // verify the required parameter 'portfolioName' is set
            if (portfolioName == null) throw new ApiException(400, "Missing required parameter 'portfolioName' when calling PortfolioManualportfoliocountryPortfolioNameIdCountryPost");
            // verify the required parameter 'idCountry' is set
            if (idCountry == null) throw new ApiException(400, "Missing required parameter 'idCountry' when calling PortfolioManualportfoliocountryPortfolioNameIdCountryPost");
    
            var path = "/Portfolio/manualportfoliocountry/{portfolioName}/{idCountry}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "portfolioName" + "}", ApiClient.ParameterToString(portfolioName));
path = path.Replace("{" + "idCountry" + "}", ApiClient.ParameterToString(idCountry));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioManualportfoliocountryPortfolioNameIdCountryPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioManualportfoliocountryPortfolioNameIdCountryPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioOperationSellDetailsGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioOperationSellDetailsGuidPortfolioSubGet");
    
            var path = "/Portfolio/operationSellDetails/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioOperationSellDetailsGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioOperationSellDetailsGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioPortfoliostatementGuidPortfolioSubGet (Guid? guidPortfolioSub, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioPortfoliostatementGuidPortfolioSubGet");
    
            var path = "/Portfolio/portfoliostatement/{guidPortfolioSub}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioPortfoliostatementGuidPortfolioSubGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioPortfoliostatementGuidPortfolioSubGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="guidPortfolioSub"></param>
        /// <param name="stockType"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioPortfoliostatementGuidPortfolioSubStockTypeGet (Guid? guidPortfolioSub, StockType stockType, string apiVersion)
        {
            // verify the required parameter 'guidPortfolioSub' is set
            if (guidPortfolioSub == null) throw new ApiException(400, "Missing required parameter 'guidPortfolioSub' when calling PortfolioPortfoliostatementGuidPortfolioSubStockTypeGet");
            // verify the required parameter 'stockType' is set
            if (stockType == null) throw new ApiException(400, "Missing required parameter 'stockType' when calling PortfolioPortfoliostatementGuidPortfolioSubStockTypeGet");
    
            var path = "/Portfolio/portfoliostatement/{guidPortfolioSub}/{stockType}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "guidPortfolioSub" + "}", ApiClient.ParameterToString(guidPortfolioSub));
path = path.Replace("{" + "stockType" + "}", ApiClient.ParameterToString(stockType));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioPortfoliostatementGuidPortfolioSubStockTypeGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioPortfoliostatementGuidPortfolioSubStockTypeGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioPortfolioszeropriceGet (string apiVersion)
        {
    
            var path = "/Portfolio/portfolioszeroprice";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioPortfolioszeropriceGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioPortfolioszeropriceGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioPortfolioviewGet (string apiVersion)
        {
    
            var path = "/Portfolio/portfolioview";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioPortfolioviewGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioPortfolioviewGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Add a new principal portfolio. (Import CEI) 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioPost (PortfolioAddVM body, string apiVersion)
        {
    
            var path = "/Portfolio";
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
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Update (if necessary) a portfolio using information from CEI 
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioPut (string apiVersion)
        {
    
            var path = "/Portfolio";
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
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioPut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioPut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioRunperformancePut (string apiVersion)
        {
    
            var path = "/Portfolio/runperformance";
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
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioRunperformancePut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioRunperformancePut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public void PortfolioSyncceiPut (string apiVersion)
        {
    
            var path = "/Portfolio/synccei";
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
            RestResponse response = (RestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, PostBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioSyncceiPut: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PortfolioSyncceiPut: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}

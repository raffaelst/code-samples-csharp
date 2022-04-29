using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class SettingsEditVM {
    /// <summary>
    /// Gets or Sets SendDailySummaryMail
    /// </summary>
    [DataMember(Name="sendDailySummaryMail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "sendDailySummaryMail")]
    public bool? SendDailySummaryMail { get; set; }

    /// <summary>
    /// Gets or Sets PushNewDividend
    /// </summary>
    [DataMember(Name="pushNewDividend", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pushNewDividend")]
    public bool? PushNewDividend { get; set; }

    /// <summary>
    /// Gets or Sets PushDividendDeposit
    /// </summary>
    [DataMember(Name="pushDividendDeposit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pushDividendDeposit")]
    public bool? PushDividendDeposit { get; set; }

    /// <summary>
    /// Gets or Sets PushChangeInPortfolio
    /// </summary>
    [DataMember(Name="pushChangeInPortfolio", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pushChangeInPortfolio")]
    public bool? PushChangeInPortfolio { get; set; }

    /// <summary>
    /// Gets or Sets AutomaticRefreshPortfolio
    /// </summary>
    [DataMember(Name="automaticRefreshPortfolio", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "automaticRefreshPortfolio")]
    public bool? AutomaticRefreshPortfolio { get; set; }

    /// <summary>
    /// Gets or Sets PushMarketOpeningAndClosing
    /// </summary>
    [DataMember(Name="pushMarketOpeningAndClosing", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pushMarketOpeningAndClosing")]
    public bool? PushMarketOpeningAndClosing { get; set; }

    /// <summary>
    /// Gets or Sets PushBreakingNews
    /// </summary>
    [DataMember(Name="pushBreakingNews", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pushBreakingNews")]
    public bool? PushBreakingNews { get; set; }

    /// <summary>
    /// Gets or Sets PushDataComYourStocks
    /// </summary>
    [DataMember(Name="pushDataComYourStocks", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pushDataComYourStocks")]
    public bool? PushDataComYourStocks { get; set; }

    /// <summary>
    /// Gets or Sets PushStocksWithAwesomeVariation
    /// </summary>
    [DataMember(Name="pushStocksWithAwesomeVariation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pushStocksWithAwesomeVariation")]
    public bool? PushStocksWithAwesomeVariation { get; set; }

    /// <summary>
    /// Gets or Sets PushRelevantFacts
    /// </summary>
    [DataMember(Name="pushRelevantFacts", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pushRelevantFacts")]
    public bool? PushRelevantFacts { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SettingsEditVM {\n");
      sb.Append("  SendDailySummaryMail: ").Append(SendDailySummaryMail).Append("\n");
      sb.Append("  PushNewDividend: ").Append(PushNewDividend).Append("\n");
      sb.Append("  PushDividendDeposit: ").Append(PushDividendDeposit).Append("\n");
      sb.Append("  PushChangeInPortfolio: ").Append(PushChangeInPortfolio).Append("\n");
      sb.Append("  AutomaticRefreshPortfolio: ").Append(AutomaticRefreshPortfolio).Append("\n");
      sb.Append("  PushMarketOpeningAndClosing: ").Append(PushMarketOpeningAndClosing).Append("\n");
      sb.Append("  PushBreakingNews: ").Append(PushBreakingNews).Append("\n");
      sb.Append("  PushDataComYourStocks: ").Append(PushDataComYourStocks).Append("\n");
      sb.Append("  PushStocksWithAwesomeVariation: ").Append(PushStocksWithAwesomeVariation).Append("\n");
      sb.Append("  PushRelevantFacts: ").Append(PushRelevantFacts).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
}

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
  public class FreeRecommendationRequest {
    /// <summary>
    /// Gets or Sets StockGuid
    /// </summary>
    [DataMember(Name="stockGuid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "stockGuid")]
    public string StockGuid { get; set; }

    /// <summary>
    /// Gets or Sets InvestmentAdvisorFreeRecommendationType
    /// </summary>
    [DataMember(Name="investmentAdvisorFreeRecommendationType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "investmentAdvisorFreeRecommendationType")]
    public FreeRecommendationType InvestmentAdvisorFreeRecommendationType { get; set; }

    /// <summary>
    /// Gets or Sets Entry
    /// </summary>
    [DataMember(Name="entry", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "entry")]
    public string Entry { get; set; }

    /// <summary>
    /// Gets or Sets Objective1
    /// </summary>
    [DataMember(Name="objective1", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "objective1")]
    public string Objective1 { get; set; }

    /// <summary>
    /// Gets or Sets Objective2
    /// </summary>
    [DataMember(Name="objective2", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "objective2")]
    public string Objective2 { get; set; }

    /// <summary>
    /// Gets or Sets Stop
    /// </summary>
    [DataMember(Name="stop", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "stop")]
    public string Stop { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FreeRecommendationRequest {\n");
      sb.Append("  StockGuid: ").Append(StockGuid).Append("\n");
      sb.Append("  InvestmentAdvisorFreeRecommendationType: ").Append(InvestmentAdvisorFreeRecommendationType).Append("\n");
      sb.Append("  Entry: ").Append(Entry).Append("\n");
      sb.Append("  Objective1: ").Append(Objective1).Append("\n");
      sb.Append("  Objective2: ").Append(Objective2).Append("\n");
      sb.Append("  Stop: ").Append(Stop).Append("\n");
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

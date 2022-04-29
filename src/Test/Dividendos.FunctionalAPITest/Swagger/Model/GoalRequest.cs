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
  public class GoalRequest {
    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets TotalValue
    /// </summary>
    [DataMember(Name="totalValue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "totalValue")]
    public string TotalValue { get; set; }

    /// <summary>
    /// Gets or Sets PortfolioGuid
    /// </summary>
    [DataMember(Name="portfolioGuid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "portfolioGuid")]
    public string PortfolioGuid { get; set; }

    /// <summary>
    /// Gets or Sets Limit
    /// </summary>
    [DataMember(Name="limit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "limit")]
    public string Limit { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class GoalRequest {\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  TotalValue: ").Append(TotalValue).Append("\n");
      sb.Append("  PortfolioGuid: ").Append(PortfolioGuid).Append("\n");
      sb.Append("  Limit: ").Append(Limit).Append("\n");
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

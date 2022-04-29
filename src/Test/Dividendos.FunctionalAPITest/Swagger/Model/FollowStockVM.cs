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
  public class FollowStockVM {
    /// <summary>
    /// Gets or Sets FollowStockGuid
    /// </summary>
    [DataMember(Name="followStockGuid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "followStockGuid")]
    public Guid? FollowStockGuid { get; set; }

    /// <summary>
    /// Gets or Sets StockGuid
    /// </summary>
    [DataMember(Name="stockGuid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "stockGuid")]
    public Guid? StockGuid { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FollowStockVM {\n");
      sb.Append("  FollowStockGuid: ").Append(FollowStockGuid).Append("\n");
      sb.Append("  StockGuid: ").Append(StockGuid).Append("\n");
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

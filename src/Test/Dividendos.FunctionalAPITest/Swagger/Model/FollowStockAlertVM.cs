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
  public class FollowStockAlertVM {
    /// <summary>
    /// Gets or Sets FollowStockGuid
    /// </summary>
    [DataMember(Name="followStockGuid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "followStockGuid")]
    public Guid? FollowStockGuid { get; set; }

    /// <summary>
    /// Gets or Sets TargetPrice
    /// </summary>
    [DataMember(Name="targetPrice", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "targetPrice")]
    public string TargetPrice { get; set; }

    /// <summary>
    /// Gets or Sets CustomMessage
    /// </summary>
    [DataMember(Name="customMessage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "customMessage")]
    public string CustomMessage { get; set; }

    /// <summary>
    /// Gets or Sets FollowStockType
    /// </summary>
    [DataMember(Name="followStockType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "followStockType")]
    public FollowStockType FollowStockType { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FollowStockAlertVM {\n");
      sb.Append("  FollowStockGuid: ").Append(FollowStockGuid).Append("\n");
      sb.Append("  TargetPrice: ").Append(TargetPrice).Append("\n");
      sb.Append("  CustomMessage: ").Append(CustomMessage).Append("\n");
      sb.Append("  FollowStockType: ").Append(FollowStockType).Append("\n");
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

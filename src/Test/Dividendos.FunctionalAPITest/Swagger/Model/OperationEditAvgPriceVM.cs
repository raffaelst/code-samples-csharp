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
  public class OperationEditAvgPriceVM {
    /// <summary>
    /// Gets or Sets GuidOperation
    /// </summary>
    [DataMember(Name="guidOperation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "guidOperation")]
    public Guid? GuidOperation { get; set; }

    /// <summary>
    /// Gets or Sets AveragePrice
    /// </summary>
    [DataMember(Name="averagePrice", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "averagePrice")]
    public string AveragePrice { get; set; }

    /// <summary>
    /// Gets or Sets NumberOfShares
    /// </summary>
    [DataMember(Name="numberOfShares", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "numberOfShares")]
    public string NumberOfShares { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class OperationEditAvgPriceVM {\n");
      sb.Append("  GuidOperation: ").Append(GuidOperation).Append("\n");
      sb.Append("  AveragePrice: ").Append(AveragePrice).Append("\n");
      sb.Append("  NumberOfShares: ").Append(NumberOfShares).Append("\n");
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

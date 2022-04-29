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
  public class FinancialProductAddVM {
    /// <summary>
    /// Gets or Sets FinancialInstitutionGuid
    /// </summary>
    [DataMember(Name="financialInstitutionGuid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "financialInstitutionGuid")]
    public string FinancialInstitutionGuid { get; set; }

    /// <summary>
    /// Gets or Sets ProductGuid
    /// </summary>
    [DataMember(Name="productGuid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "productGuid")]
    public string ProductGuid { get; set; }

    /// <summary>
    /// Gets or Sets CurrentValue
    /// </summary>
    [DataMember(Name="currentValue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currentValue")]
    public string CurrentValue { get; set; }

    /// <summary>
    /// Gets or Sets AveragePrice
    /// </summary>
    [DataMember(Name="averagePrice", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "averagePrice")]
    public string AveragePrice { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class FinancialProductAddVM {\n");
      sb.Append("  FinancialInstitutionGuid: ").Append(FinancialInstitutionGuid).Append("\n");
      sb.Append("  ProductGuid: ").Append(ProductGuid).Append("\n");
      sb.Append("  CurrentValue: ").Append(CurrentValue).Append("\n");
      sb.Append("  AveragePrice: ").Append(AveragePrice).Append("\n");
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

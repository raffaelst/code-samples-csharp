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
  public class SubPortfolioVM {
    /// <summary>
    /// Gets or Sets GuidSubPortfolio
    /// </summary>
    [DataMember(Name="guidSubPortfolio", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "guidSubPortfolio")]
    public Guid? GuidSubPortfolio { get; set; }

    /// <summary>
    /// Gets or Sets GuidPortfolio
    /// </summary>
    [DataMember(Name="guidPortfolio", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "guidPortfolio")]
    public Guid? GuidPortfolio { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets Operations
    /// </summary>
    [DataMember(Name="operations", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "operations")]
    public List<long?> Operations { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SubPortfolioVM {\n");
      sb.Append("  GuidSubPortfolio: ").Append(GuidSubPortfolio).Append("\n");
      sb.Append("  GuidPortfolio: ").Append(GuidPortfolio).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Operations: ").Append(Operations).Append("\n");
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

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
  public class CryptoSubportfolioVM {
    /// <summary>
    /// Gets or Sets GuidCryptoSubportfolio
    /// </summary>
    [DataMember(Name="guidCryptoSubportfolio", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "guidCryptoSubportfolio")]
    public Guid? GuidCryptoSubportfolio { get; set; }

    /// <summary>
    /// Gets or Sets GuidCryptoPortfolio
    /// </summary>
    [DataMember(Name="guidCryptoPortfolio", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "guidCryptoPortfolio")]
    public Guid? GuidCryptoPortfolio { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets TransactionsGuid
    /// </summary>
    [DataMember(Name="transactionsGuid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionsGuid")]
    public List<Guid?> TransactionsGuid { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CryptoSubportfolioVM {\n");
      sb.Append("  GuidCryptoSubportfolio: ").Append(GuidCryptoSubportfolio).Append("\n");
      sb.Append("  GuidCryptoPortfolio: ").Append(GuidCryptoPortfolio).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  TransactionsGuid: ").Append(TransactionsGuid).Append("\n");
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

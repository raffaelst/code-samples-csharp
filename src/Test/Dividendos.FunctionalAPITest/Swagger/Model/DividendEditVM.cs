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
  public class DividendEditVM {
    /// <summary>
    /// Gets or Sets IdDividend
    /// </summary>
    [DataMember(Name="idDividend", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "idDividend")]
    public long? IdDividend { get; set; }

    /// <summary>
    /// Gets or Sets DividendValue
    /// </summary>
    [DataMember(Name="dividendValue", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dividendValue")]
    public string DividendValue { get; set; }

    /// <summary>
    /// Gets or Sets PaymentDate
    /// </summary>
    [DataMember(Name="paymentDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "paymentDate")]
    public string PaymentDate { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class DividendEditVM {\n");
      sb.Append("  IdDividend: ").Append(IdDividend).Append("\n");
      sb.Append("  DividendValue: ").Append(DividendValue).Append("\n");
      sb.Append("  PaymentDate: ").Append(PaymentDate).Append("\n");
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

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
  public class CryptoAddVM {
    /// <summary>
    /// Gets or Sets GuidCryptoCurrency
    /// </summary>
    [DataMember(Name="guidCryptoCurrency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "guidCryptoCurrency")]
    public Guid? GuidCryptoCurrency { get; set; }

    /// <summary>
    /// Gets or Sets EventDate
    /// </summary>
    [DataMember(Name="eventDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "eventDate")]
    public string EventDate { get; set; }

    /// <summary>
    /// Gets or Sets Price
    /// </summary>
    [DataMember(Name="price", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "price")]
    public string Price { get; set; }

    /// <summary>
    /// Gets or Sets Quantity
    /// </summary>
    [DataMember(Name="quantity", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "quantity")]
    public string Quantity { get; set; }

    /// <summary>
    /// Gets or Sets Exchange
    /// </summary>
    [DataMember(Name="exchange", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "exchange")]
    public string Exchange { get; set; }

    /// <summary>
    /// Gets or Sets AcquisitionPrice
    /// </summary>
    [DataMember(Name="acquisitionPrice", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "acquisitionPrice")]
    public string AcquisitionPrice { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CryptoAddVM {\n");
      sb.Append("  GuidCryptoCurrency: ").Append(GuidCryptoCurrency).Append("\n");
      sb.Append("  EventDate: ").Append(EventDate).Append("\n");
      sb.Append("  Price: ").Append(Price).Append("\n");
      sb.Append("  Quantity: ").Append(Quantity).Append("\n");
      sb.Append("  Exchange: ").Append(Exchange).Append("\n");
      sb.Append("  AcquisitionPrice: ").Append(AcquisitionPrice).Append("\n");
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

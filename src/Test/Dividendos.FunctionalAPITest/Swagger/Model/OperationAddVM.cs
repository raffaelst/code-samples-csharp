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
  public class OperationAddVM {
    /// <summary>
    /// Gets or Sets IdStock
    /// </summary>
    [DataMember(Name="idStock", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "idStock")]
    public long? IdStock { get; set; }

    /// <summary>
    /// Gets or Sets OperationDate
    /// </summary>
    [DataMember(Name="operationDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "operationDate")]
    public string OperationDate { get; set; }

    /// <summary>
    /// Gets or Sets Price
    /// </summary>
    [DataMember(Name="price", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "price")]
    public string Price { get; set; }

    /// <summary>
    /// Gets or Sets NumberOfShares
    /// </summary>
    [DataMember(Name="numberOfShares", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "numberOfShares")]
    public string NumberOfShares { get; set; }

    /// <summary>
    /// Gets or Sets Broker
    /// </summary>
    [DataMember(Name="broker", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "broker")]
    public string Broker { get; set; }

    /// <summary>
    /// Gets or Sets RemoveBuy
    /// </summary>
    [DataMember(Name="removeBuy", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "removeBuy")]
    public bool? RemoveBuy { get; set; }

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
      sb.Append("class OperationAddVM {\n");
      sb.Append("  IdStock: ").Append(IdStock).Append("\n");
      sb.Append("  OperationDate: ").Append(OperationDate).Append("\n");
      sb.Append("  Price: ").Append(Price).Append("\n");
      sb.Append("  NumberOfShares: ").Append(NumberOfShares).Append("\n");
      sb.Append("  Broker: ").Append(Broker).Append("\n");
      sb.Append("  RemoveBuy: ").Append(RemoveBuy).Append("\n");
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

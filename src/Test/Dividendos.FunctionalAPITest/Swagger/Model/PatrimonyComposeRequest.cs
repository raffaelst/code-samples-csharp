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
  public class PatrimonyComposeRequest {
    /// <summary>
    /// Gets or Sets TraderGuid
    /// </summary>
    [DataMember(Name="traderGuid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "traderGuid")]
    public Guid? TraderGuid { get; set; }

    /// <summary>
    /// Gets or Sets ShowOnPatrimony
    /// </summary>
    [DataMember(Name="showOnPatrimony", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "showOnPatrimony")]
    public bool? ShowOnPatrimony { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PatrimonyComposeRequest {\n");
      sb.Append("  TraderGuid: ").Append(TraderGuid).Append("\n");
      sb.Append("  ShowOnPatrimony: ").Append(ShowOnPatrimony).Append("\n");
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

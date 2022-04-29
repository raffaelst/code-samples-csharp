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
  public class DeviceVM {
    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets PushToken
    /// </summary>
    [DataMember(Name="pushToken", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pushToken")]
    public string PushToken { get; set; }

    /// <summary>
    /// Gets or Sets AppVersion
    /// </summary>
    [DataMember(Name="appVersion", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "appVersion")]
    public string AppVersion { get; set; }

    /// <summary>
    /// Gets or Sets UniqueId
    /// </summary>
    [DataMember(Name="uniqueId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "uniqueId")]
    public string UniqueId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class DeviceVM {\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  PushToken: ").Append(PushToken).Append("\n");
      sb.Append("  AppVersion: ").Append(AppVersion).Append("\n");
      sb.Append("  UniqueId: ").Append(UniqueId).Append("\n");
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

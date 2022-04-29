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
  public class ChangePassword {
    /// <summary>
    /// Gets or Sets CurrentPassword
    /// </summary>
    [DataMember(Name="currentPassword", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "currentPassword")]
    public string CurrentPassword { get; set; }

    /// <summary>
    /// Gets or Sets NewPassword
    /// </summary>
    [DataMember(Name="newPassword", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "newPassword")]
    public string NewPassword { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ChangePassword {\n");
      sb.Append("  CurrentPassword: ").Append(CurrentPassword).Append("\n");
      sb.Append("  NewPassword: ").Append(NewPassword).Append("\n");
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

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
  public class Passfolio2FARequest {
    /// <summary>
    /// Gets or Sets Email
    /// </summary>
    [DataMember(Name="email", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "email")]
    public string Email { get; set; }

    /// <summary>
    /// Gets or Sets Auth
    /// </summary>
    [DataMember(Name="auth", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "auth")]
    public string Auth { get; set; }

    /// <summary>
    /// Gets or Sets Code
    /// </summary>
    [DataMember(Name="code", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "code")]
    public string Code { get; set; }

    /// <summary>
    /// Gets or Sets AuthenticatorID
    /// </summary>
    [DataMember(Name="authenticatorID", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "authenticatorID")]
    public string AuthenticatorID { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Passfolio2FARequest {\n");
      sb.Append("  Email: ").Append(Email).Append("\n");
      sb.Append("  Auth: ").Append(Auth).Append("\n");
      sb.Append("  Code: ").Append(Code).Append("\n");
      sb.Append("  AuthenticatorID: ").Append(AuthenticatorID).Append("\n");
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

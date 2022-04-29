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
  public class ClearAddRequest {
    /// <summary>
    /// Gets or Sets Identifier
    /// </summary>
    [DataMember(Name="identifier", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "identifier")]
    public string Identifier { get; set; }

    /// <summary>
    /// Gets or Sets BirthDate
    /// </summary>
    [DataMember(Name="birthDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "birthDate")]
    public string BirthDate { get; set; }

    /// <summary>
    /// Gets or Sets Password
    /// </summary>
    [DataMember(Name="password", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "password")]
    public string Password { get; set; }

    /// <summary>
    /// Gets or Sets IdUser
    /// </summary>
    [DataMember(Name="idUser", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "idUser")]
    public string IdUser { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ClearAddRequest {\n");
      sb.Append("  Identifier: ").Append(Identifier).Append("\n");
      sb.Append("  BirthDate: ").Append(BirthDate).Append("\n");
      sb.Append("  Password: ").Append(Password).Append("\n");
      sb.Append("  IdUser: ").Append(IdUser).Append("\n");
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

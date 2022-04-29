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
  public class PushContentListVM {
    /// <summary>
    /// Gets or Sets UserIds
    /// </summary>
    [DataMember(Name="userIds", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "userIds")]
    public List<string> UserIds { get; set; }

    /// <summary>
    /// Gets or Sets PushBasicContent
    /// </summary>
    [DataMember(Name="pushBasicContent", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pushBasicContent")]
    public PushBasicContentVM PushBasicContent { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PushContentListVM {\n");
      sb.Append("  UserIds: ").Append(UserIds).Append("\n");
      sb.Append("  PushBasicContent: ").Append(PushBasicContent).Append("\n");
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

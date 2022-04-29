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
  public class PushBasicContentVM {
    /// <summary>
    /// Gets or Sets Title
    /// </summary>
    [DataMember(Name="title", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; }

    /// <summary>
    /// Gets or Sets Message
    /// </summary>
    [DataMember(Name="message", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; }

    /// <summary>
    /// Gets or Sets PushRedirectType
    /// </summary>
    [DataMember(Name="pushRedirectType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pushRedirectType")]
    public PushRedirectType PushRedirectType { get; set; }

    /// <summary>
    /// Gets or Sets AppScreenNameType
    /// </summary>
    [DataMember(Name="appScreenNameType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "appScreenNameType")]
    public AppScreenNameType AppScreenNameType { get; set; }

    /// <summary>
    /// Gets or Sets ExternalRedirectURL
    /// </summary>
    [DataMember(Name="externalRedirectURL", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "externalRedirectURL")]
    public string ExternalRedirectURL { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PushBasicContentVM {\n");
      sb.Append("  Title: ").Append(Title).Append("\n");
      sb.Append("  Message: ").Append(Message).Append("\n");
      sb.Append("  PushRedirectType: ").Append(PushRedirectType).Append("\n");
      sb.Append("  AppScreenNameType: ").Append(AppScreenNameType).Append("\n");
      sb.Append("  ExternalRedirectURL: ").Append(ExternalRedirectURL).Append("\n");
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

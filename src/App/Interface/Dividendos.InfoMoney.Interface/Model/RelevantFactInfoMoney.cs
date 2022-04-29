using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.InfoMoney.Interface.Model
{

    public partial class RelevantFactInfoMoney
    {
        [JsonProperty("url_search")]
        public Uri UrlSearch { get; set; }

        [JsonProperty("data")]
        public List<DataInfoMoney> Data { get; set; }
    }

    public partial class DataInfoMoney
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Empresa")]
        public string Empresa { get; set; }

        [JsonProperty("Codigo_CVM")]
        public string CodigoCvm { get; set; }

        [JsonProperty("Nome")]
        public string Nome { get; set; }

        [JsonProperty("Categoria")]
        public string Categoria { get; set; }

        [JsonProperty("Tipo")]
        public string Tipo { get; set; }

        [JsonProperty("Especie")]
        public string Especie { get; set; }

        [JsonProperty("Referencia")]
        public string Referencia { get; set; }

        [JsonProperty("Data_Publicacao")]
        public string DataPublicacao { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("Versao")]
        public string Versao { get; set; }

        [JsonProperty("Modalidade")]
        public string Modalidade { get; set; }

        [JsonProperty("URL_Documento")]
        public string UrlDocumento { get; set; }
    }
}

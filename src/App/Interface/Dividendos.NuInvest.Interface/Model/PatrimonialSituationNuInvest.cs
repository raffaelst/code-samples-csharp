using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.NuInvest.Interface.Model
{
    public class PatrimonialSituationNuInvest
    {
        [JsonProperty("value")]
        public Value Value { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("patrimonialSituation")]
        public PatrimonialSituation PatrimonialSituation { get; set; }

        [JsonProperty("currentPatrimonialSituation")]
        public PatrimonialSituation CurrentPatrimonialSituation { get; set; }

        [JsonProperty("suggestPatrimonialSituation")]
        public PatrimonialSituation SuggestPatrimonialSituation { get; set; }
    }

    public partial class PatrimonialSituation
    {
        [JsonProperty("monthlyIncome")]
        public string MonthlyIncome { get; set; }

        [JsonProperty("bankDepositAmount")]
        public string BankDepositAmount { get; set; }

        [JsonProperty("fixedIncomeAmount")]
        public string FixedIncomeAmount { get; set; }

        [JsonProperty("variableIncomeAmount")]
        public string VariableIncomeAmount { get; set; }

        [JsonProperty("patrimonialTotalAmount")]
        public string PatrimonialTotalAmount { get; set; }
    }
}

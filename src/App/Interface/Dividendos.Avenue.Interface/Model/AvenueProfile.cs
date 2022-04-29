using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dividendos.Avenue.Interface.Model
{
    public partial class AvenueProfile
    {
        [JsonProperty("is_converted")]
        public string IsConverted { get; set; }

        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("docs_acked")]
        public string DocsAcked { get; set; }

        [JsonProperty("created")]
        public Created Created { get; set; }

        [JsonProperty("modified")]
        public Created Modified { get; set; }

        [JsonProperty("basic_info")]
        public BasicInfo BasicInfo { get; set; }

        [JsonProperty("tax_id_info")]
        public TaxIdInfo TaxIdInfo { get; set; }

        [JsonProperty("cnh_info")]
        public CnhInfo CnhInfo { get; set; }

        [JsonProperty("joint_account")]
        public JointAccount JointAccount { get; set; }

        [JsonProperty("employment_info")]
        public EmploymentInfo EmploymentInfo { get; set; }

        [JsonProperty("address_info")]
        public AddressInfo AddressInfo { get; set; }

        [JsonProperty("joint_address_info")]
        public AddressInfo JointAddressInfo { get; set; }

        [JsonProperty("biz_address_info")]
        public AddressInfo BizAddressInfo { get; set; }

        [JsonProperty("investor_profile_info")]
        public InvestorProfileInfo InvestorProfileInfo { get; set; }

        [JsonProperty("disclosures")]
        public Disclosures Disclosures { get; set; }

        [JsonProperty("agreements")]
        public Agreements Agreements { get; set; }

        [JsonProperty("reg_form")]
        public RegForm RegForm { get; set; }

        [JsonProperty("bank_info_ted")]
        public Ted BankInfoTed { get; set; }

        [JsonProperty("bank_info")]
        public BankInfo BankInfo { get; set; }

        [JsonProperty("journey")]
        public string Journey { get; set; }

        [JsonProperty("joint_disclosures")]
        public Disclosures JointDisclosures { get; set; }

        [JsonProperty("total_custody")]
        public string TotalCustody { get; set; }

        [JsonProperty("last_nps_score")]
        public string LastNpsScore { get; set; }

        [JsonProperty("account_source")]
        public string AccountSource { get; set; }
    }

    public partial class AddressInfo
    {
        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("complement")]
        public string Complement { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("neighborhood")]
        public string Neighborhood { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }

    public partial class Agreements
    {
        [JsonProperty("terms_of_use_us")]
        public string TermsOfUseUs { get; set; }

        [JsonProperty("terms_of_use_us_sign")]
        public string TermsOfUseUsSign { get; set; }

        [JsonProperty("privacy_policy")]
        public string PrivacyPolicy { get; set; }

        [JsonProperty("privacy_policy_sign")]
        public string PrivacyPolicySign { get; set; }

        [JsonProperty("customer_agreement")]
        public string CustomerAgreement { get; set; }

        [JsonProperty("customer_agreement_sign")]
        public string CustomerAgreementSign { get; set; }

        [JsonProperty("dw_agreement")]
        public string DwAgreement { get; set; }

        [JsonProperty("dw_agreement_sign")]
        public string DwAgreementSign { get; set; }

        [JsonProperty("terms_of_use_br")]
        public string TermsOfUseBr { get; set; }

        [JsonProperty("terms_of_use_br_sign")]
        public string TermsOfUseBrSign { get; set; }

        [JsonProperty("bexs_agreement")]
        public string BexsAgreement { get; set; }

        [JsonProperty("bexs_agreement_sign")]
        public string BexsAgreementSign { get; set; }

        [JsonProperty("others_agreement")]
        public string OthersAgreement { get; set; }

        [JsonProperty("apex_agreement")]
        public string ApexAgreement { get; set; }

        [JsonProperty("apex_agreement_sign")]
        public string ApexAgreementSign { get; set; }

        [JsonProperty("apex_joint_agreement")]
        public string ApexJointAgreement { get; set; }

        [JsonProperty("apex_joint_agreement_sign")]
        public string ApexJointAgreementSign { get; set; }

        [JsonProperty("joint_w8")]
        public string JointW8 { get; set; }

        [JsonProperty("joint_w8_sign")]
        public string JointW8Sign { get; set; }
    }

    public partial class BankInfo
    {
        [JsonProperty("TED")]
        public Ted Ted { get; set; }
    }

    public partial class Ted
    {
        [JsonProperty("bank")]
        public string Bank { get; set; }

        [JsonProperty("account_with_digit")]
        public string AccountWithDigit { get; set; }

        [JsonProperty("agency")]
        public string Agency { get; set; }

        [JsonProperty("fx_bank_id")]
        public string FxBankId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class BasicInfo
    {
        [JsonProperty("main_phone")]
        public string MainPhone { get; set; }

        [JsonProperty("alternate_phone")]
        public string AlternatePhone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mother_name")]
        public string MotherName { get; set; }

        [JsonProperty("father_name")]
        public string FatherName { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("marital")]
        public string Marital { get; set; }

        [JsonProperty("spouse_name")]
        public string SpouseName { get; set; }

        [JsonProperty("dob")]
        public Created Dob { get; set; }

        [JsonProperty("birth_country")]
        public string BirthCountry { get; set; }

        [JsonProperty("birth_state")]
        public string BirthState { get; set; }

        [JsonProperty("birth_city")]
        public string BirthCity { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("tax_residency")]
        public string TaxResidency { get; set; }
    }

    public partial class Created
    {
        [JsonProperty("seconds")]
        public string Seconds { get; set; }
    }

    public partial class CnhInfo
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("security_code")]
        public string SecurityCode { get; set; }
    }

    public partial class Disclosures
    {
    }

    public partial class EmploymentInfo
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }
    }

    public partial class InvestorProfileInfo
    {
        [JsonProperty("monthly_income")]
        public string MonthlyIncome { get; set; }

        [JsonProperty("net_worth")]
        public string NetWorth { get; set; }

        [JsonProperty("total_investments_amount")]
        public string TotalInvestmentsAmount { get; set; }

        [JsonProperty("time_horizon")]
        public string TimeHorizon { get; set; }

        [JsonProperty("liquidity_needs")]
        public string LiquidityNeeds { get; set; }

        [JsonProperty("investment_experience")]
        public string InvestmentExperience { get; set; }

        [JsonProperty("risk_tolerance")]
        public string RiskTolerance { get; set; }

        [JsonProperty("investment_objective")]
        public string InvestmentObjective { get; set; }
    }

    public partial class JointAccount
    {
        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("cpf")]
        public string Cpf { get; set; }

        [JsonProperty("is_joint")]
        public bool IsJoint { get; set; }

        [JsonProperty("dob")]
        public Created Dob { get; set; }

        [JsonProperty("main_phone")]
        public string MainPhone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("working_status")]
        public string WorkingStatus { get; set; }

        [JsonProperty("joint_confirmed")]
        public string JointConfirmed { get; set; }
    }

    public partial class RegForm
    {
        [JsonProperty("reg_form")]
        public string RegFormRegForm { get; set; }

        [JsonProperty("reg_form_sign")]
        public string RegFormSign { get; set; }
    }

    public partial class TaxIdInfo
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}

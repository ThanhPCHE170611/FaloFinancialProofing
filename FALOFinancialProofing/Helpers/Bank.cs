namespace FALOFinancialProofing.Helpers
{
    public class Datum
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string bin { get; set; }
        public string shortName { get; set; }
        public string logo { get; set; }
        public int transferSupported { get; set; }
        public int lookupSupported { get; set; }
        public string short_name { get; set; }
        public int support { get; set; }
        public int isTransfer { get; set; }
        public string swift_code { get; set; }
    }

    public class Bank
    {
        public string code { get; set; }
        public string desc { get; set; }
        public List<Datum> data { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<RequestBank>(myJsonResponse);
    public class BankRequest
    {
        public long accountNo { get; set; }
        public string accountName { get; set; }
        public int acqId { get; set; }
        public int amount { get; set; }
        public string addInfo { get; set; }
        public string format { get; set; }
        public string template { get; set; }
    }



    // Root myDeserializedClass = JsonConvert.DeserializeObject<BankResponse>(myJsonResponse);
    public class Data
    {
        public int acpId { get; set; }
        public string accountName { get; set; }
        public string qrCode { get; set; }
        public string qrDataURL { get; set; }
    }

    public class BankResponse
    {
        public string code { get; set; }
        public string desc { get; set; }
        public Data data { get; set; }
    }



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Transaction>(myJsonResponse);
    public class BankTransaction
    {
        public int id { get; set; }
        public string when { get; set; }
        public int amount { get; set; }
        public string description { get; set; }
        public int cusum_balance { get; set; }
        public string tid { get; set; }
        public string subAccId { get; set; }
        public string bank_sub_acc_id { get; set; }
        public string virtualAccount { get; set; }
        public string virtualAccountName { get; set; }
        public string corresponsiveName { get; set; }
        public string corresponsiveAccount { get; set; }
        public string corresponsiveBankId { get; set; }
        public string corresponsiveBankName { get; set; }
    }

    public class TransactionRequest
    {
        public int error { get; set; }
        public List<BankTransaction> data { get; set; }
    }


    //bankInformation
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class BankInformation
    {
        public int id { get; set; }
        public string accountNumber { get; set; }
        public string accountName { get; set; }
        public string accountType { get; set; }
        public object balance { get; set; }
        public string beginningSettingDate { get; set; }
        public DateTime beginningTxnDate { get; set; }
        public int beginningBalance { get; set; }
        public int creditTxnTotal { get; set; }
        public int creditTxnAmount { get; set; }
        public int debitTxnTotal { get; set; }
        public int debitTxnAmount { get; set; }
        public object lockSyncDate { get; set; }
        public object endingBalance { get; set; }
        public DateTime endingTxnDate { get; set; }
        public string currency { get; set; }
        public string swift { get; set; }
        public string citad { get; set; }
        public string serviceType { get; set; }
        public string bankName { get; set; }
        public string bankCodeName { get; set; }
        public string memo { get; set; }
        public int connectStatus { get; set; }
        public int BIN { get; set; }
    }

    public class AccountResponse
    {
        public int error { get; set; }
        public string message { get; set; }
        public List<BankInformation> data { get; set; }
    }

}

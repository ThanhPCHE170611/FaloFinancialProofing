namespace FALOFinancialProofing.FALOHomePage.Models
{
    public class TransactionDTO
    {
        public class TransactionLogsResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public List<Transaction> Data { get; set; }
        }

        public class Transaction
        {
            // Define properties that match the structure of the transaction log (adjust as needed)
            public int Id { get; set; }
            public string TransactionDetails { get; set; }
            public decimal Amount { get; set; }
            public string Status { get; set; }
            // Add other fields as necessary based on the API response
        }
    }
}

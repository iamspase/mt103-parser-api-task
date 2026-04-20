namespace SwiftMT103ApiTask.Models
{
    public class MT103Message
    {
        public long Id { get; set; }
        /// <summary>
        /// Transaction reference number (:20)
        /// </summary>
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Bank operation code (:23B)
        /// </summary>
        public string BankOperationCode { get; set; }

        /// <summary>
        /// Value date: 32A 
        /// </summary>
        public DateTime ValueDate { get; set; }

        /// <summary>
        ///  Currency: 32A
        /// </summary>
        public string Currency { get; set; }
        
        /// <summary>
        /// Interbank setteld or the amount: 32A
        /// </summary>
        public decimal InterbankSettled { get; set; }


        /// <summary>
        /// Ordering customer: 50A, F or K
        /// </summary>
        public string OrderingCustomer { get; set; }

        /// <summary>
        /// Account with Institution :57A, B, C or D
        /// </summary>
        public string AccountWithInstitution { get; set; }

        /// <summary>
        /// Recepient :59 or 59A
        /// </summary>
        public string Recepient { get; set; }

        /// <summary>
        /// Remittance Information :70
        /// </summary>
        public string RemittanceInformation { get; set; }

        /// <summary>
        /// Details of charge :71A (SHA/OUR/BEN)
        /// </summary>
        public string DetailsOfCharge { get; set; }

        public override string ToString()
        {
            return $@"
                MT103 Message:
                  ID: {Id}
                  Reference: {ReferenceNumber}
                  Bank Op Code: {BankOperationCode}
                  Value Date: {ValueDate:yyyy-MM-dd}
                  Currency: {Currency}
                  Amount: {InterbankSettled}
                  Ordering Customer: {OrderingCustomer}
                  Account With: {AccountWithInstitution}
                  Recipient: {Recepient}
                  Remittance Info: {RemittanceInformation}
                  Charge Details: {DetailsOfCharge}
                ";
        }
    }
}

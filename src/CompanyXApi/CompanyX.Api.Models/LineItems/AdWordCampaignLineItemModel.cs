namespace CompanyX.Api.Models.LineItems
{
    /// <summary>
    /// Paid search line item
    /// </summary>
    public class AdWordCampaignLineItemModel : LineItemModel
    {
       public AddWordCampaignProduct AdWordCampaign { get; set; }
    }

    /// <summary>
    /// Paid search product
    /// </summary>
    public class AddWordCampaignProduct
    {
        /// <summary>
        /// Campaign name
        /// </summary>
        public string CampaignName { get; set; }
        /// <summary>
        /// Campaign Address line 1
        /// </summary>
        public string CampaignAddressLine1 { get; set; }
        /// <summary>
        /// Campaign Post code
        /// </summary>
        public string CampaignPostCode { get; set; }
        /// <summary>
        /// Campaign radius
        /// </summary>
        public string CampaignRadius { get; set; }
        /// <summary>
        /// Lead phone number
        /// </summary>
        public string LeadPhoneNumber { get; set; }
        /// <summary>
        /// Sms phone number
        /// </summary>
        public string SMSPhoneNumber { get; set; }
        /// <summary>
        /// Unique selling point 1
        /// </summary>
        public string UniqueSellingPoint1 { get; set; }
        /// <summary>
        /// Unique selling point 2
        /// </summary>
        public string UniqueSellingPoint2 { get; set; }
        /// <summary>
        /// Unique selling point 3
        /// </summary>
        public string UniqueSellingPoint3 { get; set; }
        /// <summary>
        /// Offer
        /// </summary>
        public string Offer { get; set; }
        /// <summary>
        /// Destination
        /// </summary>
        public string DestinationURL { get; set; }
    }
}

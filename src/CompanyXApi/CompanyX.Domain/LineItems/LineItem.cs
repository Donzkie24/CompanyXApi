namespace CompanyX.Domain.LineItems
{
    /// <summary>
    /// Line item of order
    /// </summary>
    public class LineItem : DomainObject
    {
        /// <summary>
        /// product id
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// Product type
        /// </summary>
        public string ProductType { get; set; }
        /// <summary>
        /// product notes
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// category
        /// </summary>
        public string Category { get; set; }

        public AdWordCampaign AdWordCampaign { get; set; }

        public WebsiteDetail WebsiteDetail { get; set; }

    }
}

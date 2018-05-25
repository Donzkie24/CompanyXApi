namespace CompanyX.Api.Models.Orders
{
    /// <summary>
    /// Additional fields specific to partners
    /// </summary>
    public class AdditionalFields
    {
        /// <summary>
        /// Customer detail for Partner A
        /// </summary>
        public CustomerModel Customer { get; set; }
        /// <summary>
        /// Additional Info for Partner C
        /// </summary>
        public AdditionalInfoModel AdditionalInfo { get; set; }
    }
}

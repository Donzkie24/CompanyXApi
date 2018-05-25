namespace CompanyX.Api.Models.Orders
{
    /// <summary>
    /// Additional info for Partner C
    /// </summary>
    public class AdditionalInfoModel
    {
        /// <summary>
        /// Exposure Id
        /// </summary>
        public string ExposureId { get; set; }
        /// <summary>
        /// UDAC
        /// </summary>
        public string UDAC { get; set; }
        /// <summary>
        /// Related order (OrderId)
        /// </summary>
        public string RelatedOrder { get; set; }

    }
}

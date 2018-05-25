namespace CompanyX.Domain.Orders
{
    /// <summary>
    /// Additional info for partner-c
    /// </summary>
    public class AdditionalInfo : DomainObject
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

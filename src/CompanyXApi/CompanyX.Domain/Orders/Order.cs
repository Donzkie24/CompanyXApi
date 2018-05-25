using System.Collections.Generic;
using CompanyX.Domain.LineItems;

namespace CompanyX.Domain.Orders
{
    /// <summary>
    /// Order domain object
    /// </summary>
    public class Order : DomainObject
    {
        /// <summary>
        /// Order Partner
        /// </summary>
        public string Partner { get; set; }
        /// <summary>
        /// Order Id
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// Type of order
        /// </summary>
        public string TypeOfOrder { get; set; }
        /// <summary>
        /// Submitted by
        /// </summary>
        public string SubmittedBy { get; set; }
        /// <summary>
        /// Company Id
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// Company Name
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// Order line items
        /// </summary>
        public IEnumerable<LineItem> LineItems { get; set; }
        /// <summary>
        /// Order customer details
        /// </summary>
        public Customer Customer { get; set; }
        /// <summary>
        /// Order additional info fields
        /// </summary>
        public AdditionalInfo AdditionalInfo { get; set; }
    }
}

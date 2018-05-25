using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CompanyX.Api.Models.LineItems;

namespace CompanyX.Api.Models.Orders
{
    /// <summary>
    /// Order model
    /// </summary>
    public class OrderModel
    {
        /// <summary>
        /// Order Partner
        /// </summary>
        public string Partner { get; set; }
        /// <summary>
        /// Order Id
        /// </summary>
        [Required]
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
        /// Line items
        /// </summary>
        [Required]
        public IList<LineItemModel> LineItems { get; set; }

        /// <summary>
        /// Additional fields of order
        /// </summary>
        public AdditionalFields AdditionalFields { get; set; }
    }
}

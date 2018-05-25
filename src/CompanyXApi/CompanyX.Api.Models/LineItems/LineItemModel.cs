using System.ComponentModel.DataAnnotations;

namespace CompanyX.Api.Models.LineItems
{
    /// <summary>
    /// Base product (line item )
    /// </summary>
    public class LineItemModel
    {
        /// <summary>
        /// Id of the line item
        /// </summary>
        [Required]
        public int Id { get; set; }
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
    }
}

using System.ComponentModel.DataAnnotations;

namespace CompanyX.Api.Models.LineItems
{
    /// <summary>
    /// Web site details line item
    /// </summary>
    public class WebsiteDetailsLineItemModel : LineItemModel
    {
        /// <summary>
        /// Website details
        /// </summary>
        public WebsiteDetailsProduct WebsiteDetails { get; set; }
    }

    /// <summary>
    /// Web site details product
    /// </summary>
    public class WebsiteDetailsProduct
    {
        /// <summary>
        /// Template id
        /// </summary>
        [Required]
        public string TemplateId { get; set; }
        /// <summary>
        /// business name
        /// </summary>
        public string WebsiteBusinessName { get; set; }
        /// <summary>
        /// Address line 1
        /// </summary>
        public string WebsiteAddressLine1 { get; set; }
        /// <summary>
        /// Address line 2
        /// </summary>
        public string WebsiteAddressLine2 { get; set; }
        /// <summary>
        /// City
        /// </summary>
        public string WebsiteCity { get; set; }
        /// <summary>
        /// State
        /// </summary>
        public string WebsiteState { get; set; }
        /// <summary>
        /// Postal code
        /// </summary>
        public string WebsitePostCode { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        public string WebsitePhone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string WebsiteEmail { get; set; }
        /// <summary>
        /// Mobile
        /// </summary>
        public string WebsiteMobile { get; set; }
    }
}

namespace CompanyX.Domain.Orders
{
    /// <summary>
    /// Order additional fields (Customer details)
    /// </summary>
    public class Customer : DomainObject
    {
        /// <summary>
        /// First name
        /// </summary>
        public string ContactFirstName { get; set; }
        /// <summary>
        /// Last name
        /// </summary>
        public string ContactLastName { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string ContactTitle { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// Mobile
        /// </summary>
        public string ContactMobile { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string ContactEmail { get; set; }
    }
}

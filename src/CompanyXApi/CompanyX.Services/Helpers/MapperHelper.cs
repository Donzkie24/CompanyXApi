using CompanyX.Api.Models.LineItems;
using CompanyX.Api.Models.Orders;
using CompanyX.Domain.LineItems;
using CompanyX.Domain.Orders;
using Mapster;

namespace CompanyX.Services.Helpers
{
    /// <summary>
    /// Api model to Domain model helpers
    /// </summary>
    public static class MapperHelper
    {
        /// <summary>
        /// Map WebsiteDetailsProduct to WebsiteDetail
        /// </summary>
        /// <param name="websiteDetailsProduct"></param>
        /// <returns></returns>
        public static WebsiteDetail MapToWebsiteDetail(WebsiteDetailsProduct websiteDetailsProduct)
        {
            TypeAdapterConfig<WebsiteDetailsProduct, WebsiteDetail>
                .NewConfig()
                .Ignore(dest => dest.LineItemId);

            return websiteDetailsProduct.Adapt<WebsiteDetail>();
        }

        /// <summary>
        /// Map AddWordCampaignProduct to AdWordCampaign
        /// </summary>
        /// <param name="addWordCampaignProduct"></param>
        /// <returns></returns>
        public static AdWordCampaign MapToAdWordCampaign(AddWordCampaignProduct addWordCampaignProduct)
        {
            TypeAdapterConfig<AddWordCampaignProduct, AdWordCampaign>
                .NewConfig()
                .Ignore(dest => dest.LineItemId);

            return addWordCampaignProduct.Adapt<AdWordCampaign>();
        }

        /// <summary>
        /// Map LineItemModel to LineItem
        /// </summary>
        /// <param name="lineItemModel"></param>
        /// <returns></returns>
        public static LineItem MapToLineItem(LineItemModel lineItemModel)
        {
            TypeAdapterConfig<LineItemModel, LineItem>.NewConfig();

            return lineItemModel.Adapt<LineItem>();
        }
        /// <summary>
        /// Map CustomerModel to Customer
        /// </summary>
        /// <param name="customerModel"></param>
        /// <returns></returns>
        public static Customer MapToCustomer(CustomerModel customerModel)
        {
            TypeAdapterConfig<CustomerModel, Customer>
                .NewConfig();

            return customerModel.Adapt<Customer>();
        }

        /// <summary>
        /// Map AdditionalInfoModel to AdditionalInfo
        /// </summary>
        /// <param name="additionalInfoModel"></param>
        /// <returns></returns>
        public static AdditionalInfo MapToAdditionalInfo(AdditionalInfoModel additionalInfoModel)
        {
            TypeAdapterConfig<AdditionalInfoModel, AdditionalInfo >
                .NewConfig()
                .Map(dst => dst.RelatedOrder, src => src.RelatedOrder);

            return additionalInfoModel.Adapt<AdditionalInfo>();
        }

        /// <summary>
        /// Map OrderModel to Order
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        public static Order MapToOrder(OrderModel orderModel)
        {
            TypeAdapterConfig<OrderModel, Order>
                .NewConfig()
                .Ignore(dst => dst.AdditionalInfo)
                .Ignore(dst => dst.Customer)
                .Ignore(dst => dst.LineItems);

            return orderModel.Adapt<Order>();
        }
    }
}

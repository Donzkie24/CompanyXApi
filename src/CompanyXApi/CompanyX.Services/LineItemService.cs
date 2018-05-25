using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyX.Api.Models.LineItems;
using CompanyX.Base.Extensions;
using CompanyX.Base.Helpers;
using CompanyX.Dal;
using CompanyX.Domain.LineItems;
using CompanyX.Services.Helpers;
using Microsoft.Extensions.Logging;

namespace CompanyX.Services
{
    #region Interface
    /// <summary>
    /// Line item service
    /// </summary>
    public interface ILineItemService
    {
        /// <summary>
        /// Create line items from lineItemModels
        /// </summary>
        /// <param name="inputLineItems"></param>
        /// <returns></returns>
        Task<IList<LineItem>> CreateLineItemsAsync(IList<LineItemModel> inputLineItems);
    }


    #endregion

    #region Implementation

    /// <inheritdoc />
    public class LineItemService : BaseService, ILineItemService
    {
        private readonly IRepository<LineItem> _lineItemRepository;
        private readonly IRepository<AdWordCampaign> _adWordCampaignRepository;
        private readonly IRepository<WebsiteDetail> _websiteDetailRepository;

        // ReSharper disable once TooManyDependencies
        /// <summary>
        /// Constructor this has too many dependencies, 
        /// use unitOfWork with EF core manage repositories injection (https://github.com/Arch/UnitOfWork)
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="lineItemRepository"></param>
        /// <param name="adWordCampaignRepository"></param>
        /// <param name="websiteDetailRepository"></param>
        public LineItemService(ILogger<BaseService> logger, 
            IRepository<LineItem> lineItemRepository, 
            IRepository<AdWordCampaign> adWordCampaignRepository, 
            IRepository<WebsiteDetail> websiteDetailRepository) 
            : base(logger)
        {
            Guard.IsNotNull(lineItemRepository, () => lineItemRepository);
            Guard.IsNotNull(adWordCampaignRepository, () => adWordCampaignRepository);
            Guard.IsNotNull(websiteDetailRepository, () => websiteDetailRepository);

            _lineItemRepository = lineItemRepository;
            _adWordCampaignRepository = adWordCampaignRepository;
            _websiteDetailRepository = websiteDetailRepository;
        }

        public async Task<IList<LineItem>> CreateLineItemsAsync(IList<LineItemModel> inputLineItems)
        {
            Guard.IsNotNullOrEmpty(inputLineItems, () => inputLineItems);

            var result = new List<LineItem>();
            await inputLineItems.ForEachAsync(async lineItemModel =>
            {
                var lineItem = MapperHelper.MapToLineItem(lineItemModel);

                //TODO, use strategy pattern?
                switch (lineItemModel)
                {
                    case WebsiteDetailsLineItemModel item:
                        var websiteDetail = MapperHelper.MapToWebsiteDetail(item.WebsiteDetails);
                        websiteDetail.LineItemId = item.Id;
                        _websiteDetailRepository.Save(websiteDetail);
                        lineItem.WebsiteDetail = websiteDetail;
                        break;
                    case AdWordCampaignLineItemModel item:
                        var adWordCampaign = MapperHelper.MapToAdWordCampaign(item.AdWordCampaign);
                        adWordCampaign.LineItemId = item.Id;
                        _adWordCampaignRepository.Save(adWordCampaign);
                        lineItem.AdWordCampaign = adWordCampaign;
                        break;
                }

                //save line items
                _lineItemRepository.Save(lineItem);

                result.Add(lineItem);
            }).ConfigureAwait(false);

            return result;
        }
    }

    #endregion

}

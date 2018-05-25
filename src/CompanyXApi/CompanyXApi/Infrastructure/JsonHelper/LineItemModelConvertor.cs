using System;
using CompanyX.Api.Models.LineItems;
using Newtonsoft.Json.Linq;

namespace CompanyX.Api.Infrastructure.JsonHelper
{
    /// <summary>
    /// Custom json convertor for different lint item products
    /// </summary>
    public class LineItemModelConvertor : JsonCreationConverter<LineItemModel>
    {
        protected override LineItemModel Create(Type objectType, JObject jObject)
        {
            if (FieldExists("WebsiteDetails", jObject))
            {
                return new WebsiteDetailsLineItemModel();
            }
            else if (FieldExists("AdWordCampaign", jObject))
            {
                return new AdWordCampaignLineItemModel();
            }
            else
            {
                return new LineItemModel();
            }
        }

        private bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }
    }
}

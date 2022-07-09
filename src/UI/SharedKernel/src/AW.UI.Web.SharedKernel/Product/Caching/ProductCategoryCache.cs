﻿using AW.SharedKernel.Caching;
using AW.UI.Web.SharedKernel.Caching;
using AW.UI.Web.SharedKernel.Interfaces.Api;
using AW.UI.Web.SharedKernel.Product.Handlers.GetProductCategories;
using Microsoft.Extensions.Caching.Memory;

namespace AW.UI.Web.SharedKernel.Product.Caching
{
    public class ProductCategoryCache : ICache<ProductCategory>
    {
        private readonly IMemoryCache cache;
        private readonly IProductApiClient client;
        private List<ProductCategory>? products;

        public ProductCategoryCache(IMemoryCache cache, IProductApiClient client) =>
            (this.cache, this.client) = (cache, client);

        public async Task<List<ProductCategory>?> GetData()
        {
            if (!cache.TryGetValue(CacheKeys.ProductCategories, out products))
            {
                await Initialize();
            }

            return products;
        }

        public async Task<List<ProductCategory>> GetData(Func<ProductCategory, bool> predicate)
        {
            return (await GetData())!
                .Where(predicate).ToList();
        }

        public async Task Initialize()
        {
            products = await client.GetCategoriesAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(1)
            );

            cache.Set(CacheKeys.ProductCategories, products, cacheEntryOptions);
        }
    }
}
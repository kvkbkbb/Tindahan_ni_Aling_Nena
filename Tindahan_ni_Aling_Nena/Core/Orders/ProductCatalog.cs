using System;
using System.Collections.Generic;

namespace Tindahan_ni_Aling_Nena.Core.Orders
{
    /// <summary>
    /// The master list of every item that can appear in an order.
    /// This is the ONLY place prices are defined. All prices are whole pesos.
    /// </summary>
    public static class ProductCatalog
    {
        public static readonly IReadOnlyList<Product> All = new List<Product>
        {
            new Product("Candy", 1m),
            new Product("Coffee Sachet", 8m),
            new Product("Chippy", 9m),
        }.AsReadOnly();

        /// <summary>
        /// Returns one random product. Pass in a shared Random so repeated calls
        /// don't return the same value.
        /// </summary>
        public static Product GetRandom(Random rng)
        {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            return All[rng.Next(All.Count)];
        }
    }
}
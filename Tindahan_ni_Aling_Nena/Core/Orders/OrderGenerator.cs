using System;

namespace Tindahan_ni_Aling_Nena.Core.Orders
{
    /// <summary>
    /// Builds the random order for each new customer.
    /// Static: call it as OrderGenerator.Generate().
    /// </summary>
    public static class OrderGenerator
    {
        private const int MinItems = 2;
        private const int MaxItems = 5;

        // One shared Random for the whole game, so repeated calls stay varied.
        private static Random _rng = new Random();

        /// <summary>
        /// For testing: call OrderGenerator.SetSeed(42) to get the same orders every run.
        /// </summary>
        public static void SetSeed(int seed)
        {
            _rng = new Random(seed);
        }

        /// <summary>
        /// Creates a new random order with MinItems to MaxItems products.
        /// </summary>
        public static Order Generate()
        {
            // Next(min, max + 1) because the upper bound of Next is exclusive.
            int itemCount = _rng.Next(MinItems, MaxItems + 1);

            var order = new Order();
            for (int i = 0; i < itemCount; i++)
                order.AddItem(ProductCatalog.GetRandom(_rng));

            return order;
        }
    }
}
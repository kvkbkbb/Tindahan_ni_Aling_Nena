using System.Collections.Generic;

namespace Tindahan_ni_Aling_Nena.Core.Money
{
    public sealed class Denomination
    {
        public string Name { get; }
        public decimal Value { get; }
        public bool IsBill { get; }

        private Denomination(string name, decimal value, bool isBill)
        {
            Name = name;
            Value = value;
            IsBill = isBill;
        }

        // Bills
        public static readonly Denomination Bill1000 = new Denomination("₱1000 bill", 1000m, true);
        public static readonly Denomination Bill500 = new Denomination("₱500 bill", 500m, true);
        public static readonly Denomination Bill200 = new Denomination("₱200 bill", 200m, true);
        public static readonly Denomination Bill100 = new Denomination("₱100 bill", 100m, true);
        public static readonly Denomination Bill50 = new Denomination("₱50 bill", 50m, true);
        public static readonly Denomination Bill20 = new Denomination("₱20 bill", 20m, true);

        // Coins
        public static readonly Denomination Coin20 = new Denomination("₱20 coin", 20m, false);
        public static readonly Denomination Coin10 = new Denomination("₱10 coin", 10m, false);
        public static readonly Denomination Coin5 = new Denomination("₱5 coin", 5m, false);
        public static readonly Denomination Coin1 = new Denomination("₱1 coin", 1m, false);
        public static readonly Denomination Coin25c = new Denomination("25¢ coin", 0.25m, false);

        public static readonly IReadOnlyList<Denomination> All = new List<Denomination>
        {
            Bill1000, Bill500, Bill200, Bill100, Bill50, Bill20,
            Coin20, Coin10, Coin5, Coin1, Coin25c
        }.AsReadOnly();

        public override string ToString() => Name;
    }
}
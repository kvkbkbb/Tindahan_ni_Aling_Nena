namespace Tindahan_ni_Aling_Nena.Core.Orders
{
    public sealed class Product
    {
        public string Name { get; }
        public decimal Price { get; }
        public string ImageKey { get; }

        public Product(string name, decimal price, string imageKey)
        {
            Name = name;
            Price = price;
            ImageKey = imageKey;
        }

        public override string ToString() => $"{Name} (₱{Price:0.00})";
    }
}
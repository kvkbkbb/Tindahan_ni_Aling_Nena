namespace Tindahan_ni_Aling_Nena.Core.Orders
{
    public sealed class Product
    {
        public string Name { get; }
        public decimal Price { get; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public override string ToString() => $"{Name} (₱{Price:0})";
    }
}
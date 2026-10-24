using System;
using System.Collections.Generic;
using System.Linq;

namespace Tindahan_ni_Aling_Nena.Core.Orders
{
    public class Order
    {
        private readonly List<Product> _items = new List<Product>();

        public IReadOnlyList<Product> Items => _items;

        public decimal Total => _items.Sum(p => p.Price);

        public int ItemCount => _items.Count;

        // If an 'Order' with no products were to be made, this constructor would get called.
        public Order()
        {
        }

        // Overriden constructor
        public Order(IEnumerable<Product> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            _items.AddRange(items);
        }

        public void AddItem(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            _items.Add(product);
        }
    }
}
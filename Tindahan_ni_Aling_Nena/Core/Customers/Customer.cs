using System;
using System.Collections.Generic;
using System.Linq;
using Tindahan_ni_Aling_Nena.Core.Money;
using Tindahan_ni_Aling_Nena.Core.Orders;

namespace Tindahan_ni_Aling_Nena.Core.Customers
{

    public class Customer
    {
        public Order Order { get; }

        public IReadOnlyList<Denomination> PaymentPieces { get; }

        public decimal AmountPaid => PaymentPieces.Sum(d => d.Value);
        public bool IsUnderpaying => AmountPaid < Order.Total;

        public Customer(Order order, IEnumerable<Denomination> paymentPieces)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (paymentPieces == null) throw new ArgumentNullException(nameof(paymentPieces));

            Order = order;
            PaymentPieces = paymentPieces.ToList().AsReadOnly();

            if (PaymentPieces.Count == 0)
                throw new ArgumentException("A customer must hand over at least one bill or coin.", nameof(paymentPieces));
        }
    }
}
// Models/Customers/UnderPayingCustomer.cs
using TindahanNiAlingNena_Recoded.Models.Money;
using TindahanNiAlingNena_Recoded.Services;

namespace TindahanNiAlingNena_Recoded.Models.Customers;

public class UnderPayingCustomer : Customer
{
    public UnderPayingCustomer(Order order) : base(order)
    {
    }

    public override List<Denomination> Pay(PaymentGenerator paymentGenerator)
    {
        return paymentGenerator.GenerateUnderPayment(order.GetTotalCost());
    }
}
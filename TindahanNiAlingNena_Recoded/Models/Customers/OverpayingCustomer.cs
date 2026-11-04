using TindahanNiAlingNena_Recoded.Models.Money;
using TindahanNiAlingNena_Recoded.Services;

namespace TindahanNiAlingNena_Recoded.Models.Customers;

public class OverPayingCustomer : Customer
{
    public OverPayingCustomer(Order order) : base(order)
    {
        
    }

    public override List<Denomination> GeneratePayment(PaymentGenerator paymentGenerator)
    {
        return paymentGenerator.GenerateOverPayment(order.GetTotalCost());
    }
}
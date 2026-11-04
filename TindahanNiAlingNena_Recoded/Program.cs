// Program.cs
using TindahanNiAlingNena_Recoded.Models;
using TindahanNiAlingNena_Recoded.Models.Customers;
using TindahanNiAlingNena_Recoded.Models.Money;
using TindahanNiAlingNena_Recoded.Services;

ProductCatalog catalog = new ProductCatalog();
OrderGenerator orderGenerator = new OrderGenerator(catalog);
PaymentGenerator paymentGenerator = new PaymentGenerator();
CashDrawer cashDrawer = new CashDrawer();
Random random = new Random();

Console.WriteLine("=== Tindahan ni Aling Nena ===");
Console.WriteLine("Starting drawer: P" + cashDrawer.GetTotalAmount());
Console.WriteLine();

for (int customerNumber = 1; customerNumber <= 3; customerNumber++)
{
    Console.WriteLine("--- Customer " + customerNumber + " ---");

    // 1. make an order
    Order order = orderGenerator.GenerateOrder(1, 4);

    foreach (Product product in order.GetProducts())
    {
        Console.WriteLine("  " + product.GetName() + " - P" + product.GetPrice());
    }

    int orderTotal = order.GetTotalCost();
    Console.WriteLine("Total: P" + orderTotal);

    // 2. pick a random customer type
    Customer customer;
    int customerType = random.Next(3);

    if (customerType == 0)
    {
        customer = new ExactPayingCustomer(order);
    }
    else if (customerType == 1)
    {
        customer = new OverPayingCustomer(order);
    }
    else
    {
        customer = new UnderPayingCustomer(order);
    }

    Console.WriteLine("Customer type: " + customer.GetType().Name);

    // 3. the customer pays
    List<Denomination> payment = customer.GeneratePayment(paymentGenerator);
    int amountPaid = 0;

    foreach (Denomination piece in payment)
    {
        Console.WriteLine("  paid: P" + piece.Value);
        amountPaid += piece.Value;
    }

    Console.WriteLine("Amount paid: P" + amountPaid);

    // 4. the result
    if (amountPaid == orderTotal)
    {
        Console.WriteLine("Exact payment. No change needed.");
    }
    else if (amountPaid > orderTotal)
    {
        Console.WriteLine("Change owed: P" + (amountPaid - orderTotal));
    }
    else
    {
        Console.WriteLine("Short by P" + (orderTotal - amountPaid) + ". Customer: \"Kulang pa!\"");
    }

    // 5. the payment goes into the drawer
    cashDrawer.AddPieces(payment);
    Console.WriteLine("Drawer now: P" + cashDrawer.GetTotalAmount());
    Console.WriteLine();
}
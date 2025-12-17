namespace StoreManagementBlazorApp.Entities;

public enum PaymentMethod
{
    cash,
    card,
    bank_transfer,
    e_wallet
}

public class Payment
{
    public int payment_id { get; set; }
    public int order_id { get; set; }
    public decimal amount { get; set; }
    public PaymentMethod payment_method { get; set; }
    public DateTime payment_date { get; set; }
}

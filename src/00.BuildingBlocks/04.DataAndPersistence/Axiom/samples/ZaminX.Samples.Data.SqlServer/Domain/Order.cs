namespace ZaminX.Samples.Data.SqlServer.Domain;

public class Order
{
    public long Id { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Description { get; set; } = string.Empty;
}
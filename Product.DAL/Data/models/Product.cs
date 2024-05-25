namespace Product.DAL;

public class Product
{
    public Guid Id { get; set; }
    public string productName { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal price { get; set; }
    public string Image { get; set; } = "";
}

namespace Product.BL;
public class productDto
{
    public Guid Id { get; set; }
    public string productName { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal price { get; set; }
    public string Image { get; set; } = "";
}

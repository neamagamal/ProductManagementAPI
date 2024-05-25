namespace Product.BL;
public class ProductAddDto
{
    public string productName { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal price { get; set; }
    public string Image { get; set; } = "";
}

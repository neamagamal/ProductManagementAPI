namespace Product.DAL;
public class ProductRepo : GenericRepo<Product>, IProductRepo
{
    #region Fields
    private readonly ProductContext _Context;
    #endregion

    #region Ctor
    public ProductRepo(ProductContext context) : base(context)
    {
        _Context = context;
    }
    #endregion

}

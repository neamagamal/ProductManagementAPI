namespace Product.BL;
using Product.DAL;

public class ProductManager : IProductManager
{
    #region Fields
    public readonly IProductRepo _productRepo;
    #endregion
    #region Ctor

    public ProductManager(IProductRepo productRepo)
    {
        _productRepo = productRepo;
    }
    #endregion
    #region Method

    public List<productDto> GetAll()
    {


        return _productRepo.GetAll().Select(x => new productDto
        {
            Id = x.Id,
            productName = x.productName,
            Description = x.Description,
            price = x.price,
            Image = x.Image,
        }).ToList();
    }
    public productDto? GetById(Guid id)
    {
        var dpproduct = _productRepo.GetById(id);
        if (dpproduct == null)
        {
            return null;
        }
        var productDto = new productDto
        {
            Id = dpproduct.Id,
            productName = dpproduct.productName,
            Description = dpproduct.Description,
            price = dpproduct.price,
            Image = dpproduct.Image
        };
        return productDto;
    }
    public productDto Add(ProductAddDto product)
    {

        var addedProduct = new Product
        {
            Id = Guid.NewGuid(),
            productName = product.productName,
            Description = product.Description,
            Image = product.Image,
            price = product.price
        };

        _productRepo.Add(addedProduct);
        _productRepo.SaveChanges();

        var productReadDTO = new productDto
        {
            Id = addedProduct.Id,
            productName = addedProduct.productName,
            Description = addedProduct.Description,
            price = addedProduct.price,
            Image = addedProduct.Image
        };
        return productReadDTO;

    }
    public bool Update(productDto productDto)
    {
        var UpdateProduct = _productRepo.GetById(productDto.Id);
        if (UpdateProduct == null) { return false; }
        UpdateProduct.productName = productDto.productName;
        UpdateProduct.Description = productDto.Description;
        UpdateProduct.Image = productDto.Image;
        UpdateProduct.price = productDto.price;
        _productRepo.Add(UpdateProduct);
        _productRepo.SaveChanges();
        return true;
    }
    public void Delete(Guid id)
    {
        _productRepo.GetById(id);
        _productRepo.SaveChanges();
    }
    #endregion






}

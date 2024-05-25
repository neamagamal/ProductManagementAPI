namespace Product.BL;
public interface IProductManager
{
    List<productDto> GetAll();
    productDto? GetById(Guid id);
    productDto Add(ProductAddDto product);
    bool Update(productDto productDto);
    void Delete(Guid id);

}

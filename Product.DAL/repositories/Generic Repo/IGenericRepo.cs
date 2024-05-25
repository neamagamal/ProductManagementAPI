
namespace Product.DAL;
public interface IGenericRepo<TEnitiy> where TEnitiy : class
{
    List<TEnitiy> GetAll();
    TEnitiy GetById(Guid id);
    void Add(TEnitiy enitiy);
    void Update(TEnitiy enitiy);
    void Delete(Guid id);
    void SaveChanges();


}

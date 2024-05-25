namespace Product.DAL;
public class GenericRepo<TEntity> : IGenericRepo<TEntity> where TEntity : class
{
    #region Fields
    private readonly ProductContext _Context;
    #endregion
    #region Ctor
    public GenericRepo(ProductContext context)
    {

        _Context = context;

    }
    #endregion
    #region Method 
    public List<TEntity> GetAll()
    {

        return _Context.Set<TEntity>().ToList();

    }
    public TEntity GetById(Guid id)
    {
        return _Context.Set<TEntity>().Find(id);
    }

    public void Add(TEntity enitiy)
    {
        _Context.Set<TEntity>().Add(enitiy);
    }
    public void Update(TEntity enitiy)
    {

    }
    public void Delete(Guid id)
    {

        var DeletedEntity = GetById(id);
        if (DeletedEntity != null)
        {
            _Context.Set<TEntity>().Remove(DeletedEntity);
        }
    }
    public void SaveChanges()
    {
        _Context.SaveChanges();
    }

    #endregion
}

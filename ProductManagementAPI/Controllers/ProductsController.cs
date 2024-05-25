using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.BL;

namespace ProductManagementAPI.Controllers;

[Authorize(Policy = "ManagerPolicy")]
[Route("api/[controller]")]
[ApiController]

public class ProductsController : ControllerBase
{
    public readonly IProductManager _productManager;
    public ProductsController(IProductManager productManager)
    {
        _productManager = productManager;
    }
    [HttpGet]
    public ActionResult<IEnumerable<productDto>> GetProduct()
    {
        return _productManager.GetAll();
    }
    [HttpGet("{id}")]
    public ActionResult<productDto> GetProduct(Guid id)
    {
        var GetPdt = _productManager.GetById(id);
        if (GetPdt == null)
        {
            return NotFound();
        }
        return GetPdt;
    }

    [HttpPost]
    public ActionResult<productDto> Add(ProductAddDto product)
    {
        var AddPdt = _productManager.Add(product);
        return CreatedAtAction("GetProduct", new { id = Guid.NewGuid() }, product);
    }
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, productDto product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }
        var UpdatedPdt = _productManager.Update(product);
        if (UpdatedPdt == true)
        {
            return NoContent();
        }
        return NotFound();

    }
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var result = _productManager.GetById(id);
        if (result == null)
        {
            return NotFound();
        }
        _productManager.Delete(id);
        return NoContent();
    }


}

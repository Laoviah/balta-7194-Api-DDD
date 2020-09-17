using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

[Route("products")]
public class ProductController : ControllerBase{


    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context){
        
        var products = await context.Products.Include(x => x.Category).AsNoTracking().ToListAsync<Product>();

        return Ok(products);
    }

    [HttpGet]
    [Route("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Product>> GetById(int id, [FromServices] DataContext context){

        var product = await context.Products.Include(x => x.Category).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        
        return Ok(product);
    }

    [HttpGet]
    [Route("categories/{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Product>>> GetByCategory(int id, [FromServices] DataContext context){
        
        var products = await context.Products.Include(x => x.Category).AsNoTracking().Where(x => x.Category.Id == id).ToListAsync<Product>();

        return Ok(products);
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles="employee")]
    public async Task<ActionResult<List<Product>>> Post([FromBody]Product model, [FromServices] DataContext context){
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Products.Add(model);
            await context.SaveChangesAsync();

            return Ok(model);
        }
        catch
        {  
            return BadRequest(new {message = "Não foi possível criar a categoria"});
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles="manager")]
    public async Task<ActionResult<List<Product>>> Put(int id, [FromBody]Product model, [FromServices] DataContext context){
        
        if(id != model.Id)
            return NotFound(new {message="Categoria não encontrada"});

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Entry<Product>(model).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return Ok(model); 
        }
        catch
        {            
            return BadRequest(new {message = "Não foi possível atualizar a categoria"});
        }
       
    }


    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles="manager")]
    public async Task<ActionResult<List<Product>>> Delete(int id, [FromServices] DataContext context){
        
        var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if(product == null)
            return NotFound(new {message = "Categoria não encontrada"});
        
        try
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();

            return Ok(new { message= "Categoria removida com sucesso" });
        }
        catch
        {   
            return BadRequest(new { message= "Não foi possível remover a categoria" });
        }
    }
}
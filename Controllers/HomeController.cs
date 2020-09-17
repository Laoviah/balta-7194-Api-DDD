using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers{

    public class HomeController: Controller{

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> GetTask([FromServices] DataContext context){
            var employee = new User { Id = 1, Username="user1", Password="123", Role="employee"};
            var manager  = new User { Id = 2, Username="user2", Password="123", Role="manager"};
            var category = new Category { Id = 1, Title="Informática"};
            var product  = new Product { Id= 1, Category = category, Title="Mouse", Price=299, Description="Descrição teste"};

            context.Users.Add(employee);
            context.Users.Add(manager);
            context.Categories.Add(category);
            context.Products.Add(product);

            await context.SaveChangesAsync();

            return Ok(new{ message = "Dado configurados com sucesso" });

        }
    }

}
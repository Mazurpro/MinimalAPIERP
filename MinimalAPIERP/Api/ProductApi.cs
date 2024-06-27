using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MinimalAPIERP.Data;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP.Api;

internal static class ProductApi
{
    public static RouteGroupBuilder MapProductApi(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/erp")
            .WithTags("Product Api");

        // Configuración de opciones de serialización JSON
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        // GET: Obtener todos los productos
        group.MapGet("/products", async (AppDbContext db) =>
            await db.Products
                .Select(p => new { p.ProductId, p.Title, p.Price })
                .ToListAsync()
                is IList<object> products
                    ? Results.Json(products, options)
                    : Results.NotFound())
            .WithOpenApi();

        // POST: Crear un nuevo producto
        group.MapPost("/products", async (Product newProduct, AppDbContext db) =>
        {
            db.Products.Add(newProduct);
            await db.SaveChangesAsync();

            var response = new
            {
                newProduct.ProductId,
                newProduct.Title,
                newProduct.Price
            };

            return Results.Created($"/erp/products/{newProduct.ProductId}", response);
        })
        .WithOpenApi();

        // PUT: Actualizar un producto existente
        group.MapPut("/products/{productId}", async (int productId, Product updatedProduct, AppDbContext db) =>
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
            {
                return Results.NotFound();
            }

            product.Title = updatedProduct.Title;
            product.Price = updatedProduct.Price;
            // Actualiza otras propiedades según sea necesario

            await db.SaveChangesAsync();

            var response = new
            {
                product.ProductId,
                product.Title,
                product.Price
            };

            return Results.Ok(response);
        })
        .WithOpenApi();

        // DELETE: Eliminar un producto existente
        group.MapDelete("/products/{productId}", async (int productId, AppDbContext db) =>
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
            {
                return Results.NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithOpenApi();

        return group;
    }
}

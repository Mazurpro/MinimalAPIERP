using ERP;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MinimalAPIERP.Data;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MinimalAPIERP.Api;

internal static class StoreApi
{
    public static RouteGroupBuilder MapStoreApi(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/erp")
            .WithTags("Store Api");

        group.MapGet("/user", (ClaimsPrincipal user) =>
        {
            return user.Identity;

        })
        .WithOpenApi();

        group.MapGet("/store/{storeid}", async (int storeid, AppDbContext db) =>
            await db.Stores.FirstOrDefaultAsync(m => m.StoreId == storeid)
                is Store store
                    ? Results.Ok(store)
                    : Results.NotFound())
            .WithOpenApi();


        group.MapGet("/storea", async (AppDbContext db) =>
            await db.Stores.ToListAsync()
                is IList<Store> stores
                    ? Results.Ok(stores)
                    : Results.NotFound())

            .WithOpenApi();


        group.MapGet("/storeb", async (AppDbContext db, int pageSize = 10, int page = 0) =>
            await db.Stores.Skip(page * pageSize).Take(pageSize).ToListAsync()
                is IList<Store> stores
                    ? Results.Ok(stores)
                    : Results.NotFound())
            .WithOpenApi();

        group.MapGet("/storec1", async (AppDbContext db, int pageSize = 10, int page = 0) =>
            await db.Stores
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(store => new { store.StoreId, store.Name })
            .ToListAsync()
                is IList<Store> stores
                    ? Results.Ok(stores)
                    : Results.NotFound())
            .WithOpenApi();

        group.MapGet("/storec2", async (AppDbContext db, int pageSize = 10, int page = 0) =>
        {
            var data = await db.Stores
                .Skip(page * pageSize)
                .Take(pageSize)
                .Include(s => s.Rainchecks)
                .Select(store => new { store.StoreId, store.Name })
                .ToListAsync();

            return data.Any()
                ? Results.Ok(data)
                : Results.NotFound();
        })
        .WithOpenApi();

        // TODO: Mover a config
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web) {
            //PropertyNameCaseInsensitive = false,
            //PropertyNamingPolicy = null,
            WriteIndented = true,
            //IncludeFields = false,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            //ReferenceHandler = ReferenceHandler.Preserve
        };

        group.MapGet("/stored", async (AppDbContext db) =>
            await db.Stores.Include(s => s.Rainchecks).ToListAsync()
                is IList<Store> stores
                    ? Results.Ok(stores)
                    : Results.NotFound())
            .WithOpenApi();

        group.MapGet("/storee", async (AppDbContext db) =>
            await db.Stores.Include(s => s.Rainchecks).ToListAsync()
                is IList<Store> stores
                    ? Results.Json(stores, options)
                    : Results.NotFound())
            .WithOpenApi();

        group.MapGet("/storef", async (AppDbContext db) =>
            await db.Stores.Include(s => s.Rainchecks).ToListAsync()
                is IList<Store> stores
                    ? Results.Json(stores, options)
                    : Results.NotFound())
            .WithOpenApi();

        group.MapPost("/store", async (Store newStore, AppDbContext db) =>
        {
            db.Stores.Add(newStore);
            await db.SaveChangesAsync();

            var response = new 
            {
                newStore.StoreId,
                newStore.Name
            };

            return Results.Created($"/erp/store/{newStore.StoreId}", response);
        })
            .WithOpenApi();

        group.MapPut("/store/{storeid}", async (int storeid, Store updatedStore, AppDbContext db) =>
        {
            var store = await db.Stores.FirstOrDefaultAsync(m => m.StoreId == storeid);
            if (store == null)
            {
                return Results.NotFound();
            }

            store.Name = updatedStore.Name; 

            db.Stores.Update(store);

            try
            {
                await db.SaveChangesAsync();
                return Results.NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Results.BadRequest(); 
            }
        })
.WithOpenApi();

        group.MapDelete("/store/{storeid}", async (int storeid, AppDbContext db) =>
        {
            var store = await db.Stores.FirstOrDefaultAsync(m => m.StoreId == storeid);
            if (store is null)
            {
                return Results.NotFound();
            }

            db.Stores.Remove(store);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithOpenApi();

        return group;
    }
}

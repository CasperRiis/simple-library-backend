using LibraryApi.Models;
using LibraryApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryApi.Managers;

public class GenericCRUDService<T> where T : class
{
    private readonly DatabaseContext _context;

    public GenericCRUDService(DatabaseContext context)
    {
        _context = context;
    }

    private IQueryable<T> IncludeProperties(params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query;
    }

    public async Task<PagedList<T>> GetItems(int page, int pageSize, string? searchParameter, string searchProperty, params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = IncludeProperties(includes!);

        var property = typeof(T).GetProperty(searchProperty)
                            ?? throw new ArgumentNullException($"Property '{searchProperty}' does not exist on type '{typeof(T).Name}'");

        if (!string.IsNullOrEmpty(searchParameter))
        {
            query = query.Where(a => EF.Property<string>(a, searchProperty).Contains(searchParameter));
        }

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var nextPage = items.Count == pageSize ? (int?)page + 1 : null;

        return new PagedList<T>
        {
            Count = items.Count,
            Results = items,
            Next = nextPage
        };
    }

    public async Task<T> GetItem(int id, params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = IncludeProperties(includes!);

        var item = await query.SingleOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        return item != null ? item : throw new ArgumentNullException(nameof(item));
    }

    public async Task<T> GetItem(string searchParameter, string searchProperty, params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = IncludeProperties(includes!);

        var property = typeof(T).GetProperty(searchProperty)
                            ?? throw new ArgumentNullException($"Property '{searchProperty}' does not exist on type '{typeof(T).Name}'");

        var items = await query.ToListAsync();
        var item = items.FirstOrDefault(a => property.GetValue(a)?.ToString()?.ToLower().Contains(searchParameter.ToLower()) == true);
        return item != null ? item : throw new ArgumentNullException(nameof(item));
    }

    public async Task<T> AddItem(T item, string propertyName, params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = IncludeProperties(includes!);

        var property = typeof(T).GetProperty(propertyName)
                        ?? throw new ArgumentNullException($"Property '{propertyName}' does not exist on type '{typeof(T).Name}'");

        var propertyValue = property.GetValue(item)
                        ?? throw new ArgumentNullException($"Property '{propertyName}' value is null");

        var items = await query.ToListAsync();
        if (items.Any(a => property.GetValue(a)?.Equals(propertyValue) == true))
        {
            throw new Exception($"Item with same {propertyName} already exists");
        }

        _context.Set<T>().Add(item);
        await _context.SaveChangesAsync();

        return item;
    }

    public async Task<T> UpdateItem(T item, string propertyName, params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = IncludeProperties(includes!);

        var property = typeof(T).GetProperty(propertyName)
                        ?? throw new ArgumentNullException($"Property '{propertyName}' does not exist on type '{typeof(T).Name}'");

        var idProperty = typeof(T).GetProperty("Id")
                        ?? throw new ArgumentNullException($"Property 'Id' does not exist on type '{typeof(T).Name}'");

        var propertyValue = property.GetValue(item)
                        ?? throw new ArgumentNullException($"Property '{propertyName}' value is null");

        var itemId = idProperty.GetValue(item)
                        ?? throw new ArgumentNullException($"Property 'Id' value is null");

        var items = await query.ToListAsync();
        if (items.Any(a => property.GetValue(a)?.Equals(propertyValue) == true && !idProperty.GetValue(a)?.Equals(itemId) == true))
        {
            throw new Exception($"Item with same {propertyName} and different id already exists");
        }

        var dbItem = await GetItem((int)itemId, includes);
        if (dbItem == null)
        {
            throw new ArgumentNullException(nameof(dbItem));
        }

        _context.Entry(dbItem).CurrentValues.SetValues(item);
        await _context.SaveChangesAsync();
        return dbItem;
    }

    public async Task<T> DeleteItem(int id, params Expression<Func<T, object>>[]? includes)
    {
        var item = await GetItem(id, includes);
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        _context.Set<T>().Remove(item);
        await _context.SaveChangesAsync();

        return item;
    }
}
using System.Linq.Expressions;
using TodoList.Domain.Entities;

namespace TodoList.Domain
{
    public interface ITodoItemRepository
    {
        Task<TodoItem> GetByIdAsync(Guid id);
        Task<List<TodoItem>> GetAllAsync();
        Task<List<TodoItem>> GetAllAsync(Expression<Func<TodoItem, bool>> predicate);
        Task<TodoItem> AddAsync(TodoItem item);
        Task<TodoItem> UpdateAsync(TodoItem item);
        Task DeleteAsync(Guid id);
    }

}

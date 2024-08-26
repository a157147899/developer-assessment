using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoList.Domain;
using TodoList.Domain.Entities;

namespace TodoList.Infrastructure
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly TodoContext _context;
        private const int _maxRecords = 1000;

        public TodoItemRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<List<TodoItem>> GetAllAsync()
        {
            return await _context.TodoItems.OrderByDescending(x => x.CreatedDate).Take(_maxRecords).ToListAsync();
        }

        public async Task<List<TodoItem>> GetAllAsync(Expression<Func<TodoItem, bool>> predicate)
        {
            return await _context.TodoItems.Where(predicate).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<TodoItem> GetByIdAsync(Guid id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task<TodoItem> AddAsync(TodoItem item)
        {
            await _context.TodoItems.AddAsync(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<TodoItem> UpdateAsync(TodoItem item)
        {
            var existingItem = await _context.TodoItems.FindAsync(item.Id) ?? throw new InvalidOperationException("Todo item not found.");

            existingItem.IsCompleted = item.IsCompleted;
            existingItem.Description = item.Description;
            existingItem.CompletedDate = item.CompletedDate;

            await _context.SaveChangesAsync();

            return item;
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if (item != null)
            {
                _context.TodoItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

    }

}

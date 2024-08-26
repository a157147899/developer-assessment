using TodoList.Domain.Entities;

namespace TodoList.Application.Services
{
    public interface ITodoItemService
    {
        Task<List<TodoItem>> GetAllTodoItemsAsync();
        Task<TodoItem> GetTodoItemAsync(Guid id);
        Task<TodoItem> CreateTodoItemAsync(TodoItem item);
        Task<TodoItem> UpdateTodoItemAsync(TodoItem item);
        Task DeleteTodoItemAsync(Guid id);
    }
}

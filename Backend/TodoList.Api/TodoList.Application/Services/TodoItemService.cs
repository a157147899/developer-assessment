using FluentValidation;
using FluentValidation.Results;
using TodoList.Application.Validators;
using TodoList.Domain;
using TodoList.Domain.Entities;

namespace TodoList.Application.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _repository;
        private readonly ICreateTodoItemValidator _createValidator;
        private readonly IUpdateTodoItemValidator _updateValidator;

        public TodoItemService(ITodoItemRepository repository
            , ICreateTodoItemValidator createValidator
            , IUpdateTodoItemValidator updateValidator)
        {
            _repository = repository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<List<TodoItem>> GetAllTodoItemsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<TodoItem> GetTodoItemAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<TodoItem> CreateTodoItemAsync(TodoItem item)
        {
            ValidationResult validationResult = await _createValidator.ValidateAsync(item);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _repository.AddAsync(item);
        }

        public async Task<TodoItem> UpdateTodoItemAsync(TodoItem item)
        {
            ValidationResult validationResult = await _updateValidator.ValidateAsync(item);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            item.MarkAsCompleted();

            return await _repository.UpdateAsync(item);
        }

        public async Task DeleteTodoItemAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }

}

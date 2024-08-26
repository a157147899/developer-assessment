using FluentValidation;
using TodoList.Domain;
using TodoList.Domain.Entities;

namespace TodoList.Application.Validators
{
    public interface IUpdateTodoItemValidator : IValidator<TodoItem> { }

    public class UpdateTodoItemRequestValidator : AbstractValidator<TodoItem>, IUpdateTodoItemValidator
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public UpdateTodoItemRequestValidator(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;

            RuleFor(x => x.Id)
                .NotNull().NotEqual(Guid.Empty).WithMessage("Id is invalid.")
                .MustAsync(ExistTodoItemAsync).WithMessage("Could not find the todo item by Id '{PropertyValue}'");

            RuleFor(x => x.Description)
              .NotEmpty().WithMessage("Description is required.")
              .MaximumLength(200).WithMessage("Description cannot exceed 200 characters.");
        }

        private async Task<bool> ExistTodoItemAsync(Guid id, CancellationToken cancellationToken)
        {
            var todoItem = await _todoItemRepository.GetByIdAsync(id);
            return todoItem != null;
        }

    }
}

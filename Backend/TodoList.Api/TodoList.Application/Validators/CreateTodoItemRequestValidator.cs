using FluentValidation;
using TodoList.Domain;
using TodoList.Domain.Entities;

namespace TodoList.Application.Validators
{
    public interface ICreateTodoItemValidator : IValidator<TodoItem> { }

    public class CreateTodoItemRequestValidator : AbstractValidator<TodoItem>, ICreateTodoItemValidator
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public CreateTodoItemRequestValidator(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(200).WithMessage("Description cannot exceed {MaxLength} characters.")
                .MustAsync(HasNoDuplicateUncompletedTodoItemAsync).WithMessage("A todo item with the same description already exists.");
        }

        private async Task<bool> HasNoDuplicateUncompletedTodoItemAsync(string desc, CancellationToken cancellationToken)
        {
            var todoItem = await _todoItemRepository.GetAllAsync(x=>x.Description==desc && !x.IsCompleted);
            return !todoItem.Any();
        }
    }
}

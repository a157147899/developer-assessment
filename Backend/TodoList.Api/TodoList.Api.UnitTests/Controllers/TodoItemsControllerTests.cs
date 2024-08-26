using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TodoList.Api.Controllers;
using TodoList.Api.Models;
using TodoList.Application.Services;
using TodoList.Domain.Entities;
using Xunit;
using FluentValidation;

namespace TodoList.Api.Tests.Controllers
{
    public class TodoItemsControllerTests
    {
        private readonly Mock<ITodoItemService> _todoItemServiceMock;
        private readonly Mock<ILogger<TodoItemsController>> _loggerMock;
        private readonly TodoItemsController _controller;

        public TodoItemsControllerTests()
        {
            _todoItemServiceMock = new Mock<ITodoItemService>();
            _loggerMock = new Mock<ILogger<TodoItemsController>>();
            _controller = new TodoItemsController(_todoItemServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetTodoItems_ReturnsOkResult_WithListOfTodoItems()
        {
            // Arrange
            var todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid(), Description = "Test Item 1", IsCompleted = false, CreatedDate = DateTime.Now },
                new TodoItem { Id = Guid.NewGuid(), Description = "Test Item 2", IsCompleted = true, CreatedDate = DateTime.Now.AddDays(-1), CompletedDate = DateTime.Now }
            };
            _todoItemServiceMock.Setup(service => service.GetAllTodoItemsAsync()).ReturnsAsync(todoItems);

            // Act
            var result = await _controller.GetTodoItems();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<TodoItemResponse>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetTodoItem_ReturnsOkResult_WithTodoItemResponse()
        {
            // Arrange
            var todoItem = new TodoItem { Id = Guid.NewGuid(), Description = "Test Item", IsCompleted = false, CreatedDate = DateTime.Now };
            _todoItemServiceMock.Setup(service => service.GetTodoItemAsync(It.IsAny<Guid>())).ReturnsAsync(todoItem);

            // Act
            var result = await _controller.GetTodoItem(todoItem.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TodoItemResponse>(okResult.Value);
            Assert.Equal(todoItem.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetTodoItem_ReturnsNotFound_WhenTodoItemNotFound()
        {
            // Arrange
            _todoItemServiceMock.Setup(service => service.GetTodoItemAsync(It.IsAny<Guid>())).ReturnsAsync((TodoItem)null);

            // Act
            var result = await _controller.GetTodoItem(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutTodoItem_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todoItemRequest = new TodoItemRequest { Id = Guid.NewGuid(), Description = "Test Item", IsCompleted = false };

            // Act
            var result = await _controller.PutTodoItem(id, todoItemRequest);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutTodoItem_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todoItemRequest = new TodoItemRequest { Id = id, Description = "Test Item", IsCompleted = false };

            _todoItemServiceMock.Setup(service => service.UpdateTodoItemAsync(It.IsAny<TodoItem>())).ReturnsAsync(new TodoItem());

            // Act
            var result = await _controller.PutTodoItem(id, todoItemRequest);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutTodoItem_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todoItemRequest = new TodoItemRequest { Id = id, Description = "Test Item", IsCompleted = false };

            _todoItemServiceMock.Setup(service => service.UpdateTodoItemAsync(It.IsAny<TodoItem>()))
                                .ThrowsAsync(new ValidationException("Validation failed"));

            // Act
            var result = await _controller.PutTodoItem(id, todoItemRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Validation failed", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task PostTodoItem_ReturnsCreatedAtAction_WhenSuccessful()
        {
            // Arrange
            var todoItemRequest = new TodoItemRequest { Description = "New Item", IsCompleted = false };
            var todoItem = new TodoItem { Id = Guid.NewGuid(), Description = "New Item", IsCompleted = false, CreatedDate = DateTime.Now };

            _todoItemServiceMock.Setup(service => service.CreateTodoItemAsync(It.IsAny<TodoItem>())).ReturnsAsync(todoItem);

            // Act
            var result = await _controller.PostTodoItem(todoItemRequest);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<TodoItemResponse>(createdAtActionResult.Value);
            Assert.Equal(todoItem.Id, returnValue.Id);
        }

        [Fact]
        public async Task PostTodoItem_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var todoItemRequest = new TodoItemRequest { Description = "New Item", IsCompleted = false };

            _todoItemServiceMock.Setup(service => service.CreateTodoItemAsync(It.IsAny<TodoItem>()))
                                .ThrowsAsync(new ValidationException("Validation failed"));

            // Act
            var result = await _controller.PostTodoItem(todoItemRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Validation failed", badRequestResult.Value.ToString());
        }
    }
}

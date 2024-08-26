using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Models;
using TodoList.Application.Services;
using TodoList.Domain.Entities;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;
        private readonly ILogger<TodoItemsController> _logger;

        public TodoItemsController(ITodoItemService todoItemService, ILogger<TodoItemsController> logger)
        {
            _todoItemService = todoItemService;
            _logger = logger;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            try
            {
                var todoItems = await _todoItemService.GetAllTodoItemsAsync();

                return Ok(todoItems.Adapt<List<TodoItemResponse>>());
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        // GET: api/TodoItems/...
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(Guid id)
        {
            try
            {
                var todoItem = await _todoItemService.GetTodoItemAsync(id);

                if (todoItem == null)
                {
                    return NotFound();
                }

                return Ok(todoItem.Adapt<TodoItemResponse>());
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        // PUT: api/TodoItems/... 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem([FromRoute] Guid id, [FromBody] TodoItemRequest todoItemRequest)
        {
            if (id != todoItemRequest.Id)
            {
                return BadRequest();
            }

            try
            {
                await _todoItemService.UpdateTodoItemAsync(todoItemRequest.Adapt<TodoItem>());
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors.Any() ? string.Join(";", ex.Errors) : ex.Message);
            }
            catch (Exception)
            {
                return Problem();
            }

            return NoContent();
        }

        // POST: api/TodoItems 
        [HttpPost]
        public async Task<IActionResult> PostTodoItem([FromBody] TodoItemRequest todoItemRequest)
        {
            TodoItemResponse todoItemResponse;

            try
            {
                var updatedTodoItem = await _todoItemService.CreateTodoItemAsync(todoItemRequest.Adapt<TodoItem>());
                todoItemResponse = updatedTodoItem.Adapt<TodoItemResponse>();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors.Any() ? string.Join(";", ex.Errors) : ex.Message);
            }
            catch (Exception)
            {
                return Problem();
            }

            return CreatedAtAction(nameof(PostTodoItem), todoItemResponse);
        }

    }
}

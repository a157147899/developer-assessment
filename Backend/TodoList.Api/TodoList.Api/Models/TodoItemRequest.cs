using System;

namespace TodoList.Api.Models
{
    public class TodoItemRequest
    {
        public Guid? Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }

}

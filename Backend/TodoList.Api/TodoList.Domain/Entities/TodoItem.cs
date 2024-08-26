namespace TodoList.Domain.Entities
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? CompletedDate { get; set; }

        public TodoItem()
        {

        }

        /// <summary>
        /// Mark the todo item as completed
        /// </summary>
        public void MarkAsCompleted()
        {
            IsCompleted = true;
            CompletedDate = DateTime.Now;
        }

    }
}

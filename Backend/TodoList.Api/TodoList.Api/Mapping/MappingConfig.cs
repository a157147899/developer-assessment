using Mapster;
using TodoList.Api.Models;
using TodoList.Domain.Entities;

namespace TodoList.Api.Mapping
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<TodoItemRequest, TodoItem>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsCompleted, src => src.IsCompleted);

            TypeAdapterConfig<TodoItem, TodoItemResponse>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsCompleted, src => src.IsCompleted)
                .Map(dest => dest.CreatedDate, src => src.CreatedDate)
                .Map(dest => dest.CompletedDate, src => src.CompletedDate);
        }

    }
}

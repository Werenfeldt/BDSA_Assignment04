using System.Collections.Generic;

namespace Assignment4.Core
{
    public record TaskDTO
    {
        public TaskDTO(int id, string title, int? userId, string description, State taskState)
        {
            Id = id;
            Title = title;
           
            Description = description;
            AssignedToId = userId;
            State = taskState;
        }

        public int Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public int? AssignedToId { get; init; }
        public IReadOnlyCollection<string> Tags { get; init; }
        public State State { get; init; }
    }
}
using System.Collections.Generic;

namespace Assignment4.Core
{
    public record TaskDetailsDTO
    {
        public TaskDetailsDTO(int id, string title, string description, int? userId, string name, string email, IReadOnlySet<string> tags, State taskState)
        {
            Id = id;
            Title = title;
            Description = description;
            AssignedToId = userId;
            AssignedToName = name;
            AssignedToEmail = email;
            Tags = tags;
            State = taskState;
        }

        public int Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public int? AssignedToId { get; init; }
        public string AssignedToName { get; init; }
        public string AssignedToEmail { get; init; }
        public IEnumerable<string> Tags { get; init; }
        public State State { get; init; }
        
    }

}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Assignment4.Core;

namespace Assignment4.Entities
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public int? UserId { get; set; }

        public User User { get; set; }

        public string? Description { get; set; }

        [Required]
        // [Column(TypeName = "nvarchar(24)")]
        public State TaskState { get; set; }

        public ICollection<Tag> Tags { get; set; }
    }
}

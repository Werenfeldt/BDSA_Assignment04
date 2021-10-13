using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Assignment4.Core;
using Microsoft.Data.SqlClient;
using static Assignment4.Core.Response;
using static Assignment4.Core.State;

namespace Assignment4.Entities
{
    public class TaskRepository : ITaskRepository
    {
        private readonly KanbanContext _context;

        public TaskRepository(KanbanContext context)
        {
            _context = context;
        }

        public (Response Response, int TaskId) Create(TaskCreateDTO task)
        {


            var c = new Task
            {
                Title = task.Title,
                UserId = task.AssignedToId,
                Description = task.Description,
                TaskState = State.New,
                Tags = GetTags(task),
            };


            _context.Tasks.Add(c);

            _context.SaveChanges();

            return (Created, c.Id);
        }

        public Response Delete(int taskId)
        {
            var task = _context.Tasks.Find(taskId);

            if (task == null)
            {
                return NotFound;
            }
            else
            {
                if (task.TaskState.Equals(Active))
                {
                    task.TaskState = Removed;
                    _context.SaveChanges();
                    return Updated;
                }
                else if (task.TaskState.Equals(New))
                {
                    _context.Tasks.Remove(task);
                    _context.SaveChanges();

                    return Deleted;
                }
                else
                {
                    return Conflict;
                }

            }
        }

        public TaskDetailsDTO Read(int taskId)
        {
            if (_context.Tasks.Find(taskId) == null)
            {
                return null;
            }
            else
            {
                return new TaskDetailsDTO(_context.Tasks.Find(taskId).Id, _context.Tasks.Find(taskId).Title, _context.Tasks.Find(taskId).Description, _context.Tasks.Find(taskId).User.Name, GetTags(_context.Tasks.Find(taskId).Tags), _context.Tasks.Find(taskId).TaskState);
            }

        }

        public IReadOnlyCollection<TaskDTO> ReadAll()
        {
            var list = _context.Tasks;

            var DTOList = new List<TaskDTO>();
            foreach (var task in list)
            {
                DTOList = helpRead(DTOList, task);

            }
            return DTOList.AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
        {
            var list = _context.Tasks;

            var DTOList = new List<TaskDTO>();
            foreach (var task in list)
            {
                if (task.TaskState == state)
                {
                    DTOList = helpRead(DTOList, task);
                }
            }
            return DTOList.AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
        {
            var list = _context.Tasks;

            var DTOList = new List<TaskDTO>();
            foreach (var task in list)
            {
                if (GetTags(task.Tags).Contains(tag))
                {
                    DTOList = helpRead(DTOList, task);
                }
            }
            return DTOList.AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
        {
            var list = _context.Tasks;

            var DTOList = new List<TaskDTO>();
            foreach (var task in list)
            {
                if (task.UserId == userId)
                {
                    DTOList = helpRead(DTOList, task);
                }
            }
            return DTOList.AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
        {
            var list = _context.Tasks;

            var DTOList = new List<TaskDTO>();
            foreach (var task in list)
            {
                if (task.TaskState == State.Removed)
                {
                    DTOList = helpRead(DTOList, task);

                }
            }
            return DTOList.AsReadOnly();
        }

        public Response Update(TaskUpdateDTO task)
        {
            var updatedTask = _context.Tasks.Find(task.Id);

            if (updatedTask == null)
            {
                return NotFound;
            }
            else
            {
                updatedTask.Title = task.Title;
                updatedTask.Description = task.Description;
                updatedTask.Tags = GetTags(task);
                updatedTask.TaskState = task.State;
                updatedTask.UserId = task.AssignedToId;

                return Updated;
            }
        }



        public List<Tag> GetTags(TaskCreateDTO task)
        {
            var tags = new List<Tag>();
            if (task.Tags != null)
            {
                foreach (var tag in task.Tags)
                {
                    tags.Add(new Tag { Name = tag });
                }
            }
            return tags;
        }
        public IReadOnlyCollection<string> GetTags(ICollection<Tag> task)
        {
            var tags = new List<string>();
            if (task != null)
            {
                foreach (var tag in task)
                {
                    tags.Add(tag.Name);
                }
            }
            return tags;
        }

        public List<TaskDTO> helpRead(List<TaskDTO> DTOList, Task task)
        {
            if (task.User == null || task.Tags == null)
            {
                if (task.User == null && task.Tags != null)
                {
                    DTOList.Add(new TaskDTO(task.Id, task.Title, null, GetTags(task.Tags), task.TaskState));
                }
                else if (task.User != null && task.Tags == null)
                {
                    DTOList.Add(new TaskDTO(task.Id, task.Title, task.User.Name, null, task.TaskState));
                }
                else
                {
                    DTOList.Add(new TaskDTO(task.Id, task.Title, null, null, task.TaskState));
                }

            }
            else
            {
                DTOList.Add(new TaskDTO(task.Id, task.Title, task.User.Name, GetTags(task.Tags), task.TaskState));
            }

            return DTOList;
        }

        public void Dispose()
        {

        }
    }

}

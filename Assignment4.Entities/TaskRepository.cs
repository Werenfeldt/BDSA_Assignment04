using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Assignment4.Core;
using Microsoft.Data.SqlClient;

namespace Assignment4.Entities
{
    public class TaskRepository : ITaskRepository
    {
        private readonly KanbanContext _context;

        public TaskRepository(KanbanContext context)
        {
            _context = context;
        }
        // public IReadOnlyCollection<TaskDTO> All() => _context.Tasks.Select(async t => new TaskDTO(t.Id, t.Title, t.UserId, t.Description, t.TaskState)).ToList().AsReadOnly();



        public int Create(TaskDTO task)
        {
            throw new NotImplementedException();

            // var taskEntity = new Task
            // {
            //     Title = task.Title,
            //     User = task.AssignedToName,
            //     TaskState = task.State
            // };

            // _context.Tasks.Add(taskEntity);

            // _context.SaveChanges();

            // return taskEntity.Id;
        }

        public (Response Response, int TaskId) Create(TaskCreateDTO task)
        {
            throw new NotImplementedException();
        }

        public void Delete(int taskId)
        {
            var taskEntity = _context.Tasks.Find(taskId);

            _context.Tasks.Remove(taskEntity);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public TaskDetailsDTO FindById(int id)
        {
            throw new NotImplementedException();
            
            // var task = _context.Tasks.Find(id);

            

            // // var tasks = from t in _context.Tasks
            // //             where t.Id == id
            // //             select new TaskDetailsDTO(
            // //                 t.Id,
            // //                 t.Title,
            // //                 t.Description
            // //                 // new DateTime.Today(),
            // //                 // t.User.Name,
            // //                 // t.Tags, 
            // //                 // t.TaskState,


            // //                 // t.UserId,
            // //                 // t.User.Name,
            // //                 // t.User.Email,
            // //                 // GetTags(t.Tags.Select(t => t.Name).ToString()).ToList(),
            // //                 // t.TaskState
            // //             //c.Powers.Select(c => c.Name).ToHashSet()
            // //             );

            // return tasks.FirstOrDefault();
        }

        public TaskDetailsDTO Read(int taskId)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAll()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
        {
            throw new NotImplementedException();
        }

        public void Update(TaskDTO task)
        {
            // var taskEntity = _context.Tasks.Find(task.Id);

            // taskEntity.Id = task.Id;
            // taskEntity.Title = task.Title;
            // taskEntity.UserId = task.AssignedToId;
            // taskEntity.Description = task.Description;
            // taskEntity.TaskState = task.State;


            // _context.SaveChanges();
        }

        public Response Update(TaskUpdateDTO task)
        {
            throw new NotImplementedException();
        }

        Response ITaskRepository.Delete(int taskId)
        {
            throw new NotImplementedException();
        }

        private Tag GetTag(string name) => _context.Tags.FirstOrDefault(c => c.Name == name) ?? new Tag { Name = name };

        private IEnumerable<Tag> GetTags(IEnumerable<string> tags)
        {
            var existing = _context.Tags.Where(t => tags.Contains(t.Name)).ToDictionary(t => t.Name);

            foreach (var tag in tags)
            {
                yield return existing.TryGetValue(tag, out var p) ? p : new Tag { Name = tag };
            }
        }
    }


}

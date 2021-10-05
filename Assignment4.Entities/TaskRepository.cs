using System.Collections.Generic;
using Assignment4.Core;

namespace Assignment4.Entities
{
     public class TaskRepository : ITaskRepository
    {
        public IReadOnlyCollection<TaskDTO> All()
        {
            return null;
        }

        public int Create(TaskDTO task)
        {
            return 0;
        }

        public void Delete(int taskId)
        {
        }

        public TaskDetailsDTO FindById(int id)
        {
            return null;
        }

        public void Update(TaskDTO task)
        {
            
        }

        public void Dispose()
        {
        }

        IReadOnlyCollection<TaskDTO> ITaskRepository.All()
        {
            throw new System.NotImplementedException();
        }
    }
}

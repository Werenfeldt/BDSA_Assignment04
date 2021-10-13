using System;
using System.Collections.Generic;
using Assignment4.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Text.Json;
using Newtonsoft.Json;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests : IDisposable
    {
        private readonly KanbanContext _context;

        public readonly TaskRepository _repo;
        public TaskRepositoryTests()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            var context = new KanbanContext(builder.Options);
            context.Database.EnsureCreated();
            var Tag1 = new Tag { Name = "Important" };
            var Tag2 = new Tag { Name = "Not-important" };
            var Tag3 = new Tag { Name = "Boring" };


            var task1 = new Task { Title = "Make Notes", TaskState = State.New, Tags = new List<Tag> { Tag1 } };
            var task2 = new Task { Title = "Be at meeting", TaskState = State.Closed, Tags = new List<Tag> { Tag2, Tag3 } };
            var task3 = new Task { Title = "Sleep", TaskState = State.Active, Tags = new List<Tag> { Tag2 } };
            context.Tasks.Add(task1);
            context.Tasks.Add(task2);
            context.Tasks.Add(task3);
            context.Users.AddRange(
                            new User
                            {
                                Name = "Bob The Builder",
                                Email = "bob@thebuilder.com",
                                Tasks = new List<Task>{
                                task1
                                }
                            },
                            new User
                            {
                                Name = "Alice",
                                Email = "Alice@worker.com",
                                Tasks = new List<Task>(){
                                task2
                                }
                            }
                        );
            context.SaveChanges();

            _context = context;
            _repo = new TaskRepository(_context);

        }

        [Fact]
        public void Create_Task_Return_Response_And_Taskid()
        {
            //Arrange
            var entity = new TaskCreateDTO { Title = "New Task every day", AssignedToId = 1 };

            //Act
            var expected = (Response.Created, 4);
            var actual = _repo.Create(entity);

            _context.SaveChanges();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Delete_Task_With_State_new_Response_Deleted()
        {
            //Arrange

            //Act
            var expected = (Response.Deleted);
            var actual = _repo.Delete(2);

            _context.SaveChanges();

            //Assert
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void Delete_Task_With_State_Response_Conflict()
        {
            //Arrange


            //Act
            var expected = (Response.Conflict);
            var actual = _repo.Delete(3);

            _context.SaveChanges();

            //Assert
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void Delete_Task_With_State_Active_Response_Updated()
        {
            //Arrange

            //Act
            var expected = (Response.Updated);
            var actual = _repo.Delete(1);

            _context.SaveChanges();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_task_reponse_updated()
        {
            //Arrange
            var task = new TaskUpdateDTO { Id = 1, Title = "Kør hjem" };

            //Act
            var expected = (Response.Updated);
            var actual = _repo.Update(task);

            _context.SaveChanges();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_task_reponse_notFound()
        {
            //Arrange
            var task = new TaskUpdateDTO { Id = 9, Title = "Kør hjem" };

            //Act
            var expected = (Response.NotFound);
            var actual = _repo.Update(task);

            _context.SaveChanges();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_With_TaskId()
        {
            //Arrange
            var task = new TaskDetailsDTO(2, "Make Notes", null, "Bob The Builder", new List<string> { "Important" }, State.New);
            var expected = JsonConvert.SerializeObject(task);
            //Act

            var actualTask = _repo.Read(2);
            var actual = JsonConvert.SerializeObject(actualTask);

            _context.SaveChanges();

            //Assert
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void Read_All_Tasks()
        {
            //Arrange
            var task = new List<TaskDTO> {
                new TaskDTO(1,"Sleep", null, new List<string> { "Not-important" }, State.Active),
                new TaskDTO(2, "Make Notes", "Bob The Builder", new List<string> { "Important" }, State.New),
                new TaskDTO(3, "Be at meeting", "Alice", new List<string> { "Not-important", "Boring" }, State.Closed),

            };
            var expected = JsonConvert.SerializeObject(task);
            //Act

            var actualTask = _repo.ReadAll();
            var actual = JsonConvert.SerializeObject(actualTask);

            _context.SaveChanges();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_All_Tasks_With_State()
        {
            //Arrange
            var task = new List<TaskDTO> {
                new TaskDTO(1,"Sleep", null, new List<string> { "Not-important" }, State.Active),
            };
            var expected = JsonConvert.SerializeObject(task);
            //Act

            var actualTask = _repo.ReadAllByState(State.Active);
            var actual = JsonConvert.SerializeObject(actualTask);

            _context.SaveChanges();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_All_Tasks_with_Tags()
        {
            //Arrange
            var task = new List<TaskDTO> {
                new TaskDTO(1,"Sleep", null, new List<string> { "Not-important" }, State.Active),
                new TaskDTO(3, "Be at meeting", "Alice", new List<string> { "Not-important", "Boring" }, State.Closed),

            };
            var expected = JsonConvert.SerializeObject(task);
            //Act

            var actualTask = _repo.ReadAllByTag("Not-important");
            var actual = JsonConvert.SerializeObject(actualTask);

            _context.SaveChanges();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_All_Tasks_by_UserId()
        {
            //Arrange
            var task = new List<TaskDTO> {
                new TaskDTO(3, "Be at meeting", "Alice", new List<string> { "Not-important", "Boring" }, State.Closed),

            };
            var expected = JsonConvert.SerializeObject(task);
            //Act

            var actualTask = _repo.ReadAllByUser(2);
            var actual = JsonConvert.SerializeObject(actualTask);

            _context.SaveChanges();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_All_Tasks_by_StateRemoved()
        {
            //Arrange
            _context.Add(new Task { Title = "Make Notes", TaskState = State.Removed });
            var task = new List<TaskDTO> {
                new TaskDTO(4, "Make Notes",null, null, State.Removed),

            };
            _context.SaveChanges();
            var expected = JsonConvert.SerializeObject(task);
            //Act

            var actualTask = _repo.ReadAllRemoved();
            var actual = JsonConvert.SerializeObject(actualTask);



            //Assert
            Assert.Equal(expected, actual);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

using System;
using System.Collections.Generic;
using Assignment4.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

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

        // [Fact]
        // public void Read_With_TagId()
        // {
        //     //Arrange
        //     //var expected = new TaskDetailsDTO (1, ) Title = "Make Notes", TaskState = State.New, Tags = new List<Tag> { Tag1 } };

        //     //Act
            
        //     //var actual = _repo.Read(1);

        //     //_context.SaveChanges();

        //     //Assert
        //     //Assert.Equal(expected, actual);
        //}

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

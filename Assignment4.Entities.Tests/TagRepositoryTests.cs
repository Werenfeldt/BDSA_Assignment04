using System;
using System.Collections.ObjectModel;
using Assignment4.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Assignment4.Entities.Tests
{
    public class TagRepositoryTests : IDisposable
    {
        private readonly KanbanContext _context;

        public readonly TagRepository _repo;

        public TagRepositoryTests()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);
            var context = new KanbanContext(builder.Options);
            context.Database.EnsureCreated();
            // context.Tasks.Add(new Task { Title = "Delete task with tags", Tags = new[] { new Tag { Name = "Mucho Important" } }, TaskState = State.Active });
            // context.SaveChanges();
            context.Tags.Add(new Tag { Name = "Semi Important", Tasks = new[] { new Task { Title = "Delete task with tags" } } });
            context.Tags.Add(new Tag { Name = "This is an error" });
            context.SaveChanges();


            _context = context;
            _repo = new TagRepository(_context);
        }

        [Fact]
        public void Create_Tag_Return_Response_And_Tagid()
        {
            //Arrange
            var entity = new TagCreateDTO { Name = "Much important" };

            //Act
            var expected = (Response.Created, 3);
            var actual = _repo.Create(entity);

            _context.SaveChanges();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Delete_Tag_Without_tasks_response_Deleted()
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
        public void Delete_Tag_With_tasks_response_Conflict()
        {
            //Arrange

            //Act
            var expected = (Response.Conflict);
            var actual = _repo.Delete(1);

            _context.SaveChanges();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Delete_Tag_With_tasks_and_force_response()
        {
            //Arrange


            //Act
            var expected = (Response.Deleted);
            var actual = _repo.Delete(1, true);

            _context.SaveChanges();

            //Assert
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void Delete_Tag_Nonexisting()
        {
            //Arrange


            //Act
            var expected = (Response.NotFound);
            var actual = _repo.Delete(9);

            _context.SaveChanges();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_Tag()
        {
            //Arrange
            var expected = new TagDTO(2, "This is an error");

            //Act
            var actual = _repo.Read(2);

            _context.SaveChanges();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_Tag_nonexisting()
        {
            //Arrange

            //Act
            var actual = _repo.Read(6);

            _context.SaveChanges();

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public void Read_All_Tags()
        {
            //Arrange
            var list = new[] { new TagDTO(1, "Semi Important"), new TagDTO(2, "This is an error") };
            var expected = new ReadOnlyCollection<TagDTO>(list);
            //Act
            var actual = _repo.ReadAll();

            _context.SaveChanges();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_All_Tags_empty()
        {
            //Arrange
            _repo.Delete(1, true);
            _repo.Delete(2);

            //Act
            var actual = _repo.ReadAll();

            _context.SaveChanges();

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public void Update_Tag()
        {
            //Arrage
            var updatedTag = new TagUpdateDTO { Id = 2, Name = "this is NOT an error" };
            var expected = Response.Updated;
            //Act
            var actual = _repo.Update(updatedTag);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_Tag_nonexisting()
        {
            //Arrage
            var updatedTag = new TagUpdateDTO { Id = 9, Name = "this is NOT an error" };
            var expected = Response.NotFound;
            //Act
            var actual = _repo.Update(updatedTag);

            //Assert
            Assert.Equal(expected, actual);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

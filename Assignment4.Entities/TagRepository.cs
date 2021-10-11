using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Assignment4.Core;
using static Assignment4.Core.Response;

namespace Assignment4.Entities
{
    public class TagRepository : ITagRepository
    {
        private readonly KanbanContext context;

        public TagRepository(KanbanContext context)
        {
            this.context = context;
        }

        public (Response Response, int TagId) Create(TagCreateDTO tag)
        {
            var entity = new Tag { Name = tag.Name };

            context.Tags.Add(entity);
            context.SaveChanges();

            return (Created, entity.Id);
        }

        public Response Delete(int tagId, bool force = false)
        {
            var tag = context.Tags.Find(tagId);

            if (tag == null)
            {
                return NotFound;
            }
            else
            {
                if (tag.Tasks == null || tag.Tasks != null && force)
                {
                    context.Tags.Remove(tag);
                    context.SaveChanges();

                    return Deleted;
                }
                else
                {
                    return Conflict;
                }
            }
        }

        public TagDTO Read(int tagId)
        {

            if (context.Tags.Find(tagId) == null)
            {
                return null;
            }
            else
            {
                return new TagDTO(context.Tags.Find(tagId).Id, context.Tags.Find(tagId).Name);
            }
        }

        public IReadOnlyCollection<TagDTO> ReadAll()
        {
            var list = new List<TagDTO>();
            foreach (var tag in context.Tags)
            {
                var TagDTO = new TagDTO(tag.Id, tag.Name);
                list.Add(TagDTO);
            }

            if (list.Any())
            {
                return new ReadOnlyCollection<TagDTO>(list);
            }
            else
            {
                return null;
            }
        }

        public Response Update(TagUpdateDTO tag)
        {
            
            
            var updatedTag = context.Tags.Find(tag.Id);

            if(updatedTag == null){
                return NotFound;
            } else {
                updatedTag.Name = tag.Name;

                return Updated;
            }

            
            
        }
    }
}

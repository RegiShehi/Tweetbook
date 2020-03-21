using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TweetBook.Data;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public class TagService : ITagService
    {
        private readonly DataContext _context;

        public TagService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateTagAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            var created = await _context.SaveChangesAsync();

            return created > 0;
        }

        public async Task<bool> CreateTagsAsync(List<Tag> tags)
        {
            await _context.Tags.AddRangeAsync(tags);
            var created = await _context.SaveChangesAsync();

            return created > 0;
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.Include(t => t.Post).ToListAsync();
        }
    }
}

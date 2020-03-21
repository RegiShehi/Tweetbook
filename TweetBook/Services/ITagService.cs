using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public interface ITagService
    {
        Task<bool> CreateTagAsync(Tag tag);

        Task<bool> CreateTagsAsync(List<Tag> tags);

        Task<List<Tag>> GetAllTagsAsync();
    }
}

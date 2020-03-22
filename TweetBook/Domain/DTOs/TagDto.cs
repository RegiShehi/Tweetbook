using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetBook.Domain.DTOs
{
    public class TagDto
    {
        public Guid Id { get; set; }

        public string TagName { get; set; }

        public PostDto Post { get; set; }
    }
}

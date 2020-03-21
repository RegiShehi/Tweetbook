using System;
using System.Collections.Generic;
using TweetBook.Domain;

namespace TweetBook.Contracts.V1.Requests
{
    public class CreatePostRequest
    {
        public string Name { get; set; }
        public List<string> Tags { get; set; }
    }
}

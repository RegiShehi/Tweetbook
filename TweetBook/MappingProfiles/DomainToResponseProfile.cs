using AutoMapper;
using System.Linq;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Domain;

namespace TweetBook.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Post, PostResponse>()
                .ForMember(d => d.Tags, o => o.MapFrom(s => s.Tags.Select(x => new TagResponse
                {
                    Name = x.TagName
                })));
        }
    }
}

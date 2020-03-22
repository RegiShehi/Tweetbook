using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contracts.V1;
using TweetBook.Domain.DTOs;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        [Authorize(Policy = "TagViewer")]
        public async Task<IActionResult> GetAll()
        {
            var tagsDto = new List<TagDto>();
            var tags = await _tagService.GetAllTagsAsync();

            foreach (var tag in tags)
            {
                tagsDto.Add(new TagDto
                {
                    Id = tag.Id,
                    TagName = tag.TagName,
                    Post = new PostDto
                    {
                        Id = tag.Post.Id,
                        Name = tag.Post.Name,
                        UserId = tag.Post.UserId
                    }
                });
            }

            return Ok(tagsDto);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Requests;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Domain;
using TweetBook.Extensions;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    //[Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public PostsController(IPostService postService, ITagService tagService, IMapper mapper)
        {
            _postService = postService;
            _tagService = tagService;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute]Guid id)
        {
            var post = await _postService.GetPostByIdAsync(id);

            if (post == null)
                return NotFound();

            return Ok(_mapper.Map<PostResponse>(post));
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _postService.GetPostsAsync();

            return Ok(_mapper.Map<List<PostResponse>>(posts));
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody]CreatePostRequest postRequest)
        {
            var post = new Post
            {
                Name = postRequest.Name,
                UserId = HttpContext.GetUserId()
            };

            await _postService.CreatePostAsync(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{id}", post.Id.ToString());

            var tags = new List<Tag>();

            postRequest.Tags.ForEach(tag =>
            {
                tags.Add(new Tag
                {
                    PostId = post.Id,
                    TagName = tag
                });
            });

            await _tagService.CreateTagsAsync(tags);

            var response = _mapper.Map<PostResponse>(post);

            return Created(locationUri, response);
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody]UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(id, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new { error = "You do not own this post" });

            var post = await _postService.GetPostByIdAsync(id);
            post.Name = request.Name;

            var updated = await _postService.UpdatePostAsync(post);

            if (updated)
                return Ok(_mapper.Map<PostResponse>(post));

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(id, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new { error = "You do not own this post" });

            var deleted = await _postService.DeletePostAsync(id);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}

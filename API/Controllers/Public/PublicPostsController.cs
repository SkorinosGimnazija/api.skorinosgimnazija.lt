namespace API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Posts;
    using Application.Posts.Dtos;
    using Domain.Auth;
    using Domain.CMS;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

    [Route("posts")]
    public class PublicPostsController : BaseApiController
    {
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PublicPostDetailsDto>> GetPublicPost(int id, CancellationToken ct)
        {
            return await Mediator.Send(new PublicPostDetails.Query(id), ct);
        }

        [HttpGet("{domain}/{language}")]
        public async Task<ActionResult<List<PublicPostDto>>> GetPublicPosts(string domain, string language, int pageNr,
            CancellationToken ct)
        {
            return await Mediator.Send(new PublicPostList.Query(domain, language, pageNr), ct);
        }

        [HttpGet("{domain}/{language}/search/{text:minlength(3)}")]
        public async Task<ActionResult<List<PublicPostDto>>> SearchPublicPosts(string domain, string language, string text, CancellationToken ct)
        {
            return await Mediator.Send(new PublicPostSearchList.Query(domain, language, text), ct);
        }
    }
}
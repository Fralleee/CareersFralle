using CareersFralle.Models;
using CareersFralle.Services;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace CareersFralle.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostsService _postService;
        private readonly IElasticClient _elasticClient;

        public PostsController(IPostsService postService, IElasticClient elasticClient)
        {
            _postService = postService;
            _elasticClient = elasticClient;
        }

        public async Task<JsonResult> Search(string keyword)
        {
            var result = await _elasticClient.SearchAsync<Post>(
                             s => s.Query(
                                 q => q.QueryString(
                                     d => d.Query('*' + keyword + '*')
                                 )).Size(10));

            return Json(result.Documents);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _postService.GetPosts());
        }

        public async Task<IActionResult> Generate(int count = 20)
        {
            await _postService.Generate(count);
            return RedirectToAction(nameof(Index));
        }
    }
}

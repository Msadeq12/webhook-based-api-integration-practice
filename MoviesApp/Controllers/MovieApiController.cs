using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Entities;
using MoviesApp.Models;

namespace MoviesApp.Controllers
{
    [ApiController]
    [Route("/api/movies")]
    public class MovieApiController : Controller
    {
        private readonly MovieDbContext _context;

        public MovieApiController(MovieDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Planning to do something with stream rights request from MSS
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GrantRights([FromBody] StreamRightsModel request)
        {
            
            string result = $"Movie: {request.MovieName} requested for MSS with key: {request.Key}";
            await Console.Out.WriteLineAsync(result);

            return Ok(result);

        }


        /*[HttpGet]
        public async Task<IActionResult> AddMovietoMSS()
        {
            var movieResult = await _context.Movies.OrderBy(m => m.MovieId).LastAsync();
            
            if(movieResult == null)
            {
                return NotFound();
            }

            return Ok(movieResult);
        }*/
    }
}

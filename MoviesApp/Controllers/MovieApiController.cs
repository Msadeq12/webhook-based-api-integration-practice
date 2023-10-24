using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Entities;

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
        public async Task<ActionResult> GrantRights()
        {
            return Ok();
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

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
        /// 
        /// </summary>
        /// <returns>Returns the latest movie addition to MPC</returns>
        
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

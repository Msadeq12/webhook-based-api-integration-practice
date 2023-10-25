using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Entities;
using MoviesApp.Models;
using Newtonsoft.Json;

namespace MoviesApp.Controllers
{
    [ApiController]
    [Route("/api/notify")]
    public class NotifyApiController : Controller
    {
        private HttpClient _client;
        private MovieDbContext _movieDbContext;

        public NotifyApiController(MovieDbContext movieDbContext)
        {
            _client = new HttpClient();
            _movieDbContext = movieDbContext;
        }

        public async Task<ActionResult> GetAllMovies()
        {
            return Ok(await _movieDbContext.Movies.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetMovieById(int id)
        {
            var movie = await _movieDbContext.Movies.FindAsync(id);

            if(movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        /// <summary>
        /// Uses another DTO MovieFromMPC to add a new movie to MSS database
        /// to resolve Rating and ProductionStudioId binding conflict
        /// </summary>
        /// <param name="movieFromMPC">JSON data coming from MPC</param>
        /// <returns>Adds MPC new movie to MSS database</returns>
        [HttpPost]
        public async Task<ActionResult> AddMovieFromMPC([FromBody] MovieFromMPC movieFromMPC)
        {
            Movie newMovie = new Movie
            {
                Name = movieFromMPC.Name,
                GenreId = movieFromMPC.GenreId,
                Year = movieFromMPC.Year,
                Rating = 1,
                ProductionStudioId = 2
            };

            _movieDbContext.Movies.Add(newMovie);
            await _movieDbContext.SaveChangesAsync();

            return CreatedAtAction("GetMovieById", new {id = newMovie.MovieId}, newMovie);
        }

        
        /*[HttpGet]
        public async Task<IActionResult> GetMovieFromMPC()
        {
            string url = "https://localhost:7082/api/movies";

            HttpResponseMessage responseFromMPC = _client.GetAsync(url).Result;
            Movie? movieFromMPC;
            Console.WriteLine("Message from MPC: " + JsonConvert.SerializeObject(responseFromMPC));

            if (responseFromMPC.IsSuccessStatusCode)
            {
                movieFromMPC = responseFromMPC.Content.ReadFromJsonAsync<Movie>().Result;

                Console.WriteLine($"Movie from MPC: {movieFromMPC.MovieId}, {movieFromMPC.Name}, {movieFromMPC.GenreId}, {movieFromMPC.Year}");

                if (_movieDbContext.Movies.Find(movieFromMPC.MovieId) == null)
                {
                    Movie newMovie = new Movie
                    {
                        Name = movieFromMPC.Name,
                        GenreId = movieFromMPC.GenreId,
                        Year = movieFromMPC.Year,
                        Rating = 1,
                        ProductionStudioId = 2
                    };

                    _movieDbContext.Movies.Add(newMovie);
                    _movieDbContext.SaveChanges();
                }
            }

            return Ok();
        }*/


        
        
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MoviesApp.Entities;
using MoviesApp.Models;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace MoviesApp.Controllers
{
    public class MovieController : Controller
    {
        public MovieController(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
            _client = new HttpClient();

            /*string url = "https://localhost:7082/api/movies";

            HttpResponseMessage responseFromMPC = _client.GetAsync(url).Result;
            Movie? movieFromMPC;
            Console.WriteLine("Message from MPC: " + JsonConvert.SerializeObject(responseFromMPC));

            if (responseFromMPC.IsSuccessStatusCode)
            {
                movieFromMPC = responseFromMPC.Content.ReadFromJsonAsync<Movie>().Result;

                Console.WriteLine($"Movie from MPC: {movieFromMPC.MovieId}, {movieFromMPC.Name}, {movieFromMPC.GenreId}, {movieFromMPC.Year}");

                if (_movieDbContext.Movies.Any(m => m.Name == movieFromMPC.Name) == false)
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
            }*/
        }

        [HttpGet()]
        public IActionResult List()
        {
            var allMovies = _movieDbContext.Movies
                    .Include(m => m.Genre)
                    .Include(m => m.ProductionStudio)
                    .OrderBy(m => m.Name).ToList();

            return View(allMovies);
        }

        [HttpGet()]
        public IActionResult Add()
        {
            MovieViewModel movieViewModel = new MovieViewModel()
            {
                Genres = _movieDbContext.Genres.OrderBy(g => g.Name).ToList(),
                Studios = _movieDbContext.Studios.OrderBy(s => s.Name).ToList(),
                ActiveMovie = new Movie()
            };

            return View(movieViewModel);
        }

        [HttpPost()]
        public IActionResult Add(MovieViewModel movieViewModel)
        {
            if (ModelState.IsValid)
            {
                // it's valid so we want to add the new movie to the DB:
                _movieDbContext.Movies.Add(movieViewModel.ActiveMovie);
                _movieDbContext.SaveChanges();
                return RedirectToAction("List", "Movie");
            }
            else
            {
                // it's invalid so we simply return the movie object
                // to the Edit view again:
                movieViewModel.Genres = _movieDbContext.Genres.OrderBy(g => g.Name).ToList();
                movieViewModel.Studios = _movieDbContext.Studios.OrderBy(s => s.Name).ToList();
                return View(movieViewModel);
            }
        }


        [HttpGet()]
        public IActionResult Edit(int id)
        {
            MovieViewModel movieViewModel = new MovieViewModel()
            {
                Genres = _movieDbContext.Genres.OrderBy(g => g.Name).ToList(),
                Studios = _movieDbContext.Studios.OrderBy(s => s.Name).ToList(),
                ActiveMovie = _movieDbContext.Movies.Find(id)
            };

            return View(movieViewModel);
        }

        [HttpPost()]
        public IActionResult Edit(MovieViewModel movieViewModel)
        {
            if (ModelState.IsValid)
            {
                // it's valid so we want to update the existing movie in the DB:
                _movieDbContext.Movies.Update(movieViewModel.ActiveMovie);
                _movieDbContext.SaveChanges();
                return RedirectToAction("List", "Movie");
            }
            else
            {
                movieViewModel.Genres = _movieDbContext.Genres.OrderBy(g => g.Name).ToList();
                movieViewModel.Studios = _movieDbContext.Studios.OrderBy(s => s.Name).ToList();
                return View(movieViewModel);
            }
        }

        [HttpGet()]
        public IActionResult Delete(int id)
        {
            // Find/retrieve/select the movie to edit and then pass it to the view:
            var movie = _movieDbContext.Movies.Find(id);
            return View(movie);
        }

        [HttpPost()]
        public IActionResult Delete(Movie movie)
        {
            // Simply remove the movie and then redirect back to the all movies view:
            _movieDbContext.Movies.Remove(movie);
            _movieDbContext.SaveChanges();
            return RedirectToAction("List", "Movie");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="movieId">Selected movie by Id from MSS database</param>
        /// <returns>Send POST request to MPC for streaming rights</returns>
        [HttpPost]
        public IActionResult RequestRights(int id)
        {
            string url = "https://localhost:7082/api/movies/";
            var selectedMovie = _movieDbContext.Movies.Find(id);

            //gets apikey from appsettings.json under productionstudiosettings
            var apiKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ProductionStudioSettings")["ApiKey"];

            StreamRightsModel requestRights = new()
            {
                MovieName = selectedMovie.Name,
                Key = apiKey
            };


            HttpResponseMessage message = _client.PostAsJsonAsync<StreamRightsModel>(url, requestRights).Result;

     
            if (message.IsSuccessStatusCode)
            {
                Console.WriteLine("Message sent: " + JsonConvert.SerializeObject(requestRights));
            }

            else
            {
                Console.WriteLine("Error code for stream rights: " + message.StatusCode);
            }
            

            return RedirectToAction("List");

        }

        private MovieDbContext _movieDbContext;
        private readonly HttpClient _client;
    }
}

using Microsoft.AspNetCore.Mvc;
using MoviesApp.Entities;
using MoviesApp.Models;

namespace MoviesApp.Controllers
{
    public class StreamController : Controller
    {
        private MovieDbContext _context;

        public StreamController(MovieDbContext context)
        {
            _context = context;
                
        }

        [HttpGet]
        public ViewResult Page()
        {
            List<StreamingPartner> partners = _context.Partners.ToList();

            if(partners.Count == 0)
            {
                ViewBag.PartCount = "There are no streaming partners available at this time.";
            }

            return View(partners);
        }

        [HttpGet]
        public ViewResult AddPartner()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddPartner(StreamingPartnerModel partner)
        {
            
            if (ModelState.IsValid)
            {
                StreamingPartner newPartner = new StreamingPartner
                {
                    Url = partner.Url,
                    ApiKey = Guid.NewGuid().ToString()
                };

                _context.Partners.Add(newPartner);
                _context.SaveChanges();
            }

            else
            {
                return View(partner);
            }

            return RedirectToAction("Page", "Stream");
        }

    }
}

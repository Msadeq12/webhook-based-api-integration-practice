using MoviesApp.Models;
using MoviesApp.Entities;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace MoviesApp.Helper
{
    
    public static class NotificationSender
    {
        public static void Send(MovieDbContext context, Movie movie, HttpClient client)
        {
            var partners = context.Partners.ToList();

            foreach (var partner in partners)
            {
                Console.WriteLine("url" + partner.Url);
                Console.WriteLine("message:" + JsonConvert.SerializeObject(movie));

                StringContent movieMessage = new StringContent(
                    JsonConvert.SerializeObject(movie), Encoding.UTF8, "application/json");

                HttpResponseMessage notification = client.PostAsync(partner.Url, movieMessage).Result;
                //HttpResponseMessage notification = _client.PostAsJsonAsync("https://localhost:7083/api/notify", movieViewModel.ActiveMovie).Result;

                Console.WriteLine(notification.StatusCode);

            }

        }

    }
}

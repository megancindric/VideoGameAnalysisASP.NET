using ASP_NET_Video_Games_API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace ASP_NET_Video_Games_API.Controllers
{
    //Route evaluates to /api/Games
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        // Utilizes dependency injection to create DB context
        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        // Return type IActionResult default for .NET API
        public IActionResult GetAllGames()
        {
            // Var sets this as runtime-determined data type
            var videoGames = _context.VideoGames.ToList();
            return Ok(videoGames);
        }

        [HttpGet("{gameId}")]
        public IActionResult GetGameById(int gameId)
        {
            var videoGame = _context.VideoGames.Where(vg => vg.Id == gameId);
            return Ok(videoGame);
        }
        [Route("[action]/{gameName}")]
        [HttpGet]
        public IActionResult GetGameSales(string gameName)
        {
            var videoGame = _context.VideoGames.Where(vg => vg.Name == gameName);
            return Ok(videoGame);
        }
        [Route("[action]/{year}")]
        [HttpGet]
        public IActionResult GetConsoleSalesSinceYear(int year)
        {
            //Creating a dictionary to hold response (string and decimal)
            var responseDictionary = new Dictionary<string, double>();
            //Finding all games since the given year
            //Selecting distinct consoles for that year
            //Converting results to list to prevent error when querying within foreach loop
            var consoles = _context.VideoGames.Where(vg => vg.Year >= year).Select(vg => vg.Platform).Distinct().ToList();
            //Loop through each console, find matching games, sum global sales
            foreach(string console in consoles)
            {
                var matchingGameSales = _context.VideoGames.Where(vg => vg.Platform == console && vg.Year >= year).Select(vg => vg.GlobalSales).Sum();
                responseDictionary.Add(console, matchingGameSales);
            }
            return Ok(responseDictionary);
        }
    }
}

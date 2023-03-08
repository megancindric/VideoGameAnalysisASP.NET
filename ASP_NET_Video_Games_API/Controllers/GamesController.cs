using ASP_NET_Video_Games_API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Runtime.InteropServices;

namespace ASP_NET_Video_Games_API.Controllers
{
    //Route evaluates to /api/Games
    [Route("api/")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        // Utilizes dependency injection to create DB context
        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Route("[action]")]
        [HttpGet]
        // Return type IActionResult default for .NET API
        public IActionResult GetAllGames()
        {
            // Var sets this as runtime-determined data type
            var videoGames = _context.VideoGames.OrderBy(vg => vg.Rank).ToList();
            return Ok(videoGames);
        }

        [Route("[action]")]
        [HttpGet]
        // Return type IActionResult default for .NET API
        public IActionResult GetAllGameYears()
        {
            // Var sets this as runtime-determined data type
            var videoGameYears = _context.VideoGames.Select(vg => vg.Year).Distinct().Where(y => y != null).OrderByDescending(x => x).ToList();
            return Ok(videoGameYears);
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
            //Query for all games matching this ID, 
            var responseDictionary = new Dictionary<string, double>();

            var videoGames = _context.VideoGames.Where(vg => vg.Name == gameName);
            foreach ( var videoGame in videoGames)
            {
                responseDictionary[videoGame.Platform] = videoGame.GlobalSales;
            }
            return Ok(responseDictionary);
        }
        [Route("[action]/{year}")]
        [HttpGet]
        public IActionResult GetPlatformSalesSinceYear(int year)
        {
            //Creating a dictionary to hold response (string and decimal)
            var responseDictionary = new Dictionary<string, double>();
            //Finding all games since the given year
            //Selecting distinct consoles for that year
            //Converting results to list to prevent error when querying within foreach loop
            var platforms = _context.VideoGames.Where(vg => vg.Year >= year).Select(vg => vg.Platform).Distinct().ToList();
            //Loop through each console, find matching games, sum global sales
            foreach(string platform in platforms)
            {
                var matchingGameSales = _context.VideoGames.Where(vg => vg.Platform == platform && vg.Year >= year).Select(vg => vg.GlobalSales).Sum();
             
                responseDictionary.Add(platform, matchingGameSales);
            }
            return Ok(responseDictionary);
        }

        [Route("[action]/{gameName}")]
        [HttpGet]
        public IActionResult Search(string gameName)
        {
            var matchingGames = _context.VideoGames.Where(vg => vg.Name.Equals(gameName)).ToList();
            return Ok(matchingGames);
        }

        [Route("[action]/{platform}")]
        [HttpGet]
        public IActionResult SearchByPlatform(string platform)
        {
            var matchingGames = _context.VideoGames.Where(vg => vg.Platform.Equals(platform)).ToList();
            return Ok(matchingGames);
        }
    }
}

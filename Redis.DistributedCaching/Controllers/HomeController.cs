using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Redis.DistributedCaching.Models;
using System.Diagnostics;
using System.Text;

namespace Redis.DistributedCaching.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly IDistributedCache _distributedCache;

        public HomeController(ILogger<HomeController> logger, IDistributedCache distributedCache)
        {
            _logger = logger;
            _distributedCache = distributedCache;
        }

        #region SetName - GetName Redis


        [HttpGet("SetName/{name}/{surname}")]
        public async Task<IActionResult> Set(string name, string surname)
        {
            //await _distributedCache.SetStringAsync("isim", name);
            //await _distributedCache.SetAsync("soyisim", Encoding.UTF8.GetBytes(surname));

            await _distributedCache.SetStringAsync("isim", name, options: new () 
            {
               AbsoluteExpiration = DateTime.UtcNow.AddSeconds(15),
               SlidingExpiration = TimeSpan.FromSeconds(10)
            });
            await _distributedCache.SetAsync("soyisim", Encoding.UTF8.GetBytes(surname), options: new()
            {
                AbsoluteExpiration = DateTime.UtcNow.AddSeconds(15),
                SlidingExpiration = TimeSpan.FromSeconds(10)
            });

            return Ok();
        }
        [HttpGet("GetName")]
        public async Task<IActionResult> Get()
        {
            var name = await _distributedCache.GetStringAsync("isim");
            var surnameBinary = await _distributedCache.GetAsync("soyisim");
            var surname = Encoding.UTF8.GetString(surnameBinary);

            //var value = new
            //{
            //    name = name,
            //    surname = surname,
            //};
            //return Ok(value);

            return Ok(new
            {
                name,
                surname
            });
        }

        #endregion



         
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

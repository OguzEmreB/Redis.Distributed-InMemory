using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Redis.InMemoryCaching.Models;
using System.Diagnostics;
using System.Xml.Linq;

namespace Redis.InMemoryCaching.Controllers
{
    public class HomeController : Controller
    {
        readonly IMemoryCache _memoryCache;

       
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }


        #region Caache 1


        //[HttpGet]
        //public object Get()
        //{
        //    _memoryCache.Set("name", "ogz");
        //    _memoryCache.Set("isim", "emr");

        //    var result = new
        //    {
        //        Name = _memoryCache.Get<string>("name"),
        //        Isim = _memoryCache.Get<string>("isim")
        //    };
        //    return result;
        //}

        //[HttpGet]
        //public IActionResult Get()
        //{
        //    _memoryCache.Set("name", "ogz");
        //    _memoryCache.Set("isim", "emr");

        //    var result = new
        //    {
        //        Name = _memoryCache.Get<string>("name"),
        //        Isim = _memoryCache.Get<string>("isim")
        //    };
        //    return Ok(result.ToString());
        //}
        #endregion

        #region SetName GetName

        [HttpGet("SetName/{name}")]
        public IActionResult SetName(string name)
        {
            _memoryCache.Set("name", name);

            return Ok();
        }

        [HttpGet("GetName")]
        public IActionResult GetName()
        {

            if (_memoryCache.TryGetValue<string>("name", out string name)) // name alýnýrsa name'e gönder
            {
                return Ok(name);
            }

            return Ok("isim boþ");

            //return Ok(_memoryCache.Get<string>("name")) ;

        }

        #endregion 

        #region SetDate GetDate

        [HttpGet("SetDate")]
        public IActionResult SetDate()
        {
            _memoryCache.Set("date", DateTime.UtcNow, options: new()
            {
                AbsoluteExpiration = DateTime.UtcNow.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });

            return Ok();
        }

        [HttpGet("GetDate")]
        public IActionResult GetDate()
        {
 
            return Ok(_memoryCache.Get<DateTime>("date")); 
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

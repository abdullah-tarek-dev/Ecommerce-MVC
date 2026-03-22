
namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homerepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homerepository)
        {
            _logger = logger;
            _homerepository = homerepository;
        }

        public async Task<IActionResult> Index(string sterm ="" , int generyid=0)
        {
            var books =await _homerepository.GetBooksAsync(sterm , generyid);
            return View(books);
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

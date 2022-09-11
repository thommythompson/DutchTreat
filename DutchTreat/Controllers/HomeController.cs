using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DutchTreat.Models;
using DutchTreat.Services;
using System.Net.Sockets;
using System.Xml.Linq;

namespace DutchTreat.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMailService _mailService;

    public HomeController(ILogger<HomeController> logger, IMailService mailService)
    {
        _logger = logger;
        _mailService = mailService;
    }

    [Route("")]
    [HttpGet("Home")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("Contact")]
    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost("Contact")]
    public IActionResult Contact(ContactModel model)
    {
        if (ModelState.IsValid)
        {
            // Send Email
            _mailService.SendMessage("test@test.nl", "Test Message", "This is the body");
            ViewData["UserMessage"] = "Mail Sent";
            ModelState.Clear();
        }
        else
        {
            // Show Error
        }
        return View();
    }

    [HttpGet("About")]
    public IActionResult About()
    {
        throw new InvalidOperationException("Error occured");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
using AutoMapper;
using CourseApp.DAL;
using CourseApp.Models;
using CourseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CourseApp.Controllers
{
    public class EmbedController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<FileController> _logger;
        private readonly ApplicationContext _context;

        public EmbedController(IMapper mapper, ILogger<FileController> logger, ApplicationContext context)
        {
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public IActionResult UploadEmbed(EmbedVideoVM model)
        {
            var uri = model.URI;
            // Allows support for varying types of Youtube URL and embed links
            string[] validAuthorities = { "https:\\/\\/www.youtube.com", "youtube.com", "www.youtube.com", "youtu.be", "www.youtu.be" };
            // Matches for Youtube Id URI
            var YoutubeRegex = "(?:.+?)?(?:\\/v\\/|watch\\/|\\?v=|\\&v=|youtu\\.be\\/|\\/v=|^youtu\\.be\\/)([a-zA-Z0-9_-]{11})+";
            Regex regexExtractId = new Regex(YoutubeRegex, RegexOptions.Compiled);
            try
            {
                string authority = new UriBuilder(uri).Uri.Authority.ToLower();
                //check if the url is a youtube url
                if (validAuthorities.Contains(authority))
                {
                    //and extract the id
                    var regRes = regexExtractId.Match(uri.ToString());
                    if (regRes.Success)
                    {
                        var embedLink = $"{validAuthorities[0]}/embed/{regRes.Groups[1].Value}";
                        model.URI = embedLink;
                    }

                    var entity = new EmbedModel
                    {
                        SectionId = model.Id,
                        ResourceLink = model.URI
                    };
                    _context.Add(entity);
                    _context.SaveChanges();
                }
                return RedirectToAction("Edit", "Author", new { id = model.CourseId, selectedSection = model.ParentSectionId });
            }
            catch
            {
                return View();
            }
        }
    }
}


using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseApp.DAL;
using CourseApp.Configuration;
using CourseApp.Helpers;
using CourseApp.Services;
using CourseApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace CourseApp.Controllers
{
    public class ImageController : Controller
    {
        private readonly IBlobStorageService _blobService;
        private readonly IMapper _mapper;
        private readonly ILogger<FileController> _logger;
        private readonly ApplicationContext _context;
        private readonly FileConfigurations _fileOptions;

        public ImageController(IBlobStorageService blobService, IMapper mapper, ILogger<FileController> logger, ApplicationContext context, IOptions<FileConfigurations> fileOptions)
        {
            _blobService = blobService;
            _mapper = mapper;
            _logger = logger;
            _context = context;
            _fileOptions = fileOptions.Value;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetImage(long id)
        {
            var entity = _context.Files.FirstOrDefault(x => x.Id == id);
            if (entity != default)
            {
                var stream = _blobService.DownloadFileFromBlob(entity.Uri, entity.MimeType);
                return File(stream, entity.MimeType, entity.Name);
            }
            return BadRequest();
        }
        [HttpGet]
        public IActionResult GetBannerImage(long id)
        {
            var entity = _context.Courses.FirstOrDefault(x => x.Id == id);
            if (entity != default && entity.BannerURL != default)
            {
                var stream = _blobService.DownloadFileFromBlob(entity.BannerURL, "content/image");
                return File(stream, "content/image");
            }
            return BadRequest();
        }
        [HttpGet]
        public IActionResult GetAvatarImage(long id)
        {
            var entity = _context.Users.FirstOrDefault(x => x.Id == id);
            if (entity != default && entity.ProfileImage != default)
            {
                var stream = _blobService.DownloadFileFromBlob(entity.ProfileImage, "content/image");
                return File(stream, "content/image");
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> UploadBannerImage(FileUploadVM model, long id)
        {
            IFormFile formFile = Request.Form.Files.FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (formFile.Length > 0)
                {
                    if (!FileExtensions.IsImageType(formFile.ContentType))
                    {
                        return BadRequest("The file provided is not of an acceptable format.");
                    }
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var filePath = $"{model.CourseId}/{model.Id}";
                    var fileNamePath = $"{filePath}/{fileName}.{Path.GetExtension(formFile.FileName)}";
                    string mimeType = formFile.ContentType;
                    byte[] fileData = new byte[formFile.Length];

                    using (var ms = new MemoryStream())
                    {
                        await formFile.CopyToAsync(ms);
                        fileData = ms.ToArray();
                    }
                    try
                    {
                        _blobService.UploadFileToBlob(fileNamePath, fileData, mimeType);
                        var entity = _context.Courses.FirstOrDefault(x => x.Id == model.CourseId);

                        if (entity != default)
                        {
                            if (entity.BannerURL != null)
                                //Delete the old image from blob
                                _blobService.DeleteBlobData(entity.BannerURL);

                            //Set new image URL
                            entity.BannerURL = fileNamePath;

                            //Update Entity and Save
                            _context.Update(entity);
                            await _context.SaveChangesAsync();
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Failed To Upload File For Course:{model.CourseId} Section:{model.Id}!");
                    }
                }
            }
            return RedirectToAction("Edit", "Author", new { id = model.CourseId, selectedSection = model.ParentSectionId });
        }

        [HttpPost]
        public async Task<IActionResult> UploadAvatarImage(FileUploadVM model)
        {
            IFormFile formFile = Request.Form.Files.FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (formFile.Length > 0)
                {
                    if (!FileExtensions.IsImageType(formFile.ContentType))
                    {
                        return BadRequest("The file provided is not of an acceptable format.");
                    }
                    var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var filePath = $"{userId}";
                    var fileNamePath = $"{filePath}/{fileName}.{Path.GetExtension(formFile.FileName)}";
                    string mimeType = formFile.ContentType;
                    byte[] fileData = new byte[formFile.Length];

                    using (var ms = new MemoryStream())
                    {
                        await formFile.CopyToAsync(ms);
                        fileData = ms.ToArray();
                    }
                    try
                    {
                        _blobService.UploadFileToBlob(fileNamePath, fileData, mimeType);
                        var entity = _context.Users.FirstOrDefault(x => x.Id == userId);

                        if (entity != default)
                        {
                            if (entity.ProfileImage != null)
                                //Delete the old image from blob
                                _blobService.DeleteBlobData(entity.ProfileImage);

                            //Set new image URL
                            entity.ProfileImage = fileNamePath;

                            //Update Entity and Save
                            _context.Update(entity);
                            await _context.SaveChangesAsync();
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Failed To Upload File For Course:{model.CourseId} Section:{model.Id}!");
                    }
                }
            }
            return RedirectToAction("Index", "Account");
        }
    }
}

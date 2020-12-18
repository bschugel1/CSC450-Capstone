using AutoMapper;
using CourseApp.DAL;
using CourseApp.Models;
using CourseApp.Configuration;
using CourseApp.Services;
using CourseApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApp.Controllers
{
    public class FileController : Controller
    {
        private readonly IBlobStorageService _blobService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ApplicationContext _context;
        private readonly FileConfigurations _fileOptions;

        public FileController(IBlobStorageService blobService, IMapper mapper, ILogger<FileController> logger, ApplicationContext context, IOptions<FileConfigurations> fileOptions)
        {
            _blobService = blobService;
            _mapper = mapper;
            _logger = logger;
            _context = context;
            _fileOptions = fileOptions.Value;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile(FileUploadVM model)
        {
            IFormFile formFile = Request.Form.Files.FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (formFile.Length > 0)
                {
                    if (!_fileOptions.AllowedExtensions.Contains(Path.GetExtension(formFile.FileName)))
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

                        var entity = new FileModel
                        {
                            SectionId = model.Id,
                            Name = model.Title,
                            Uri = fileNamePath,
                            MimeType = mimeType
                        };

                        _context.Add(entity);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Failed To Upload File For Course:{model.CourseId} Section:{model.Id}!");
                    }
                }
            }
            return RedirectToAction("Edit", "Author", new { id = model.CourseId, selectedSection = model.ParentSectionId });
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(long id)
        {
            var entity = _context.Files.FirstOrDefault(x => x.Id == id);

            if (entity == default)
            {
                return BadRequest("File does not exist!");
            }

            var stream = _blobService.DownloadFileFromBlob(entity.Uri, entity.MimeType);
            return File(stream, entity.MimeType, $"{entity.Name}.{Path.GetExtension(entity.Uri)}");
        }
    }
}



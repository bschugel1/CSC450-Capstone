using System;
using System.Collections.Generic;
using System.IO;

namespace CourseApp.Helpers
{
    public static class FileExtensions
    {
        private static readonly IDictionary<string, string> ImageMimeDictionary = new Dictionary<string, string>
            {
        { ".bmp", "image/bmp" },
        { ".dib", "image/bmp" },
        { ".gif", "image/gif" },
        { ".svg", "image/svg+xml" },
        { ".jpe", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".jpg", "image/jpeg" },
        { ".png", "image/png" },
        { ".pnz", "image/png" }
        };
        internal static bool isImage;

        public static bool IsImage(this string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                throw new ArgumentNullException(nameof(file));
            }

            var extension = Path.GetExtension(file);
            return ImageMimeDictionary.ContainsKey(extension.ToLower());
        }

        public static bool IsImageType(this string file)
        {
            var acceptedTypes = new List<string> { "image/png", "image/jpeg", "image/svg+xml", "image/bmp", "image/jpg" };
            if (string.IsNullOrEmpty(file))
            {
                throw new ArgumentNullException(nameof(file));
            }
            return acceptedTypes.Contains(file.ToLower());
        }
    }
}


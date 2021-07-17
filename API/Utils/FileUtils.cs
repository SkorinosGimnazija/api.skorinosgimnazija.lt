namespace API.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Http;

    internal static class FileUtils
    {
        private static readonly Dictionary<string, List<byte[]>> _fileSignatures = new()
        {
            // https://www.filesignatures.net
            {
                ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },
            {
                ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                }
            },
        };

        public static bool IsValid(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_fileSignatures.TryGetValue(ext, out var signatures))
            {
                return false;
            }

            using var reader = new BinaryReader(file.OpenReadStream());
            var headerBytes = reader.ReadBytes(signatures.Max(x => x.Length));

            return signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
        }
    }
}
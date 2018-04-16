using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UniqueFileExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = Configuration.FromArguments(args);
            if (config.IsValid)
            {
                var files = GetFilesToExport(config);

                ExportFiles(config.Destination, files);
            }
            Console.ReadKey();
        }

        private static Dictionary<string, string> GetFilesToExport(Configuration config)
        {
            return Directory
                .EnumerateFiles(config.Source, "*", config.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                .Select(f => new FileToHash(f))
                .Distinct()
                .ToDictionaryWithUniqueValues(f => f.Filename, GenerateUniqueFilename);
        }

        private static string GenerateUniqueFilename(FileToHash file, int iteration)
        {
            if (iteration == 0)
            {
                return Path.GetFileName(file.Filename);
            }

            return $"{Path.GetFileNameWithoutExtension(file.Filename)} ({iteration}){Path.GetExtension(file.Filename)}";
        }

        private static void ExportFiles(string dest, Dictionary<string, string> files)
        {
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }

            foreach (var file in files)
            {
                File.Copy(file.Key, Path.Combine(dest, file.Value), true);
            }
        }
    }
}

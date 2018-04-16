using System;
using System.IO;
using System.Security.Cryptography;

namespace UniqueFileExporter
{
    public class FileToHash : IEquatable<FileToHash>
    {
        private readonly string filename;

        public FileToHash(string filename)
        {
            this.filename = filename;
        }

        private string hash;

        public string Hash
        {
            get
            {
                if (hash == null)
                {
                    using (var md5 = MD5.Create())
                    {
                        using (var stream = File.OpenRead(Filename))
                        {
                            hash = Convert.ToBase64String(md5.ComputeHash(stream));
                        }
                    }
                }
                return hash;
            }
        }

        public string Filename => filename;

        public bool Equals(FileToHash other)
        {
            if (other == null) return false;

            return other.Hash == Hash;
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode();
        }
    }
}

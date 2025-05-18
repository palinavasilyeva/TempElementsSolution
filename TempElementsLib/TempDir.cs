using System;
using System.IO;

namespace TempElementsLib
{
    public class TempDir : IDisposable, ITempElement
    {
        private readonly DirectoryInfo directoryInfo;
        private bool disposed = false;

        public bool IsDisposed => disposed;

        public TempDir()
        {
            string dirPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            directoryInfo = Directory.CreateDirectory(dirPath);
        }

        public DirectoryInfo DirectoryInfo => directoryInfo;

        public void Dispose()
        {
            if (!disposed)
            {
                if (directoryInfo.Exists)
                {
                    directoryInfo.Delete(true);
                }
                disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        ~TempDir()
        {
            Dispose();
        }
    }
}

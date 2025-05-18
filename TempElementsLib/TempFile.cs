using System;
using System.IO;
using System.Text;

namespace TempElementsLib
{
    public class TempFile : IDisposable, ITempFile
    {
        protected readonly FileStream fileStream;
        protected readonly FileInfo fileInfo;
        protected bool disposed = false;

        public TempFile()
        {
            string tempFilePath = Path.GetTempFileName();
            fileInfo = new FileInfo(tempFilePath);
            fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }

        public TempFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !Path.IsPathRooted(filePath))
                throw new ArgumentException("Invalid file path.", nameof(filePath));

            fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists) fileInfo.Delete();
            fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
        }

        public FileStream FileStream => fileStream;
        public FileInfo FileInfo => fileInfo;

        public void AddText(string value)
        {
            if (disposed) throw new ObjectDisposedException(nameof(TempFile));
            byte[] info = Encoding.UTF8.GetBytes(value);
            fileStream.Write(info, 0, info.Length);
            fileStream.Flush();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    fileStream.Close();
                }
                fileInfo.Delete();
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TempFile()
        {
            Dispose(false);
        }
    }

    public interface ITempFile : ITempElement
    {
        FileStream FileStream { get; }
        FileInfo FileInfo { get; }
    }

    public interface ITempElement { }
}

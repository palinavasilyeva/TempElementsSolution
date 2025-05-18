using System;
using System.IO;
using System.Text;

namespace TempElementsLib
{
    public class TempTxtFile : TempFile
    {
        private StreamWriter writer;
        private bool disposed = false;
        public bool IsDisposed => disposed;


        public TempTxtFile() : base()
        {
            writer = new StreamWriter(fileStream, Encoding.UTF8, 1024, leaveOpen: true)
            {
                AutoFlush = true
            };
        }

        public TempTxtFile(string filePath) : base(filePath)
        {
            writer = new StreamWriter(fileStream, Encoding.UTF8, 1024, leaveOpen: true)
            {
                AutoFlush = true
            };
        }

        public void Write(string value)
        {
            EnsureNotDisposed();
            writer.Write(value);
        }

        public void WriteLine(string value)
        {
            EnsureNotDisposed();
            writer.WriteLine(value);
        }

        public string ReadLine()
        {
            EnsureNotDisposed();
            fileStream.Position = 0;
            using var reader = new StreamReader(fileStream, Encoding.UTF8, false, 1024, leaveOpen: true);
            return reader.ReadLine();
        }

        public string ReadAllText()
        {
            EnsureNotDisposed();
            fileStream.Position = 0;
            using var reader = new StreamReader(fileStream, Encoding.UTF8, false, 1024, leaveOpen: true);
            return reader.ReadToEnd();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    writer?.Dispose();
                }
                disposed = true;
                base.Dispose(disposing);
            }
        }

        private void EnsureNotDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(TempTxtFile));
        }
    }
}

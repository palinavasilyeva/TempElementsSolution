using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace TempElementsLib
{
    public class TempElementsList : IDisposable
    {
        private List<ITempElement> elements = new List<ITempElement>();

        public IReadOnlyCollection<ITempElement> Elements => new ReadOnlyCollection<ITempElement>(elements);

        public void AddElement<T>() where T : ITempElement, new()
        {
            var element = new T();
            elements.Add(element);
        }

        public void MoveElementTo<T>(T element, string newPath) where T : ITempElement
        {
            if (element is TempFile file)
            {
                file.FileInfo.MoveTo(newPath);
            }
            else if (element is TempDir dir)
            {
                Directory.Move(dir.DirectoryInfo.FullName, newPath);
            }
        }

        public void DeleteElement<T>(T element) where T : ITempElement
        {
            if (element is IDisposable disposable)
            {
                disposable.Dispose();
            }
            elements.Remove(element);
        }

        public void RemoveDestroyed()
        {
            elements.RemoveAll(e =>
            {
                if (e is IDisposable disposable)
                {
                    if (e is TempFile tf) return tf.IsDisposed;
                    if (e is TempDir td) return td.IsDisposed;
                }
                return false;
            });
        }

        public void Dispose()
        {
            foreach (var element in elements)
            {
                if (element is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            elements.Clear();
        }
    }
}

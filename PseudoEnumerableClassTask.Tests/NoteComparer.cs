using System;
using System.Collections.Generic;
using System.Text;
using Notebook;

namespace PseudoEnumerableClassTask.Tests
{
    public class NoteComparer : IComparer<Note>
    {
        public int Compare(Note x, Note y)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            return x.CompareTo(y);
        }
    }
}

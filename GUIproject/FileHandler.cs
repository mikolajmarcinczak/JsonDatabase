using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GUIproject
{
    class FileHandler
    {
        public string filename { get; set; }

        public FileHandler(string filen)
        {
            this.filename = filen + ".json";
        }

        public void ReadFile(string filename)
        {
            FileStream read = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
        }
        public void WriteFile(string filename)
        {
            FileStream write = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
        }
    }
}

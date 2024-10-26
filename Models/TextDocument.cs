using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Editty.Models
{
    public class TextDocument
    {
        public FlowDocument Content { get; set; }
        public bool IsOpen { get; set; }
        public string FilePath { get; set; }
        public string FileExtension { get; set; }
        public string FileName { get; set; }
        public bool IsChanged { get; set; }
        public TextDocument()
        {
            Content = new FlowDocument();
            IsOpen = false;
            FilePath = "temp.rtf";
            FileExtension = ".rtf";
            FileName = "temp.rtf";
            IsChanged = false;
        }
    }
}

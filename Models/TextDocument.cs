using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Editty.Models
{
    public class TextDocument
    {
        public FlowDocument Content { get; set; }

        public TextDocument()
        {
            Content = new FlowDocument();
        }
    }
}

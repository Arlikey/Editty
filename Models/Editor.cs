using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Editty.Models
{
    public class Editor
    {
        public IEnumerable<FontFamily> FontFamilies { get; set; }

        public Editor()
        {
            FontFamilies = Fonts.SystemFontFamilies;
        }
    }
}

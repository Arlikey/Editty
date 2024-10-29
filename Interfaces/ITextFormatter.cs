using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Editty.Interfaces
{
    public interface ITextFormatter
    {
        public void ToggleBold();
        public void ToggleItalic();
        public void ToggleUnderline();
        public void ToggleAlignLeft();
        public void ToggleAlignCenter();
        public void ToggleAlignRight();
        public void ApplyTextColor(Color color);
        public void ApplyFontFamily(FontFamily fontFamily);
        public void ApplyFontSize(double size);
        public void ApplyTextFormatting(DependencyProperty property, object value);
    }
}

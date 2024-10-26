using Editty.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Editty.UserControls
{
    public class BaseAsideControl : UserControl
    {
        public BaseAsideControl()
        {
            Background = new SolidColorBrush(Color.FromArgb(255, 241, 241, 241));
            MouseEnter += BaseControl_MouseEnter;
            MouseLeave += BaseControl_MouseLeave;
        }

        public void BaseControl_MouseLeave(object sender,MouseEventArgs e)
        {
            AnimationHelper.BackgroundColorFade(this, Color.FromArgb(255, 241, 241, 241), 0.25);
        }

        public void BaseControl_MouseEnter(object sender, MouseEventArgs e)
        {
            AnimationHelper.BackgroundColorFade(this, Color.FromArgb(255, 230, 230, 230), 0.25);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace Editty.Helpers
{
    public static class AnimationHelper
    {
        public static void BackgroundColorFade(Control control, Color color, double durationInSeconds)
        {
            AnimationTimeline fadeIn = new ColorAnimation(color, TimeSpan.FromSeconds(durationInSeconds));
            control.Background.BeginAnimation(SolidColorBrush.ColorProperty, fadeIn);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace key_assist {
    class Util {
        public static void 
        RunLater(Action action) {
            Application.Current.Dispatcher.BeginInvoke(action);
        }

        public static BitmapImage
        getImage(String path) {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        public static void PlaceElement(FrameworkElement element, int x, int y) {
            PlaceElement(element, x, y, 0);
        }

        public static void EnableAA(DependencyObject obj) {
            RenderOptions.SetBitmapScalingMode(obj, BitmapScalingMode.HighQuality);
        }

        public static void PlaceElement(FrameworkElement element, int x, int y, int extra_thinkness) {

            bool is_upward = y > 0,
                 is_leftward = x < 0;

            element.HorizontalAlignment = HorizontalAlignment.Center;
            element.VerticalAlignment = VerticalAlignment.Center;

            element.Margin = new Thickness {
                Left = (!(is_leftward) ? Math.Abs(x) : 0) + extra_thinkness,
                Right = (is_leftward ? Math.Abs(x) : 0) + extra_thinkness,
                Top = (!(is_upward) ? Math.Abs(y) : 0) + extra_thinkness,
                Bottom = (is_upward ? Math.Abs(y) : 0) + extra_thinkness
            };
        }
    }
}

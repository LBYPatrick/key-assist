using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace key_assist {
    class Util {
        public static void 
        RunLater(Action action) {
            Application.Current.Dispatcher.BeginInvoke(action);
        }
    }
}

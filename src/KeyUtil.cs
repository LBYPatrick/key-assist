using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;

namespace key_assist
{
    class KeyUtil {

        public const int NUM_KEY = 0,
                         CAPS_KEY = 1,
                         SCROLL_KEY = 2,
                         KEY_COUNT = 3;
        private static bool [] _current_status = new bool[3],
                               _last_status = new bool[3],
                               _difference = new bool[3];

        public static void 
        update() {

            Util.RunLater(delegate () {

                //Get Key States
                _current_status[NUM_KEY] = Keyboard.IsKeyToggled(Key.NumLock);
                _current_status[CAPS_KEY] = Keyboard.IsKeyToggled(Key.CapsLock);
                _current_status[SCROLL_KEY] = Keyboard.IsKeyToggled(Key.Scroll);

                //Make Comparison
                for (int i = 0; i < KEY_COUNT; ++i) {
                    _difference[i] = (_current_status[i] != _last_status[i]);
                }

                //Copy values

                for(int i = 0; i < KEY_COUNT; ++i) {
                    _last_status[i] = _current_status[i];
                }
            });
        }

        public static bool 
        IsKeyToggled(int keyIndex) {

            return _current_status[keyIndex];
        }

        public static bool
        IsKeyChanged(int keyIndex) {

            return _difference[keyIndex];
        }

        public static bool
        IsKeyboardChanged() {

            for(int i = 0; i < _difference.Length; ++i) {
                if (_difference[i] == true) return true;
            }
            return false;
        }

    }
}

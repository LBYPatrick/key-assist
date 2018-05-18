using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace key_assist {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private const int INTERVAL = 15;
        private int _window_alive_time = 0;
        private int _window_width;
        private int _window_height;
        private bool _window_visible;

        public void ShowWindowThenHide() {

            //Just make it show longer...
            if (_window_visible) {
                if (_window_alive_time < 1300) {
                    _window_alive_time = 3500;
                }
                return;
            }

            else {
                Console.WriteLine("Create Thread");
                new Thread(delegate () {

                    ShowWindow();
                    _window_alive_time = 3500;

                    while (_window_alive_time > 0) {

                        Thread.Sleep(200);
                        _window_alive_time -= 200;
                    }

                    Console.WriteLine("OUT");
                    _window_alive_time = 0;

                    HideWindow();
                }).Start();
            }
        }

        public void
        InitWindow() {

            //Set appropriate window size
            _window_height = (int)(SystemParameters.VirtualScreenHeight / 5);
            _window_width = (int)(SystemParameters.VirtualScreenWidth * 0.6);
            this.Top = SystemParameters.VirtualScreenHeight / 2 + 150;
            this.Left = SystemParameters.VirtualScreenWidth / 2 - _window_width / 2;
            this.Height = _window_height;
            this.Width = _window_width;

            //Set Startup Location and window state
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.WindowState = WindowState.Normal;
            this.ShowInTaskbar = false;
            this.ShowActivated = false;

            //Now hide the window
            this.ShowActivated = false;
            this.Opacity = 0;
            _window_visible = false;

        }

        public void
        InitIcons() {
            caps_tile.Source = Util.getImage("Icons/lock.png");
            num_tile.Source = Util.getImage("Icons/lock.png");
        }

        public
        MainWindow() {
                InitializeComponent();
                InitWindow();
                InitIcons();
                ListenKeys();
        }

        public void
        ListenKeys() {

            new Thread(delegate () {
                while (true) {

                    KeyUtil.update();

                    if (KeyUtil.IsKeyboardChanged()) {

                        if (KeyUtil.IsKeyChanged(KeyUtil.CAPS_KEY)) {
                            Util.RunLater((Action)delegate () {
                                caps_tile.Opacity = KeyUtil.IsKeyToggled(KeyUtil.CAPS_KEY) ? 1 : 0.5;
                            });
                        }

                        if (KeyUtil.IsKeyChanged(KeyUtil.NUM_KEY)) {
                            Util.RunLater((Action)delegate () {
                                num_tile.Opacity = KeyUtil.IsKeyToggled(KeyUtil.NUM_KEY) ? 1 : 0.5;
                            });
                        }

                        ShowWindowThenHide();
                    }
                    Thread.Sleep(INTERVAL);
                }
            }).Start();
        }

        private void ShowWindow() {
            Util.RunLater(delegate(){
                this.Opacity = 1;
                this.ShowActivated = true;
                _window_visible = true;
            });
        }

        private void HideWindow() {
            Util.RunLater(delegate () {
                this.ShowActivated = false;
                this.Opacity = 0;
                //this.Height = 0;
                //this.Width = 0;
            });
            _window_visible = false;
        }
    }
}

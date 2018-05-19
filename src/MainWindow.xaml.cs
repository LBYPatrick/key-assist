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

        public const int SCAN_INTERVAL = 20;
        public const int WINDOW_KEEP_TIME = 1500;
        public const double TRANSPARENCY = 0.7;
        public const int NUM_TILES = 2;
        private int _window_alive_time = 0;

        private int _window_width;
        private int _window_height;
        private int _tile_length;
        private int _tile_margin;

        private bool _window_visible;

        public void ShowWindowThenHide() {

            //Just make it show longer...
            if (_window_visible) {
                if (_window_alive_time < 1300) {
                    _window_alive_time = WINDOW_KEEP_TIME;
                }
                return;
            }

            else {
                new Thread(delegate () {

                    ShowWindow();
                    _window_alive_time = WINDOW_KEEP_TIME;

                    while (_window_alive_time > 0) {

                        Thread.Sleep(200);
                        _window_alive_time -= 200;
                    }
                    _window_alive_time = 0;

                    HideWindow();
                }).Start();
            }
        }
        public void
        InitWindow() {

            //Set appropriate window size
            _window_height = (int)(SystemParameters.VirtualScreenHeight / 8);
            _window_width = (int)(SystemParameters.VirtualScreenWidth * tile_grid.ColumnDefinitions.Count * 0.08);
            this.Top = SystemParameters.VirtualScreenHeight / 2 + 200;
            this.Left = SystemParameters.VirtualScreenWidth/2 - _window_width/2;
            this.Height = _window_height;
            this.Width = _window_width;

            //Set Window Background color
            this.Background.Opacity = TRANSPARENCY;

            //Set Startup Location and window state
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.WindowState = WindowState.Normal;

            //Now hide the window
            this.ShowActivated = false;
            this.Opacity = 0;
            this.ShowInTaskbar = false;
            _window_visible = false;

        }

        public void
        InitTiles() {

            _tile_length = _window_height * 3 / 5;
            _tile_margin = _window_height / 10;
            


            //Load Source
            caps_tile.Source = Util.getImage("Icons/caps_lock.png");
            num_tile.Source = Util.getImage("Icons/num_lock.png");

            caps_tile.Height =  _tile_length;
            caps_tile.Width  =  _tile_length;
            num_tile.Height = _tile_length;
            num_tile.Height = _tile_length;

            //Enable Anti-aliasing
            Util.EnableAA(caps_tile);
            Util.EnableAA(num_tile);

            //Put to center
            Util.PlaceElement(caps_tile, 0, 0, 10);
            Util.PlaceElement(num_tile, 0, 0, 10);
            
        }

        public
        MainWindow() {
                InitializeComponent();
                InitWindow();
                InitTiles();
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
                    Thread.Sleep(SCAN_INTERVAL);
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
            });
            _window_visible = false;
        }
    }
}

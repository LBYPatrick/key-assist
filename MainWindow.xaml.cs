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

        public void
        InitWindow() {

            //Set appropriate window size
            this.Width = SystemParameters.VirtualScreenWidth * 0.6;
            this.Height = SystemParameters.VirtualScreenHeight / 5;


        }

        public
        MainWindow() {

            InitializeComponent();
            InitWindow();
            ListenKeys();

        }

        public void
        ListenKeys() {

            new Thread(delegate () {
                try {
                    while (true) {

                        KeyUtil.update();

                        if (KeyUtil.IsKeyChanged(KeyUtil.CAPS_KEY)) {

                            Util.RunLater((Action)delegate () {
                                button_1.Content = KeyUtil.IsKeyToggled(KeyUtil.CAPS_KEY) ? "CAPS: ON" : "CAPS: OFF";
                            });
                        }

                        if (KeyUtil.IsKeyChanged(KeyUtil.NUM_KEY)) {
                            Util.RunLater((Action)delegate () {
                                button_2.Content = KeyUtil.IsKeyToggled(KeyUtil.NUM_KEY) ? "NUM : ON" : "NUM: OFF";
                            });
                        }
                        
                        Thread.Sleep(INTERVAL);
                    }
                }
                catch (Exception e) { Console.WriteLine(e);}

            }).Start();
        }
    }
}

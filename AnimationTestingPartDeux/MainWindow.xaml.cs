using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnimationTestingPartDeux
{
    using System.Timers;

    using AnimationTesting.ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer testTimer;

        private MainWindowViewModel viewModel = new MainWindowViewModel();

        public MainWindow()
        {
            this.DataContext = this.viewModel;

            InitializeComponent();

            this.testTimer = new Timer(100);
            testTimer.AutoReset = true;
            testTimer.Elapsed += this.testTimer_Elapsed;
            testTimer.Start();
        }

        void testTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.RealX.Text = Canvas.GetLeft(rectangle).ToString("0.00");
                this.RealY.Text = Canvas.GetTop(rectangle).ToString("0.00");
            }));
        }
    }
}

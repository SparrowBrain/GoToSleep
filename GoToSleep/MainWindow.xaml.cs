using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace GoToSleep
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TimeOnly _sleepTimeStart = new TimeOnly(22, 30);
        private readonly TimeOnly _sleepTimeEnd = new TimeOnly(03, 00);

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Task.Run(MainLoop);
        }

        private async Task MainLoop()
        {
            try
            {
                while (true)
                {
                    var theTime = TimeOnly.FromDateTime(DateTime.Now);
                    if (theTime > _sleepTimeStart || theTime < _sleepTimeEnd)
                    {
                        Shutdown();
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Task.Run(MainLoop);
            }
        }

        private void Shutdown()
        {
            Process.Start("shutdown", "/s /t 10");
        }
    }
}
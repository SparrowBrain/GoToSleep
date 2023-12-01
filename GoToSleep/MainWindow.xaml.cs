using System;
using System.Collections.Generic;
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
        private readonly Dictionary<DayOfWeek, TimeOnly> _sleepTimeStart;
        private readonly TimeOnly _sleepTimeStartWeekday = new TimeOnly(22, 00);
        private readonly TimeOnly _sleepTimeStartWeekend = new TimeOnly(22, 15);
        private readonly TimeOnly _sleepTimeEnd = new TimeOnly(03, 00);

        public MainWindow()
        {
            InitializeComponent();
            _sleepTimeStart = new Dictionary<DayOfWeek, TimeOnly>()
            {
                { DayOfWeek.Monday, _sleepTimeStartWeekday },
                { DayOfWeek.Tuesday, _sleepTimeStartWeekday },
                { DayOfWeek.Wednesday, _sleepTimeStartWeekday },
                { DayOfWeek.Thursday, _sleepTimeStartWeekday },
                { DayOfWeek.Friday, _sleepTimeStartWeekend },
                { DayOfWeek.Saturday, _sleepTimeStartWeekend },
                { DayOfWeek.Sunday, _sleepTimeStartWeekday },
            };
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
                    var dayOfWeek = DateTime.Now.DayOfWeek;
                    if (theTime > _sleepTimeStart[dayOfWeek] || theTime < _sleepTimeEnd)
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
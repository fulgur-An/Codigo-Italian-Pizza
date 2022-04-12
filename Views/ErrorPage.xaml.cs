using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ItalianPizza.Views
{
    /// <summary>
    /// Lógica de interacción para InvalidFieldsPage.xaml
    /// </summary>
    public partial class InvalidFieldsPage : Page
    {
        private TimeSpan timeSpan;
        private DispatcherTimer dispatcherTimer;

        public InvalidFieldsPage()
        {
            InitializeComponent();
            StarTimer(5);
        }

        #region GUI Methods

        public void StarTimer(int seconds)
        {
            timeSpan = TimeSpan.FromSeconds(seconds);

            dispatcherTimer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                TimeTextBlock.Text = timeSpan.ToString("ss");
                char[] timeFormat = (TimeTextBlock.Text).ToCharArray();
                TimeTextBlock.Text = "(" + Char.ToString(timeFormat[1]) + ")";

                if (timeSpan == TimeSpan.Zero)
                {
                    dispatcherTimer.Stop();
                    TimeTextBlock.Text = "(5)";
                    InvalidFieldsGrid.Visibility = Visibility.Hidden;
                }

                timeSpan = timeSpan.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);

            dispatcherTimer.Start();
        }

        #endregion
    }
}

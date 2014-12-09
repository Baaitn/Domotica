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

namespace Domotica
{
    public partial class ucLampDetail : UserControl
    {
        private static DispatcherTimer RefreshTimer = new DispatcherTimer();
        private ccLamp _lamp;
        static ucLampDetail()
        {
            RefreshTimer.Interval = TimeSpan.FromSeconds(1);
        }
        public ucLampDetail(ccLamp lamp)
        {
            InitializeComponent();
            this.Lamp = lamp;
            //ucEventhandlers
            this.Loaded += ucLampDetail_Loaded;
            this.Unloaded += ucLampDetail_Unloaded;
            RefreshTimer.Tick += Refresh_Tick;
            chkBrand.PreviewMouseUp += chkBrand_PreviewMouseUp;
        }
        public ccLamp Lamp
        {
            get { return _lamp; }
            set { _lamp = value; }
        }
        private void ucLampDetail_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshTimer.Start();
            txtNaam.Text = Lamp.Name;
            chkBrand.IsChecked = Lamp.Brand;
        }
        private void ucLampDetail_Unloaded(object sender, RoutedEventArgs e)
        {
            RefreshTimer.Stop();
        }
        private void Refresh_Tick(object sender, EventArgs e)
        {
            chkBrand.IsChecked = Lamp.Brand;
        }
        private void chkBrand_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Lamp.ToggleBrand();
        }
    }
}

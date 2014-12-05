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
            RefreshTimer.Start();
        }
        public ucLampDetail(ccLamp lamp)
        {
            InitializeComponent();
            this.Lamp = lamp;
            this.Loaded += ucLampDetail_Loaded;
            RefreshTimer.Tick += Refresh_Tick;
            //ucEventhandlers
            chkIsAan.PreviewMouseUp += chkIsAan_PreviewMouseUp;
        }
        public ccLamp Lamp
        {
            get { return _lamp; }
            set { _lamp = value; }
        }
        private void ucLampDetail_Loaded(object sender, RoutedEventArgs e)
        {
            txtNaam.Text = Lamp.Name;
        }
        private void Refresh_Tick(object sender, EventArgs e)
        {
            chkIsAan.IsChecked = Lamp.IsAan;
        }
        private void chkIsAan_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Lamp.ToggleIsAan();
        }
    }
}

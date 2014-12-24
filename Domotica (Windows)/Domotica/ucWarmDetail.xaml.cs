using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class ucWarmDetail : UserControl
    {
        private static DispatcherTimer RefreshTimer = new DispatcherTimer();
        private ccWarm _warm;
        static ucWarmDetail()
        {
            RefreshTimer.Interval = TimeSpan.FromSeconds(1);
        }
        public ucWarmDetail(ccWarm warm)
        {
            InitializeComponent();
            this.Warm = warm;
            //ucEvents
            this.Loaded += ucWarmDetail_Loaded;
            this.Unloaded += ucWarmDetail_Unloaded;
            RefreshTimer.Tick += Refresh_Tick;
            chkBrand.PreviewMouseUp += chkBrand_PreviewMouseUp;
            chkAuto.PreviewMouseUp += chkAuto_PreviewMouseUp;
            txtGewenst.TextChanged += txtGewenst_TextChanged;
        }
        public ccWarm Warm
        {
            get { return _warm; }
            set { _warm = value; }
        }
        private void ucWarmDetail_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshTimer.Start();
            txtNaam.Text = Warm.Name;
            chkBrand.IsChecked = _warm.Brand;
            chkAuto.IsChecked = _warm.Auto;
            txtGewenst.Text = _warm.Gewenst.ToString();
            txtHuidig.Text = _warm.Huidig.ToString();
        }
        private void ucWarmDetail_Unloaded(object sender, RoutedEventArgs e)
        {
            RefreshTimer.Stop();
        }
        private void Refresh_Tick(object sender, EventArgs e)
        {
            if (IsLoaded)
            {
                chkBrand.IsChecked = _warm.Brand;
                chkAuto.IsChecked = _warm.Auto;
                if (!txtGewenst.IsFocused) { txtGewenst.Text = _warm.Gewenst.ToString(); }
                txtHuidig.Text = _warm.Huidig.ToString();
            }
        }
        private void chkBrand_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Warm.ToggleBrand();
        }
        private void chkAuto_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Warm.ToggleAuto();
        }
        private void txtGewenst_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtGewenst.Text))
            {
                Warm.ChangeGewenst(Convert.ToDouble(txtGewenst.Text.ToString(), new NumberFormatInfo())); //Convert.ToDouble() wil een provider nu dat er binding wordt toegepast
            }
        }
    }
}

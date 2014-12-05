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
    public partial class ucWarmDetail : UserControl
    {
        private static DispatcherTimer RefreshTimer = new DispatcherTimer();
        private ccWarm _warm;
        static ucWarmDetail()
        {
            RefreshTimer.Interval = TimeSpan.FromSeconds(1);
            RefreshTimer.Start();
        }
        public ucWarmDetail(ccWarm warm)
        {
            InitializeComponent();
            this.Warm = warm;
            this.Loaded += ucWarmDetail_Loaded;
            RefreshTimer.Tick += Refresh_Tick;
            //ucEventhandlers
            chkIsAan.PreviewMouseUp += chkIsAan_PreviewMouseUp;
            chkIsAuto.PreviewMouseUp += chkIsAuto_PreviewMouseUp;
            txtTGewenst.TextChanged += txtTGewenst_TextChanged;
        }
        public ccWarm Warm
        {
            get { return _warm; }
            set { _warm = value; }
        }
        private void ucWarmDetail_Loaded(object sender, RoutedEventArgs e)
        {
            txtNaam.Text = Warm.Name;
        }
        private void Refresh_Tick(object sender, EventArgs e)
        {
            chkIsAan.IsChecked = _warm.IsAan;
            chkIsAuto.IsChecked = _warm.IsAuto;
            if (!txtTGewenst.IsFocused) { txtTGewenst.Text = _warm.TGewenst.ToString(); }
            txtTHuidig.Text = _warm.THuidig.ToString();
        }
        private void chkIsAan_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Warm.ToggleIsAan();
        }
        private void chkIsAuto_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Warm.ToggleIsAuto();
        }
        private void txtTGewenst_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtTGewenst.Text))
            {
                Warm.ChangeTGewenst(Convert.ToDouble(txtTGewenst.Text.ToString()));
            }
        }
    }
}

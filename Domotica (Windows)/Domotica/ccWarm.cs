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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Domotica
{
    public class ccWarm : Control
    {
        private Rectangle rctGray;
        private Rectangle rctRed;
        private static DispatcherTimer RefreshTimer = new DispatcherTimer();
        private clsOPCNode _isaan;
        private clsOPCNode _isauto;
        private clsOPCNode _tgewenst;
        private clsOPCNode _thuidig;
        private ucWarmDetail _warmdetail;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            rctGray = GetTemplateChild("rctGray") as Rectangle;
            rctRed = GetTemplateChild("rctRed") as Rectangle;
        }
        static ccWarm()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ccWarm), new FrameworkPropertyMetadata(typeof(ccWarm)));
            RefreshTimer.Interval = TimeSpan.FromSeconds(1);
            RefreshTimer.Start();
        }
        public ccWarm(clsOPCNode isaan, clsOPCNode isauto, clsOPCNode tgewenst, clsOPCNode thuidig)
        {
            this._isaan = isaan;
            this._isauto = isauto;
            this._tgewenst = tgewenst;
            this._thuidig = thuidig;
            this._warmdetail = new ucWarmDetail(this);
            this.Loaded += ccWarm_Loaded;
            RefreshTimer.Tick += Refresh_Tick;
        }
        public ucWarmDetail WarmDetail
        {
            get { return _warmdetail; } 
        }
        public Boolean IsAan
        {
            get { return (Boolean)_isaan.Value; }
            set { _isaan.Value = value; }
        }
        public Boolean IsAuto
        {
            get { return (Boolean)_isauto.Value; }
            set { _isauto.Value = value; }
        }
        public Double TGewenst
        {
            get { return (Double)_tgewenst.Value; }
            set { _tgewenst.Value = value; }
        }
        public Double THuidig
        {
            get { return (Double)_thuidig.Value; }
            set { _thuidig.Value = value; }
        }
        private void ccWarm_Loaded(object sender, RoutedEventArgs e)
        {
            WijzigWarm();
        }
        private void Refresh_Tick(object sender, EventArgs e)
        {
            WijzigWarm();
        }
        private void WijzigWarm()
        {
            if (rctGray == null) return;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = rctGray.Opacity;
            animation.To = (IsAan) ? 0 : 1;
            animation.Duration = new TimeSpan(0, 0, 0, 0, 500);
            Storyboard.SetTarget(animation, rctGray);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            storyboard.Begin(this);
        }
        public void ToggleIsAan()
        {
            if (IsAan) { IsAan = false; } else { IsAan = true; }
        }
        public void ToggleIsAuto()
        {
            if (IsAuto) { IsAuto = false; } else { IsAuto = true; }
        }
        public void ChangeTGewenst(Double value)
        {
            TGewenst = value;
        }
    }
}

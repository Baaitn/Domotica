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
        private clsOPCNode _brandnode;
        private clsOPCNode _autonode;
        private clsOPCNode _gewenstnode;
        private clsOPCNode _huidignode;
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
        }
        public ccWarm(clsOPCNode brandnode, clsOPCNode autonode, clsOPCNode gewenstnode, clsOPCNode huidignode)
        {
            this._brandnode = brandnode;
            this._autonode = autonode;
            this._gewenstnode = gewenstnode;
            this._huidignode = huidignode;
            this._warmdetail = new ucWarmDetail(this);
            //ccEventhandlers
            this.Loaded += ccWarm_Loaded;
            RefreshTimer.Tick += Refresh_Tick;
        }
        public ucWarmDetail WarmDetail
        {
            get { return _warmdetail; } 
        }
        public Boolean Brand
        {
            get { return (Boolean)_brandnode.Value; }
            set { _brandnode.Value = value; }
        }
        public Boolean Auto
        {
            get { return (Boolean)_autonode.Value; }
            set { _autonode.Value = value; }
        }
        public Double Gewenst
        {
            get { return (Double)_gewenstnode.Value; }
            set { _gewenstnode.Value = value; }
        }
        public Double Huidig
        {
            get { return (Double)_huidignode.Value; }
            set { _huidignode.Value = value; }
        }
        private void ccWarm_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshTimer.Start();
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
            animation.To = (Brand) ? 0 : 1;
            animation.Duration = new TimeSpan(0, 0, 0, 0, 500);
            Storyboard.SetTarget(animation, rctGray);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            storyboard.Begin(this);
        }
        public void ToggleBrand()
        {
            if (Brand) { Brand = false; } else { Brand = true; }
        }
        public void ToggleAuto()
        {
            if (Auto) { Auto = false; } else { Auto = true; }
        }
        public void ChangeGewenst(Double value)
        {
            Gewenst = value;
        }
    }
}

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
    public class ccLamp : Control
    {
        private Ellipse ellGray;
        private Ellipse ellYellow;
        private static DispatcherTimer RefreshTimer = new DispatcherTimer();
        private clsOPCNode _isaan;
        private ucLampDetail _lampdetail;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ellGray = GetTemplateChild("ellGray") as Ellipse;
            ellYellow = GetTemplateChild("ellYellow") as Ellipse;
        }
        static ccLamp()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ccLamp), new FrameworkPropertyMetadata(typeof(ccLamp)));
            RefreshTimer.Interval = TimeSpan.FromSeconds(1);
            RefreshTimer.Start();
        }
        public ccLamp(clsOPCNode isaan)
        {
            this._isaan = isaan;
            this._lampdetail = new ucLampDetail(this);
            this.Loaded += ccLamp_Loaded;
            RefreshTimer.Tick += Refresh_Tick;
        }
        public ucLampDetail LampDetail
        {
            get { return _lampdetail; } 
        }
        public Boolean IsAan
        {
            get { return (Boolean)_isaan.Value; }
            set { _isaan.Value = value; }
        }
        private void ccLamp_Loaded(object sender, RoutedEventArgs e)
        {
            WijzigLamp();
        }
        private void Refresh_Tick(object sender, EventArgs e)
        {
            WijzigLamp();
        }
        private void WijzigLamp()
        {
            if (ellGray == null) return;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = ellGray.Opacity;
            animation.To = (IsAan) ? 0 : 1;
            animation.Duration = new TimeSpan(0, 0, 0, 0, 500);
            Storyboard.SetTarget(animation, ellGray);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            storyboard.Begin(this);
        }
        public void ToggleIsAan()
        {
            if (IsAan) { IsAan = false; } else { IsAan = true; } //IsAan = (IsAan) ? true : false;
        }
    }
}

using Microsoft.Expression.Interactivity.Layout;
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

namespace Domotica
{
    public partial class MainWindow : Window
    {
        private clsOPCNode[] clsOPCNodes = new clsOPCNode[4];
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<clsOPCServer> servers = clsOPCServer.GetOPCServers();
            foreach (clsOPCServer server in servers)
            {
                cboOPCServers.Items.Add(server);
            }
            foreach (clsOPCServer server in cboOPCServers.Items) //Automatisch selecteren van de kepserver, want klikken op items in een combobox is lastig ;)
            {
                if (server.ToString().Equals("Kepware.KEPServerEX.V5"))
                {
                    cboOPCServers.SelectedIndex = cboOPCServers.Items.IndexOf(server);
                }
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            List<clsOPCCSV> list = new List<clsOPCCSV>();
            foreach (Control control in grdMain.Children.OfType<Control>())
            {
                if (control is ccLamp || control is ccWarm)
                {
                    TranslateTransform transform = control.RenderTransform as TranslateTransform;
                    list.Add(new clsOPCCSV() { Id = (String)control.Tag, X = transform.X, Y = transform.Y });
                }
            }
            clsOPCCSV.SaveCSV("Domotica.csv", list);
        }
        private void cboOPCServers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clsOPCServer server = (clsOPCServer)cboOPCServers.SelectedItem;
            foreach (clsOPCNode node in server.GetOPCNodes())
            {
                //clsOPCTRVNode trvnode = new clsOPCTRVNode(node);
                //trvOPCNodes.Items.Add(trvnode);
                ToonControls(node);
            }
        }
        private void ToonControls(clsOPCNode node)
        {
            if (node.IsLeaf)
            {
                if (node.Name.ToLower().StartsWith("lamp")) { ToonLamp(node); }
                if (node.Name.ToLower().StartsWith("warm"))
                {
                    //Nodes komen 1 per 1 toe (zie methode hierboven) al werden ze wel gesorteerd. Er kan dus verondersteld worden dat alle nodes van 1 verwarming na elkaar binnenkomen en bewaard worden in een array.
                    if (node.Name.ToLower().Contains("brand")) { clsOPCNodes[0] = node; }
                    if (node.Name.ToLower().Contains("auto")) { clsOPCNodes[1] = node; }
                    if (node.Name.ToLower().Contains("gewenst")) { clsOPCNodes[2] = node; }
                    if (node.Name.ToLower().Contains("huidig")) { clsOPCNodes[3] = node; }
                    //Als de array opgevuld is met een node op elke plaats, dan pas word deze getoond. Na het tonen word de array gecleared om een nieuwe verwarming te kunnen maken.
                    if ((clsOPCNodes[0] != null) & (clsOPCNodes[1] != null) & (clsOPCNodes[2] != null) & (clsOPCNodes[3] != null))
                    {
                        ToonWarm(clsOPCNodes[0], clsOPCNodes[1], clsOPCNodes[2], clsOPCNodes[3]);
                        clsOPCNodes = new clsOPCNode[4];
                    }
                }
            }
            else
            {
                foreach (clsOPCNode subnode in node.GetOPCNodes())
                {
                    ToonControls(subnode);
                }
            }
        }
        private void ToonLamp(clsOPCNode node)
        {
            ccLamp lamp = new ccLamp(node);
            grdMain.Children.Add(lamp);
            lamp.Name = Truncate(node.Name); //Aangezien de prefix en suffix nutteloze informatie zijn voor een gebruiker worden deze weggehaald, enkel de naam blijft over en word getoond.
            lamp.Tag = node.ItemId; //Een 2de plaats in een control waar je 'iets' kan opslaan, dit kan gelijk welk type zijn. Nu wordt het .ItemId van een node ingevuld om zo een unieke naam te krijgen bij het opslaan van posities.
            lamp.Width = 20;
            lamp.Height = 20;
            lamp.MouseDown += ToonLampDetails;
            lamp.SetValue(Grid.RowProperty, 1);
            lamp.SetValue(Grid.ColumnProperty, 0);
            lamp.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            lamp.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            lamp.Margin = new Thickness(10, 10, 0, 0);
            lamp.RenderTransform = new TranslateTransform();
            MouseDragElementBehavior behavior = new MouseDragElementBehavior();
            behavior.Attach(lamp);
            clsOPCCSV csvdata = clsOPCCSV.ReadCSV("Domotica.csv", node.ItemId);
            if (csvdata != null)
            {
                TranslateTransform transform = lamp.RenderTransform as TranslateTransform;
                transform.X = csvdata.X;
                transform.Y = csvdata.Y;
            }
        }
        private void ToonLampDetails(object sender, MouseButtonEventArgs e)
        {
            grdDetails.Children.Clear();
            ccLamp lamp = sender as ccLamp;
            if (lamp == null) return;
            grdDetails.Children.Add(lamp.LampDetail);
        }
        private void ToonWarm(clsOPCNode isaannode, clsOPCNode isautonode, clsOPCNode tgewenstnode, clsOPCNode thuidignode)
        {
            ccWarm warm = new ccWarm(isaannode, isautonode, tgewenstnode, thuidignode);
            grdMain.Children.Add(warm);
            String Name = Truncate(isaannode.Name); //Als basis van de naam gebruiken we de .Name van isaannode waar we de pre & suffix van verwijderen, de overschot bevat echter ook nog de suffix 'Brand'.
            warm.Name = Name.Substring(0, Name.Length - 5); //Om ook deze 'Brand' weg te halen maken we nog een subselectie.
            warm.Tag = isaannode.ItemId; //Aangezien er maar 1 control geplaatst wordt, kan de .ItemId van de isaannode gebruikt bij het opslaan en laden van posities. Alle nodes van een verwarming zitten samen. 
            warm.Width = 20;
            warm.Height = 20;
            warm.MouseDown += ToonWarmDetails;
            warm.SetValue(Grid.RowProperty, 1);
            warm.SetValue(Grid.ColumnProperty, 0);
            warm.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            warm.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            warm.Margin = new Thickness(10, 10, 0, 0);
            warm.RenderTransform = new TranslateTransform();
            MouseDragElementBehavior behavior = new MouseDragElementBehavior();
            behavior.Attach(warm);
            clsOPCCSV csvdata = clsOPCCSV.ReadCSV("Domotica.csv", isaannode.ItemId);
            if (csvdata != null)
            {
                TranslateTransform transform = warm.RenderTransform as TranslateTransform;
                transform.X = csvdata.X;
                transform.Y = csvdata.Y;
            }
        }
        private void ToonWarmDetails(object sender, MouseButtonEventArgs e)
        {
            grdDetails.Children.Clear();
            ccWarm warm = sender as ccWarm;
            if (warm == null) return;
            grdDetails.Children.Add(warm.WarmDetail);
        }
        private String Truncate(String text)
        {
            //Prefix: Vanaf 5de tot en met laatste teken overhouden.
            if (text.StartsWith("lamp"))
            {
                text = text.Substring(4, text.Length - 4); //-4 omdat je de eerste 4 tekens skipt
            }
            if (text.StartsWith("warm"))
            {
                text = text.Substring(4, text.Length - 4);
            }
            //Suffix: Vanaf 1ste tot en met X voor het laatste teken overhouden.
            if (text.EndsWith("Read"))
            {
                text = text.Substring(0, text.Length - 4); //-4 omdat 'Read' 4 tekens is
            }
            if (text.EndsWith("Write"))
            {
                text = text.Substring(0, text.Length - 5);
            }
            //Return wat overblijft
            return text;
        }
    }
}

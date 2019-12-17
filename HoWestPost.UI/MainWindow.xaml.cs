using HoWestPost.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace HoWestPost.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

       
        int[] tripTime = new int[] { 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90 };

        bool IsPrior = false;

        int PriorsTeller = 0;
        private DeliveryProcessor deliveryProcessor;
        ObservableCollection<Delivery> deliveries = new ObservableCollection<Delivery>();
        public ObservableCollection<Delivery> Deliveries { get { return deliveries; } }

        public MainWindow()
        {
            InitializeComponent();
            deliveryProcessor = new DeliveryProcessor();
            listViewPakkets.ItemsSource = deliveries;
            cmbTripTime.ItemsSource = tripTime;
            cmbTripTime.SelectedIndex = 0;
        }

        private void BtnMini_Click(object sender, RoutedEventArgs e)
        {


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}

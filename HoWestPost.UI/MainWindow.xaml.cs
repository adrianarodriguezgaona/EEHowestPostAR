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

       
        int[] tripTime = new int[] {35, 40, 45, 50, 55, 60,65,70,75,80,85,90};

        bool isPrior;

        string priorString;

        int timeChoose;

        int counterForSetUpSending = 0;

        private Delivery delivery;
      
        private DeliveryProcessor deliveryProcessor;

        private ObservableCollection<Delivery> deliveries = new ObservableCollection<Delivery>();
        public ObservableCollection<Delivery> Deliveries { get { return deliveries; } }

        public MainWindow()
        {
            InitializeComponent();
            timeChoose = 0;                
            listViewPakkets.ItemsSource = deliveries;
            cmbTripTime.ItemsSource = tripTime;
            cmbTripTime.SelectedIndex = 0;           
        }

      

        private void IsPriorOrNot()
        {
            if (chBoxPrior.IsChecked == true)
            {
                isPrior = true;
            }
            else
                isPrior = false;
        }

        string StringforPrior()
        {
            if (isPrior)
            {
                priorString = "pakket (prior)";
            }
            else
                priorString = "pakket";

            return priorString;
        }

        // aanpassing = als prior gekozen is dan is standaard travelTimeToDestination =30, SetUpSending wordt hier (niet meer in de constructor) uitgevoerd enkel als
        // counterForSetUpSending == 1 ( dit telt ook voor BtnStandaard and BtnMaxi)
        private void BtnMini_Click(object sender, RoutedEventArgs e)
        {
            counterForSetUpSending++;
            IsPriorOrNot();
            StringforPrior();
            timeChoose = (int)cmbTripTime.SelectedValue;
            if (isPrior)
            {
                var currentDeliveryPrior = new Delivery(PackageType.Mini, 30 , isPrior, priorString, 1);
                deliveries.Add(currentDeliveryPrior);
            }
            else
            {
                var currentDelivery = new Delivery(PackageType.Mini, timeChoose, isPrior, priorString, 1);
                deliveries.Add(currentDelivery);
            }
           
            if (counterForSetUpSending == 1)
            {
                SetUpSending();
            }

        }

        private void BtnStandard_Click(object sender, RoutedEventArgs e)
        {
            counterForSetUpSending++;
            IsPriorOrNot();
            StringforPrior();
            timeChoose = (int)cmbTripTime.SelectedValue;
            if (isPrior)
            {
                var currentDeliveryPrior= new Delivery(PackageType.Standard, 30 , isPrior, priorString, 1.2M);
                deliveries.Add(currentDeliveryPrior);
            }
            else
            {
                var currentDelivery = new Delivery(PackageType.Standard, timeChoose, isPrior, priorString, 1.2M);
                deliveries.Add(currentDelivery);
            }
           
            if (counterForSetUpSending == 1)
            {
                SetUpSending();
            }
        }

        private void BtnMaxi_Click(object sender, RoutedEventArgs e)
        {
            counterForSetUpSending++;
            IsPriorOrNot();
            StringforPrior();
            timeChoose = (int)cmbTripTime.SelectedValue;
            if (isPrior)
            {
                var currentDeliveryPrior = new Delivery(PackageType.Maxi, 30 , isPrior, priorString, 1.5M);
                deliveries.Add(currentDeliveryPrior);
            }
            else
            {
                var currentDelivery = new Delivery(PackageType.Maxi, timeChoose, isPrior, priorString, 1.5M);
                deliveries.Add(currentDelivery);
            }
           
            if (counterForSetUpSending == 1)
            {
                SetUpSending();
            }
        }

        // StartSending van deliveryProcessor wordt hiet uitgevoerd ( niet meer in constructor), delivery en deliveryProcessor hier geïnstantieerd , method DeletefromList
        private void SetUpSending()
        {

            if (deliveries.Count > 0)
            {
                prgBarTime.Value = 0;
                deliveryProcessor = new DeliveryProcessor();
                delivery = new Delivery(deliveries[0].PackageType, deliveries[0].TravelTime, deliveries[0].IsPrior, deliveries[0].PriorString, deliveries[0].Factor);
                deliveryProcessor.CurrentDelivery = delivery;
                lblPakketType.Content = delivery.PackageType;
                var timeforLabel = Decimal.ToInt32(delivery.TravelTime * delivery.Factor);
                lblTripTime.Content = $"{timeforLabel} min";
                PrintPrior();
                DeleteFromList();

                deliveryProcessor.StartSending();
                //invocation lijst:
                deliveryProcessor.OnDelivering += PakketIsSending;
                deliveryProcessor.OnSend += Reset;
            }

        }

        private void PrintPrior()
        {
            if (delivery.IsPrior == true)
                lblPrior.Content = "Ja";
            else
                lblPrior.Content = "Nee";
        }

        // FIFO dus eerste IN pakket (index 0) wordt eerst OUT
        private void DeleteFromList()
        {
            if (deliveries.Count > 0)
            {
                deliveries.RemoveAt(0);
            }
        }
        // Method zo goed als volledig aangepast: nu krijgt als parameter een object sender en een DeliveryEventArgs en wordt uitgevoerd door Ondelivering event
        private void PakketIsSending(object sender, DeliveryEventArgs e)
        {
            if (Dispatcher.CheckAccess())
            {
                lblResterend.Content = $"{e.RemainingTime} min";
                prgBarTime.Maximum = e.TotalTimeSendingInt;
                prgBarTime.Value = e.PassedTime;
            }
            else
            {
                //dispatch to MAIN thread
                Dispatcher.Invoke(() => { PakketIsSending(sender, e); });
            }

        }

        // Nieuwe methode = wordt uitgevoerd door OnSend event 
        private void Reset(object sender, DeliveryEventArgs e)
        {
            if (Dispatcher.CheckAccess())
            {
                deliveryProcessor.Stop();               
                if (deliveries.Count == 0)
                {
                    counterForSetUpSending = 0;
                }
                SetUpSending();
              
            }

            else
            {
                //dispatch to MAIN thread
                Dispatcher.Invoke(() => { Reset(sender, e); });
            }
        }


        // afbeelding wordt getoond bij event window loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            imgPakket.Source = new BitmapImage(new Uri("assets/images/pakket.png", UriKind.Relative));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            deliveryProcessor.Stop();
            Close();
        }

    }
}

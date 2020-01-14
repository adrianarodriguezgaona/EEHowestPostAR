using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace HoWestPost.Domain
{
    // nu krijgt de delegate en object sender en een delyveryEventArgs, naam aangepast  naar DeliveryEventHandler

    public delegate void DeliveryEventHandler(object sender, DeliveryEventArgs e);
    public class DeliveryProcessor
    {
        //TODO proces deliveries...  
        private int passedTime;
       
        private Thread senderThread;
        private bool isSending;

        private decimal totalTimeSending = 0M;

        private int remainingTime;
      
        private Delivery currentDelivery;
        public Delivery CurrentDelivery
        {
            get { return currentDelivery; }
            set { currentDelivery = value; }
        }

        private int totalTimeSendingInt = 0;
     
        public event DeliveryEventHandler OnDelivering;
        public event DeliveryEventHandler OnSend;

        public DeliveryProcessor()
        {
            passedTime = 0;
            isSending = false;   
        
        }

        public void StartSending()
        {
            if (senderThread == null)
            {
                isSending = true;
                senderThread = new Thread(() => { SendingProcedure(); });

                //door de thread als background in te stellen wordt het 'onderdeel' van de main thread
                senderThread.IsBackground = true;

                senderThread.Start();
            }
        }

        private void SendingProcedure()
        {
            //wordt uitgevoerd in subthread
            while (isSending)
            {
                Send();
            }
        }

        // method send bijna volledig aangepast = Ondelivering kan invoke zijn als passedTime < totamTimeSendingInt is en OnSend als passedTime == totalTimeSendingInt
        private void Send()
        {           
                // 10 "minutes" = 1 second
               
                Thread.Sleep(100);
           
                if(passedTime < totalTimeSendingInt) { 
                  passedTime ++;
                }
                totalTimeSending = currentDelivery.TravelTime * currentDelivery.Factor;
                totalTimeSendingInt = Decimal.ToInt32(totalTimeSending);
                remainingTime = totalTimeSendingInt - passedTime;

               if (OnDelivering != null)
               { 
                 OnDelivering?.Invoke(this, new DeliveryEventArgs(passedTime,remainingTime,totalTimeSendingInt));
               }

               if (passedTime == totalTimeSendingInt)
               {
                  if (OnSend != null)
                  {
                    OnSend?.Invoke(this, new DeliveryEventArgs(passedTime, remainingTime, totalTimeSendingInt));
                  }

               }
        }

        public void Stop()
        {           
            isSending = false;
            senderThread = null;
        }


    }


}

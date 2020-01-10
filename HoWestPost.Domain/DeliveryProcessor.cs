using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace HoWestPost.Domain
{
    public delegate void PakketSendingEventHandler(object sender, int passedTime, int remainingTime, int totalTimeSendingInt);
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
     
        public event PakketSendingEventHandler OnDelivering;
        public event PakketSendingEventHandler OnSend;

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

        private void Send()
        {
            if (currentDelivery.IsPrior)
            {
                Thread.Sleep(90);
            }
            else
                // 10 minutes(label) = 1 second
            Thread.Sleep(100);
           
                if(passedTime < totalTimeSendingInt) { 
                  passedTime ++;
                }
                totalTimeSending = currentDelivery.TravelTime * currentDelivery.Factor;
                totalTimeSendingInt = Decimal.ToInt32(totalTimeSending);
                remainingTime = totalTimeSendingInt - passedTime;

               if (OnDelivering != null)
               { 
                 OnDelivering?.Invoke(this, passedTime, remainingTime, totalTimeSendingInt);
               }

               if (passedTime == totalTimeSendingInt)
               {
                  if (OnSend != null)
                  {
                    OnSend?.Invoke(this, passedTime, remainingTime, totalTimeSendingInt);
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

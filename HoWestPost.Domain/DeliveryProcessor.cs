using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace HoWestPost.Domain
{
    public delegate void PakketSendingEventHandler(object sender);
    public class DeliveryProcessor
    {
        //TODO proces deliveries...  
        private Delivery currentDelivery;
        private Thread senderThread;
        private bool SendingIsRunning;

       
        public event PakketSendingEventHandler Delivering;


        public void StartSending(object sender)
        {
            if (senderThread == null)
            {
                SendingIsRunning = true;
                senderThread = new Thread(() => { SendingProcedure(); });

              
                senderThread.IsBackground = true;

                senderThread.Start();
            }
        }

        private void SendingProcedure()
        {
            throw new NotImplementedException();
        }

        public void AddToList(object sender)
        {

        }
    }


}

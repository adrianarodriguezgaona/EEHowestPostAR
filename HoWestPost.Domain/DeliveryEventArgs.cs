using System;
using System.Collections.Generic;
using System.Text;

namespace HoWestPost.Domain
{
    public class DeliveryEventArgs : EventArgs
    {
       
            public DeliveryEventArgs(
                    int PassedTime,
                    int RemainingTime,
                    int TotalTimeSendingInt)
            {
            this.PassedTime = PassedTime;
            this.RemainingTime = RemainingTime;
            this.TotalTimeSendingInt = TotalTimeSendingInt;
              
            }

            public int PassedTime { get; set; }
            public int RemainingTime { get; set; }
            public int TotalTimeSendingInt { get; set; }
            
    }
}

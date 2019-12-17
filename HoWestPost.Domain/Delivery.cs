using System;
using System.Collections.Generic;
using System.Text;

namespace HoWestPost.Domain
{
    public class Delivery
    {
        public PackageType PackageType { get; private set; }
        public int TravelTime { get; private set; }
        public bool IsPrior { get; private set; }

        public Delivery(PackageType packageType, int travelTimeToDestination, bool isPriorOrNot)
        {
            this.PackageType = packageType;
            this.TravelTime = travelTimeToDestination;
            this.IsPrior = isPriorOrNot;
        }
    }

}

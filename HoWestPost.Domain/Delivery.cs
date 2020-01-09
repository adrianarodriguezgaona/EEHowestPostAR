using System;
using System.Collections.Generic;
using System.Text;

namespace HoWestPost.Domain
{
    public class Delivery
    {
        private PackageType packageType;

        public PackageType PackageType
        {
            get { return packageType; }
        }

        private int travelTime;

        public int TravelTime
        {
            get { return travelTime; }
           
        }

        private bool isPrior;

        public bool IsPrior
        {
            get { return isPrior; }
           
        }

        private string priorString;

        public string PriorString
        {
            get { return priorString; }
           
        }

        private decimal factor = 0M;

        public  decimal Factor
        {
            get { return factor; }
           
        }



        public Delivery(PackageType packageType, int travelTimeToDestination, bool isPriorOrNot, string priorString, decimal factor)
        {
            this.packageType = packageType;
            this.travelTime = travelTimeToDestination;
            this.isPrior = isPriorOrNot;
            this.priorString = priorString;
            this.factor = factor;

        }


    }

}

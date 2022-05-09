using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    public class Screen
    {
        #region Variables

        //Object Props
        public string ScName;

        public float Balance;
        public float PriceF;
        public float PriceH;

        public int Capacity;
        public int TotSeatSold = 0;
        public int EmptySeats;

        //Related to Seat
        public int SoldSeatsF = 0;
        public int SoldSeatsH = 0;
        public int SoldSeatsTotF = 0;
        public int SoldSeatsTotH = 0;

        public int RefSeatsF = 0;
        public int RefSeatsH = 0;
        public int RefSeatsTotF = 0;
        public int RefSeatsTotH = 0;

        //Related to Price - Revenue
        public float RevTot = 0;
        public float RevF = 0;
        public float RevH = 0;
        public float RevTotF = 0;
        public float RevTotH = 0;

        public float RefRevF = 0;
        public float RefRevH = 0;
        public float RefRevTotal = 0;

        #endregion

        #region Num of Seats

        public Screen()
        {

        }

        public Screen(string scName, int capacity, int emptySeats, float balance, float priceF, float priceH)
        {
            ScName = scName;
            Capacity = capacity;
            EmptySeats = emptySeats;
            Balance = balance;
            PriceF = priceF;
            PriceH = priceH;
        }


        public static int NumSeats(int seats, int tot) // Num of seats per Full priced or Reduced priced tickets
        {
            tot += seats;
            return tot;
        }

        public static int NumTotSeat(int seatsF, int seatsH) // Total Num of seats
        {
            int tot = seatsF + seatsH;
            return tot;
        }

        public static int NumEmptySeats(int capacity, int totSeatsSold, int emptySeats) // Num of empty seats
        {
            emptySeats = capacity - totSeatsSold;
            return emptySeats;
        }

        #endregion

        #region Price Calculation

        public static float ReducedPrice(float fPrice, float discount) // reduced price
        {
            float redPrice = fPrice - (discount * fPrice) / 100;
            return redPrice;
        }

        public static float Cost(float price, int seat)
        {
            float rev = price * seat;
            return rev;
        }


        public static float Sum(float revF, float revH)
        {
            float rev = revF + revH;
            return rev;
        }

        public static float Refund(float revF, float revH, float refund)
        {
            refund = refund - revF - revH;
            return refund;
        }

        #endregion
    }
}

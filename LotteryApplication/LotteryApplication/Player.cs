using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryApplication
{
    public class Player
    {
        public string Name { get; set; } = "Player ";
        public decimal Balance { get; set; } = 10;
        public int NumberOfLotteryTickets { get; set; }
        public List<LotteryTicket> LotteryTickets { get; set; } = new List<LotteryTicket>();
        public bool GrandPrizeWinner { get; set; }
        public bool SecondPlaceWinner { get; set; }
        public bool ThirdPlaceWinner { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal FirstPlaceEarnings { get; set; }
        public decimal SecondPlaceEarnings { get; set; }
        public decimal ThirdPlaceEarnings { get; set; }
    }
}

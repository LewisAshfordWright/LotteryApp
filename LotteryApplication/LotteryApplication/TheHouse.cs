using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryApplication
{
    public class TheHouse
    {
        public int TicketsSold { get; set; }
        public List<LotteryTicket> AllPurchasedTickets { get; set; } = new List<LotteryTicket>();
        public decimal HouseRevenue { get; set; }
        public decimal HouseProfit { get; set; }
        public decimal GrandPrize { get; set; }
        public decimal SecondTierPrize { get; set; }
        public decimal ThirdTierPrize { get; set; }
        public LotteryTicket WinningTicket { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LotteryApplication
{
    public class LotteryTicket
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int[] Numbers { get; set; }
        public bool GrandPrizeTicket { get; set; }
        public bool SecondPlaceTicket { get; set; }
        public bool ThirdPlacetTicket { get; set; }

        public LotteryTicket()
        {
            Numbers = new int[4];
            Random randNum = new Random();
            Numbers = Enumerable.Range(1, 25).OrderBy(x => randNum.Next()).Take(4).ToArray();
            Array.Sort(Numbers);
        }
    }
}

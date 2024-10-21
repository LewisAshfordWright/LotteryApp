using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LotteryApplication
{
    public class Helper
    {
        public static bool NumberOfTicketsValid(int selectedAmountOfTickets)
        {
            return (selectedAmountOfTickets <= 0 || selectedAmountOfTickets > 10) ? false : true;
        }

        public static bool BalanceValid(decimal balance, int input)
        {
            return balance < input ? false : true;
        }

        public static List<Player> CreateCpuPlayers()
        {
            var playerList = new List<Player>();
            Random rnd = new Random();

            int numberOfCpuPlayers = rnd.Next(10, 15);
            for (int i = 0; i < numberOfCpuPlayers; i++)
            {
                Player player = new Player();
                player.NumberOfLotteryTickets = rnd.Next(1, 11);
                GeneratePlayerLotteryTickets(player.NumberOfLotteryTickets, player);
                player.Name = player.Name + $"{i + 2}";
                player.Balance = player.Balance - player.NumberOfLotteryTickets;
                playerList.Add(player);
            }

            return playerList;
        }

        public static void GeneratePlayerLotteryTickets(int numberBought, Player player)
        {
            for (int i = 0; i < numberBought; i++)
            {
                player.LotteryTickets.Add(new LotteryTicket());
            }
        }

        private static int GetNumberOfTicketsSold(List<Player> allPlayers)
        {
            int ticketCount = 0;
            foreach (var player in allPlayers)
            {
                ticketCount = ticketCount + player.NumberOfLotteryTickets;
            }
            return ticketCount;
        }

        private static List<LotteryTicket> GetAllLotteryTickets(List<Player> allPlayers)
        {
            var allTickets = new List<LotteryTicket>();
            foreach (var player in allPlayers)
            {
                allTickets.AddRange(player.LotteryTickets);
            }
            return allTickets;
        }

        private static decimal CalculateHouseRevenue(List<Player> allPlayers)
        {
            decimal houseRevenue = 0;
            foreach (var player in allPlayers)
            {
                houseRevenue = houseRevenue + player.NumberOfLotteryTickets;
            }

            return houseRevenue;
        }

        private static LotteryTicket GetWinningTicket(List<Player> allPlayers)
        {
            var allTickets = new List<LotteryTicket>();
            foreach (var player in allPlayers)
            {
                allTickets.AddRange(player.LotteryTickets);
            }
            var random = new Random();
            int index = random.Next(allTickets.Count);
            return allTickets[index];
        }

        public static Dictionary<LotteryTicket, Player> SecondPlaceWinners(List<Player> allPlayers, TheHouse house)
        {
            var secondPlacePlayers = new Dictionary<LotteryTicket, Player>();
            var winningTicketNumbers = house.WinningTicket.Numbers;
            var totalTickets = house.TicketsSold;
            var maximumWinnerAmount = Math.Round((totalTickets * 0.1), MidpointRounding.AwayFromZero);
            foreach (var player in allPlayers)
            {
                foreach (var lotteryTicket in player.LotteryTickets)
                {
                    if (!HasTicketWonAlready(lotteryTicket))
                    {
                        if ((lotteryTicket.Numbers.Contains(winningTicketNumbers[0]) && lotteryTicket.Numbers.Contains(winningTicketNumbers[1]) && lotteryTicket.Numbers.Contains(winningTicketNumbers[2]))
                            || (lotteryTicket.Numbers.Contains(winningTicketNumbers[0]) && lotteryTicket.Numbers.Contains(winningTicketNumbers[1]) && lotteryTicket.Numbers.Contains(winningTicketNumbers[3]))
                            || (lotteryTicket.Numbers.Contains(winningTicketNumbers[0]) && lotteryTicket.Numbers.Contains(winningTicketNumbers[2]) && lotteryTicket.Numbers.Contains(winningTicketNumbers[3]))
                            || (lotteryTicket.Numbers.Contains(winningTicketNumbers[1]) && lotteryTicket.Numbers.Contains(winningTicketNumbers[2]) && lotteryTicket.Numbers.Contains(winningTicketNumbers[3])))
                        {
                            lotteryTicket.SecondPlaceTicket = true;
                            secondPlacePlayers.Add(lotteryTicket, player);
                        }
                    }
                }
            }

            if (secondPlacePlayers.Count() > maximumWinnerAmount)
            {
                secondPlacePlayers = ReturnStreamlinedWinningPlayersAndAddToProfit(house, secondPlacePlayers, maximumWinnerAmount);
            }
            else if (secondPlacePlayers.Count() == 0)
            {
                house.HouseProfit = house.HouseProfit + house.SecondTierPrize;
            }

            SetIfWinner(secondPlacePlayers, 2);
            return secondPlacePlayers;
        }

        public static Dictionary<LotteryTicket, Player> ThirdPlaceWinners(List<Player> allPlayers, TheHouse house)
        {
            var thirdPlacePlayers = new Dictionary<LotteryTicket, Player>();
            var winningTicketNumbers = house.WinningTicket.Numbers;
            var totalTickets = house.TicketsSold;
            var maximumWinnerAmount = Math.Round((totalTickets * 0.2), MidpointRounding.AwayFromZero);

            foreach (var player in allPlayers)
            {
                foreach (var lotteryTicket in player.LotteryTickets)
                {
                    if (!HasTicketWonAlready(lotteryTicket))
                    {
                        if ((lotteryTicket.Numbers.Contains(winningTicketNumbers[0]) && lotteryTicket.Numbers.Contains(winningTicketNumbers[1]))
                        || (lotteryTicket.Numbers.Contains(winningTicketNumbers[0]) && lotteryTicket.Numbers.Contains(winningTicketNumbers[2]))
                        || (lotteryTicket.Numbers.Contains(winningTicketNumbers[0]) && lotteryTicket.Numbers.Contains(winningTicketNumbers[3]))
                        || (lotteryTicket.Numbers.Contains(winningTicketNumbers[1]) && lotteryTicket.Numbers.Contains(winningTicketNumbers[2]))
                        || (lotteryTicket.Numbers.Contains(winningTicketNumbers[1]) && lotteryTicket.Numbers.Contains(winningTicketNumbers[3]))
                        || (lotteryTicket.Numbers.Contains(winningTicketNumbers[2]) && lotteryTicket.Numbers.Contains(winningTicketNumbers[3])))
                        {
                            lotteryTicket.ThirdPlacetTicket = true;
                            thirdPlacePlayers.Add(lotteryTicket, player);
                        }
                    }
                }
            }

            if (thirdPlacePlayers.Count() > maximumWinnerAmount)
            {
                thirdPlacePlayers = ReturnStreamlinedWinningPlayersAndAddToProfit(house, thirdPlacePlayers, maximumWinnerAmount);
            }
            else if (thirdPlacePlayers.Count() == 0)
            {
                house.HouseProfit = house.HouseProfit + house.ThirdTierPrize;
            }

            SetIfWinner(thirdPlacePlayers, 3);
            return thirdPlacePlayers;
        }

        private static void SetIfWinner(Dictionary<LotteryTicket, Player> players, int place)
        {
            foreach (var player in players)
            {
                switch (place)
                {
                    case 2:
                        player.Value.SecondPlaceWinner = true;
                        break;
                    case 3:
                        player.Value.ThirdPlaceWinner = true;
                        break;
                }
            }
        }
        private static Dictionary<LotteryTicket, Player> ReturnStreamlinedWinningPlayersAndAddToProfit(TheHouse house, Dictionary<LotteryTicket, Player> winningPlayers, double maximumWinnerAmount)
        {
            var remainingRevenue = winningPlayers.Count() - maximumWinnerAmount;
            while (winningPlayers.Count > maximumWinnerAmount)
            {
                var lastPlayerInDictionary = winningPlayers.Last();
                winningPlayers.Remove(lastPlayerInDictionary.Key);
            }
            house.HouseProfit = house.HouseProfit + (decimal)remainingRevenue;

            return winningPlayers;
        }

        private static decimal CalculateFinalPrize(List<Player> allPlayers)
        {
            return CalculateHouseRevenue(allPlayers) / 2;
        }

        private static double CalculateSecondPrize(List<Player> allPlayers)
        {
            return (double)CalculateHouseRevenue(allPlayers) * 0.3;
        }

        private static double CalculateThirdPrize(List<Player> allPlayers)
        {
            return (double)CalculateHouseRevenue(allPlayers) * 0.1;
        }

        public static TheHouse SetUpTheHouse(List<Player> allPlayers)
        {
            var theHouse = new TheHouse()
            {
                TicketsSold = GetNumberOfTicketsSold(allPlayers),
                AllPurchasedTickets = GetAllLotteryTickets(allPlayers),
                HouseRevenue = CalculateHouseRevenue(allPlayers),
                WinningTicket = GetWinningTicket(allPlayers),
                GrandPrize = CalculateFinalPrize(allPlayers),
                SecondTierPrize = (decimal)CalculateSecondPrize(allPlayers),
                ThirdTierPrize = (decimal)CalculateThirdPrize(allPlayers),
            };
            theHouse.HouseProfit = theHouse.ThirdTierPrize;

            return theHouse;
        }

        public static Player GetWinningPlayer(List<Player> allPlayers, LotteryTicket winningTicket, decimal prizeMoney)
        {
            var winner = new Player();
            foreach (var player in allPlayers)
            {
                foreach (var ticket in player.LotteryTickets)
                {
                    if (ticket == winningTicket)
                    {
                        ticket.GrandPrizeTicket = true;
                        player.GrandPrizeWinner = true;
                        player.Balance = player.Balance + prizeMoney;
                        player.TotalEarnings = prizeMoney;
                        player.FirstPlaceEarnings = prizeMoney;
                        winner = player;
                    }
                }
            }
            return winner;
        }

        private static bool HasTicketWonAlready(LotteryTicket ticket)
        {
            return (ticket.GrandPrizeTicket || ticket.SecondPlaceTicket) ? true : false;
        }

        public static void CalculatePlayerWinnings(Dictionary<LotteryTicket, Player> players, TheHouse house, int prize)
        {
            if (players.Count > 0)
            {
                double amountPerPlayerToDistribute = 0;
                switch (prize)
                {
                    case 2:
                        amountPerPlayerToDistribute = (double)house.SecondTierPrize / players.Values.Count();
                        break;
                    case 3:
                        amountPerPlayerToDistribute = (double)house.ThirdTierPrize / players.Values.Count();
                        break;
                }

                amountPerPlayerToDistribute = Math.Round(amountPerPlayerToDistribute, 2, MidpointRounding.AwayFromZero);
                foreach (var player in players.Values)
                {
                    player.TotalEarnings = player.TotalEarnings += (decimal)amountPerPlayerToDistribute;
                    player.Balance = player.Balance += (decimal)amountPerPlayerToDistribute;
                    switch (prize)
                    {
                        case 2:
                            player.SecondPlaceEarnings = player.SecondPlaceEarnings += (decimal)amountPerPlayerToDistribute;
                            break;
                        case 3:
                            player.ThirdPlaceEarnings = player.ThirdPlaceEarnings += (decimal)amountPerPlayerToDistribute;
                            break;
                    }
                }
            }
        }
    }
}

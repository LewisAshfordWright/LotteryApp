using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LotteryApplication
{
    public class Program
    {
        static void Main(string[] args)
        {
            string userInput;
            var player1 = new Player();
            player1.Name = player1.Name + "1";
            var cpuPlayers = Helper.CreateCpuPlayers();

            Console.WriteLine($"Welcome to the Bede Lottery, {player1.Name}. \n");
            Console.WriteLine("* Your balance is currently " + string.Format("{0:C}", player1.Balance) +
                $"\n* Tickets are {string.Format("{0:C}", 1)} each \n");

            while (true)
            {
                Console.WriteLine("How many tickets would you like to purchase?");
                userInput = Console.ReadLine();
                Console.WriteLine();
                int.TryParse(userInput, out int number);
                if (Helper.NumberOfTicketsValid(number) && Helper.BalanceValid(player1.Balance, number))
                {
                    player1.Balance = player1.Balance - number;
                    Console.WriteLine("You have purchased " + userInput + " tickets \n" +
                        "Your balance is now " + string.Format("{0:C}", player1.Balance));
                    player1.NumberOfLotteryTickets = number;
                    Helper.GeneratePlayerLotteryTickets(number, player1);
                    break;
                }
                else if (!Helper.NumberOfTicketsValid(number) && Helper.BalanceValid(player1.Balance, number))
                {
                    Console.WriteLine("**ERROR** You have entered an invalid amount.\n" +
                        "Please select an amount of 10 or less.");
                }
                else if (Helper.NumberOfTicketsValid(number) && !Helper.BalanceValid(player1.Balance, number))
                {
                    Console.WriteLine("**ERROR** You have insufficient funds for this amount of tickets.\n" +
                        $"Your current balance is: {string.Format("{0:C}", player1.Balance)}.");
                }
                else if (!Helper.NumberOfTicketsValid(number) && !Helper.BalanceValid(player1.Balance, number))
                {
                    Console.WriteLine("**ERROR** You have entered an invalid amount and also have insufficient funds for this amount of tickets.\n" +
                        $"Your current balance is: {string.Format("{0:C}", player1.Balance)}.");
                }
            }

            Console.WriteLine($"\n{cpuPlayers.Count} other CPU players have purchased tickets.");

            foreach (var player in cpuPlayers)
            {
                Console.WriteLine($"{player.Name} has bought {player.NumberOfLotteryTickets} tickets.");
            }

            System.Threading.Thread.Sleep(1000);

            Console.WriteLine("\nGood luck to all players!");

            var allPlayers = new List<Player>();
            allPlayers.Add(player1);
            allPlayers.AddRange(cpuPlayers);

            var theHouse = new TheHouse();
            theHouse = Helper.SetUpTheHouse(allPlayers);

            Console.WriteLine($"\nThe total number of tickets that have been bought is: {theHouse.TicketsSold}");

            System.Threading.Thread.Sleep(2000);

            Console.WriteLine("Here we go. Revealing winners in:");
            for (int tick = 5; tick >= 0; tick--)
            {
                Console.WriteLine("\r{0:0}\n", tick);
                System.Threading.Thread.Sleep(1000);
            }

            Console.WriteLine($"\nThe winning numbers of Today's draw are: {string.Join(",", theHouse.WinningTicket.Numbers)}\n");

            System.Threading.Thread.Sleep(2000);

            var winningPlayer = Helper.GetWinningPlayer(allPlayers, theHouse.WinningTicket, theHouse.GrandPrize);
            var secondPlaceWinners = Helper.SecondPlaceWinners(allPlayers, theHouse);
            var thirdPlaceWinners = Helper.ThirdPlaceWinners(allPlayers, theHouse);

            Helper.CalculatePlayerWinnings(secondPlaceWinners, theHouse, 2);
            Helper.CalculatePlayerWinnings(thirdPlaceWinners, theHouse, 3);

            Console.WriteLine("Ticket Draw Results:\n" +
                $"* Grand Prize:\n{winningPlayer.Name} has won {string.Format("{0:C}", theHouse.GrandPrize)}");

            System.Threading.Thread.Sleep(2000);

            Console.WriteLine("\n* Second Tier Prize:");

            if (secondPlaceWinners.Count != 0)
            {
                foreach (var kvp in secondPlaceWinners.Values.Distinct())
                {
                    Console.WriteLine(string.Format("{0} has won ", kvp.Name) + $"{string.Format("{0:C}", kvp.SecondPlaceEarnings)}");
                }
            }
            else
            {
                Console.WriteLine("Looks like no one won the Second Tier Prize this time. Oh well... More money for us.");
            }

            System.Threading.Thread.Sleep(2000);

            Console.WriteLine("\n* Third Tier Prize:");

            if (thirdPlaceWinners.Count != 0)
            {
                foreach (var kvp in thirdPlaceWinners.Values.Distinct())
                {
                    Console.WriteLine(string.Format("{0} has won ", kvp.Name) + $"{string.Format("{0:C}", kvp.ThirdPlaceEarnings)}");
                }
            }
            else
            {
                Console.WriteLine("Wow... Not one of you even won the Third Tier Prize. Guess we've fleeced you pretty well.");
            }

            System.Threading.Thread.Sleep(2000);

            if (winningPlayer.Name == player1.Name)
            {
                Console.WriteLine("\nYOU WON BIG! WELL DONE!\n" + "Looks like the drinks are on you tonight...\n");
            }
            else if (secondPlaceWinners.Any(x => x.Value.Name == player1.Name))
            {
                Console.WriteLine("\nYou won a decent amount! Well done!\n" + "Get yourself a few pints in tonight!\n");
            }
            else if (thirdPlaceWinners.Any(x => x.Value.Name == player1.Name))
            {
                Console.WriteLine("\nYou won a little bit! Well done!\n" + "Put this towards a pint or another lottery ticket.\n");
            }
            else if ((winningPlayer.Name != player1.Name
                || secondPlaceWinners.Any(x => x.Value.Name != player1.Name)
                || thirdPlaceWinners.Any(x => x.Value.Name != player1.Name))
                && player1.Balance == 0)
            {
                Console.WriteLine("\nBad luck, you didn't win this time. And you blew your budget.\n" + "Maybe don't shoot for the moon next time...\n");
            }
            else
            {
                Console.WriteLine("\nBad luck, you didn't win this time.\n" + "You can always try again.\n");
            }

            Console.WriteLine("Congratulations to the Winners!.\n" +
                $"House Profit: {string.Format("{0:C}", theHouse.HouseProfit)}");

            System.Threading.Thread.Sleep(4000);
            Console.WriteLine("\nThanks for playing.");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("\nGoodbye.");
            System.Threading.Thread.Sleep(2000);
        }
    }
}
using System;
using System.Collections.Generic;

namespace SnakesAndLadders
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Player> players = new List<Player>()
            {
                new Player("Robert"),
                new Player("Stannis"),
                new Player("Renley")
            };

            List<Snake> snakes = new List<Snake>()
            {
                new Snake(17, 7),
                new Snake(54, 34),
                new Snake(62, 19),
                new Snake(64, 60),
                new Snake(87, 36),
                new Snake(92, 73),
                new Snake(95, 75),
                new Snake(98, 79)
            };

            List<Ladder> ladders = new List<Ladder>()
            {
                new Ladder(1, 38),
                new Ladder(4, 14),
                new Ladder(9, 31),
                new Ladder(21, 42),
                new Ladder(28, 84),
                new Ladder(51, 67),
                new Ladder(72, 91),
                new Ladder(80, 99)
            };

            Game game = new Game(snakes, ladders, players);
            Console.WriteLine("Game Started");
            Console.WriteLine("Name\tCurrent Position");
            int playersRemaining = players.Count;
            while (game.Winner == null)
            {
                Random random = new Random();
                for (int i=0; i < players.Count; i++)
                {
                    int diceValue = random.Next(1, 6);
                    game.Roll(players[i], diceValue);
                    game.DisplayStatus();
                    if (game.Winner != null)
                    {
                        Console.WriteLine(game.Winner.Name+" reached 100");
                        game.Players.Remove(players[i]);
                        if (--playersRemaining != 1)
                            game.Winner = null;
                        else
                            break;
                    }
                }
            }

            Console.WriteLine(game.Players[0].Name + " lost LOL!");

        }
    }

    public class Snake
    {
        public int Start { get; set; }
        public int End { get; set; }
        public Snake(int start, int end)
        {
            Start = start;
            End = end;
        }
    }

    public class Ladder
    {
        public int Start { get; set; }
        public int End { get; set; }
        public Ladder(int start, int end)
        {
            Start = start;
            End = end;
        }
    }

    public class Player
    {
        static int playerId = 1;
        public int Id { get; set; }
        public string Name { get; set; }

        public int CurrentPos { get; set; }
        public Player(string name)
        {
            Name = name;
            CurrentPos = 0;
            Id = GetUniqueId();
        }

        public int GetUniqueId()
        {
            return playerId++;
        }
    }

    public class Game
    {
        public List<Player> Players { get; set; }
        public int CurrentTurn { get; set; }
        public Player Winner { get; set; }
        public Dictionary<int,int> SnakesAndLadders { get; set; }
        public Game(List<Snake> snakes, List<Ladder> ladders, List<Player> players)
        {
            Players = players;
            SnakesAndLadders = new Dictionary<int, int>();
            foreach (var snake in snakes)
            {
                SnakesAndLadders[snake.Start] = snake.End;
            }

            foreach (var ladder in ladders)
            {
                SnakesAndLadders[ladder.Start] = ladder.End;
            }

            CurrentTurn = 0;
            Winner = null;
        }

        public bool Roll(Player player, int diceValue)
        {
            if (Winner != null || diceValue > 6 || Players[CurrentTurn].Id != player.Id)
                return false;

            int destination = Players[CurrentTurn].CurrentPos + diceValue;
            
            if(destination <= 100)
            {
                if (SnakesAndLadders.ContainsKey(destination))
                {
                    Players[CurrentTurn].CurrentPos = SnakesAndLadders[destination];
                }
                else
                {
                    Players[CurrentTurn].CurrentPos = destination;
                }
            }

            if(destination == 100)
            {
                Winner = player;
            }

            NextPlayer();
            return true;
        }

        private void NextPlayer()
        {
            CurrentTurn = (CurrentTurn + 1) % Players.Count;
        }

        internal void DisplayStatus()
        {
            foreach (var player in Players)
            {
                Console.WriteLine(player.Name+"\t"+player.CurrentPos);
            }
        }
    }
}

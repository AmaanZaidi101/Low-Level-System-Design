using System;
using System.Collections.Generic;
using System.Linq;

namespace Bowling
{
    class Program
    {
        static void Main(string[] args)
        {
            Player p1 = new Player("Thor");
            Player p2 = new Player("Loki");
            Player p3 = new Player("Hela");
            Player p4 = new Player("Odin");

            List<Player> players = new List<Player>()
            {
                p1,p2,p3,p4
            };

            Game g = new Game();
            int index = g.CreateSession(players);

            //scores for players
            List<int> s1 = new List<int>();
            List<int> s2 = new List<int>();
            List<int> s3 = new List<int>();
            List<int> s4 = new List<int>();

            int score;
            for (int i = 0; i < 20; i++)
            {
                Random r = new Random();
                score = Math.Abs(r.Next() % 10);
                s1.Add(score);
                g.Roll(index, p1, score);
                score = Math.Abs(r.Next() % 10);
                s2.Add(score);
                g.Roll(index, p2, score);
                score = Math.Abs(r.Next() % 10);
                s3.Add(score);
                g.Roll(index, p3, score);
                score = Math.Abs(r.Next() % 10);
                s4.Add(score);
                g.Roll(index, p4, score);
            }

            Console.Write("Player 1 ");
            foreach (var s in s1)
            {
                Console.Write(s+" ");
            }
            Console.WriteLine();

            Console.Write("Player 2 ");
            foreach (var s in s2)
            {
                Console.Write(s + " ");
            }
            Console.WriteLine();

            Console.Write("Player 3 ");
            foreach (var s in s3)
            {
                Console.Write(s + " ");
            }
            Console.WriteLine();

            Console.Write("Player 4 ");
            foreach (var s in s4)
            {
                Console.Write(s + " ");
            }
            Console.WriteLine(); ;

            g.CreateSession(players);
            g.CreateSession(players);
            g.CreateSession(players);
            g.DeclareWinner(index);
            g.CreateSession(players);
        }
    }

    public class Player
    {
        static readonly int MAX_ROLLS_ALLOWED = 23;
        public string Name { get; set; }
        public int Score { get; set; }
        public bool CanPlay { get; set; }
        public int CurrentRoll { get; set; }

        private int[] rolls;
        private bool firstRoll;
        private int frameIndex;
        
        
        public Player(string name)
        {
            Name = name;
            rolls = new int[MAX_ROLLS_ALLOWED];
            Score = 0;
            firstRoll = true;
            frameIndex = 0;
            CanPlay = true;
            CurrentRoll = 0;
        }

        public bool IsStrike()
        {
            return firstRoll && rolls[frameIndex] == 10;
        }

        public bool IsSpare()
        {
            return rolls[frameIndex] + rolls[frameIndex + 1] == 10;
        }

        public void Roll(int pins)
        {
            if (!CanPlay)
                return;
            rolls[CurrentRoll++] = pins;
            updateScore();
        }

        private void updateScore()
        {
            if(IsStrike())
            {
                Score += 20; //10 for pins + 10 bonus points
                rolls[CurrentRoll++] = 0; //skip next turn
                frameIndex += 2; //jump frame 
                if (frameIndex >= MAX_ROLLS_ALLOWED)
                    CanPlay = false;
            }
            else
            {
                if(frameIndex >= MAX_ROLLS_ALLOWED -1)
                {
                    Score += rolls[frameIndex];
                    CanPlay = false;
                }
                else if(firstRoll)
                {
                    firstRoll = false;
                }
                else
                {
                    if (IsSpare())
                        Score += 5;

                    Score += (rolls[frameIndex] + rolls[frameIndex + 1]);
                    frameIndex += 2;
                    firstRoll = true;

                    if (frameIndex >= MAX_ROLLS_ALLOWED - 3)
                        CanPlay = false;
                }
            }
        }
    }

    public class GameSession
    {
        static int gameSessionId = 1;
        public int Alley { get; set; }
        public int Id { get; set; }
        public List<Player> Players { get; set; }

        public GameSession()
        {
            Alley = -1;
            Id = GetUniqueId();
            Players = new List<Player>();
        }

        private int GetUniqueId()
        {
            return gameSessionId++;
        }

        internal void MakeRoll(Player player, int pins)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                if(Players[i].Name == player.Name)
                {
                    player.Roll(pins);
                }
            }
        }

        internal bool DeclareWinner()
        {
            int maxScore = 0;
            Player winner = null;
            foreach (var player in Players)
            {
                if (player.CanPlay)
                {
                    Console.WriteLine($@"Player {player.Name} hasn't completed yet. The current scire: {player.Score}");
                    Console.WriteLine("Match in Progress...");
                    return false;
                }
                if(player.Score > maxScore)
                {
                    maxScore = player.Score;
                    winner = player;
                }
            }
            if(winner != null)
            {
                Console.WriteLine("The winner is " + winner.Name);
            }

            Game.MakeActive(Alley);
            return true;
        }
    }

    public class Game
    {
        public Dictionary<int, GameSession> GameSessions { get; set; }
        static Stack<int> alleys;
        public Game()
        {
            alleys = new Stack<int>(new int[] { 1, 2, 3, 4 });
            GameSessions = new Dictionary<int, GameSession>();
        }

        public int CreateSession(List<Player> players)
        {
            try
            {
                int alleyVal = alleys.Pop();
                GameSession gameSession = new GameSession();
                gameSession.Players = players;
                gameSession.Alley = alleyVal;
                GameSessions.Add(gameSession.Id, gameSession);
                return gameSession.Id;
            }
            catch (Exception)
            {
                Console.WriteLine("All allies are occupied");
                return 0;
            }
        }

        internal static void MakeActive(int alley)
        {
            alleys.Push(alley);
        }

        internal void Roll(int index, Player player, int pins)
        {
            GameSession g = GameSessions[index];
            g.MakeRoll(player, pins);
        }

        internal void DeclareWinner(int index)
        {
            bool winnerFlag = GameSessions[index].DeclareWinner();

            if (!winnerFlag)
            {
                Console.WriteLine("No winners yet");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq;

namespace Contracts
{
    public class Game
    {
        //Player player1 { get; set; }
        //Player player2 { get; set; }
        public List<Player> Players {get; set; }

        public Game()
        {
            Players = new List<Player>();
        }

        public Game(List<Player> pPlayers)
        {
            this.Players = pPlayers;
        }

        /// <summary>
        /// Resolves the match/game
        /// </summary>
        /// <returns>the winner object</returns>
        public Player Resolve()
        {
            if(Players == null || Players.Count() <= 0)
            {
                return null;
            }

            //if draw, then 1st player wins
            if(Players[0].Choice == Players[1].Choice)
            {
                return Players[0];
            }

            var tmp = (from p in Players
                          select p.Choice).ToList();

            var choices = string.Join("," ,tmp);

            //R vs P
            if(choices.Contains("R") && choices.Contains("P"))
            {
                return Players.Where(p => p.Choice == "P").FirstOrDefault();
            }

            //P vs S
            if (choices.Contains("P") && choices.Contains("S"))
            {
                return Players.Where(p => p.Choice == "S").FirstOrDefault();
            }

            //R vs S
            if (choices.Contains("R") && choices.Contains("S"))
            {
                return Players.Where(p => p.Choice == "R").FirstOrDefault();
            }

            return null;

        }

    }
}
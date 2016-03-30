using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contracts
{
    public class Tournament
    {

        public List<Game> Games { get; set; }

        public Tournament() {
            Games = new List<Game>();
        }

        public Tournament(List<Game> pGames)
        {
            this.Games = pGames;
        }

        public List<Player> Resolve()
        {

            List<Player> winners = new List<Player>();
            
            foreach(var g in Games)
            {
                winners.Add(g.Resolve());
            }

            return winners;
        }


      


    }
}
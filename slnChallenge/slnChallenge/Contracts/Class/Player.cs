using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contracts
{
    public class Player
    {
        public string Name { get; set; }
        public string Choice { get; set; }
        public bool IsWinner { get; set; }

        public Player() { }

    }
}
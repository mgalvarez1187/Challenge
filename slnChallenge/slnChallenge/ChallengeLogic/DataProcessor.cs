using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeLogic
{
    public static class DataProcessor
    {

        /// <summary>
        /// Takes a string and returns tournaments
        /// </summary>
        /// <param name="val">string containing the championship data</param>
        /// <returns>a list of torunaments </returns>
        public static List<Tournament> GetChampionshipFromString(string val)
        {
            try
            {
                //string val = "[[\"Armando\", \"P\"],[\"Dave\", \"S\"]]";
                // val = "[[[ [\"Armando\", \"P\"], [\"Dave\", \"S\"] ],[ [\"Richard\", \"R\"], [\"Michael\", \"S\"] ],],[[ [\"Allen\", \"S\"], [\"Omer\", \"P\"] ],[ [\"John\", \"R\"], [\"Robert\", \"P\"] ]]]";

                val = val.Trim();
                val = val.Replace(" ", "");

                List<Tournament> lTourn = new List<Tournament>();

                //currents
                Tournament tournament = new Tournament();
                Game game = new Game();
                Player player = new Player();

                for (int i = 0; i <= val.Length - 1; i++)
                {

                    //opens new tournament, game and player info
                    if (val[i] == '[' && val[i + 1] == '[' && val[i + 2] == '[')
                    {
                        tournament = new Tournament();
                        continue;
                    }

                    // abre juego y abre info
                    if (val[i] == '[' && val[i + 1] == '[')
                    {
                        game = new Game();
                        continue;
                    }

                    // info opening
                    if (val[i] == '[' && val[i + 1] == '\"')
                    {
                        //delimitate
                        int delim = val.IndexOf("]", i);
                        int qty = (delim - i) + 1;
                        player = ExtractPlayerFromString(val.Substring(i, qty));
                        game.Players.Add(player);

                        //2 players per game
                        if (game.Players.Count() == 2)
                        {
                            tournament.Games.Add(game);
                        }

                        continue;
                    }

                    if ((i + 2) <= (val.Length - 1)) //avoid out of range
                    {
                        //closing tournament way 1
                        if (val[i] == ']' && val[i + 1] == ']' && val[i + 2] == ']')
                        {
                            lTourn.Add(tournament);
                            continue;
                        }

                        //closing tournament way 2
                        if (val[i] == ']' && val[i + 1] == ']' && val[i + 2] == ',' && val[i + 3] == ']')
                        {
                            lTourn.Add(tournament);
                            continue;
                        }
                    }


                }

                return lTourn;

            }
            catch (Exception ex)
            {
                //pending log
                // ..
                return null;
            }
        }

        /// <summary>
        /// Extracts the player data from a formated string
        /// </summary>
        /// <param name="info"></param>
        /// <returns>a new player</returns>
        private static Player ExtractPlayerFromString(string info)
        {
            try
            {
                Player player = new Player();

                //remove unused chars
                info = info.Replace("[", "").Replace("]", "").Replace("\"", "");
                var data = info.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                player.IsWinner = false;
                player.Name = data[0];
                player.Choice = data[1];

                return player;
            }
            catch (Exception ex)
            {
                var err = ex;
                return null;
            }
        }


        public static List<Player> ResolveTournaments(List<Tournament> lTournaments)
        {
            try
            {
                List<Player> winners = new List<Player>();

                //new plain list
                foreach (var t in lTournaments)
                {
                    foreach (var g in t.Games)
                    {
                        winners.Add(g.Resolve());
                    }
                }

                while (winners.Count() > 2)
                {
                    Tournament T = new Tournament();
                    List<Game> lGames = new List<Game>();

                    //calculate new games count
                    int gameCount = winners.Count() / 2;
                    
                    for(int j=0; j< gameCount; j++)
                    {
                        Game gameX = new Game();
                        lGames.Add(gameX);
                    }

                    int index = 0;
                    foreach(var p in winners)
                    {
                        lGames[index].Players.Add(p);

                        //2 players per game
                        if (lGames[index].Players.Count() == 2)
                        {
                            index += 1;
                        }

                    }

                    Player tmpPlayer=null;
                    //iwinners.Count()f there were an odd player, save for the later
                    if( (winners.Count() % 2) > 1)
                    {
                        tmpPlayer = winners[winners.Count() - 1];
                    }

                    T.Games = lGames;
                    winners= T.Resolve();
                                     
                    //if there were a pending player to include
                    if(tmpPlayer != null)
                    {
                        winners.Add(tmpPlayer);
                    }

                }//while




                return winners;

            }
            catch (Exception ex)
            {
                //log
                //..
                return null;
            }
        }

    }
}

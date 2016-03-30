using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using Contracts;
using ChallengeLogic;
using System.Web.Helpers;

namespace WSChallgeneAPI.Controllers
{
    [AllowAnonymous]
    public class ChampionshipController : ApiController
    {


        #region Resources

        //championship/
        public IEnumerable<string> Get()
        {
            return new string[] { "test", "hi" };
        }

        /// <summary>
        /// Stores the first and second place of a tournament, with their respective scores.
        /// </summary>
        /// <param name="first">first place</param>
        /// <param name="second">second place</param>
        /// <returns>a json string containing the result (success/error)</returns>
        [HttpPost]
        [Route("championship/Result")]
        public string Result(string first, string second)
        {
            string response = "{status: 'error'}";

            //save first
            if (SaveResults(first, true))
            {

                //then, save second
                if (SaveResults(second, false))
                {
                    response = "{status: 'success'}";
                }
            }


            return response;
        }

        /// <summary>
        /// Retrieves the top players of all championships. Returns the list of player names based on the 'count parameter' provided.
        /// </summary>
        /// <param name="pCount">the number of players to retrieve</param>
        /// <returns></returns>
        [HttpPost]
        [Route("championship/top")]
        public string Top(string count)
        {
            string result = "";

            result = TopPlayers(Convert.ToInt32(count));

            result = "{players: [" + result + "]}";

            return result;
        }

        //e.g. [[[ ["Armando", "P"], ["Dave", "S"] ],[ ["Richard", "R"], ["Michael", "S"] ],],[[ ["Allen", "S"], ["Omer", "P"] ],[ ["John", "R"], ["Robert", "P"] ]]]
        [HttpPost]
        [Route("championship/new")]
        public string New(string val)
        {
            string result = "";
            try
            {
                //string val = "[[\"Armando\", \"P\"],[\"Dave\", \"S\"]]";
                // string val = "[[[ [\"Armando\", \"P\"], [\"Dave\", \"S\"] ],[ [\"Richard\", \"R\"], [\"Michael\", \"S\"] ],],[[ [\"Allen\", \"S\"], [\"Omer\", \"P\"] ],[ [\"John\", \"R\"], [\"Robert\", \"P\"] ]]]";

                //juegos impares! : 

                var lTournaments = DataProcessor.GetChampionshipFromString(val);
                var winners = DataProcessor.ResolveTournaments(lTournaments);

                Player champion = new Player(), second = new Player();

                if (winners != null)
                {
                    //at the end, there will be 2 players, so play the Final
                    Game Final = new Game();
                    Final.Players = winners;

                    champion = Final.Resolve();
                    second = Final.Players.Where(p => p != champion).Select(p => p).FirstOrDefault();

                    //save in DB
                    if (!SaveResults(champion, second))
                    {
                        result = "Error";
                    }

                }

                result = "{winner: [\"" + champion.Name + "\",\"" + champion.Choice + "\"]}";

                return result;

            }
            catch (Exception ex)
            {
                //log
                //..                
                return "Error";
            }
        }

        #endregion

        #region Privates

        /// <summary>
        /// Saves the finalists in DB
        /// </summary>
        /// <param name="pFirst">Champion</param>
        /// <param name="pSecond">Second place</param>
        /// <returns>TRUE: If success</returns>
        private bool SaveResults(Player pFirst, Player pSecond)
        {
            try
            {

                model1 model = new model1();

                var tmp1 = (from p in model.Scores
                           where p.UserName.ToUpper() == pFirst.Name.ToUpper()
                            select p
                              ).FirstOrDefault();

                if (tmp1 == null)//new record
                {
                    model.Scores.Add(new Scores { UserName = pFirst.Name, Score = 3 });
                }
                else
                {
                    tmp1.Score += 3;
                    model.Entry(tmp1).State = System.Data.Entity.EntityState.Modified;
                }

                //second
                var tmp2 = (from p in model.Scores
                            where p.UserName.ToUpper() == pSecond.Name.ToUpper()
                            select p
                            ).FirstOrDefault();

                if (tmp2 == null)//new record
                {
                    model.Scores.Add(new Scores { UserName = pSecond.Name, Score = 1});
                }
                else
                {
                    tmp2.Score += 1;
                    model.Entry(tmp2).State = System.Data.Entity.EntityState.Modified;
                }

                model.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                //log
                //..
                return false;
            }

        }

        /// <summary>
        /// Saves the score for the player
        /// </summary>
        /// <param name="pPlayer">player info</param>
        /// <param name="pIsFirst">if first is set to true, then 3 points will be saved. Otherwise, 1 point.</param>
        /// <returns>TRUE if operation succeded.</returns>
        private bool SaveResults(string pPlayerName, bool pIsFirst)
        {
            try
            {
                short score = 0;
                if (pIsFirst)
                {
                    score = 3;
                }
                else
                {
                    score = 1;
                }

                //save user and score in DB
                model1 model = new model1();

                var tmp1 = (from p in model.Scores
                            where p.UserName.ToUpper() == pPlayerName.ToUpper()
                            select p
                              ).FirstOrDefault();

                if (tmp1 == null)//new record
                {
                    model.Scores.Add(new Scores { UserName = pPlayerName, Score = score });
                }
                else
                {
                    tmp1.Score += 3;
                    model.Entry(tmp1).State = System.Data.Entity.EntityState.Modified;
                }

                model.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves a number of top player
        /// </summary>
        /// <param name="pCount">number of player to retrieve</param>
        /// <returns>a string containing a the name of top players</returns>
        private string TopPlayers(int pCount)
        {
            try
            {

                model1 model = new model1();

                //select top N
                var top = (from s in model.Scores
                           orderby s.Score descending
                           select s.UserName).Take(pCount);

                string list = "";

                //concat
                list = string.Join(",", top);

                return list;

            }
            catch (Exception ex)
            {
                //log
                //..
                return "Error";
            }
        }

        #endregion

    }
}

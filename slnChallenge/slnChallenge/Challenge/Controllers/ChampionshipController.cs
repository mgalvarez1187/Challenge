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

namespace Challenge.Controllers
{
    [AllowAnonymous]
    public class ChampionshipController : ApiController
    {
        //championship/
        public IEnumerable<string> Get()
        {
            return new string[] { "test", "hi" };
        }

        [HttpGet]
        [Route("championship/result")]
        public string Result()
        {
            //string val = "[[\"Armando\", \"P\"],[\"Dave\", \"S\"]]";
            string val = "[[[ [\"Armando\", \"P\"], [\"Dave\", \"S\"] ],[ [\"Richard\", \"R\"], [\"Michael\", \"S\"] ],],[[ [\"Allen\", \"S\"], [\"Omer\", \"P\"] ],[ [\"John\", \"R\"], [\"Robert\", \"P\"] ]]]";

            string result = "status: ", status="";

            var lTournaments = DataProcessor.GetChampionshipFromString(val);
            var winners = DataProcessor.ResolveTournaments(lTournaments);

            return result;
        }
        

    }
}

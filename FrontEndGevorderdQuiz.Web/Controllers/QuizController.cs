using FrontEndGevorderdQuiz.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FrontEndGevorderdQuiz.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private static List<QuizVraag> _vragen = new List<QuizVraag>()
        {
            new QuizVraag()
            {
                Id = 0, // Elke vraag heeft een unieke ID! Best is om te beginnen met 0 en dan steeds +1
                Vraag = "XHR is de afkorting van ...",
                MogelijkAntwoord0 = "XHtmlRev2",
                MogelijkAntwoord1 = "XmlHttpRequest",
                MogelijkAntwoord2 = "XboxHyperloopRecursion",
                MogelijkAntwoord3 = "Geen van bovenstaande",
                JuisteAntwoordIndex = 1
            },
            new QuizVraag()
            {
                Id = 1,
                Vraag = "De grondlegger van het World Wide Web is ...",
                MogelijkAntwoord0 = "Linus Torvalds",
                MogelijkAntwoord1 = "Nikola Tesla",
                MogelijkAntwoord2 = "Bill Gates",
                MogelijkAntwoord3 = "Tim Berners-Lee",
                JuisteAntwoordIndex = 3
            },
            // TODO: voeg hier nog 20 andere cursus-gerelateerde vragen aan toe.
        };

        /// <summary>
        /// Deze methode geeft alle vragen terug als een JSON array.
        /// Op te roepen via een HTTP GET call naar /api/quiz
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<QuizVraag> Get()
        {
            return _vragen;
        }

        /// <summary>
        /// Deze methode ontvangt alle antwoorden van de speler in één keer.
        /// Op te roepen via een HTTP POST naar /api/quiz met een JSON array in de body.
        /// Je zal dus in de browser elk antwoord moeten bijhouden (vb. in een JavaScript array) en
        /// vervolgens de volledige array omzetten naar JSON.
        /// </summary>
        /// <param name="antwoorden">JSON array met de antwoorden van de speler.</param>
        /// <returns>De score (percentage) van de speler.</returns>
        [HttpPost]
        public QuizResultaat Post([FromBody] IEnumerable<QuizAntwoord> antwoorden)
        {
            int aantalJuisteAntwoorden = 0;

            // Alle antwoorden verwerken
            foreach (QuizAntwoord antwoord in antwoorden)
            {
                // Ophalen van de vraag horende bij het antwoord.
                QuizVraag vraag = _vragen
                    .FirstOrDefault(v => v.Id == antwoord.VraagId);

                // Is het gegeven antwoord ook het juiste antwoord?
                if (vraag.JuisteAntwoordIndex == antwoord.GekozenAntwoordIndex)
                {
                    aantalJuisteAntwoorden++;
                }
            }

            // Berekenen van de score
            // (met die (float) zetten we een geheel getal om naar een komma-getal zodat er geen gehele deling gebeurt).
            float scorePercentage = ((float)aantalJuisteAntwoorden / (float)_vragen.Count) * 100;

            // Terugsturen van het resultaat als antwoord op de POST call.
            return new QuizResultaat()
            {
                Percentage = scorePercentage
            };
        }
    }
}

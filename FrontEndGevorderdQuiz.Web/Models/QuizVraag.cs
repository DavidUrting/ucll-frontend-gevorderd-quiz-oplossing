using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FrontEndGevorderdQuiz.Web.Models
{
    /// <summary>
    /// Objecten van deze klasse stellen één vraag van de quiz voor.
    /// </summary>
    public class QuizVraag
    {
        private static int _currentVraagIndex = 0;

        public QuizVraag()
        {
            // Telkens er een nieuwe vraag wordt aangemaakt wordt de 
            // _currentVraagIndex als Id gebruikt waarna deze met 1 
            // verhoogd wordt.
            Id = _currentVraagIndex++;
        }

        /// <summary>
        /// Elke vraag heeft een unieke ID.
        /// Deze ID is van belang want het wordt gebruikt in QuizAntwoord.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// De eigenlijke vraag.
        /// </summary>
        public string Vraag { get; set; }

        /// <summary>
        /// Hieronder worden dan de 4 mogelijke antwoorden opgelijst.
        /// Slechts één van deze antwoorden is juist.
        /// Het cijfer 0, 1, 2 of 3 stelt de index voor.
        /// </summary>
        public string MogelijkAntwoord0 { get; set; }
        public string MogelijkAntwoord1 { get; set; }
        public string MogelijkAntwoord2 { get; set; }
        public string MogelijkAntwoord3 { get; set; }

        /// <summary>
        /// Index van het juiste antwoord. Dus: 0, 1, 2 of 3.
        /// Met het [JsonIgnore] attribuut wordt ervoor gezorgd dat de index van het juiste antwoord
        /// niet meegestuurd wordt naar de browser. Een front end gevorderd student zou het dan wel 
        /// heel gemakkelijk hebben om alle juiste antwoorden te vinden via de Developers tools :)
        /// </summary>
        [JsonIgnore]
        public int JuisteAntwoordIndex { get; set; }
    }
}

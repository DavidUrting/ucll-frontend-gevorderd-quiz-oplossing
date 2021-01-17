using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndGevorderdQuiz.Web.Models
{
    /// <summary>
    /// Resultaat van het spel.
    /// De score wordt als percentage teruggegeven (een getal tussen 0 en 100).
    /// </summary>
    public class QuizResultaat
    {
        public float Percentage { get; set; }
    }
}

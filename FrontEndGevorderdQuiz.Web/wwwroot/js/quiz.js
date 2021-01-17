// In deze variabelen wordt de 'state' van de quiz bijgehouden.
let vragen = [];
let huidigeVraagIndex = -1;
let antwoorden = [];
let mogelijkeAntwoordenRadioOptions = document.querySelectorAll('#vragen input[type="radio"]');

// Deze fetch wordt uitgevoerd van zodra de browser dit quiz.js bestand heeft ingeladen.
// De vragen worden opgehaald via een GET Ajax call naar de backend.
// Opgelet: de callbacks in de then()'s worden asynchroon uitgevoerd!
fetch('/api/quiz')
    .then((response) => response.json())
    .then((v) => {
        // Het 'v' object komt qua structuur overeen met een enumerable van de C# class QuizVraag.
        // Er zit dus een array van vraag-objecten in.
        vragen = v;         // De vragen worden 'onthouden' zodat we de backend even niet meer moeten storen...
        gaNaarVolgende();   // En vervolgens wordt de eerste vraag getoond. (De gaNaarVolgende() function zal eerst huidigeVraagIndex++ doen, dus de index gaat van -1 naar 0.)
    });

// Vervolgens wordt er een 'change' event handler gekoppeld aan alle radio options...
// (Ter info: de GET Ajax call kan op dit moment nog steeds bezig zijn. Maar dat kan geen kwaad want deze code is niet afhankelijk van het resultaat van die GET call.)
// Als de speler een antwoord kiest zal onderstaande handler dus afgaan: deze pusht een QuizAntwoord object op de array van antwoorden.
// Daarna wordt er naar de volgende vraag gegaan of naar het resultaat indien er geen vragen meer zijn. Ook hier zorgt gaNaarVolgende() function voor.
for (let i = 0; i < mogelijkeAntwoordenRadioOptions.length; i++) {
    // Het selecteren van een radio option triggert een 'change' event.
    mogelijkeAntwoordenRadioOptions[i].addEventListener('change', function (e) {
        // Het antwoord van de speler wordt 'onthouden' in de array. Deze array zal later doorgestuurd worden via een POST Ajax call.
        antwoorden.push({
            vraagId: vragen[huidigeVraagIndex].id,
            gekozenAntwoordIndex: parseInt(this.value) // 'this' verwijst naar de radio option waarop geklikt werd. In value zit dus "0", "1", "2" of "3". Er moet en parseInt gebeuren omdat de backend numbers verwacht en geen strings!
        });

        // ga naar de volgende vraag of toon de resultaten.
        gaNaarVolgende();
    });
}

// Deze functie gaat naar de volgende vraag.
// Of als er geen vragen meer zijn wordt het resultaat getoond.
// (Ter info: de GET Ajax call kan op dit moment nog steeds bezig zijn. Maar dat kan geen kwaad want deze code wordt pas een eerste keer aangeroepen in de then() op lijn 13.
function gaNaarVolgende() {
    // Naar de volgende vraag gaan.
    huidigeVraagIndex++;

    // Controleren of er nog vragen zijn.
    if (huidigeVraagIndex < vragen.length) {
        // Er zijn nog vragen...
        document.querySelector('#vragen legend').innerText = vragen[huidigeVraagIndex].vraag;
        for (let i = 0; i < mogelijkeAntwoordenRadioOptions.length; i++) {
            // Een eventueel vorige antwoord 'unselecten' want een 'change' event gaat niet af voor een radio option dat al geselecteerd is.
            mogelijkeAntwoordenRadioOptions[i].checked = false;
        }
        document.getElementById('mogelijkAntwoord0').innerText = vragen[huidigeVraagIndex].mogelijkAntwoord0;
        document.getElementById('mogelijkAntwoord1').innerText = vragen[huidigeVraagIndex].mogelijkAntwoord1;
        document.getElementById('mogelijkAntwoord2').innerText = vragen[huidigeVraagIndex].mogelijkAntwoord2;
        document.getElementById('mogelijkAntwoord3').innerText = vragen[huidigeVraagIndex].mogelijkAntwoord3;
    } else {
        // Er zijn geen vragen meer... 
        // Het resultaat kan nu door de backend berekend worden.
        fetch('/api/quiz', {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(antwoorden) // Bemerk dat hier gewoon de antwoorden array naar JSON wordt omgezet.
            })
            .then((response) => response.json()) // En de POST geeft ook weer een JSON terug. Dit moet omgezet worden naar een JavaScript object.
            .then((resultaat) => {
                // Het 'resultaat' object komt qua structuur overeen met de C# class QuizAntwoord.
                // Er zit dus een property 'percentage' in.
                document.querySelector("#resultaat h2").innerText = resultaat.percentage > 80 ? "Gewonnen!" : "Verloren :(";
                document.querySelector("#resultaat h2").className = resultaat.percentage > 80 ? "bg-success" : "bg-danger";
                document.getElementById("score").innerText = Math.round(resultaat.percentage);
                document.getElementById("vragen").className = "hidden"; // De section 'vragen' mag nu verborgen worden.
                document.getElementById("resultaat").className = ""; // En de section 'resultaat' kan getoond worden.
        });
    }
}
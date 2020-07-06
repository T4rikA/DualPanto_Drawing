using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechIO;
using System.Threading.Tasks;

namespace PantoDrawing
{   
    public class Level1 : MonoBehaviour
    {
        // Start is called before the first frame update
        public async Task StartLevel(LineDraw lineDraw, SpeechIn speechIn, SpeechOut speechOut)
        {
            await speechOut.Speak("Welcome to Panto Drawing");
            await speechOut.Speak("Explore your drawing area. Say yes when you're ready.");
            await speechIn.Listen(new Dictionary<string, KeyCode>() { { "yes", KeyCode.Y }});
            await speechOut.Speak("Introduction finished, start level one.");
            lineDraw.TraceLine("Mouth");
            await speechOut.Speak("Here you can feel the first half of a mouth.");
            lineDraw.FindStartingPoint("Mouth");
            await speechOut.Speak("Draw the second half. Turn the lower Handle to start drawing.");
            lineDraw.canDraw = true;
            await speechOut.Speak("Say done when you're ready.");
            lineDraw.canDraw = false;
            LineRenderer secondMouth = lineDraw.lines["line"+(lineDraw.lineCount-1)];
            secondMouth.name = "Mouth2";
            lineDraw.CombineLines("Mouth", "Mouth2", true); //they will be both one line in "Mouth", invert the second line
            lineDraw.TraceLine("Mouth");
        }
    }
}

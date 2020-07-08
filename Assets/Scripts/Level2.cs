using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechIO;
using System.Threading.Tasks;

namespace PantoDrawing
{   
    public class Level2 : MonoBehaviour
    {
        // Start is called before the first frame update
        public async Task StartLevel(LineDraw lineDraw, SpeechIn speechIn, SpeechOut speechOut)
        {
            LineRenderer eye = GameObject.Find("Eye").GetComponent<LineRenderer>();
            lineDraw.TraceLine(eye);
            await speechOut.Speak("Here you can feel a human eye.");
            await speechOut.Speak("Draw the second eye now.");
            lineDraw.canDraw = true;
            await speechOut.Speak("Say circle when you're ready, then yes to finish.");
            await speechIn.Listen(new Dictionary<string, KeyCode>() { { "yes", KeyCode.Y }});
            lineDraw.canDraw = false;
            LineRenderer secondEye = lineDraw.lines["line"+(lineDraw.lineCount-1)];
            secondEye.name = "Eye2";
            await lineDraw.TraceLine(secondEye);
        }
    }
}

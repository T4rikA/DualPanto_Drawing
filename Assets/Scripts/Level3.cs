using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechIO;
using System.Threading.Tasks;

namespace PantoDrawing
{   
    public class Level3 : MonoBehaviour
    {
        // Start is called before the first frame update
        public async Task StartLevel(LineDraw lineDraw, SpeechIn speechIn, SpeechOut speechOut)
        {
            await speechOut.Speak("This time you can find two eyes and a mouth. Draw a nose in the right spot");
            await speechOut.Speak("Use the voice commands One, Two and Three and see what is happening.");   
            lineDraw.canDraw = true;
            while(await speechIn.Listen(new string[] {"One"}) != "One");
            LineRenderer eye1 = GameObject.Find("Eye1").GetComponent<LineRenderer>();
            lineDraw.TraceLine(eye1);
            while(await speechIn.Listen(new string[] {"Two"}) != "Two");
            LineRenderer mouth = GameObject.Find("Mouth").GetComponent<LineRenderer>();
            lineDraw.TraceLine(mouth);
            await speechOut.Speak("Can you find your drawn nose? Say yes or done when you're ready.");
            while(await speechIn.Listen(new string[] {"Yes"}) != "Yes");
            lineDraw.canDraw = false;
            await lineDraw.TraceLine(lineDraw.lines["line"+(lineDraw.lineCount-1)]);
        }
    }
}

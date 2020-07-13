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
            await speechOut.Speak("Can you find your drawn nose? Say yes or done when you're ready.");
            while(await speechIn.Listen(new string[] {"Yes"}) != "Yes");
            lineDraw.canDraw = false;
            await lineDraw.TraceLine(lineDraw.lines["line"+(lineDraw.lineCount-1)]);
        }
    }
}

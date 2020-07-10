using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechIO;
using System.Threading.Tasks;

namespace PantoDrawing
{   
    public class Level4 : MonoBehaviour
    {
        // Start is called before the first frame update
        public async Task StartLevel(LineDraw lineDraw, SpeechIn speechIn, SpeechOut speechOut)
        {
            await speechOut.Speak("Here you can find the first half of a face.");
            await speechOut.Speak("Draw the second half.");   
            lineDraw.canDraw = true;
            await speechOut.Speak("Say yes or done when you're ready.");
            await speechIn.Listen(new Dictionary<string, KeyCode>() { { "yes", KeyCode.Y }});
            lineDraw.canDraw = false;
        }
    }
}

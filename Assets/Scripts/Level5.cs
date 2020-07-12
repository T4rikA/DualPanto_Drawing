using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechIO;
using System.Threading.Tasks;

namespace PantoDrawing
{   
    public class Level5 : MonoBehaviour
    {
        // Start is called before the first frame update
        public async Task StartLevel(LineDraw lineDraw, SpeechIn speechIn, SpeechOut speechOut)
        {
            await speechOut.Speak("Now you have an empty paper, draw your own face. Feel free to use the options command for all tools.");
            lineDraw.canDraw = true;
            await speechOut.Speak("Say yes or done when you're ready.");
            //await speechIn.Listen(new Dictionary<string, KeyCode>() { { "yes", KeyCode.Y }});
            lineDraw.canDraw = false;
        }
    }
}

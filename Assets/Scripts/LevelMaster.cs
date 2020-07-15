using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechIO;
using System.Threading.Tasks;
namespace PantoDrawing
{
    public abstract class LevelMaster : MonoBehaviour
    {
        public bool ready;
        public bool drawn;
        public int currentLineCount;

        public LevelMaster(){
            ready = false;
            drawn = false;
        }

        public abstract Task StartLevel(LineDraw lineDraw, SpeechIn speechIn, SpeechOut speechOut);

        async protected Task WaitFunction(bool flag){
            Debug.Log("waiting");
            while(!ready)await Task.Delay(100);
            ready = false;
            drawn = false;
            Debug.Log("waiting over");
        }

        async protected Task WaitForDrawing(LineDraw lineDraw, SpeechOut speechOut, int currentLineCount)
        {
            lineDraw.canDraw = true;
            while(currentLineCount == lineDraw.lineCount){
                ready = false;
                await speechOut.Speak("Say yes when you're ready drawing.");
                await WaitFunction(ready);
            }
        }
    }
}
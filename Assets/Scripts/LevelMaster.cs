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

    public LevelMaster(){
        ready = false;
    }

    public abstract Task StartLevel(LineDraw lineDraw, SpeechIn speechIn, SpeechOut speechOut);
}
}
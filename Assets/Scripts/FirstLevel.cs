using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLevel : MonoBehaviour
{
    /*private SpeechIn speechIn;
    private SpeechOut speechOut;


    void Awake()
    {
        speechIn = new SpeechIn(onRecognized, commands.Keys.ToArray());
        speechOut = new SpeechOut();
    }

    async void levelOne()
    {
        await speechOut.Speak("Welcome to level one. Here you can feel the first half of a mouth. Draw the second half.");
        /*await lineDraw.TraceLine("Mouth");
        here müssen wir dann also die zweite Hälfte des Mundes malen und auch speichern
        lineDraw.CreateLine();*/
        /*await speechOut.Speak("Say yes or done when you're ready.");

        await speechIn.Listen(new Dictionary<string, KeyCode>() { { "yes", KeyCode.Y }, { "done", KeyCode.D } });
    }*/
}

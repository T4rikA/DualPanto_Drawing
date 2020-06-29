using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using SpeechIO;
using System.Linq;

public class GameManager : MonoBehaviour
{
    
    UpperHandle upperHandle;
    LowerHandle lowerHandle;
    private SpeechIn speechIn;
    private SpeechOut speechOut;
    Dictionary<string, KeyCode> commands = new Dictionary<string, KeyCode>() {
        { "yes", KeyCode.Y },
        { "no", KeyCode.N },
        { "done", KeyCode.D }
    };
    
    void Awake()
    {
        speechIn = new SpeechIn(onRecognized, commands.Keys.ToArray());
        speechOut = new SpeechOut();

        /*if (level < 0 || level >= enemyConfigs.Length)
        {
            Debug.LogWarning($"Level value {level} < 0 or >= enemyConfigs.Length. Resetting to 0");
            level = 0;
        }*/
    }
    

    void Start()
    {
        upperHandle = GetComponent<UpperHandle>();
        lowerHandle = GetComponent<LowerHandle>();
        Debug.Log("Before Introduction");

        Introduction();
        
    }

    async void onRecognized(string message)
    {
        Debug.Log("SpeechIn recognized: " + message);
    }

    async void Introduction()
    {
        await speechOut.Speak("Welcome to Panto Drawing");
        // TODO: 1. Introduce obstacles in level 2 (aka 1)
        await Task.Delay(1000);
        /*RegisterColliders();

        if (introduceLevel)
        {
            await IntroduceLevel();
        }*/

        await speechOut.Speak("Feel for yourself. Say yes or done when you're ready.");
        //string response = await speechIn.Listen(commands);
        await speechIn.Listen(new Dictionary<string, KeyCode>() { { "yes", KeyCode.Y }, { "done", KeyCode.D } });

        await speechOut.Speak("Introduction finished, game starts.");

        //await ResetGame();
    }
}

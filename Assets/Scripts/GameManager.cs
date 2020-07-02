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
        //{ "yes", KeyCode.Y },
        { "no", KeyCode.N },
        //{ "done", KeyCode.D },
        { "add", KeyCode.A }
    };
    
    private int levelNumber = 1;
    public bool doLevel = true;

    //public FirstLevel firstLevel; um die level ggf auszulagern in ein eigenes Skript aber das mag grad nciht

    private LineDraw lineDraw;

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
        lineDraw = GameObject.Find("Plane").GetComponent<LineDraw>();
        Debug.Log("Before Introduction");
        speechIn.StartListening();
        Introduction();
        
        
    }


    async void onRecognized(string message)
    {
        //WIP
        switch (message)
        {
            case "add":
                string name;
                //name = await speechIn.Listen();
                break;
            case "repeat":
                await speechOut.Repeat();
                break;
            case "quit":
                await speechOut.Speak("Thanks for using our application. Closing down now...");
                OnApplicationQuit();
                Application.Quit();
                break;
            /*case "options":
                string commandlist = "";
                foreach (string command in commands.Keys)
                {
                    commandlist += command + ", ";
                }
                await speechOut.Speak("currently available commands: " + commandlist);
                break;*/
        }
    }
    
    private void OnApplicationQuit()
    {
        speechOut.Stop();
        speechIn.StopListening();
    }

    async void Introduction()
    {
        await speechOut.Speak("Welcome to Panto Drawing");
        // TODO: 1. Introduce obstacles in level 2 (aka 1)
        await Task.Delay(1000);
        RegisterColliders();
        await speechOut.Speak("Feel for yourself. Say yes or done when you're ready.");
        //string response = await speechIn.Listen(commands);
        await speechIn.Listen(new Dictionary<string, KeyCode>() { { "yes", KeyCode.Y }, { "done", KeyCode.D } });

        await speechOut.Speak("Introduction finished, start level one.");

        //await ResetGame();
        if (doLevel)
        {
            IntroductionThroughLevel();
        }
    }

    
    void RegisterColliders()
    {
        PantoCollider[] colliders = FindObjectsOfType<PantoCollider>();
        foreach (PantoCollider collider in colliders)
        {
            Debug.Log(collider);
            collider.CreateObstacle();
            collider.Enable();
        }
    }

    void IntroductionThroughLevel()
    {
        for(int i = 1; i <= 5; i++ ){
            switch(i)
        {
            case 1:
                levelOne();
                break;
            /*case 2:
                await levelTwo();
                levelNumber++;
                break;
            case 3:
                await levelThree();
                levelNumber++;
                break;
            case 4:
                await levelFour();
                levelNumber++;
                break;
            case 5:
                await levelFive();
                levelNumber++;
                break;*/
            default:
                Debug.Log("Defaul level case");
                break;
        }
        }
        
    }

    async void levelOne()
    {
        await speechOut.Speak("Welcome to level one. Here you can feel the first half of a mouth. Draw the second half.");
        /*await lineDraw.TraceLine("Mouth");
        here müssen wir dann also die zweite Hälfte des Mundes malen und auch speichern
        lineDraw.CreateLine();*/
        await speechOut.Speak("Say yes or done when you're ready.");

        await speechIn.Listen(new Dictionary<string, KeyCode>() { { "yes", KeyCode.Y }, { "done", KeyCode.D } });
    }

    /*void async levelTwo(){ return;}

    void async levelThree(){ return; }

    void async levelFour(){return;}

    void async levelFive(){return;}

    void draw(){return;}*/
}

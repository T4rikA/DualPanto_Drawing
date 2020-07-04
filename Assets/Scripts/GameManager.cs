using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using SpeechIO;
using System.Linq;


namespace PantoDrawing
{
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
            { "circle", KeyCode.C }
        };
        
        public bool doLevel = true;
        public bool debugTest = false;

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
            lineDraw = GameObject.Find("Panto").GetComponent<LineDraw>();
            lineDraw.canDraw = false;
            Debug.Log("Before Introduction");
            speechIn.StartListening();
            RegisterColliders();
            if(!debugTest)
            {
                Debug.Log(debugTest);
                Introduction();
            } else
            {
                lineDraw.canDraw = true;
            }
        }


        async void onRecognized(string message)
        {
            //WIP
            switch (message)
            {
                case "circle":
                    Debug.Log("circle");
                    string name = "eye1";
                    //await speechIn.Listen();
                    lineDraw.canDraw = false;
                    break;
                case "repeat":
                    await speechOut.Repeat();
                    break;
                case "quit":
                    await speechOut.Speak("Thanks for using our application. Closing down now...");
                    OnApplicationQuit();
                    Application.Quit();
                    break;
                case "done":
                    lineDraw.canDraw = false;
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
            await speechOut.Speak("Explore your drawing area. Say yes when you're ready.");
            //string response = await speechIn.Listen(commands);
            await speechIn.Listen(new Dictionary<string, KeyCode>() { { "yes", KeyCode.Y }});

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
                //Debug.Log(collider);
                collider.CreateObstacle();
                collider.Enable();
            }
        }

        async void IntroductionThroughLevel()
        {
            for(int i = 1; i <= 5; i++ ){
                switch(i)
            {
                case 1:
                    lineDraw.TraceLine("Mouth");
                    await speechOut.Speak("Here you can feel the first half of a mouth.");
                    lineDraw.FindStartingPoint("Mouth");
                    await speechOut.Speak("Draw the second half. Turn the lower Handle to start drawing.");
                    lineDraw.canDraw = true;
                    await speechOut.Speak("Say yes when you're ready.");
                    await speechIn.Listen(new Dictionary<string, KeyCode>() { { "yes", KeyCode.Y }});
                    lineDraw.canDraw = false;
                    LineRenderer secondMouth = lineDraw.lines["line"+(lineDraw.lineCount-1)];
                    secondMouth.name = "Mouth2";
                    lineDraw.CombineLines("Mouth", "Mouth2", true); //they will be both one line in "Mouth", invert the second line
                    lineDraw.TraceLine("Mouth");
                    break;

                case 2:
                    lineDraw.TraceLine("Eye");
                    await speechOut.Speak("Here you can feel a human eye.");
                    await speechOut.Speak("Draw the second eye now using the voice command circle.");
                    lineDraw.canDraw = true;
                    await speechOut.Speak("Say yes when you're ready.");
                    await speechIn.Listen(new Dictionary<string, KeyCode>() { { "yes", KeyCode.Y }});
                    lineDraw.canDraw = false;
                    LineRenderer secondEye = lineDraw.lines["line"+(lineDraw.lineCount-1)];
                    secondEye.name = "Eye2";
                    break;
                /*case 3:
                    await speechOut.Speak("Using the voice command 'show' you can find other drawn objects. Use the command 'show eyes' and 'show mouth'.");
                    await lineDraw.TraceLine("Mouth");
                    await lineDraw.TraceLine("Eye");
                    await lineDraw.TraceLine("Eye2");
                    await speechOut.Speak("Draw a nose in the right spot. Turn the it-Handle to start you drawing. Name it also. Doing so you can create subdrawings.");   
                    await speechOut.Speak("Say yes or done when you're ready.");
                    lineDraw.canDraw = true;
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
                    Debug.Log("Default level case");
                    break;
            }
            }
            
        }

        

        async void levelThree()
        {
            await speechOut.Speak("Using the voice command 'show' you can find other drawn objects. Use the command 'show eyes' and 'show mouth'.");

            await speechOut.Speak("Draw a nose in the right spot. Turn the it-Handle to start you drawing. Name it also. Doing so you can create subdrawings.");   
            
            await speechOut.Speak("Say yes or done when you're ready.");
            
            //zeichnen bis drawing = false
        }
}
    /*void async levelFour(){return;}

    void async levelFive(){return;}

    void draw(){return;}*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using SpeechIO;
using System.Linq;
using UnityEngine.SceneManagement;
using DualPantoFramework;

namespace PantoDrawing
{
    public class GameManager : MonoBehaviour
    {
        static GameManager instance;
        UpperHandle upperHandle;
        LowerHandle lowerHandle;
        private SpeechIn speechIn;
        private SpeechOut speechOut;
        int level = 1;


        /*public Dictionary<string, KeyCode> commands = new Dictionary<string, KeyCode>() {
            { "yes", KeyCode.Y },
            { "no", KeyCode.N },
            { "done", KeyCode.D },
            { "circle", KeyCode.C }
        };*/
        
        public Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>() {
            { "test", () =>
                {
                    Debug.Log("it works!!!");
                }}
        };

        bool levelMode = false;

        //public FirstLevel firstLevel; um die level ggf auszulagern in ein eigenes Skript aber das mag grad nciht

        private LineDraw lineDraw;

        void Awake()
        {
            speechIn = new SpeechIn(onRecognized, keywords.Keys.ToArray());
            speechOut = new SpeechOut();
        }
        

        void Start()
        {
            upperHandle = GetComponent<UpperHandle>();
            lowerHandle = GetComponent<LowerHandle>();
            lineDraw = GameObject.Find("Panto").GetComponent<LineDraw>();
            Debug.Log("Before Introduction");
            speechIn.StartListening();
            RegisterColliders();
            keywords.Add("baum", () => {Debug.Log("baum");});
            level = SceneManager.GetActiveScene().buildIndex;
            if(!levelMode)
            {
                Debug.Log(levelMode);
                Levels();
            } else
            {
                lineDraw.canDraw = true;
            }
        }


        async void onRecognized(string message)
        {
            switch (message)
            {
                case "circle":
                    Debug.Log("circle");
                    lineDraw.CreateCircle();
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
                case "options":
                string commandlist = "";
                foreach (KeyValuePair<string, System.Action> command in keywords)
                {
                    commandlist += command.Value + ", ";
                }
                await speechOut.Speak("currently available commands: " + commandlist);
                break;
                default:
                defaultSpeech(message);
                break;
            }
        }

        private void defaultSpeech(string text)
        {
            System.Action keywordAction;
            // if the keyword recognized is in our dictionary, call that Action.
            if (keywords.TryGetValue(text, out keywordAction))
            {
                keywordAction.Invoke();
            }
        }
        
        private void OnApplicationQuit()
        {
            speechOut.Stop();
            speechIn.StopListening();
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

        async void Levels()
        {
            switch(level)
            {
                case 1:
                    Level1 level1 = new Level1();
                    await level1.StartLevel(lineDraw, speechIn, speechOut);
                    LevelCompleted();
                    break;
                case 2:
                    Level2 level2 = new Level2();
                    await level2.StartLevel(lineDraw, speechIn, speechOut);
                    LevelCompleted();
                    break;
                case 3:
                    Level3 level3 = new Level3();
                    await level3.StartLevel(lineDraw, speechIn, speechOut);
                    LevelCompleted();
                    break;
                case 4:
                    Level4 level4 = new Level4();
                    await level4.StartLevel(lineDraw, speechIn, speechOut);
                    LevelCompleted();
                    break;
                case 5:
                    Level5 level5 = new Level5();
                    await level5.StartLevel(lineDraw, speechIn, speechOut);
                    LevelCompleted();
                    break;
                default:
                    Debug.Log("Default level case");
                    lineDraw.canDraw = true;
                    break;
            }
        }

        public async void LevelCompleted()
        {
            await speechOut.Speak("You completed the level");
            LoadScene((level+1) % (SceneManager.sceneCountInBuildSettings));
        }

        public void LoadScene(int index)
        {
            Debug.Log("Load scene with index: "+index);
            SceneManager.LoadScene(index);
        }

        async void levelThree()
        {
            await speechOut.Speak("Using the voice command 'show' you can find other drawn objects. Use the command 'show eyes' and 'show mouth'.");

            await speechOut.Speak("Draw a nose in the right spot. Turn the it-Handle to start you drawing. Name it also. Doing so you can create subdrawings.");   
            
            await speechOut.Speak("Say yes or done when you're ready.");
            
            //zeichnen bis drawing = false
        }

        void ResetGame()
        {
            level = 0;
            LoadScene(level);
        }

        public void RestartLevel()
        {
            LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        async Task GameOver()
        {
            await speechOut.Speak("Thanks for using PantoDraw.");
            Application.Quit();
        }
}
    /*void async levelFour(){return;}

    void async levelFive(){return;}

    void draw(){return;}*/
}

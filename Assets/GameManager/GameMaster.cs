using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.LowLevel;


public class GameMaster : NetworkComponent
{
    public bool GameStarted = false;
    public bool GameEnded = false;
    public GameObject P1Start;
    public GameObject P2Start;
    public GameObject P3Start;
    public GameObject P4Start;
    public GameObject ResultsPanel;
    //public GameObject[] Levels;
    public Vector3 SpawnPosition;
    public TextMeshProUGUI scoreTextResults;
    public int elapsedScore;
    public int elapsedTime = 0;
    public int playerCount;
    public int player1Score;
    public int player2Score;
    public int player3Score;
    public int player4Score;

    //AUDIO
    private AudioHandler audioHandler;
    private AudioSource audioSource;

    public override void HandleMessage(string flag, string value)
    {
        if (flag == "GAMESTART" && IsClient){
			GameStarted = true;
			NPM[] npm = GameObject.FindObjectsOfType<NPM>();
			foreach (NPM playerManager in npm){
				playerManager.transform.GetChild(0).gameObject.SetActive(false);
			}
		}
        //=====MUSIC=====
        if (flag == "SONGSTOP" && IsClient){
            //if (audioSource.isPlaying){ //Check if music is already playing
                audioSource.Stop();
            //} //End if
        } //End if
        if (flag == "SONG2" && IsClient){
            if (!audioSource.isPlaying){ //Check if music is already playing
                audioSource.clip = audioHandler.GetSlot2();
                audioSource.Play();
            } //End if
        } //End if
        if (flag == "SONG3" && IsClient){
            if (!audioSource.isPlaying){ //Check if music is already playing
                audioSource.clip = audioHandler.GetSlot3();
                audioSource.Play();
            } //End if
        } //End if
        if (flag == "SONG4" && IsClient){
            if (!audioSource.isPlaying){ //Check if music is already playing
                audioSource.clip = audioHandler.GetSlot4();
                audioSource.Play();
            } //End if
        } //End if
        if (flag == "SONG5" && IsClient){
            if (!audioSource.isPlaying){ //Check if music is already playing
                audioSource.clip = audioHandler.GetSlot5();
                audioSource.Play();
            } //End if
        } //End if
        //=====END MUSIC=====
        //=====LVL=====
        /*
        if (flag == "LVL1" && IsClient)
        {
            //Debug.Log("LVL1flagged", Levels[0]);
            //GameObject.Find("LVL1").SetActive(true);
            //GameObject.Find("LVL2").SetActive(false);
            //GameObject.Find("LVL3").SetActive(false);
            Levels[0].SetActive(true);
            Debug.Log("LVL1flagged");
        }
        if (flag == "LVL2" && IsClient)
        {
            //Debug.Log("LVL2flagged", Levels[1]);
            //GameObject.Find("LVL2").SetActive(true);
            //GameObject.Find("LVL1").SetActive(false);
            //GameObject.Find("LVL3").SetActive(false);
            Levels[1].SetActive(true);
            Debug.Log("LVL2flagged");
        }
        if (flag == "LVL3" && IsClient)
        {
            //Debug.Log("LVL3flagged", Levels[2]);
            //GameObject.Find("LVL3").SetActive(true);
            //GameObject.Find("LVL1").SetActive(false);
            //GameObject.Find("LVL2").SetActive(false);
            Levels[2].SetActive(true);
            Debug.Log("LVL3flagged");
        }
        //=====END LVL=====
        */
        if (flag == "SCOREONE" && IsClient)
        {
            player1Score = int.Parse(value);
            Debug.Log("Score 1 recieve");
        }

        if (flag == "SCORETWO" && IsClient)
        {
            player2Score = int.Parse(value);
            Debug.Log("Score 2 recieve");
        }

        if (flag == "SCORETHREE" && IsClient)
        {
            player3Score = int.Parse(value);
        }

        if (flag == "SCOREFOUR" && IsClient)
        {
            player4Score = int.Parse(value);
        }

        if (flag == "GAMEEND" && IsClient){
			GameEnded = true;
            Debug.Log("waciassac");
            ResultsPanel = GameObject.FindGameObjectWithTag("Results");
            ResultsPanel.transform.GetChild(0).gameObject.SetActive(true);
            scoreTextResults.text = player1Score.ToString() + "\n" + player2Score.ToString() + "\n" + player3Score.ToString() + "\n" + player4Score.ToString();
            ResultsPanel.GetComponent<ScoreResults>().scoreText = scoreTextResults;
		}
    }

    public override void NetworkedStart()
    {
        P1Start = GameObject.Find("P1Start");
        P2Start = GameObject.Find("P2Start");
        P3Start = GameObject.Find("P3Start");
        P4Start = GameObject.Find("P4Start");
    }

    public override IEnumerator SlowUpdate()
    {
        if (IsServer)
        {
            NPM[] players;
            bool tempGameStarted = false;
            do
            {
                players = GameObject.FindObjectsOfType<NPM>();
                tempGameStarted = true;

                foreach (NPM n in players)
                {
                    if(!n.IsReady)
                    {
                        tempGameStarted = false;
                    }
                }
                yield return new WaitForSeconds(.1f);

            } while (!tempGameStarted || players.Length < 2);

            GameStarted = tempGameStarted;
            players = GameObject.FindObjectsOfType<NPM>();
			foreach (NPM n in players){
                switch(n.Owner)
                {
                    case 0:
                        SpawnPosition = P1Start.transform.position;
                        break;
                    case 1:
                        SpawnPosition = P2Start.transform.position;
                        break;
                    case 2:
                        SpawnPosition = P3Start.transform.position;
                        break;
                    case 3:
                        SpawnPosition = P4Start.transform.position;
                        break;
                }                
                MyCore.NetCreateObject(n.CharSelected, n.Owner, SpawnPosition, Quaternion.identity);
                MyCore.NetCreateObject(9, -1, SpawnPosition, Quaternion.identity);
                playerCount++;
			}
            
            SendUpdate("GAMESTART", "1");
            MyCore.NotifyGameStart();

            SendUpdate("SONGSTOP", "1"); //Stop Lobbying music in client
            //=====MUSIC DICEROLLER=====
            int songRNG = Random.Range(2, 6);
            switch (songRNG)
            {
                case 2:
                    SendUpdate("SONG2", "1"); //play music slot2 from AudioHandler
                    break;
                case 3:
                    SendUpdate("SONG3", "1"); //play music slot3 from AudioHandler                    
                    break;
                case 4:
                    SendUpdate("SONG4", "1"); //play music slot4 from AudioHandler
                    break;
                case 5:
                    SendUpdate("SONG5", "1"); //play music slot5 from AudioHandler
                    break;
                default:
                    break;
            } //End switch songRNG
            //=====END MUSIC DICEROLLER=====
            //=====LVL=====
            /*
            int lvlRNG = Random.Range(1, 4);
            switch (lvlRNG)
            {
                case 1:
                    SendUpdate("LVL1", "1");
                    Debug.Log("LVL1picked");
                    break;
                case 2:
                    SendUpdate("LVL2", "1");
                    Debug.Log("LVL2picked");
                    break;
                case 3:
                    SendUpdate("LVL3", "1");
                    Debug.Log("LVL3picked");
                    break;
                default:
                    break;
            } //End switch lvlRNG
            */
            while (GameStarted && !GameEnded)
            {
                //Non-player Spawn Locations. Randomize their selection, making sure to not spawn on the same location.
                Vector3 Corner1 = new Vector3(-9.3f, 0, 5.36f);
                Vector3 Corner2 = new Vector3(9.3f, 0, 5.36f);
                Vector3 Corner3 = new Vector3(-9.3f, 0, -5.36f);
                Vector3 Corner4 = new Vector3(9.3f, 0, -5.36f);
                Vector3 SideL = new Vector3(-9, 0, 0);
                Vector3 SideR = new Vector3(-9, 0, 0);

                if (elapsedTime % 20 == 19) //Spawns ItemSpeed
                {
                    Quaternion rotation = Quaternion.Euler(60f, 0f, 180f);
                    GameObject temp = MyCore.NetCreateObject(5, -1, Corner1, rotation);
                }
                if (elapsedTime % 20 == 19) //Spawns ItemHealth
                {
                    Quaternion rotation = Quaternion.Euler(-30f, 0f, 90f);
                    GameObject temp = MyCore.NetCreateObject(6, -1, Corner2, rotation);
                }
                if (elapsedTime % 20 == 19) //Spawns ItemReload
                {
                    Quaternion rotation = Quaternion.Euler(-30f, 0f, 90f);
                    GameObject temp = MyCore.NetCreateObject(7, -1, Corner3, rotation);
                }
                if (elapsedTime % 20 == 14) //NPCB
                {
                    GameObject temp = MyCore.NetCreateObject(10, -1, new Vector3(10,0,2), this.transform.rotation);
                }
                if (elapsedTime % 20 == 4) //NPCC
                {
                    GameObject temp = MyCore.NetCreateObject(11, -1, new Vector3(-10,0,-2), this.transform.rotation);
                }
                yield return new WaitForSeconds(1);
                elapsedTime++;
                if (playerCount < 2)
                {
                    GameEnded = true;
                }
            }
            SendUpdate("SCOREONE", player1Score.ToString());
            SendUpdate("SCORETWO", player2Score.ToString());
            SendUpdate("SCORETHREE", player3Score.ToString());
            SendUpdate("SCOREFOUR", player4Score.ToString());
            ResultsPanel.gameObject.SetActive(true);
            scoreTextResults.text = player1Score.ToString() + "\n" + player2Score.ToString() + "\n" + player3Score.ToString() + "\n" + player4Score.ToString();
            ResultsPanel.GetComponent<ScoreResults>().scoreText = scoreTextResults;
            SendUpdate("GAMEEND", "1");
            yield return new WaitForSeconds(30);
            MyCore.UI_Quit();
        }

            yield return new WaitForSeconds(.1f);
    }
    // Start is called before the first frame update
    void Start()
    {
        elapsedScore = Random.Range(1, 101);

        //Music
        GameObject musicObject = GameObject.Find("Music");
        audioHandler = musicObject.GetComponent<AudioHandler>();
        audioSource = musicObject.GetComponent<AudioSource>();

        //Levels
        /*
        Levels = new GameObject[3];
        Levels[0] = GameObject.Find("LVL1");
        Levels[0].SetActive(false);
        Debug.Log("LVL1found");
        Levels[1] = GameObject.Find("LVL2");
        Levels[1].SetActive(false);
        Debug.Log("LVL2found");
        Levels[2] = GameObject.Find("LVL3");
        Levels[2].SetActive(false);
        Debug.Log("LVL3found");
        */
        /*
        GameObject.Find("LVL1").SetActive(false);
        GameObject.Find("LVL2").SetActive(false);
        GameObject.Find("LVL3").SetActive(false);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class GameMaster : NetworkComponent
{
    public bool GameStarted = false;
    public bool GameEnded = false;
    public GameObject P1Start;
    public GameObject P2Start;
    public GameObject P3Start;
    public GameObject P4Start;
    public GameObject ResultsPanel;
    public Vector3 SpawnPosition;
    public int elapsedScore;
    public int elapsedTime = 0;
    public override void HandleMessage(string flag, string value)
    {
        if (flag == "GAMESTART" && IsClient){
			GameStarted = true;
			NPM[] npm = GameObject.FindObjectsOfType<NPM>();
			foreach (NPM playerManager in npm){
				playerManager.transform.GetChild(0).gameObject.SetActive(false);
			}
		}

        if (flag == "GAMEEND" && IsClient){
			GameEnded = true;
            Debug.Log("waciassac");
            ResultsPanel = GameObject.FindGameObjectWithTag("Results");
            ResultsPanel.transform.GetChild(0).gameObject.SetActive(true);
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
			}

            SendUpdate("GAMESTART", "1");
            MyCore.NotifyGameStart();
            while(GameStarted && !GameEnded)
            {
                if (elapsedTime % 20 == 19) //Spawns ItemSpeed
                {
                    Quaternion rotation = Quaternion.Euler(60f, 0f, 180f);
                    GameObject temp = MyCore.NetCreateObject(5, -1, new Vector3(0,0,0), rotation);
                }
                if (elapsedTime % 20 == 19) //Spawns ItemHealth
                {
                    Quaternion rotation = Quaternion.Euler(-30f, 0f, 90f);
                    GameObject temp = MyCore.NetCreateObject(6, -1, new Vector3(0, 2, 2), rotation);
                }
                if (elapsedTime % 20 == 19) //Spawns ItemReload
                {
                    Quaternion rotation = Quaternion.Euler(-30f, 0f, 90f);
                    GameObject temp = MyCore.NetCreateObject(7, -1, new Vector3(0, -2, -2), rotation);
                }
                if (elapsedTime % 20 == 14)
                {
                    GameObject temp = MyCore.NetCreateObject(8, -1, new Vector3(10,0,2), this.transform.rotation);
                }
                if (elapsedTime % 20 == 4)
                {
                    GameObject temp = MyCore.NetCreateObject(8, -1, new Vector3(-10,0,-2), this.transform.rotation);
                }
                yield return new WaitForSeconds(1);
                elapsedTime++;
                if (elapsedTime > 60)
                {
                    GameEnded = true;
                }
            }
            ResultsPanel.gameObject.SetActive(true);
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

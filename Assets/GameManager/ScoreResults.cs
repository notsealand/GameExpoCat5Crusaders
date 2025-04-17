using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreResults : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int elapsedScore;
    public int player1Results;
    public int player2Results;
    public int player3Results;
    public int player4Results;
    public GameObject gameMaster;

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = GameObject.FindGameObjectWithTag("Results");
        //elapsedScore = gameMaster.GetComponent<GameMaster>().elapsedScore;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(gameMaster.GetComponent<GameMaster>().GameEnded == true)
        {
            /*Debug.Log(gameMaster.GetComponent<GameMaster>().player2Score);
            player1Results = gameMaster.GetComponent<GameMaster>().player1Score;
            player2Results = gameMaster.GetComponent<GameMaster>().player2Score;
            player3Results = gameMaster.GetComponent<GameMaster>().player3Score;
            player4Results = gameMaster.GetComponent<GameMaster>().player4Score;*/
            //scoreText = gameMaster.GetComponent<GameMaster>().scoreTextResults;
        }
    }
}

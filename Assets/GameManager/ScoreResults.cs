using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreResults : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int elapsedScore;
    public GameObject gameMaster;

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = GameObject.Find("GameMaster");
        elapsedScore = Random.Range(1, 101);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameMaster.GetComponent<GameMaster>().GameEnded == true)
        {
            scoreText.text = elapsedScore.ToString();
        }
    }
}

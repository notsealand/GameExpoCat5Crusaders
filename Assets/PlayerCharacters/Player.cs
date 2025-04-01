using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using UnityEngine.UI;
public class Player : NetworkComponent
{
    public Text PlayerName;
    public string PName = "<Default>";
    public GameObject P1Start;
    public GameObject P2Start;
    public GameObject P3Start;
    public GameObject P4Start;

    public override void HandleMessage(string flag, string value)
    {
       switch(flag)
       {
            case "CHAR":
                if(IsServer)
                {
                    SendUpdate("CHAR", value);
                }
                if(IsClient)
                {
                    PlayerName.text = value;
                }
                break;
       }
    }

    public override void NetworkedStart()
    {
    
    }

    public override IEnumerator SlowUpdate()
    {
      while(IsConnected)
        {

            if(IsServer)
            {
                NPM[] players;
                players = GameObject.FindObjectsOfType<NPM>();
			    foreach (NPM n in players){
                    if (Owner == n.Owner)
                    {
                        PName = n.PName;
                    }
                }

                if(IsDirty)
                {
                    SendUpdate("CHAR", PName.ToString());
                }
            }
            if(IsLocalPlayer)
            {
                
            }
            

            
            yield return new WaitForSeconds(.1f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

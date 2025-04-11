using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Player : NetworkComponent
{
    public Text PlayerName;
    public string PName = "<Default>";
    public GameObject P1Start;
    public GameObject P2Start;
    public GameObject P3Start;
    public GameObject P4Start;
    public GameObject Cannonball;
    public int itemID;
    public float LastX;
    public float LastY;
    public int health = 3;
    public Rigidbody MyRig;
    Vector3 yea = new Vector3(1,0,0);
    float Speed = 4.0f;
    bool reload = true;

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
                    //PlayerName.text = value;
                }
                break;

            case "PNAME":
                if (IsClient)
                {
                    PName = value;
                    //this.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = PName;
                }
                break;

            case "MOVE":
                if (IsServer)
                {
                    char[] temp = { '(', ')' };
                    string[] args = value.Trim(temp).Split(',');
                    LastX = float.Parse(args[0]);
                    LastY = float.Parse(args[1]);
                }
                break;

            case "FIRE":
                if(IsServer)
                {
                    SendUpdate("FIRESEND", "1");
                    if(reload)
                    {
                        //GameObject temp = GameObject.Instantiate(Cannonball, this.transform.position+yea.normalized, this.transform.rotation);
                    }
                }
                break;

            case "FIRESEND":
                Debug.Log("wawa");
                if (IsLocalPlayer)
                {
                    if(reload == false)
                    {
                        //yield return new WaitForSeconds(5);
                        reload = true;
                    }
                }
                break;

            case "SPEED":
                Debug.Log("Speed up");
                /*Item = GameObject.Find("Item");
                Destroy(Item.gameObject);*/
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
                    SendUpdate("PNAME", PName);
                    IsDirty = false;
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
        MyRig = this.GetComponent<Rigidbody>();
    }

    public void Mover(InputAction.CallbackContext context)
    {
        Vector2 input = context.action.ReadValue<Vector2>();
        if (IsLocalPlayer)
        {
            SendCommand("MOVE", input.ToString());
        }
    }

    /*public void Shoot(InputAction.CallbackContext context)
    {
        if (IsServer)
        {
            if(reload)
            {
                GameObject temp = GameObject.Instantiate(Cannonball, this.transform.position+yea.normalized, this.transform.rotation);
                reload = false;
            }
        }
        if(IsLocalPlayer)
        {
            SendCommand("FIRE", "1");
            Debug.Log("baba");
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            MyRig.velocity = new Vector3(LastX, MyRig.velocity.y, LastY) * Speed;
        }
    }

    public IEnumerator OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Item" && IsServer)
        {
            Debug.Log("Collision With Item");
            Speed = Speed*2;
            SendUpdate("SPEED", "-1");
            itemID = other.GetComponent<NetworkID>().NetId;
            MyCore.NetDestroyObject(itemID);
            yield return new WaitForSeconds(5);
            Speed = Speed/2;
        }
    }
}

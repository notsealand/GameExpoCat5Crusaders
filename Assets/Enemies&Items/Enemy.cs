using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool goodbye = false;
    Vector3 yea;

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Rigidbody>().position.x > 0)
        {
            yea = new Vector3(-1,0,0);
        } else {
            yea = new Vector3(1,0,0);
        }
        StartCoroutine(ttl());
    }

    public IEnumerator ttl()
    {
        yield return new WaitForSeconds(10);
        goodbye = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().velocity = yea.normalized * 2;
        if (goodbye)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tools : MonoBehaviour
{


    public GameObject circle;

    public Transform tool_parent;
  // Start is called before the first frame update
    void Start()
    {

        GameObject circle1 = Instantiate(circle, new Vector3(15, 0, 5), Quaternion.identity);
        circle1.transform.parent = tool_parent.transform;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

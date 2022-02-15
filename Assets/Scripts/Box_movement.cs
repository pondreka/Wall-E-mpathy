using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box_movement : MonoBehaviour
{
    [SerializeField]
    public float speed;
    Rigidbody conveyor;

    // Start is called before the first frame update
    void Start()
    {
        conveyor = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = conveyor.position;
        conveyor.position += Vector3.left * speed * Time.fixedDeltaTime;
        conveyor.MovePosition(pos);


    }
}

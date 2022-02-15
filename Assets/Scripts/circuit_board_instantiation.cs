using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Valve.VR.InteractionSystem;
using Valve.VR.InteractionSystem.Sample;
using Random = UnityEngine.Random;
public class circuit_board_instantiation : MonoBehaviour
{
    //General
    public float spawnPeriodinSeconds = 30f;
    private float nextSpawnTime = 0.0f;
    public GameObject spawnPosition;
    private GameObject needToFixParent;
    private float circuit_size = 0.05f; // x:x field
    private float scaleMe;
    private Interactable interactable;
    
    //circuitBoard
    public GameObject circuitBoardPrefab;
    private GameObject circuitBoard;
    private GameObject circuitBoardParent;
    

    //circles
    private GameObject circlesParent;
    private float[] possiblePositions = new float[]{1,2,3,4,5,6,7,8,9};
    public int numberOfCirclesPerLine = 3;
    public GameObject circlePrefab;
    public float percentOfMissingCircles = 0.1f;
    
    //lines
    private GameObject linesParent;
    public GameObject linesPrefab;
    public float percentOfMissingLines = 0.1f;

    private bool spawning = false;
    private void Awake()
    {
        scaleMe = 1 / circuit_size;
        for (int i = 0; i < possiblePositions.Length; i++)
        {
            possiblePositions[i] = possiblePositions[i] / scaleMe;
        }
    }


    // This script will simply instantiate the Prefab when the game starts.
    
    private void CreateCircuitBoard()
    {
        circuitBoardParent = new GameObject("circuitBoardParent");
        circuitBoardParent.transform.tag = "Plate";
        
        circuitBoardParent.transform.position = spawnPosition.transform.position;
        circuitBoardParent.AddComponent<BoxCollider>().center = new Vector3(0.25f,0,0.25f);
        circuitBoardParent.GetComponent<BoxCollider>().size = new Vector3(0.75f, 0.05f, 0.75f);
        circuitBoardParent.AddComponent<Rigidbody>();
        circuitBoardParent.AddComponent<Interactable>();
        circuitBoardParent.AddComponent<fixed_board_script>();
        
        circuitBoardParent.transform.parent = this.gameObject.transform;
        circuitBoard = Instantiate(circuitBoardPrefab, spawnPosition.transform.position, Quaternion.identity);
        circuitBoard.transform.parent = circuitBoardParent.transform;
        
    }

    public GameObject getCircleParent()
    {
        return circlesParent;
    }

    public GameObject getLinesParent()
    {
        return linesParent;
    }

    public void startSpawning()
    {
        spawning = true;
    }

    void Start()
    {
        spawning = true;
        nextSpawnTime += Time.time;
    }
    void Update()
    {

        
        
        
        if (spawning)
        {
            Debug.Log("start spawning platines");
            //start a new board every x seconds
            if(Time.time> nextSpawnTime)
            {
                nextSpawnTime += spawnPeriodinSeconds;
            
                CreateCircuitBoard();
                CreateCircles();
            }
        }
        
 

        
        
    }

    private void CreateCircles()
    {

        circlesParent = new GameObject("circlesParent");
        linesParent = new GameObject("linesParent");
        needToFixParent = new GameObject("needToFixParent");
        circlesParent.transform.parent = circuitBoardParent.transform;
        linesParent.transform.parent = circuitBoardParent.transform;
        needToFixParent.transform.parent = circuitBoardParent.transform;
        
        Vector3 old_pos = spawnPosition.transform.position;

        // circles at 10 different z-axis values
        for (float i = 1; i < 10; i++)
        {
            int last_drawn_x_pos = 0;
            for (int j = 0; j < numberOfCirclesPerLine; j++)
            {
                if (last_drawn_x_pos == 8)
                    break;
                int drawn_x_pos = Random.Range(last_drawn_x_pos + 1, 8);
                

                // get drawn_x_pos from array
                float x = possiblePositions[drawn_x_pos];
                last_drawn_x_pos = drawn_x_pos;


                Vector3 new_pos = new Vector3(x, 0, i / scaleMe) + spawnPosition.transform.position;
               

                if (i > 1 || j >= 1)
                {
                    CreateLines(old_pos, new_pos);


                }

                GameObject circle1 = Instantiate(circlePrefab, new_pos, Quaternion.identity);
                circle1.transform.parent = circlesParent.transform;
                old_pos = new_pos;

                // deleting a circle <make it black>
                if (Random.value < percentOfMissingCircles)
                {
                    
                    var lineRenderer = circle1.GetComponentInChildren<Renderer>();
                    lineRenderer.material.SetColor("_Color", Color.black);
                    circle1.transform.parent = needToFixParent.transform;
                    circle1.gameObject.transform.GetChild(0).gameObject.tag = "needToFix";

                }

            }
        }
    }





    private void CreateLines(Vector3 old_pos, Vector3 new_pos)
    {

        GameObject line1 = Instantiate(linesPrefab, old_pos, Quaternion.identity);
        line1.transform.parent = linesParent.transform;
        float lineDistance = Vector3.Distance(old_pos,
            new_pos);

        line1.transform.localScale +=
            new Vector3(lineDistance - line1.transform.localScale.x, 0.0f, 0.0f);
        float angle = Vector2.Angle(new Vector2(old_pos.x, old_pos.z),
            new Vector2(new_pos.x, new_pos.z));
        angle = GetAngle(old_pos.x, old_pos.z, new_pos.x, new_pos.z);
        line1.transform.Rotate(0, angle - 90, 0, Space.Self);

        // deleting a line <make it black>
        if (Random.value < 0.1)
        {
            var lineRenderer = line1.GetComponentInChildren<Renderer>();
            lineRenderer.material.SetColor("_Color", Color.black);
            line1.transform.parent = needToFixParent.transform;
            line1.gameObject.transform.GetChild(0).gameObject.tag = "needToDraw";
        }
        
            // TODO
            // create array of size lineDividedByy 
            //
    }
        
        public float GetAngle(float X1, float Y1, float X2, float Y2) {
 
            // take care of special cases - if the angle
            // is along any axis, it will return NaN,
            // or Not A Number.  This is a Very Bad Thing(tm).
            if (Y2 == Y1) {
                return (X1 > X2) ? -90:90;
            }
            if (X2 == X1) {
                return (Y2 > Y1) ? 0 : -180;
            }
       
            float tangent = (X2 - X1) / (Y2 - Y1);
            // convert from radians to degrees
            double ang = (float) Mathf.Atan(tangent) * 57.2958;
            // the arctangent function is non-deterministic,
            // which means that there are two possible answers
            // for any given input.  We decide which one here.
            if (Y2-Y1 < 0) ang -= 180;
 
 
            // NOTE that this does NOT need to be normalised.  Arctangent
            // always returns an angle that is within the 0-360 range.
       
 
            // barf it back to the calling function
            return (float) ang;
   
        }
    
    
}
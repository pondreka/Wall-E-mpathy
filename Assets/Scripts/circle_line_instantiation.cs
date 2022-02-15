using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class circle_line_instantiation : MonoBehaviour
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject circle;
    public GameObject line;
    public Transform linesParent;
    public Transform circlesParent;
    public Transform needToFixParent;
    private float circuit_size = 0.05f; // x:x field
    private float scaleMe;
    public int numberOfCirclesPerLine = 3;
    public float percentOfMissingLines = 0.1f;
    public float percentOfMissingCircles = 0.1f;
    public bool divideLines = true;
    public int linesDividedBy = 20;
    private float[] possiblePositions = new float[]{1,2,3,4,5,6,7,8,9};
    public GameObject spawnPosition;


    private void Awake()
    {
        scaleMe = 1 / circuit_size;
        for (int i = 0; i < possiblePositions.Length; i++)
        {
            possiblePositions[i] = possiblePositions[i] / scaleMe;
        }
    }


    void StartDrawing()
    {
        
        // create array to draw random circle positons
       
           
        Vector3 old_pos = spawnPosition.transform.position;
        for (float i = 1; i < 10; i++)
        {
            int last_drawn_x_pos = 0;
            for (int j = 0; j < numberOfCirclesPerLine; j++)
            {
                if (last_drawn_x_pos == 8)
                    break;
                print("last drawn: "+ last_drawn_x_pos);
                int drawn_x_pos = Random.Range(last_drawn_x_pos+1, 8);
                
                print("drawn x pos" + drawn_x_pos);
                
                
                // get drawn_x_pos from array
                float x = possiblePositions[drawn_x_pos];
                last_drawn_x_pos = drawn_x_pos;
                    
                
                
                

                Vector3 new_pos = new Vector3(x, 0, i/scaleMe)+spawnPosition.transform.position;
                print("New pos. 0-1, 0, 0-1" + new_pos);

                if (i > 1 || j>=1)
                {
                    print("Do lines now:");
                    if (divideLines == false)
                    {


                        GameObject line1 = Instantiate(line, old_pos, Quaternion.identity);
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
                            print("deleting line");
                            var lineRenderer = line1.GetComponentInChildren<Renderer>();
                            lineRenderer.material.SetColor("_Color", Color.black);
                            line1.transform.parent = needToFixParent.transform;
                            line1.gameObject.transform.GetChild(0).gameObject.tag = "needToDraw";
                        }
                    }
                    else
                    {
                        float lineDistance = Vector3.Distance(old_pos, new_pos);
                        lineDistance = lineDistance / linesDividedBy;
                        float angle = Vector2.Angle(new Vector2(old_pos.x, old_pos.z),
                            new Vector2(new_pos.x, new_pos.z));
                        angle = GetAngle(old_pos.x, old_pos.z, new_pos.x, new_pos.z);
                        print("this is the angle"+ angle);
                        for (int k = 0; k < linesDividedBy; k++)
                        {
                            GameObject line1 = Instantiate(line, old_pos, Quaternion.identity);
                            line1.transform.parent = linesParent.transform;
                            line1.transform.localScale += new Vector3(lineDistance - line1.transform.localScale.x, 0.0f, 0.0f);
                            line1.transform.Rotate(0, angle - 90, 0, Space.Self);

                            float angle2 = angle * Mathf.PI / 180f;
                            old_pos = new Vector3(Mathf.Sin(angle2) * lineDistance + old_pos.x, 0.0f,
                                Mathf.Cos(angle2) * lineDistance+old_pos.z);
                            
                            if (Random.value < percentOfMissingLines && k > 5 && k <15)
                            {
                                
                                print("deleting line: " + k );
                                var lineRenderer = line1.GetComponentInChildren<Renderer>();
                                lineRenderer.material.SetColor("_Color", Color.black);
                                line1.transform.parent = needToFixParent.transform;
                                line1.gameObject.transform.GetChild(0).gameObject.tag = "needToDraw";
                            }

                        }
                            
                        // TODO
                        // create array of size lineDividedByy 
                        // each line same direction and angle,
                    }

                }
                
                print("Instantiate circle at:" + new_pos);
                GameObject circle1 = Instantiate(circle, new_pos, Quaternion.identity);
                circle1.transform.parent = circlesParent.transform;
                old_pos = new_pos;
                
                // deleting a circle <make it black>
                if (Random.value < percentOfMissingCircles)
                {
                    print("deleting circle");
                    var lineRenderer = circle1.GetComponentInChildren<Renderer>();
                    lineRenderer.material.SetColor("_Color", Color.black);
                    circle1.transform.parent = needToFixParent.transform;
                    circle1.gameObject.transform.GetChild(0).gameObject.tag = "needToFix";

                }
                
            }

            
       
        }
     
        

        
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
    public void new_function()
    {
        foreach(Transform t in circlesParent.transform)
            Destroy(t.gameObject);
        foreach(Transform t in linesParent.transform)
            Destroy(t.gameObject);
        StartDrawing();
        
    }

    public void newBoard()
    {
        StartDrawing();
    }


}
using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation.Examples;
using UnityEngine;
using System.IO;

public class MovementControl : MonoBehaviour
{
    private PathFollower pathScript;

    private bool stopped = false;
    private bool down = true;
    private bool up = false;
    private bool crash = false;
    private bool grab = true;
    private bool end = true;
    
    
    private GameObject robot;
    private GameObject box;
    private Animator boxAnimator;
    private Animator robotAnimator;
    
    private int grabCount = 0;
    public int grabMax = 5;
    private int boxCount = 0;
    public int boxMax = 5;

    public GameObject triggerPlate;
    public GameObject plateAnimationParent;

    private AudioSource sound;
    

    // Start is called before the first frame update
    void Start()
    {
        pathScript = this.gameObject.GetComponent<PathFollower>();
        box = this.transform.GetChild(2).gameObject;
        
        if (this.transform.GetChild(0).gameObject.activeSelf)
        {
            robot = this.transform.GetChild(0).gameObject;
        }
        else
        {
            robot = this.transform.GetChild(1).gameObject;
        }
        
        robotAnimator = robot.GetComponent<Animator>();
        boxAnimator = box.GetComponent<Animator>();

        sound = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stopped)
        {
            robotAnimator.SetBool("Stopped", true);
            if (robotAnimator.GetCurrentAnimatorStateInfo(0).IsName("Abstellen"))
            {
                
                if (down)
                {
                    boxAnimator.SetTrigger("Down");
                    down = false;
                    up = true;
                }
                
            }
            
        }
        else
        {
            robotAnimator.SetBool("Stopped", false);
            if (robotAnimator.GetBool("Full"))
            {
                robotAnimator.SetBool("Full", false);
            }
        }

        if (triggerPlate.GetComponent<TriggerPlate>().GetTriggered())
        {
            robotAnimator.SetBool("Grab", true);

            if (robotAnimator.GetCurrentAnimatorStateInfo(0).IsName("Greifen"))
            {
                if (grab)
                {
                    robotAnimator.SetBool("Grab", false);
                    triggerPlate.GetComponent<TriggerPlate>().SetTrigger(false);
                    grabCount++;
                
                    plateAnimationParent.GetComponent<Animator>().SetTrigger("Grab");
                    grab = false;
                }
            }
        }
        else
        {
            robotAnimator.SetBool("Grab", false);
            if (!grab && robotAnimator.GetCurrentAnimatorStateInfo(0).IsName("Waiting"))
            {
                plateAnimationParent.transform.GetChild(0).gameObject.SetActive(false);
                plateAnimationParent.transform.GetChild(0).SetParent(box.transform);
                box.transform.GetChild(0).transform.localPosition = box.transform.localPosition;
                grab = true;
            }
        }

        if (grabCount == grabMax)
        {
            robotAnimator.SetBool("Full", true);
            
            if (robotAnimator.GetCurrentAnimatorStateInfo(0).IsName("Aufheben"))
            {
                if (up)
                {
                    boxAnimator.SetTrigger("Up");
                    down = true;
                    up = false;
                }
                
            }
            
            if (!crash)
            {
                if (!robotAnimator.GetCurrentAnimatorStateInfo(0).IsName("Aufheben") 
                    && !robotAnimator.GetCurrentAnimatorStateInfo(0).IsName("Waiting")
                    && !robotAnimator.GetCurrentAnimatorStateInfo(0).IsName("Greifen"))
                {
                    stopped = false;
                    pathScript.StartMotion();
                    grabCount = 0;
                    boxCount++;
                }
            }
            else
            {
                robotAnimator.SetTrigger("Crash");
                if (robotAnimator.GetCurrentAnimatorStateInfo(0).IsName("Crash"))
                {
                    if (end)
                    {
                        string path = Application.dataPath + "/Log.txt";
                        //Create File if it doesn't exist
                        if (!File.Exists(path))
                        {
                            File.WriteAllText(path, "Experiment log \n\n");
                        }
                        //Content of the file
                        string content = "START: " + System.DateTime.Now +  "\n";
                        //Add some to text to it
                        File.AppendAllText(path, content);
                        
                        sound.Play();
                        
                        GameObject plate1 = box.transform.GetChild(0).gameObject;
                        GameObject plate2 = box.transform.GetChild(1).gameObject;
                        GameObject plate3 = box.transform.GetChild(2).gameObject;
                    
                        plate1.SetActive(true);
                        plate2.SetActive(true);
                        plate3.SetActive(true);
                        plate1.transform.SetParent(this.transform);
                        plate2.transform.SetParent(this.transform);
                        plate3.transform.SetParent(this.transform);

                        end = false;
                    }
                    
                    
                }
            }
            
        }

        if (boxCount == boxMax)
        {
            crash = true;
        }
    }

    private void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("StopPoint"))
        {
            pathScript.StopMotion();
            stopped = true;
        }
    }
    
    
}

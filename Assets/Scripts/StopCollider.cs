using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StopCollider : MonoBehaviour
{
    private bool end = false;


    private void OnTriggerEnter(Collider other)
    {
        if (end) return;
        if (other.CompareTag("Plate")) return;
        
        Debug.Log(other.name);
        end = true;
        string path = Application.dataPath + "/Log.txt";
        //Create File if it doesn't exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Experiment log \n\n");
        }
        //Content of the file
        string content = "END: " + System.DateTime.Now +  "\n";
        //Add some to text to it
        File.AppendAllText(path, content);
        UnityEditor.EditorApplication.isPlaying = false;


    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public GameObject spawning;
    // Start is called before the first frame update
    public void LoadScene()
      { 
          
          SceneManager.LoadScene("Scenes/Lager");
         
         
       }
    private void OnTriggerEnter(Collider other)

    {
        Debug.Log(other.name);
        LoadScene();
        
    }
}
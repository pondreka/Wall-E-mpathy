using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour

{
    public Button yourButton;
    public Dropdown m_Dropdown;
    int m_DropdownValue;
    public InputField participantname; 
    private string partname;
    private string Scene;
 
   


    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        

    }

    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
        m_DropdownValue = m_Dropdown.value;
        Debug.Log(m_DropdownValue);
        partname = participantname.text;
        switch (m_DropdownValue)
        {
            case 0:
                Scene = "Vorraum";
                break;

            case 1:
                Scene = "Vorraum";
                break;

            default:
                print("Incorrect intelligence level.");
                break;
        }
        CreateText();
        SceneManager.LoadScene(Scene);
        
        
    }
    public void CreateText()
    {
        //Path of the file
        string path = Application.dataPath + "/Log.txt";
        //Create File if it doesn't exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Experiment log \n\n");
        }
        //Content of the file
        string content = "Experiment date: " + System.DateTime.Now + " Participant Number is "+ partname + " ConditionNumber is " + m_DropdownValue+ "\n";
        //Add some to text to it
        File.AppendAllText(path, content);
    }
}
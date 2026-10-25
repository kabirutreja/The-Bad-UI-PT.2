using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class first : MonoBehaviour
{
    // Start is called before the first frame update
     void Awake()
    {
      if (PlayerPrefs.GetInt("IsFirstTime", 0) == 0)
      {
        PlayerPrefs.SetInt("IsFirstTime", 1);
        PlayerPrefs.Save();
        
      }  
      else
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
   
}

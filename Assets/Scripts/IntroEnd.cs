
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroEnd : MonoBehaviour
{
    public string SceneName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void SceneChange()
    {
        Debug.Log("a");
        StartCoroutine(LoadScene());
    }
        IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneName);
    }
}

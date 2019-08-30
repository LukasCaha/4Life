using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public AudioSource teleport;
    private string destination;

    public void LoadScene(string name)
    {
        destination = name;
        if (teleport != null)
        {
            StartCoroutine("Teleport");
        }
        else
        {
            SceneManager.LoadScene(destination);
        }
    }

    IEnumerator Teleport()
    {
        teleport.Play();
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(destination);
    }
}

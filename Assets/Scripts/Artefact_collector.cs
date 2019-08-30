using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Artefact_collector : MonoBehaviour
{
    public static Artefact_collector instance = null;

    [System.Serializable]
    public struct artefact
    {
        public string name;
        public bool collected;
        public artefact(string n, bool c)
        {
            name = n;
            collected = c;
        }
    }

    private GameObject artefactDisplay;
    public GameObject levelWin;
    public GameObject battleUI;

    public Shield shield;
    public artefact[] artefacts;

    public Animator background;
    public Animator text;
    public Animator button;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (artefactDisplay == null)
        {
            artefactDisplay = GameObject.Find("#Display");
        }
        if (artefactDisplay != null)
        {
            UpdateCollection();
        }
    }

    public void ArtefactCollected(string name)
    {
        for (int i = 0; i < artefacts.Length; i++)
        {
            if (artefacts[i].name == name/* && artefacts[i].collected == false*/)
            {
                artefacts[i].collected = true;
                levelWin.SetActive(true);
                background.SetTrigger("Fade in");
                text.gameObject.SetActive(true);
                text.SetTrigger("Fade in");
                button.transform.parent.gameObject.SetActive(true);
                button.SetTrigger("Fade in");
                battleUI.SetActive(false);
                StartCoroutine("TimeStop");
                shield.canTakeDamage = false;
                break;
            }
        }
    }

    IEnumerator TimeStop()
    {
        yield return new WaitForSeconds(4.5f);
        Time.timeScale = 0;
    }

    public void SceneChanged()
    {
        background.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        text.GetComponent<Text>().color = new Color(0, 0, 0, 0);
        button.GetComponent<Text>().color = new Color(0, 0, 0, 0);
        background.SetTrigger("Reset");
        text.SetTrigger("Reset");
        text.gameObject.SetActive(false);
        button.SetTrigger("Reset");
        levelWin.SetActive(false);
        StopCoroutine("TimeStop");
        Time.timeScale = 1;
    }

    void UpdateCollection()
    {
        for (int id = 0; id < artefacts.Length; id++)
        {
            if (artefacts[id].collected)
            {
                artefactDisplay.transform.GetChild(id).gameObject.SetActive(true);
                artefactDisplay.transform.GetChild(id).GetComponentInChildren<Text>().text = artefacts[id].name;
            }
            else
            {
                artefactDisplay.transform.GetChild(id).gameObject.SetActive(false);
            }
        }

        for (int overflow = artefacts.Length; overflow < artefactDisplay.transform.childCount; overflow++)
        {
            artefactDisplay.transform.GetChild(overflow).gameObject.SetActive(false);
        }
        artefactDisplay = null;
    }
}

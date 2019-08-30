using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shield : MonoBehaviour
{
    public Text soulsText;
    public Text artefactText;
    public Text timeText;
    public Slider hpSlider;

    public SpriteRenderer shield;
    public Sprite[] shieldStates;
    public AudioSource hitSound;

    private GameObject artefact;

    public bool canTakeDamage = true;
    public int maxPower;
    public int power;

    private void Awake()
    {
        artefact = GameObject.Find("#Artefact");
        ChangePower(maxPower - power);
        hpSlider.maxValue = maxPower;
        hpSlider.value = power;
        hpSlider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Color.red, Color.green, ((float)power) / maxPower);
    }

    private void Update()
    {
        float distance = float.MaxValue;
        if (artefact == null)
        {
            artefact = GameObject.Find("#Artefact");
            artefactText.text = "Artefact collected";
        }
        else
        {
            distance = Vector2.Distance(transform.position, artefact.transform.position);
            artefactText.text = "Distance: " + (int)distance + " ft";
        }

        timeText.text = "Time:" + (int)Time.time;
    }

    public void ChangePower(int amount)
    {
        if (!canTakeDamage && amount < 0)
        {
            return;
        }
        power += amount;
        power = Mathf.Min(power, maxPower);
        hpSlider.value = power;
        hpSlider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Color.red, Color.green, ((float)power) / maxPower);
        soulsText.text = "Souls: " + power+"/"+maxPower;
        if (power <=9)
        {
            shield.sprite = shieldStates[Mathf.Max(0, power - 1)];
        }
        else
        {
            shield.sprite = shieldStates[8];
            shield.transform.localScale = Vector2.one * Mathf.Clamp( Mathf.Pow(1.01f,power-9), 0,2);
        }
        if (amount <0)
        {
            hitSound.Play();
        }
        if (power <= 0)
        {
            ChangePower(maxPower - power);
            SceneManager.LoadScene("Death scene");
        }
    }

    public void ChangeMaxPower(int amount)
    {
        maxPower += amount;
        ChangePower(amount);
        hpSlider.maxValue = maxPower;
        hpSlider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Color.red, Color.green, ((float)power) / maxPower);
    }
}

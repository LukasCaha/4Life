using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_stats : MonoBehaviour
{
    public int maxHP = 100;
    public int HP;
    public Shield playerStats;
    public AudioSource hitSound;
    public Slider hpSlider;
    public GameObject dropPrefab;

    private void Start()
    {
        HP = maxHP;
        hpSlider.maxValue = maxHP;
        hpSlider.value = HP;
        hpSlider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Color.red, Color.green, ((float)HP) / maxHP);
    }

    public void GetDamage(int amount)
    {
        HP -= amount;
        hpSlider.value = HP;
        hpSlider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Color.red, Color.green, ((float)HP) / maxHP);
        hitSound.Play();
        if (HP <= 0)
        {
            StartCoroutine("Die");
            if (playerStats == null)
            {
                playerStats = GameObject.Find("#Spawner").GetComponent<Spawn_player>().player.GetComponentInChildren<Shield>();
            }
            //playerStats.ChangePower(1);
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.14f);
        GameObject drop = Instantiate(dropPrefab, transform.position, Quaternion.identity)as GameObject;
        drop.GetComponent<Pickupable>().SetPickupType(Random.Range(0,3), Random.Range(0, 5));
        Destroy(gameObject);
    }
}

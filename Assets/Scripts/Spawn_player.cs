using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_player : MonoBehaviour
{
    public string playerName = "Pepe";
    public bool sceneContainsPlayer = true;
    public bool sceneContainsBattleUI = true;
    public GameObject player;
    
    void Awake()
    {
        if (player == null)
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        player = GameObject.Find(playerName);
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody2D>().angularVelocity = 0;
        player.GetComponent<Rigidbody2D>().Sleep();
        player.transform.position = transform.position;


        Behaviour[] components = player.GetComponentsInChildren<Behaviour>();
        SpriteRenderer[] renders = player.GetComponentsInChildren<SpriteRenderer>();
        if (sceneContainsPlayer)
        {
            foreach (Behaviour c in components)
            {
                c.enabled = true;
            }
            foreach (SpriteRenderer c in renders)
            {
                c.enabled = true;
            }
        }
        else
        {
            foreach (Behaviour c in components)
            {
                c.enabled = false;
            }
            foreach (SpriteRenderer c in renders)
            {
                c.enabled = false;
            }
        }

        if (sceneContainsBattleUI)
        {
            player.GetComponent<Artefact_collector>().battleUI.SetActive(true);
        }
        else
        {
            player.GetComponent<Artefact_collector>().battleUI.SetActive(false);
        }
    }
}

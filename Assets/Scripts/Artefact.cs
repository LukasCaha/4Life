using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artefact : MonoBehaviour
{
    public string artefactName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Artefact_collector>().ArtefactCollected(artefactName);
            Destroy(gameObject);
        }
    }
}

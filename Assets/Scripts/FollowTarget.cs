using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public bool followPlayer;

    private GameObject target;
    Vector3 offset;
    bool cameraShakeOn = false;
    float magnitude;
    float count;

    private void Start()
    {
        target = GameObject.Find("#Spawner").GetComponent<Spawn_player>().player;
        offset = transform.position - target.transform.position;
    }

    void Update()
    {
        if (followPlayer)
        {
            if (target == null)
            {
                target = GameObject.Find("#Spawner").GetComponent<Spawn_player>().player;
                offset = transform.position - target.transform.position;
            }

            if (!cameraShakeOn)
            {
                transform.position = target.transform.position + offset;
            }
        }
    }

    public void CameraShake(float magnitudeA, float countA)
    {
        magnitude = magnitudeA;
        count = countA;
        cameraShakeOn = true;
        StartCoroutine("Shake");
    }

    IEnumerator Shake()
    {
        if (followPlayer)
        {
            for (int i = 0; i < count; i++)
            {
                transform.position = target.transform.position + offset + (Vector3)Random.insideUnitCircle * magnitude;
                yield return null;
            }
        }
        else
        {
            Vector3 originalPosition = transform.position;
            for (int i = 0; i < count; i++)
            {
                transform.position = originalPosition + (Vector3)Random.insideUnitCircle * magnitude;
                yield return null;
            }
            transform.position = originalPosition;
        }
        cameraShakeOn = false;
    }
}

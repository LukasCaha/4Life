using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_trajectory : MonoBehaviour
{
    public float bulletSpeed;
    public Quaternion direction;
    public float explosionPower;
    public int damage;
    public float timeToLive = 2f;
    public float shakeMagnitude;
    public float shakeCount;
    public GameObject objectsParent;
    public GameObject explosionEffect;
    public GameObject explosionSound;
    public string myTag;
    public string targetTag;

    public LayerMask indestructable;
    public LayerMask getsKnockback;

    private void Start()
    {
        StartCoroutine("TTL");
    }

    IEnumerator TTL()
    {
        yield return new WaitForSeconds(timeToLive);
        Explode();
    }

    void Update()
    {
        BulletMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == myTag)
        {
            return;
        }

        if (collision.tag == targetTag)
        {
            if (collision.GetComponent<Enemy_stats>() != null)
            {
                collision.GetComponent<Enemy_stats>().GetDamage(damage);
            }

            if (collision.GetComponentInChildren<Shield>() != null)
            {
                collision.GetComponentInChildren<Shield>().ChangePower(-damage);
            }
        }
        
        if (indestructable == (indestructable | (1 << collision.gameObject.layer)))
        {
            Explode();
        }

        if (getsKnockback == (getsKnockback | (1 << collision.gameObject.layer)))
        {
            Explode();
        }
    }

    private void BulletMovement()
    {
        transform.Translate(Vector2.right * Time.deltaTime * bulletSpeed);
    }


    void Explode()
    {
        Vector2 explosionPosition = gameObject.transform.position;

        GameObject effect = Instantiate(explosionEffect, explosionPosition, Quaternion.identity) as GameObject;
        effect.GetComponent<ParticleSystem>().startSize *= explosionPower / 800;
        effect.GetComponent<ParticleSystem>().startLifetime *= explosionPower / 800;
        effect.GetComponent<ParticleSystem>().Play();
        Destroy(effect, 1f);
        GameObject sound = Instantiate(explosionSound, explosionPosition, Quaternion.identity) as GameObject;
        sound.GetComponent<AudioSource>().Play();
        Destroy(sound, 1f);
        Camera.main.GetComponent<FollowTarget>().CameraShake(shakeMagnitude, shakeCount);

        foreach (Transform go in objectsParent.transform)
        {
            if (go.GetComponent<Rigidbody2D>() != null)
            {
                Rigidbody2D targetRb = go.GetComponent<Rigidbody2D>();
                Vector2 targetPosition = go.transform.position;
                Vector2 impactDirection = (targetPosition - explosionPosition).normalized;
                float distance = Vector2.Distance(explosionPosition, targetPosition);
                float knockbackPower = explosionPower / (distance * distance);


                targetRb.AddForce(impactDirection * knockbackPower);
            }
        }
        Destroy(gameObject);
    }
}

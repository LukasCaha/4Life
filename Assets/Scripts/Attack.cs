using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject player;
    public GameObject gun;
    public float reactionRadius;
    public float speed;
    public float hitCooldown = 1;

    public GameObject bulletPrefab;
    public Transform barrelEnd;
    public ParticleSystem muzzleFlash;
    public AudioSource fireSound;
    public GameObject objectsParent;
    
    private float flipOffset;
    private float angle;
    public float reloadTime = 1;

    private float timeOfReload = 0;
    private Animator animator;

    private void Start()
    {
        objectsParent = GameObject.Find("#Objects");
        player = GameObject.Find("#Spawner").GetComponent<Spawn_player>().player;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AimGun();

        if (PlayerDistance() < reactionRadius)
        {
            ShootGun();
        }

        animator.SetBool("Walking", false);
        if (PlayerDistance() < 1.5f * reactionRadius)
        {
            WalkToPlayer();
        }
    }

    void WalkToPlayer()
    {
        Vector2 direction = Vector2.right;
        if (player.transform.position.x < transform.position.x)
        {
            direction *= -1;
        }
        transform.localScale = new Vector2(Mathf.Sign(player.transform.position.x - transform.position.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);

        transform.Translate(direction * speed * Time.deltaTime);

        animator.SetBool("Walking", true);
    }

    void AimGun()
    {
        Vector3 difference = player.transform.position - gun.transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + flipOffset);
        angle = rotZ + flipOffset;
        if (angle > 90 || angle < -90)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
            gun.transform.localScale = new Vector2(-Mathf.Abs(gun.transform.localScale.x), -Mathf.Abs(gun.transform.localScale.y));
        }
        else
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            gun.transform.localScale = new Vector2(Mathf.Abs(gun.transform.localScale.x), Mathf.Abs(gun.transform.localScale.y));
        }
    }

    float PlayerDistance()
    {
        return Vector2.Distance(this.transform.position, player.transform.position);
    }

    void ShootGun()
    {
        if (timeOfReload > Time.time)
        {
            return;
        }
        timeOfReload = Time.time + reloadTime;
        muzzleFlash.Play();
        fireSound.Play();
        GameObject bullet = Instantiate(bulletPrefab, barrelEnd.position, gun.transform.rotation) as GameObject;
        float facingDirection = Mathf.Sign(transform.localScale.x);
        bullet.GetComponent<Bullet_trajectory>().direction = transform.rotation;//facingDirection * Vector2.right;
        bullet.GetComponent<Bullet_trajectory>().objectsParent = objectsParent;
        Destroy(bullet, 2f);
    }
}

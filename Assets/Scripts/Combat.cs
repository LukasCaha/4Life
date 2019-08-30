using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Combat : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform barrelEnd;
    public ParticleSystem muzzleFlash;
    public AudioSource fireSound;
    public GameObject objectsParent;

    public float angleOffset;
    private float flipOffset;
    private float angle;
    public float reloadTime = 1;
    private float bulletSpeed = 20;
    private float explosionPower = 800;

    private float timeOfReload = 0;

    public Text rpm;
    public Text speed;
    public Text power;

    private void Start()
    {
        UpdateStats();
    }

    public void UpdateStats()
    {
        bulletSpeed = bulletPrefab.GetComponent<Bullet_trajectory>().bulletSpeed;
        explosionPower = bulletPrefab.GetComponent<Bullet_trajectory>().explosionPower;
        rpm.text = "RPM: " + (int)(60.0f / reloadTime) + " bullets";
        speed.text = "Bullet velocity: " + (int)(bulletSpeed) + "m/s";
        power.text = "Power: " + (int)(explosionPower);
    }

    void Update()
    {
        AimGunToMouse();
        if (Input.GetButton("Fire1") && timeOfReload < Time.time)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        timeOfReload = Time.time + reloadTime;
        muzzleFlash.Play();
        fireSound.Play();
        GameObject bullet = Instantiate(bulletPrefab, barrelEnd.position, transform.rotation) as GameObject;
        float facingDirection = Mathf.Sign(transform.localScale.x);
        bullet.GetComponent<Bullet_trajectory>().direction = transform.rotation;
        bullet.GetComponent<Bullet_trajectory>().bulletSpeed = bulletSpeed;
        bullet.GetComponent<Bullet_trajectory>().explosionPower = explosionPower;
        if (objectsParent == null)
        {
            objectsParent = GameObject.Find("#Objects");
        }
        bullet.GetComponent<Bullet_trajectory>().objectsParent = objectsParent;
    }

    private void AimGunToMouse()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + flipOffset);
        angle = rotZ + flipOffset;
        if (angle > 90 || angle < -90)
        {
            transform.parent.localScale = new Vector2(-Mathf.Abs(transform.parent.localScale.x), transform.parent.localScale.y);
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), -Mathf.Abs(transform.localScale.y));
        }
        else
        {
            transform.parent.localScale = new Vector2(Mathf.Abs(transform.parent.localScale.x), transform.parent.localScale.y);
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
        }
    }

    public int GetDirection()
    {
        if (angle > 90 || angle < -90)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }

    HashSet<string> pickupDuplicates = new HashSet<string>();
    
    public void UpgradeWeapon(string attribute, string id)
    {
        if (pickupDuplicates.Add(attribute + id))
        {
            switch (attribute)
            {
                case "#RPM":
                    reloadTime *= 0.9f;
                    break;
                case "#SPE":
                    bulletSpeed *= 1.1f;
                    break;
                case "#POW":
                    explosionPower *= 1.1f;
                    break;
                default:
                    break;
            }
        }
        UpdateStats();
    }
}

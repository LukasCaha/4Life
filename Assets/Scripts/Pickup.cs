using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Combat pistol;
    public Combat rifle;
    public Combat shotgun;
    public Shield shield;
    public LayerMask pickupable;


    private void Start()
    {
        shield = GetComponentInChildren<Shield>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pickup")
        {
            UpgradeGun(collision.gameObject);
        }

        if (pickupable == (pickupable | (1 << collision.gameObject.layer)))
        {
            switch (collision.GetComponent<Pickupable>().GetPickupType())
            {
                case DropDownType.Gun:
                    /*collision.name = "#RPM " + collision.name;
                    UpgradeGun(collision.gameObject);*/
                    break;
                case DropDownType.HP:
                    HealPlayer();
                    break;
                case DropDownType.Shield:
                    UpgradeShield();
                    break;
                default:
                    break;
            }
            Destroy(collision.gameObject);
        }
    }

    void UpgradeGun(GameObject collision)
    {
        int firstSpace = collision.name.IndexOf(' ');
        firstSpace = Mathf.Max(0, firstSpace);
        string name = collision.name.Substring(0, firstSpace);
        string id = collision.name.Substring(firstSpace + 1);
        switch (name)
        {
            case "#RPM":
                rifle.UpgradeWeapon(name, id);
                Destroy(collision.gameObject);
                break;
            case "#SPE":
                rifle.UpgradeWeapon(name, id);
                Destroy(collision.gameObject);
                break;
            case "#POW":
                rifle.UpgradeWeapon(name, id);
                Destroy(collision.gameObject);
                break;
            default:
                break;
        }
    }

    void HealPlayer()
    {
        shield.ChangePower(1);
    }

    void UpgradeShield()
    {
        shield.ChangeMaxPower(1);
    }
}

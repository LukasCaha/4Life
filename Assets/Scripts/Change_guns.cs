using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_guns : MonoBehaviour
{
    public GameObject[] guns;
    public int activeGun = 0;

    private void Start()
    {
        SetActiveGunNo(activeGun);
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel")>0)
        {
            activeGun++;
            activeGun = (activeGun + guns.Length) % guns.Length;
            SetActiveGunNo(activeGun);
        }
        if (Input.GetAxis("Mouse ScrollWheel")< 0)
        {
            activeGun--;
            activeGun = (activeGun + guns.Length) % guns.Length;
            SetActiveGunNo(activeGun);
        }

        if (guns.Length >= 1 && Input.GetKeyDown(KeyCode.Alpha1)) { SetActiveGunNo(0); }
        if (guns.Length >= 2 && Input.GetKeyDown(KeyCode.Alpha2)) { SetActiveGunNo(1); }
        if (guns.Length >= 3 && Input.GetKeyDown(KeyCode.Alpha3)) { SetActiveGunNo(2); }
        if (guns.Length >= 4 && Input.GetKeyDown(KeyCode.Alpha4)) { SetActiveGunNo(3); }
        if (guns.Length >= 5 && Input.GetKeyDown(KeyCode.Alpha5)) { SetActiveGunNo(4); }
        if (guns.Length >= 6 && Input.GetKeyDown(KeyCode.Alpha6)) { SetActiveGunNo(5); }
        if (guns.Length >= 7 && Input.GetKeyDown(KeyCode.Alpha7)) { SetActiveGunNo(6); }
        if (guns.Length >= 8 && Input.GetKeyDown(KeyCode.Alpha8)) { SetActiveGunNo(7); }
        if (guns.Length >= 9 && Input.GetKeyDown(KeyCode.Alpha9)) { SetActiveGunNo(8); }

    }

    void SetActiveGunNo(int number)
    {
        foreach (GameObject gun in guns)
        {
            gun.SetActive(false);
        }
        guns[number].SetActive(true);
        guns[number].GetComponent<Combat>().UpdateStats();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideMenu : MonoBehaviour
{
    public Animator menu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            menu.SetTrigger("Out");
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            menu.SetTrigger("In");
        }
    }
}

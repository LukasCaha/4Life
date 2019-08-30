using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PickupImages
{
    public string type;
    public Sprite[] imageColors;

    public PickupImages(string type, Sprite[] imageColors)
    {
        this.type = type;
        this.imageColors = imageColors;
    }
}

public enum DropDownType
{
    Gun,
    HP,
    Shield
};
public enum DropDownColor
{
    Blue,
    Green,
    Orange,
    Purple,
    Red
};

public class Pickupable : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public DropDownType pickupType = DropDownType.Gun;
    private int typeCount;
    public DropDownColor pickupColor = DropDownColor.Blue;
    private int colorCount;
    public PickupImages[] images;

    private void Start()
    {
        Initialization();

        rb.AddForce(Random.insideUnitCircle.normalized * 200);
        UpdateAppearance();
    }

    private void Initialization()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        typeCount = System.Enum.GetNames(typeof(DropDownType)).Length;
        colorCount = System.Enum.GetNames(typeof(DropDownColor)).Length;
    }

    public void SetPickupType(int type, int color)
    {
        Initialization();
        
        type = Mathf.Min(typeCount-1, type);
        color = Mathf.Min(colorCount-1, color);

        pickupType = (DropDownType)type;
        pickupColor = (DropDownColor)color;
        UpdateAppearance();
    }

    void UpdateAppearance()
    {
        sr.sprite = images[(int)pickupType].imageColors[(int)pickupColor];
    }

    public DropDownType GetPickupType()
    {
        return pickupType;
    }

    public DropDownColor GetPickupColor()
    {
        return pickupColor;
    }
}

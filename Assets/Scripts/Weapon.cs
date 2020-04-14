using UnityEngine;

[System.Serializable]
public class Weapon 
{
    public string name = "Revolver";
    public float range = 1000f;
    public int headDamage = 100;
    public int bodyDamage = 30;
    public int legsDamage = 20;
    public float fireRate = 0.5f;
    public int bulletCapacity = 6;
    public float reloadTime = 3.4f;

    public int calculateDamage(string bodyLocation)
    {
        switch(bodyLocation)
        {
            case "PlayerBody":
                return bodyDamage;
            case "PlayerHead":
                return headDamage;
            case "PlayerLegs":
                return legsDamage;
            default:
                return 0;
        }
    }
}

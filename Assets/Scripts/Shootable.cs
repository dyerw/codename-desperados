using UnityEngine;

public class Shootable : MonoBehaviour
{
    public enum ShootableType 
    {
        Terrain,
        Player
    }

    public ShootableType shootableType;
}

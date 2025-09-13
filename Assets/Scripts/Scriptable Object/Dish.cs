using UnityEngine;

[CreateAssetMenu(fileName = "Dish", menuName = "Scriptable Objects/Dish")]
public class Dish : ScriptableObject
{
    public string dishName;
    public Sprite dishSprite;
}

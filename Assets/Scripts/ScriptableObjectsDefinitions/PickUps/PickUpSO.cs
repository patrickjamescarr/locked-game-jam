using UnityEngine;

[CreateAssetMenu(menuName = "Items/Pick Up")]
public class PickUpSO : ScriptableObject
{
	public Sprite sprite;
	public int count;
	public bool permanent;
}

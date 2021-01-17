using UnityEngine;

public class DamageTextController : MonoBehaviour
{
    public float duration = 1.0f; 

    void Start()
    {
        Destroy(this.gameObject, duration);
    }
}

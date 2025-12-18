using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;

    //Sets the HP bar scale to 1, making it look full, as pokemon starts at full hp
    public void SetHP(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }
}

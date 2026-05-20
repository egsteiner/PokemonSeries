using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;

    public bool IsUpdating { get; private set; }

    //Sets the HP bar scale to 1, making it look full, as pokemon starts at full hp
    public void SetHP(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }

    //Change the HP value "smoothly"
    public IEnumerator SetHPSmooth(float newHp)
    {
        IsUpdating = true;

        float curHp = health.transform.localScale.x;
        float changeAmt = curHp - newHp;
        //While past HP is higher than resulting hp, reduce by tiny amount per run
        while (curHp - newHp > Mathf.Epsilon)
        {
            curHp -= changeAmt * Time.deltaTime;
            health.transform.localScale = new Vector3(curHp, 1f);
            yield return null;
        }
        health.transform.localScale = new Vector3(newHp, 1f);

        IsUpdating = false;
    }
}

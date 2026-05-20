using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    [SerializeField] Color highlightedColor;

    public Color HighlightedColor => highlightedColor;

    public static GlobalSettings i { get; private set; }

    private void Awake()
    {
        i = this;
    }
}

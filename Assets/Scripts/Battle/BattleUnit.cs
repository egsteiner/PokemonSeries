using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class BattleUnit : MonoBehaviour
{
    
    [SerializeField] bool isPlayerUnit;
    [SerializeField] BattleHud hud;

    public BattleHud Hud
    {
        get { return hud; }
    }

    public bool IsPlayerUnit
    {
        get { return isPlayerUnit; }
    }

    public Pokemon Pokemon { get; set; }

    Color originalColor;

    Image image;
    Vector3 originalPos;

    private void Awake()
    {
        image = GetComponent<Image>();
        //position relative to canvas, rather than position in the world
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }
    
    //Make new Pokemon instance using the provided pokemon, get back sprite if player's, front sprite if enemy's
    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;
        if (isPlayerUnit)
            image.sprite = Pokemon.Base.BackSprite;

        else image.sprite = Pokemon.Base.FrontSprite;

        hud.gameObject.SetActive(true);

        hud.SetData(pokemon);

        transform.localScale = new Vector3(1, 1, 1);
        image.color = originalColor;
        PlayEnterAnimation();
    }

    public void Clear()
    {
        hud.gameObject.SetActive(false);
    }

    // Have Pokemon move into frame when entering
    public void PlayEnterAnimation()
    {
        if (isPlayerUnit)
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        else
            image.transform.localPosition = new Vector3(500f, originalPos.y);

        image.transform.DOLocalMoveX(originalPos.x, 1f);
    }

    //Have Pokemon move forward slightly when attacking
    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit)
           sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        else
           sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    
    // Have Pokemon 'flash' when taking damage
    public IEnumerator PlayHitAnimation()
    {
        //var sequence = DOTween.Sequence();
        for (int i = 0; i < 2; ++i)
        { 

            var sequence = DOTween.Sequence();

            sequence.Append(image.DOFade(0f, 0.1f));
            sequence.Append(image.DOFade(1f, 0.1f));
            yield return new WaitForSeconds(0.25f);
        }
        
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }

    public IEnumerator PlayCaptureAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(0, 0.5f));
        //sequence.Join(transform.DOMoveY(originalPos.y - 50f, 0.5f));          SENDS POKEMON TO STRATOSPHERE
        sequence.Join(transform.DOLocalMoveY(originalPos.y + 50f, 0.5f));
        sequence.Join(transform.DOScale(new Vector3(0.3f, 0.3f, 1f), 0.5f));
        yield return sequence.WaitForCompletion();
    }

    public IEnumerator PlayBreakOutAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(1, 0.5f));
        //sequence.Join(transform.DOMoveY(originalPos.y - 50f, 0.5f));          SENDS POKEMON TO STRATOSPHERE
        sequence.Join(transform.DOLocalMoveY(originalPos.y, 0.5f));
        sequence.Join(transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f));
        yield return sequence.WaitForCompletion();
    }
}

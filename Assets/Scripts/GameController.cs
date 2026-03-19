using UnityEngine;

public enum GameState { FreeRoam, Battle}

// Script that controls what gameplay is active (Ex: Free roam vs active battle)
public class GameController : MonoBehaviour
{

    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;

    private void Awake()
    {
        ConditionsDB.Init();
    }
    private void Start()
    {
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
    }

    // When starting a battle, switch to Battle camera and mode, get the Party and Wild Pokemon, Start the battle
    void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PokemonParty>();
        var wildPokemon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildPokemon();

        battleSystem.StartBattle(playerParty, wildPokemon);
    }

    // When battle is over, switch to Free Roam
    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    // If Game State is switched to FreeRoam, PlayerController script constantly updates, if Game State is Battle, BattleSystem script constantly updates instead
    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if ( (state == GameState.Battle))
        {
            battleSystem.HandleUpdate();    
        }
    }

}

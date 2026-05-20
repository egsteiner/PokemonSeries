using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Teleports player to a different position WITHOUT switching scenes
public class LocationPortal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] DestinationIdentifier destinationPortal;
    [SerializeField] Transform spawnPoint;

    PlayerController player;
    public void OnPlayerTriggered(PlayerController player)
    {
        player.Character.Animator.IsMoving = false;
        this.player = player;
        StartCoroutine(Teleport());
    }

    public bool TriggerRepeatedly => false;

    Fader fader;
    private void Start()
    {
        fader = FindAnyObjectByType<Fader>();
    }

    IEnumerator Teleport()
    {
        

        GameController.Instance.PauseGame(true);
        yield return fader.FadeIn(0.5f);

        

        var destPortal = FindObjectsByType<LocationPortal>(FindObjectsSortMode.None).First(x => x != this && x.destinationPortal == this.destinationPortal);
        player.Character.SetPositionAndSnapToTile(destPortal.SpawnPoint.position);

        yield return fader.FadeOut(0.5f);
        GameController.Instance.PauseGame(false);

      
    }

    public Transform SpawnPoint => spawnPoint;
}


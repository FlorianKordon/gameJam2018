using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStateController : MonoBehaviour
{
    public GameObject playerCharacter;
    public Checkpoint[] checkpoints;

    private PlayerMovementController _pmc;

    // GLOBAL CONTROLLERS
    private GameLogicController _glc;
    private SceneController _sc;

    private void Awake()
    {
        _glc = FindObjectOfType<GameLogicController>();
        _sc = FindObjectOfType<SceneController>();
        _pmc = playerCharacter.GetComponent<PlayerMovementController>();

        _glc.PlayerDiedEvent += OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        StartCoroutine(RespawnPlayer());
    }

    private IEnumerator RespawnPlayer()
    {
        yield return StartCoroutine(_sc.TimedSingleFade(0.5f, 0, 1f));
        playerCharacter.transform.position = Checkpoint.currentActive.transform.position;
        _pmc.Alive = true;
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(_sc.TimedSingleFade(0.5f, 0, 0f));
    }

    private void OnDisable()
    {
        _glc.PlayerDiedEvent -= OnPlayerDeath;
    }

    private IEnumerator DoAfter(float delay, System.Action operation)
    {
        yield return new WaitForSeconds(delay);
        operation();
    }
}

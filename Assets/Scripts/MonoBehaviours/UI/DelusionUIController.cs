using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DelusionUIController : MonoBehaviour
{
    public float maxDelusion = 100f;
    public float currentDelusion = 1f;
    public float delusionGain = 0.5f;
    public float deathPenalty = 10f;
    public Image bar;

    private Animator anim;
    private int _closeToInsanityHashParam;

    private GameLogicController _glc;
    private SceneController _sc;

    private void Awake()
    {
        _glc = FindObjectOfType<GameLogicController>();
        _sc = FindObjectOfType<SceneController>();
        _glc.PlayerDiedEvent += OnPlayerDied;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        Delusion();
        _closeToInsanityHashParam = Animator.StringToHash("CloseToInsanity");
    }

    private void Update()
    {
        IncreaseDelusion(delusionGain * Time.deltaTime);
    }

    private void IncreaseDelusion(float del)
    {
        if (currentDelusion + del < maxDelusion)
            currentDelusion += del;
        else
        {
            currentDelusion = maxDelusion;
            Debug.Log("Insanity!!");
            _sc.FadeAndLoadScene("Level1");
        }
        Delusion();
    }

    private void Delusion()
    {
        bar.fillAmount = currentDelusion / maxDelusion;
        if (bar.fillAmount >= 0.8)
        {
            bar.color = Color.red;
            anim.SetBool(_closeToInsanityHashParam, true);
        }
        else if (bar.fillAmount >= 0.5f)
        {
            bar.color = Color.magenta;
        }
    }

    private void OnPlayerDied()
    {
        IncreaseDelusion(deathPenalty);
    }

    private void OnDisable()
    {
        _glc.PlayerDiedEvent -= OnPlayerDied;
    }
}

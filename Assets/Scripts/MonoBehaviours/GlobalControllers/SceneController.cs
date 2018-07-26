using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{

    // Action: Specific type of delegate that has no return type, and in this case, no parameters
    public event Action BeforeSceneUnload;
    public event Action AfterSceneLoad;

    public CanvasGroup faderCanvasGroup;

    [Tooltip("Specifiy how long the fade between scene-black-scene should be.")]
    public float fadeDuration = 1f;

    [Tooltip("Specifiy startup scene, which will be faded in upon game start.")]
    public string startingSceneName = "Menu";

    private bool isFading;

    private IEnumerator Start()
    {
        // Init with black screen (alpha = 1
        faderCanvasGroup.alpha = 1f;

        // Start scene loading and wait until it finished
        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName));

        // Start coroutine to fade to alpha=0
        StartCoroutine(Fade(0f));
    }

    public void FadeAndLoadScene(string sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndSwitchScene(sceneName));
        }
    }

    public IEnumerator TimedCrossFade(float fadeTime)
    {
        fadeDuration = fadeTime;
        yield return StartCoroutine(Fade(1f));
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(Fade(0f));
        fadeDuration = 1f;
    }

    public IEnumerator TimedSingleFade(float fadeTime, float delay, float fadeValue)
    {
        fadeDuration = fadeTime;
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(Fade(fadeValue));
        fadeDuration = 1f;
    }

    private IEnumerator FadeAndSwitchScene(string sceneName)
    {
        // 1) Start fading to black (alpha=1) and wait until it finished
        yield return StartCoroutine(Fade(1f));

        // If there are some subscribers, notify them about scene pre-loading
        if (BeforeSceneUnload != null)
        {
            BeforeSceneUnload();
        }

        // 2) Unload current scene async and wait for it
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // 3) Load specified scene and set it active afterswards
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        // If there are some subscribers, report scene post-loading
        if (AfterSceneLoad != null)
        {
            AfterSceneLoad();
        }

        // 4) Start fading to alpha=0 and wait until it finished
        yield return StartCoroutine(Fade(0f));

        // 5) Reset default fadeDuration as it could have been modified before scene loading routine
        fadeDuration = 1f;
    }

    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Set the newly loaded scene as the currently active scene 
        // (last scene in SceneManager List starting from zero) 
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    private IEnumerator Fade(float finalAlpha)
    {
        isFading = true;
        faderCanvasGroup.blocksRaycasts = true;

        // We work out the difference between our current alpha and the final alpha
        // By dividing the difference of alphas by the specified time, we get the speed
        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        isFading = false;
        faderCanvasGroup.blocksRaycasts = false;
    }
}

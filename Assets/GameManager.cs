using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;


    public AudioSource sfxSource;
    public AudioClip beeSfx;
    public AudioClip fireplaceSfx;
    public AudioClip carCrashSfx;

    public int timeOut = 300; // 5 mins

    public int remainingTime;
    public PostProcessProfile profile;
    public float defaultVignette = 0.42f;

    public TimerDisplay timerDisplay;
    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var vig = profile.GetSetting<Vignette>();
        vig.intensity.value = defaultVignette;

        Debug.Log("OnSceneLoaded: " + scene.name);
        if (scene.name == "Level_0")
        {
            StartCoroutine(Timer());
        }
        timerDisplay = FindObjectOfType<TimerDisplay>();
        if(timerDisplay != null)
        {
            timerDisplay.UpdateTime(remainingTime);
        }
    }
    IEnumerator Timer()
    {
        remainingTime = timeOut;
        while(remainingTime > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            remainingTime--;

            if (timerDisplay != null)
            {
                timerDisplay.UpdateTime(remainingTime);
            }

        }
        Debug.Log("Time out!!!");
        // enter Fail state/screen.
        SceneManager.LoadScene("FailScreen");
    }
    public void EndLevel()
    {
        Debug.Log("Level ended");
        var fade = FindObjectOfType<FadeEffect>();
        if (fade != null)
        {
            fade.StartFadeOut(NextScene);
        }
    }
    private void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayCarCrash()
    {
        sfxSource.PlayOneShot(carCrashSfx);
    }
    public void PlayBeeSfx()
    {
        sfxSource.PlayOneShot(beeSfx);
    }

}

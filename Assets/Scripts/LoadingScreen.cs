using TMPro;
using Tweens;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Slider loadingBar;

    private Canvas canvas;
    private Camera canvasCamera;
    private Color backgroundColor;
    private Color transparentBackgroundColor;
    private Tween tween; // use to fade in and out
    private float loadingPercentage;

    public bool FullyLoaded { get; private set; } = false;
    public float LoadingPercentage
    {
        get => loadingPercentage;
        set
        {
            loadingPercentage = Mathf.Clamp01(value);

            loadingBar.value = loadingPercentage;
        }
    }

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvasCamera = Camera.main;
        canvas.worldCamera = canvasCamera;

        backgroundColor = background.color;
        transparentBackgroundColor = backgroundColor;
        transparentBackgroundColor.a = 0;
    }

    private void Update()
    {
        if (Camera.main != canvasCamera)
        {
            canvasCamera = Camera.main;
            canvas.worldCamera = canvasCamera;
        }
    }

    public void Enable()
    {
        FullyLoaded = false;
        gameObject.SetActive(true);

        LoadingPercentage = 0;
        background.gameObject.SetActive(true);
        background.color = transparentBackgroundColor;

        TweenColor(true);
    }

    public void Disable()
    {
        text.gameObject.SetActive(false);
        loadingBar.gameObject.SetActive(false);

        TweenColor(false);
    }

    private void OnEnableComplete()
    {
        FullyLoaded = true;

        background.color = backgroundColor;
        text.gameObject.SetActive(true);
        loadingBar.gameObject.SetActive(true);
    }

    private void OnDisableComplete()
    {
        background.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    private void TweenColor(bool enable)
    {
        tween?.Stop();

        Color start = background.color;
        Color end = enable ? backgroundColor : transparentBackgroundColor;

        void update(float percentage)
        {
            background.color = Color.Lerp(start, end, percentage);
        }

        tween = TweenManager.DoTweenCustom(update, 1f);

        if (enable)
            tween.SetOnComplete(OnEnableComplete);
        else
            tween.SetOnComplete(OnDisableComplete);
    }
}

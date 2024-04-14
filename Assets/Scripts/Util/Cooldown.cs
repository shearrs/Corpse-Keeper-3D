using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class Cooldown
{
    [SerializeField, Min(0f)] private float cooldownTime = 1;
    private float currentTime;
    private bool done = true;
    private CancellationTokenSource tokenSource;

    public float CooldownTime { get => cooldownTime; set => cooldownTime = value; }
    public float CurrentTime { get => currentTime; }
    public float Percentage { get => currentTime / cooldownTime; }
    public bool Done { get => done || (!done && tokenSource == null); }

    public Cooldown()
    {
    }

    public Cooldown(float cooldownTime)
    {
        CooldownTime = cooldownTime;
    }

    ~Cooldown()
    {
        tokenSource?.Dispose();
    }

    public void StartTimer()
    {
        if (Done)
        {
            tokenSource ??= new CancellationTokenSource();

            TimerAsync(tokenSource.Token);
        }
    }

    public void StopTimer()
    {
        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource = null;
        }

        done = true;
    }

    public void RestartTimer()
    {
        StopTimer();
        StartTimer();
    }

    private async void TimerAsync(CancellationToken token)
    {
        done = false;

        currentTime = 0;

        while (currentTime < cooldownTime)
        {
            currentTime += Time.deltaTime;

            if (!Application.isPlaying)
            {
                StopTimer();

                return;
            }
            else if (token.IsCancellationRequested)
            {
                if (done)
                    StopTimer();

                return;
            }

            await Task.Yield();
        }

        done = true;
    }
}
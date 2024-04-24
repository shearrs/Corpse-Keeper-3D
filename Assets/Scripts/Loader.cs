using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : PersistentSingleton<Loader>
{
    [SerializeField] private LoadingScreen loadingScreen;

    private void Start()
    {
        Load("MainMenu");
    }

    public static void Load(string scene) => Instance.InstanceLoad(scene);
    private void InstanceLoad(string scene)
    {
        loadingScreen.Enable();

        StartCoroutine(IELoadScene(scene));
    }

    public static void LoadUnLoad(string sceneToLoad, string sceneToUnLoad) => Instance.InstanceLoad(sceneToLoad, sceneToUnLoad);
    private void InstanceLoad(string sceneToLoad, string sceneToUnLoad)
    {
        loadingScreen.Enable();
        
        StartCoroutine(IELoadUnLoadScene(sceneToLoad, sceneToUnLoad));
    }

    public static void UnLoad(string scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }

    private IEnumerator IELoadScene(string scene)
    {
        while (!loadingScreen.FullyLoaded)
            yield return new WaitForSeconds(1);

        AsyncOperation async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        while (!async.isDone)
        {
            loadingScreen.LoadingPercentage = async.progress / 0.9f;

            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        loadingScreen.Disable();
    }

    private IEnumerator IELoadUnLoadScene(string sceneToLoad, string sceneToUnLoad)
    {
        while (!loadingScreen.FullyLoaded)
            yield return new WaitForSeconds(1);

        AsyncOperation unloadAsync = SceneManager.UnloadSceneAsync(sceneToUnLoad);

        while (!unloadAsync.isDone)
            yield return null;

        StartCoroutine(IELoadScene(sceneToLoad));
    }
}
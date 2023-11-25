using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lybell.Libs;

public class GameManager : SingletonMonoBehaviour<GameManager>, ISaveManager
{
	private string currentScene = "Title";
	private ISaveManager saveManager = new PrefsSaveManager();
	void Start()
	{
		currentScene = SceneManager.GetActiveScene().name;
	}
	public void ToTitle()
	{
		SceneManager.LoadScene("Title");
		currentScene = "Title";
	}
	public void GameStart()
	{
		SceneManager.LoadScene("Ingame");
		currentScene = "Ingame";
	}
	public void GameClear()
	{
		if(currentScene != "Ingame") return;
		StartCoroutine(SetGameClear());
	}
	private IEnumerator SetGameClear()
	{
		// to ingame scene
		ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
		TimeManager timeManager = FindObjectOfType<TimeManager>();
		scoreManager.SetRemainTime(timeManager.currentTime);
		ScoreData scoreData = scoreManager.scoreData;
		bool isGoodEnding = scoreManager.isGoodEnding;
		string toLoadScene = isGoodEnding ? "GoodEnd" : "BadEnd";

		Fader fader = GameObject.Find(Constants.faderName)?.GetComponent<Fader>();
		yield return fader?.Fade(1f);
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(toLoadScene);
		yield return new WaitUntil(() => asyncLoad.isDone);

		// to next scene
		ResultManager resultManager = FindObjectOfType<ResultManager>();
		resultManager.SetScoreData(scoreData);
		currentScene = toLoadScene;
	}
	public void SaveFlag(string key, bool flag)
    {
        this.saveManager.SaveFlag(key, flag);
    }
    public bool GetFlag(string key)
    {
        return this.saveManager.GetFlag(key);
    }
    public bool GetFlag(string key, bool defaultValue)
    {
        return this.saveManager.GetFlag(key, defaultValue);
    }
    public void SaveFloat(string key, float value)
    {
        this.saveManager.SaveFloat(key, value);
    }
    public float GetFloat(string key)
    {
        return this.saveManager.GetFloat(key);
    }
    public float GetFloat(string key, float defaultValue)
    {
        return this.saveManager.GetFloat(key, defaultValue);
    }
    public void ApplySave()
    {
        this.saveManager.ApplySave();
    }
    public void Reset()
    {
        this.saveManager.Reset();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour 
{
	public static NoloVR_Playform noloPlayform;

	[SerializeField]
	private string mainSceneName = "Test";

	void Awake()
	{
		noloPlayform = NoloVR_Playform.InitPlayform();

		DontDestroyOnLoad(transform.gameObject);
	}

	void Start()
	{
		SceneManager.LoadSceneAsync(mainSceneName);
	}

}

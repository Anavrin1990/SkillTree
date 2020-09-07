using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class AutoSaveExtension
{
	static AutoSaveExtension()
	{
		EditorApplication.playModeStateChanged += _ =>
		{
			if (!EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isPlaying) return;
				
			EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
			AssetDatabase.SaveAssets();
			Debug.Log("Auto saved");
		};
	}
}
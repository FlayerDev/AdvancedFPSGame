
#if UNITY_EDITOR
using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AutoSave
{
    private static DateTime nextSaveTime;


    static AutoSave()
    {
        EditorApplication.playModeStateChanged += (PlayModeStateChange state) => {

            if (!EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isPlaying) return;

            Debug.Log("Auto-saving all open scenes... " + state);
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
        };

        nextSaveTime = DateTime.Now.AddMinutes(1);
        EditorApplication.update += Update;
        Debug.Log("Added callback.");
    }

    private static void Update()
    {
        if (nextSaveTime > DateTime.Now || EditorApplication.isPlayingOrWillChangePlaymode) return;

        nextSaveTime = nextSaveTime.AddMinutes(1);

        Debug.Log("AutoSave Scenes: " + DateTime.Now.ToShortTimeString());
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
    }
}
#endif
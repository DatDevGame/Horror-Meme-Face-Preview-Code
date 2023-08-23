////using UnityEditor.SceneManagement;
//using UnityEditor;
//using UnityEngine;
//public static class ApplicationStart
//{
//    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
//    private static void OnStartBeforeSceneLoad()
//    {
//        //Debug.unityLogger.logEnabled = false;
//        Application.lowMemory += OnLowMemory;

////#if UNITY_EDITOR
////        EditorApplication.playModeStateChanged += LoadDefaultScene;
////#endif
//    }



//    private static void OnLowMemory()
//    {
//        Resources.UnloadUnusedAssets();
//    }
////#if UNITY_EDITOR
////    const bool IGNORE_LOADING_SCENE = true;
////    static void LoadDefaultScene(PlayModeStateChange state)
////    {
////        if (state == PlayModeStateChange.ExitingEditMode)
////        {
////            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
////        }

////        if (state == PlayModeStateChange.EnteredPlayMode)
////        {
////            EditorSceneManager.LoadScene(IGNORE_LOADING_SCENE ? 1: 0);
////        }
////    }
////#endif
//}
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
public static class TypingErrorSetup {
 public static void Build(){ReviewSkinBuilder.Open();var g=Object.FindAnyObjectByType<TypingArtFrame>().gameObject;var v=g.GetComponent<TypingErrorArt>()??g.AddComponent<TypingErrorArt>();v.frames=Enumerable.Range(0,6).Select(i=>AssetDatabase.LoadAssetAtPath<Sprite>("Assets/TypecasterIntegration/Art/LegacyCollected/UITextVFX/VFX/typing_error_brackets/typing_error_brackets_"+i.ToString("00")+".png")).ToArray();if(v.frames.Any(s=>!s))throw new System.Exception("Existing error sprites missing");EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());}
}
#endif

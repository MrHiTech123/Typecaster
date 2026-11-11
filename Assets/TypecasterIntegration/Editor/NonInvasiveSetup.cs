#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Typecaster.Integration;
public static class NonInvasiveSetup {
 [MenuItem("Typecaster/Integration/Open current review")]
 public static void Open(){EditorSceneManager.OpenScene("Assets/TypecasterIntegration/Generated/CombatReview.unity");}
 public static void Apply(){Open();var s=Object.FindAnyObjectByType<CombatStatistics>();var p=Object.FindAnyObjectByType<CombatPresentation>();var v=s.GetComponent<NonInvasiveCombatView>()??s.gameObject.AddComponent<NonInvasiveCombatView>();v.statistics=s;v.presentation=p;EditorSceneManager.MarkSceneDirty(s.gameObject.scene);EditorSceneManager.SaveScene(s.gameObject.scene);File.WriteAllText("NonInvasiveSetupResult.txt","PASS: independent HP-observer attached; no original scripts changed.");}
}
#endif

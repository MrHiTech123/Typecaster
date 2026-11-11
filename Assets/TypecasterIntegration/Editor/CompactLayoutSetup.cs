#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using TMPro;
public static class CompactLayoutSetup {
 public static void Build(){
  ReviewSkinBuilder.Open();var p=Object.FindAnyObjectByType<Typecaster.Integration.CombatPresentation>();
  foreach(var pair in new[]{(p.hero,.22f),(p.enemy,.78f)}){var r=pair.Item1.rectTransform;r.anchorMin=r.anchorMax=new Vector2(pair.Item2,.27f);r.pivot=new Vector2(.5f,0);r.anchoredPosition=Vector2.zero;}
  var stats=Object.FindAnyObjectByType<CombatStatistics>();var so=new SerializedObject(stats);
  var player=(RectTransform)so.FindProperty("playerHealthBar").objectReferenceValue;var enemy=(RectTransform)so.FindProperty("enemyHealthBar").objectReferenceValue;
  var playerRoot=(RectTransform)player.parent;var enemyRoot=(RectTransform)enemy.parent;
  playerRoot.anchorMin=playerRoot.anchorMax=new Vector2(0,1);playerRoot.pivot=new Vector2(0,.5f);playerRoot.anchoredPosition=new Vector2(28,-68);playerRoot.localScale=new Vector3(.4f,.4f,1);
  enemyRoot.anchorMin=enemyRoot.anchorMax=new Vector2(1,1);enemyRoot.pivot=new Vector2(1,.5f);enemyRoot.anchoredPosition=new Vector2(-28,-68);enemyRoot.localScale=new Vector3(.98f,.4f,1);
  foreach(var fill in new[]{player,enemy}){fill.anchorMin=fill.anchorMax=new Vector2(0,.5f);fill.pivot=new Vector2(0,.5f);fill.anchoredPosition=Vector2.zero;fill.localScale=Vector3.one;}
  foreach(var t in Object.FindObjectsByType<TMP_Text>(FindObjectsInactive.Include,FindObjectsSortMode.None))if(t.text.Trim()=="HP"){t.rectTransform.anchorMin=t.rectTransform.anchorMax=new Vector2(0,1);t.rectTransform.pivot=new Vector2(0,.5f);t.rectTransform.anchoredPosition=new Vector2(28,-52);t.fontSize=12;t.rectTransform.sizeDelta=new Vector2(60,20);}
  EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
 }
}
#endif


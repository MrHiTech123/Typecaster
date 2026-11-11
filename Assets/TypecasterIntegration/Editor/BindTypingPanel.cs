#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
public static class BindTypingPanel {
 public static void Build(){
  EditorSceneManager.OpenScene("Assets/TypecasterIntegration/Generated/CombatSkinReview.unity");
  var frame=Object.FindAnyObjectByType<TypingArtFrame>();
  var tex=AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/TypecasterIntegration/Art/LegacyCollected/UITextVFX/Panels/dialogue_paper.png");
  var sprite=Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(.5f,.5f),100,0,SpriteMeshType.FullRect,new Vector4(5,5,5,5));
  var path="Assets/TypecasterIntegration/Generated/TypingPanelNineSlice.asset";var existing=AssetDatabase.LoadAssetAtPath<Sprite>(path);if(existing){EditorUtility.CopySerialized(sprite,existing);Object.DestroyImmediate(sprite);sprite=existing;EditorUtility.SetDirty(sprite);}else AssetDatabase.CreateAsset(sprite,path);
  frame.inputPanel=sprite;EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());AssetDatabase.SaveAssets();
  File.WriteAllText("TypingPanelBinding.txt","Bound existing dialogue_paper to original live BackGround through additive visual component. Original TMP objects restyled, text/input/timer/callbacks untouched. No new PNG imported.");
 }
}
#endif

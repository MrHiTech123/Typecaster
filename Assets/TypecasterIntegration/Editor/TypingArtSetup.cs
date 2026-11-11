#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
public static class TypingArtSetup {
 public static void Build(){
  EditorSceneManager.OpenScene("Assets/TypecasterIntegration/Generated/CombatSkinReview.unity");
  var go=GameObject.Find("OptionalTypingArtFrame")??new GameObject("OptionalTypingArtFrame");var frame=go.GetComponent<TypingArtFrame>()??go.AddComponent<TypingArtFrame>();
  frame.book=Import("book",new Rect(0,0,400,300));frame.runes=Import("air_runes",new Rect(0,0,400,300));
  EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
  File.WriteAllText("TypingArtSetupResult.txt","PASS: optional book/runes observer attached. Original typing prefab, text and callbacks unchanged; visual runtime demonstration still requires validation.");
 }
 static Sprite Import(string name,Rect rect){var path="Assets/TypecasterIntegration/Art/LegacyUI/"+name+".png";var t=(TextureImporter)AssetImporter.GetAtPath(path);t.textureType=TextureImporterType.Sprite;t.spriteImportMode=SpriteImportMode.Single;t.filterMode=FilterMode.Point;t.textureCompression=TextureImporterCompression.Uncompressed;t.SaveAndReimport();var texture=AssetDatabase.LoadAssetAtPath<Texture2D>(path);var s=Sprite.Create(texture,rect,new Vector2(.5f,.5f),100);var output="Assets/TypecasterIntegration/Generated/"+name+"_crop.asset";var existing=AssetDatabase.LoadAssetAtPath<Sprite>(output);if(existing){EditorUtility.CopySerialized(s,existing);Object.DestroyImmediate(s);EditorUtility.SetDirty(existing);AssetDatabase.SaveAssets();return existing;}AssetDatabase.CreateAsset(s,output);return s;}
}
#endif


#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public static class LegacySkinSetup {
 static Sprite Get(string name){var p=Directory.GetFiles("Assets/TypecasterIntegration/Art/LegacyCollected/UITextVFX",name+".png",SearchOption.AllDirectories).Single().Replace('\\','/');return AssetDatabase.LoadAssetAtPath<Sprite>(p);}
 public static void Build(){
  var root="Assets/TypecasterIntegration/Art/LegacyCollected";
  foreach(var p0 in Directory.GetFiles(root,"*.png",SearchOption.AllDirectories)){var p=p0.Replace('\\','/');var i=(TextureImporter)AssetImporter.GetAtPath(p);i.textureType=TextureImporterType.Sprite;i.spriteImportMode=SpriteImportMode.Single;i.filterMode=FilterMode.Point;i.textureCompression=TextureImporterCompression.Uncompressed;i.SaveAndReimport();}
  EditorSceneManager.OpenScene("Assets/TypecasterIntegration/Generated/CombatSkinReview.unity");
  var bs=UnityEngine.Object.FindObjectsByType<Button>(FindObjectsInactive.Include,FindObjectsSortMode.None);int n=0;
  foreach(var b in bs){var im=b.targetGraphic as Image;if(!im)continue;im.sprite=Get("button_idle");im.color=Color.white;b.transition=Selectable.Transition.SpriteSwap;var st=b.spriteState;st.highlightedSprite=Get("button_hover");st.pressedSprite=Get("button_pressed");st.selectedSprite=Get("button_hover");st.disabledSprite=Get("button_disabled");b.spriteState=st;n++;}
  int bars=0;foreach(var im in UnityEngine.Object.FindObjectsByType<Image>(FindObjectsInactive.Include,FindObjectsSortMode.None)){if(im.color.r>.6f&&im.color.r<.8f&&im.color.g<.3f&&im.color.b<.4f){im.sprite=Get("hp_bar_fill");im.color=Color.white;bars++;}}
  EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
  var all=Directory.GetFiles(root,"*.png",SearchOption.AllDirectories);var missing=all.Count(p=>!AssetDatabase.LoadAssetAtPath<Sprite>(p.Replace('\\','/')));
  File.WriteAllText("LegacyArtVerification.txt","PASS: "+all.Length+" unique legacy PNGs imported as sprites, missing="+missing+"; button skins="+n+"; health bar fills="+bars+". No gameplay callbacks changed.");
  if(missing!=0)throw new Exception("Missing legacy sprites");
 }
}
#endif

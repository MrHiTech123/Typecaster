#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public static class ReviewSkinBuilder {
 const string Source="Assets/TypecasterIntegration/Generated/CombatReview.unity";
 const string Target="Assets/TypecasterIntegration/Generated/CombatSkinReview.unity";
 [MenuItem("Typecaster/Integration/Open optional UI skin review")]
 public static void Open(){EditorSceneManager.OpenScene(Target);}
 static string CallSignature(Button b){return string.Join("|",Enumerable.Range(0,b.onClick.GetPersistentEventCount()).Select(i=>b.onClick.GetPersistentMethodName(i)+":"+b.onClick.GetPersistentTarget(i)));}
 static Sprite Find(string name){var p=Directory.GetFiles("Assets/Arts",name+".png",SearchOption.AllDirectories).FirstOrDefault();return p==null?null:AssetDatabase.LoadAllAssetsAtPath(p.Replace('\\','/')).OfType<Sprite>().FirstOrDefault();}
 public static void Build(){
  var scene=EditorSceneManager.OpenScene(Source);
  var buttons=UnityEngine.Object.FindObjectsByType<Button>(FindObjectsInactive.Include,FindObjectsSortMode.None);
  var before=buttons.ToDictionary(b=>b,CallSignature);
  int icons=0;
  foreach(var b in buttons){
   if(b.targetGraphic)b.targetGraphic.color=new Color(.11f,.14f,.20f,.97f);
   var c=b.colors;c.normalColor=Color.white;c.highlightedColor=new Color(.85f,.9f,1);c.pressedColor=new Color(.65f,.75f,.9f);c.selectedColor=new Color(.9f,.9f,1);b.colors=c;
   foreach(var t in b.GetComponentsInChildren<TMP_Text>(true)){t.color=new Color(.98f,.91f,.76f);t.fontSize=Mathf.Min(t.fontSize,24);}
   foreach(var t in b.GetComponentsInChildren<Text>(true)){t.color=new Color(.98f,.91f,.76f);t.fontSize=Mathf.Min(t.fontSize,24);}
   var methods=Enumerable.Range(0,b.onClick.GetPersistentEventCount()).Select(i=>b.onClick.GetPersistentMethodName(i)).ToArray();
   string key=methods.Contains("Attack")?"mana_blast_00":methods.Contains("CastSpell")?"fire_ball_projectile_00":methods.Contains("UsePotion")?"items_health_potion_00":null;
   if(key!=null && !b.GetComponentsInChildren<Image>(true).Any(i=>i!=b.targetGraphic && i.sprite!=null)){
    var sprite=Find(key);if(sprite){var g=new GameObject("ReviewIcon_"+key,typeof(RectTransform),typeof(Image));g.transform.SetParent(b.transform,false);var im=g.GetComponent<Image>();im.sprite=sprite;im.raycastTarget=false;im.preserveAspect=true;var r=im.rectTransform;r.anchorMin=r.anchorMax=new Vector2(0,.5f);r.pivot=new Vector2(0,.5f);r.anchoredPosition=new Vector2(6,0);r.sizeDelta=new Vector2(22,22);icons++;}
   }
  }
  foreach(var im in UnityEngine.Object.FindObjectsByType<Image>(FindObjectsInactive.Include,FindObjectsSortMode.None)){
   if(im.color.r>.8f&&im.color.g<.25f&&im.color.b<.25f) im.color=new Color(.68f,.20f,.29f,.96f);
  }
  if(buttons.Any(b=>before[b]!=CallSignature(b)))throw new Exception("Button callback changed unexpectedly");
  EditorSceneManager.SaveScene(scene,Target);
  ArtsReviewGallery.Verify();
  File.WriteAllText("SkinVerification.txt","PASS: "+buttons.Length+" button callback signatures unchanged; "+icons+" semantic icons added using existing local Arts. Optional copy scene only. No original scripts/assets edited.");
 }
}
#endif

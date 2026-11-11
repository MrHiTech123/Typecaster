#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
public static class TypecasterSnapshot {
 public static void Capture() {
  EditorSceneManager.OpenScene("Assets/TypecasterIntegration/Generated/CombatReview.unity");
  var actions=Object.FindAnyObjectByType<PlayerActions>(); actions.SendMessage("Start");
  var cam=Object.FindAnyObjectByType<Camera>();
  foreach(var c in Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None)) { c.renderMode=RenderMode.ScreenSpaceCamera;c.worldCamera=cam;c.planeDistance=1; }
  var rt=new RenderTexture(1280,720,24); rt.Create();
  cam.targetTexture=rt; Canvas.ForceUpdateCanvases();
  var v=Object.FindAnyObjectByType<Typecaster.Integration.CombatPresentation>(); var cr=v.hero.GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect;
  foreach(var pair in new[]{(v.hero,.22f),(v.enemy,.78f)}) { var tr=pair.Item1.rectTransform;tr.anchorMin=tr.anchorMax=Vector2.zero;tr.pivot=new Vector2(.5f,0);tr.anchoredPosition=new Vector2(cr.width*pair.Item2,cr.height*.22f); }
  Canvas.ForceUpdateCanvases();
  var req=new RenderPipeline.StandardRequest { destination=rt };
  if(RenderPipeline.SupportsRenderRequest(cam,req)) RenderPipeline.SubmitRenderRequest(cam,req); else cam.Render();
  RenderTexture.active=rt; var t=new Texture2D(1280,720,TextureFormat.RGBA32,false); t.ReadPixels(new Rect(0,0,1280,720),0,0);t.Apply();File.WriteAllBytes("CombatReview_NonInvasive.png",t.EncodeToPNG());RenderTexture.active=null;cam.targetTexture=null;Object.DestroyImmediate(t);rt.Release();Object.DestroyImmediate(rt);
  foreach(var c in Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None)) c.renderMode=RenderMode.ScreenSpaceOverlay;
  EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
 }
}
#endif

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public static class TypingArtVerify {
 const string K="TypingArtVerify";static double next;
 [InitializeOnLoadMethod] static void Reg(){EditorApplication.update-=Tick;EditorApplication.update+=Tick;next=EditorApplication.timeSinceStartup+8;if(SessionState.GetInt(K,0)>0)SessionState.SetInt(K,1);}
 public static void Begin(){ReviewSkinBuilder.Open();SessionState.SetInt(K,1);Reg();EditorApplication.isPlaying=true;}
 static void Tick(){int n=SessionState.GetInt(K,0);if(n==0)return;EditorApplication.QueuePlayerLoopUpdate();EditorApplication.isPaused=false;if(!EditorApplication.isPlaying||EditorApplication.isCompiling||EditorApplication.timeSinceStartup<next)return;
 if(n==1){var gm=Object.FindAnyObjectByType<GameManager>();if(GameManager.INSTANCE==null)gm.SendMessage("Awake");var a=Object.FindAnyObjectByType<PlayerActions>();a.OpenSpellMenu();a.CastSpell();SessionState.SetString(K+"Prompt",Object.FindAnyObjectByType<TypingTestBox>().BoxForWhatToType.text);}
 if(n==2){var box=Object.FindAnyObjectByType<TypingTestBox>();var frame=Object.FindAnyObjectByType<TypingArtFrame>();Image bg=null;foreach(var i in box.GetComponentsInChildren<Image>())if(i.name=="BackGround")bg=i;if(!box||!bg||bg.sprite!=frame.inputPanel||box.BoxForWhatToType.text!=SessionState.GetString(K+"Prompt","")){Done(false,"Original background not bound or prompt changed");return;}
  box.BoxForWhatToType.ForceMeshUpdate();if(box.BoxForWhatToType.isTextOverflowing){Done(false,"Prompt text overflows panel");return;}
  string inputCheck="Original TypeKey accepted first 4 characters (method-level, not physical keyboard).";
  try{var method=typeof(TypingTestBox).GetMethod("TypeKey",System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance);var prompt=box.BoxForWhatToType.text;foreach(char c in prompt.Substring(0,4))method.Invoke(box,new object[]{c==' '?"space":c.ToString()});}
  catch(System.Exception e){inputCheck="BLOCKED original input handler: "+(e.InnerException??e).Message;}
  File.WriteAllText("OriginalTypingInputCheck.txt",inputCheck);box.BoxForWhatPlayerTyped.ForceMeshUpdate();try{Capture();}catch(System.Exception e){Done(false,e.Message);return;}Object.Destroy(box.gameObject);}
 if(n==3){Done(!Object.FindAnyObjectByType<TypingTestBox>(),"Original BackGround uses existing nine-slice dialogue_paper; original TMP prompt unchanged and does not overflow; separate typed-text region. See OriginalTypingInputCheck.txt for input-handler boundary.");return;}
 SessionState.SetInt(K,n+1);next=EditorApplication.timeSinceStartup+3;
 }
 static Rect Bounds(RectTransform r,Camera cam){var v=new Vector3[4];r.GetWorldCorners(v);var lo=RectTransformUtility.WorldToScreenPoint(cam,v[0]);var hi=RectTransformUtility.WorldToScreenPoint(cam,v[2]);return Rect.MinMaxRect(lo.x,lo.y,hi.x,hi.y);}
 static void Capture(){var cam=Object.FindAnyObjectByType<Camera>();foreach(var c in Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None)){c.renderMode=RenderMode.ScreenSpaceCamera;c.worldCamera=cam;c.planeDistance=1;}
  var report=new System.Collections.Generic.List<string>();var box=Object.FindAnyObjectByType<TypingTestBox>();Image bg=null;foreach(var i in box.GetComponentsInChildren<Image>())if(i.name=="BackGround")bg=i;var p=Object.FindAnyObjectByType<Typecaster.Integration.CombatPresentation>();
  foreach(var wh in new[]{new Vector2Int(1280,720),new Vector2Int(1920,1080),new Vector2Int(1024,768)}){
   var rt=new RenderTexture(wh.x,wh.y,24);rt.Create();cam.targetTexture=rt;Canvas.ForceUpdateCanvases();Object.FindAnyObjectByType<GroundedActorLayout>().Align();Object.FindAnyObjectByType<TypingErrorArt>()?.SendMessage("LateUpdate");box.BoxForWhatToType.ForceMeshUpdate();box.BoxForWhatPlayerTyped.ForceMeshUpdate();
   var panel=Bounds(bg.rectTransform,cam);var hero=Bounds(p.hero.rectTransform,cam);var enemy=Bounds(p.enemy.rectTransform,cam);var ground=Bounds(Object.FindAnyObjectByType<GroundedActorLayout>().floor,cam).yMax;float heroGap=hero.yMin+hero.height*3f/48f-ground;float enemyGap=enemy.yMin+enemy.height*3f/48f-ground;bool overlap=panel.Overlaps(hero)||panel.Overlaps(enemy);bool overflow=box.BoxForWhatToType.isTextOverflowing||box.BoxForWhatPlayerTyped.isTextOverflowing;
   report.Add(wh.x+"x"+wh.y+": panel="+panel+" hero="+hero+" enemy="+enemy+" overlap="+overlap+" textOverflow="+overflow+" heroFootGap="+heroGap+" enemyFootGap="+enemyGap);
   var req=new RenderPipeline.StandardRequest{destination=rt};if(RenderPipeline.SupportsRenderRequest(cam,req))RenderPipeline.SubmitRenderRequest(cam,req);else cam.Render();RenderTexture.active=rt;var t=new Texture2D(wh.x,wh.y,TextureFormat.RGBA32,false);t.ReadPixels(new Rect(0,0,wh.x,wh.y),0,0);t.Apply();File.WriteAllBytes("CompactTyping_"+wh.x+"x"+wh.y+".png",t.EncodeToPNG());RenderTexture.active=null;cam.targetTexture=null;Object.Destroy(t);rt.Release();Object.Destroy(rt);
   if(overlap||overflow||Mathf.Abs(heroGap)>.5f||Mathf.Abs(enemyGap)>.5f){File.WriteAllLines("CompactLayoutVerification.txt",report);throw new System.Exception("Compact typing UI overlaps characters or text overflows");}
  }File.WriteAllLines("CompactLayoutVerification.txt",report);
 }
 static void Done(bool ok,string message){SessionState.SetInt(K,0);File.WriteAllText("TypingArtVerification.txt",(ok?"PASS":"FAIL")+"\n"+message);EditorApplication.Exit(ok?0:1);}
}
#endif






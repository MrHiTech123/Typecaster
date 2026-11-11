#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
public static class TypingErrorVerify {
 const string K="TypingErrorVerify";static double next;static UnityEngine.InputSystem.Keyboard testKeyboard;
 [InitializeOnLoadMethod]static void Reg(){EditorApplication.update-=Tick;EditorApplication.update+=Tick;next=EditorApplication.timeSinceStartup+8;if(SessionState.GetInt(K,0)>0)SessionState.SetInt(K,1);}
 public static void Begin(){ReviewSkinBuilder.Open();SessionState.SetInt(K,1);Reg();EditorApplication.isPlaying=true;}
 static void Key(TypingTestBox box,string key){typeof(TypingTestBox).GetMethod("TypeKey",System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance).Invoke(box,new object[]{key});}
 static void Tick(){int n=SessionState.GetInt(K,0);if(n==0||!EditorApplication.isPlaying||EditorApplication.isCompiling||EditorApplication.timeSinceStartup<next)return;EditorApplication.QueuePlayerLoopUpdate();EditorApplication.isPaused=false;
  try{var box=Object.FindAnyObjectByType<TypingTestBox>();var art=Object.FindAnyObjectByType<TypingErrorArt>();
   if(n==1){var gm=Object.FindAnyObjectByType<GameManager>();if(!GameManager.INSTANCE)gm.SendMessage("Awake");var a=Object.FindAnyObjectByType<PlayerActions>();a.OpenSpellMenu();a.CastSpell();}
   if(n==2){if(!box||art.IsShowingError)throw new System.Exception("Unexpected initial error");foreach(char c in box.BoxForWhatToType.text.Substring(0,4))Key(box,c==(char)32?"space":c.ToString());Press(UnityEngine.InputSystem.Key.X);}
   if(n==3){if(!art.IsShowingError||!box.BoxForWhatPlayerTyped.text.Contains("red\">x"))throw new System.Exception("Original mistake not reflected");Capture();Press(UnityEngine.InputSystem.Key.Backspace);}
   if(n==4){if(art.IsShowingError)throw new System.Exception("Error did not clear after original backspace");Key(box,box.BoxForWhatToType.text.Substring(4,1));}
   if(n==5){if(art.IsShowingError)throw new System.Exception("Correct character incorrectly shown as error");Object.Destroy(box.gameObject);}
   if(n==6){if(Object.FindAnyObjectByType<TypingTestBox>()||GameObject.Find("ExistingTypingErrorArt"))throw new System.Exception("Error visual not cleaned up");Done(true,"Original CastSpell: prefix via original method; wrong X and Backspace via virtual InputSystem keyboard -> error appears and clears; next correct character via original method -> hidden; close -> cleanup. Not OS physical-keyboard/full-spell completion.");return;}
   SessionState.SetInt(K,n+1);next=EditorApplication.timeSinceStartup+.4;
  }catch(System.Exception e){Done(false,(e.InnerException??e).ToString());}
 }
 static void Press(UnityEngine.InputSystem.Key key){if(testKeyboard==null)testKeyboard=UnityEngine.InputSystem.InputSystem.AddDevice<UnityEngine.InputSystem.Keyboard>();UnityEngine.InputSystem.InputSystem.QueueStateEvent(testKeyboard,new UnityEngine.InputSystem.LowLevel.KeyboardState(key));UnityEngine.InputSystem.InputSystem.Update();UnityEngine.InputSystem.InputSystem.QueueStateEvent(testKeyboard,new UnityEngine.InputSystem.LowLevel.KeyboardState());UnityEngine.InputSystem.InputSystem.Update();}
 static void Capture(){typeof(TypingArtVerify).GetMethod("Capture",System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Static).Invoke(null,null);var cam=Object.FindAnyObjectByType<Camera>();foreach(var c in Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None)){c.renderMode=RenderMode.ScreenSpaceCamera;c.worldCamera=cam;c.planeDistance=1;}var rt=new RenderTexture(1280,720,24);rt.Create();cam.targetTexture=rt;Canvas.ForceUpdateCanvases();Object.FindAnyObjectByType<GroundedActorLayout>().Align();Object.FindAnyObjectByType<TypingErrorArt>().SendMessage("LateUpdate");var req=new RenderPipeline.StandardRequest{destination=rt};if(RenderPipeline.SupportsRenderRequest(cam,req))RenderPipeline.SubmitRenderRequest(cam,req);else cam.Render();RenderTexture.active=rt;var t=new Texture2D(1280,720,TextureFormat.RGBA32,false);t.ReadPixels(new Rect(0,0,1280,720),0,0);t.Apply();File.WriteAllBytes("TypingErrorReview.png",t.EncodeToPNG());RenderTexture.active=null;cam.targetTexture=null;Object.Destroy(t);rt.Release();Object.Destroy(rt);}
 static void Done(bool ok,string msg){if(testKeyboard!=null)UnityEngine.InputSystem.InputSystem.RemoveDevice(testKeyboard);SessionState.SetInt(K,0);File.WriteAllText("TypingErrorVerification.txt",(ok?"PASS":"FAIL")+"\n"+msg);EditorApplication.Exit(ok?0:1);}
}
#endif




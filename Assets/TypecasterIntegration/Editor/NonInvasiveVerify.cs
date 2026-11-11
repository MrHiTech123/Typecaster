#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
public static class NonInvasiveVerify {
 const string K="NonInvasiveVerify";static double next;
 [InitializeOnLoadMethod] static void Reg(){EditorApplication.update-=Tick;EditorApplication.update+=Tick;next=EditorApplication.timeSinceStartup+8;if(SessionState.GetInt(K,0)>0)SessionState.SetInt(K,1);}
 public static void Begin(){NonInvasiveSetup.Open();SessionState.SetInt(K,1);Reg();EditorApplication.isPlaying=true;}
 static void Tick(){int n=SessionState.GetInt(K,0);if(n==0)return;EditorApplication.QueuePlayerLoopUpdate();EditorApplication.isPaused=false;if(!EditorApplication.isPlaying||EditorApplication.isCompiling||EditorApplication.timeSinceStartup<next)return;var a=Object.FindAnyObjectByType<PlayerActions>();var s=Object.FindAnyObjectByType<CombatStatistics>();
 if(n==1){s.playerHP=100;s.enemyHP=100;a.Attack();a.Attack();if(s.playerHP!=60||s.enemyHP!=80){Done(false,"Original immediate/double-click behavior changed");return;}}
 if(n==2){if(s.playerHP!=60||s.enemyHP!=80){Done(false,"Visual observer changed health");return;}a.Defense();if(s.playerHP!=40){Done(false,"Original skip behavior changed");return;}a.UsePotion();}
 if(n==3){Done(s.playerHP==80&&s.enemyHP==80,"Original immediate actions and double-click behavior preserved; presentation observer did not change health.");return;}
 SessionState.SetInt(K,n+1);next=EditorApplication.timeSinceStartup+6;
 }
 static void Done(bool ok,string message){SessionState.SetInt(K,0);File.WriteAllText("NonInvasiveVerification.txt",(ok?"PASS":"FAIL")+"\n"+message);EditorApplication.Exit(ok?0:1);}
}
#endif

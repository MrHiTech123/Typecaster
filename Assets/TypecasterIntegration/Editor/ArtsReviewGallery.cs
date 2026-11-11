#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
public class ArtsReviewGallery : EditorWindow {
 class Group { public string name; public string[] paths; }
 List<Group> groups=new List<Group>(); Vector2 scroll; string filter=""; bool animate=true; float fps=8;
 [MenuItem("Typecaster/Integration/Review all local Arts")]
 public static void Open(){GetWindow<ArtsReviewGallery>("Local Arts review");}
 void OnEnable(){Refresh();EditorApplication.update+=Tick;}
 void OnDisable(){EditorApplication.update-=Tick;}
 void Tick(){if(animate)Repaint();}
 void Refresh(){
  var files=Directory.GetFiles("Assets/Arts","*.png",SearchOption.AllDirectories).Select(x=>x.Replace('\\','/')).OrderBy(x=>x).ToArray();
  groups=files.Where(x=>!x.Contains("/Environment/")&&!Path.GetFileNameWithoutExtension(x).EndsWith("_strip")&&!x.EndsWith("_4f.png"))
   .GroupBy(x=>Regex.Replace(x,@"_\d{2}\.png$", ""))
   .Select(g=>new Group{name=g.Key,paths=g.OrderBy(x=>x).ToArray()}).ToList();
    if(File.Exists("LegacyArtDedup.csv")){
   var legacy=File.ReadAllLines("LegacyArtDedup.csv").Skip(1).Select(l=>l.Split(',').Select(c=>c.Trim('"').Replace('\\','/')).ToArray()).Where(a=>a.Length==5).ToArray();
   groups.AddRange(legacy.Where(a=>!a[1].Contains("_strip")).GroupBy(a=>a[0]+"/"+Regex.Replace(a[1],@"_\d{2}\.png$", "")).Select(g=>new Group{name="Legacy/"+g.Key,paths=g.OrderBy(a=>a[1]).Select(a=>a[4]).ToArray()}));
  }
  groups.AddRange(files.Where(x=>x.Contains("/Environment/")).Select(x=>new Group{name=x,paths=new[]{x}}));
 }
 void OnGUI(){
  EditorGUILayout.HelpBox("REVIEW ONLY: original Assets/Arts files. Animated groups are previews, NOT proof of gameplay integration. No importer or game-code edits.",MessageType.Info);
  EditorGUILayout.BeginHorizontal();filter=EditorGUILayout.TextField("Filter",filter);animate=GUILayout.Toggle(animate,"Animate");fps=EditorGUILayout.Slider(fps,1,16);if(GUILayout.Button("Refresh",GUILayout.Width(65)))Refresh();EditorGUILayout.EndHorizontal();
  scroll=EditorGUILayout.BeginScrollView(scroll);
  foreach(var g in groups.Where(g=>g.name.IndexOf(filter,StringComparison.OrdinalIgnoreCase)>=0)){
   EditorGUILayout.BeginVertical("box");EditorGUILayout.LabelField(g.name.Replace("Assets/Arts/",""),EditorStyles.boldLabel);
   int i=animate?(int)(EditorApplication.timeSinceStartup*fps)%g.paths.Length:0;
   var t=AssetDatabase.LoadAssetAtPath<Texture2D>(g.paths[i]);
   EditorGUILayout.BeginHorizontal();var r=GUILayoutUtility.GetRect(160,120,GUILayout.Width(160));EditorGUI.DrawRect(r,new Color(.12f,.14f,.18f));if(t)GUI.DrawTexture(r,t,ScaleMode.ScaleToFit,true);
   EditorGUILayout.BeginVertical();GUILayout.Label(g.paths.Length+" frame(s) | "+(t?t.width+" x "+t.height:"MISSING"));GUILayout.Label("Preview frame "+i);if(GUILayout.Button("Select source",GUILayout.Width(130))){Selection.activeObject=t;EditorGUIUtility.PingObject(t);}EditorGUILayout.EndVertical();EditorGUILayout.EndHorizontal();EditorGUILayout.EndVertical();
  }EditorGUILayout.EndScrollView();
 }
 public static void Verify(){
  var paths=Directory.GetFiles("Assets/Arts","*.png",SearchOption.AllDirectories).Select(x=>x.Replace('\\','/')).OrderBy(x=>x).ToArray();
  var missing=paths.Where(p=>AssetDatabase.LoadAssetAtPath<Texture2D>(p)==null).ToArray();
  var lines=new List<string>{"path,status,width,height,usage"};
  foreach(var p in paths){var t=AssetDatabase.LoadAssetAtPath<Texture2D>(p);lines.Add(p+","+(t?"loaded":"MISSING")+","+(t?t.width:0)+","+(t?t.height:0)+",review catalog; see checklist for gameplay status");}
  File.WriteAllLines("ArtsReviewInventory.csv",lines);
  File.WriteAllText("ArtsGalleryVerification.txt",(missing.Length==0?"PASS":"FAIL")+": "+paths.Length+" original PNGs load; "+missing.Length+" missing. Preview groups exclude duplicate strips; CSV includes every PNG.");
  if(missing.Length>0)throw new Exception("Missing local Arts textures");
 }
}
#endif


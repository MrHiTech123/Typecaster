#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Typecaster.EnvironmentAuto
{
    // Explicit opt-in only. Never modifies original scenes, code, or build settings.
    public static class Builder
    {
        const string Root = "Assets/TypecasterEnvironmentAuto";
        const string Source = Root + "/Source";
        public static string LastOutputDirectory { get; private set; }
        static readonly string[] Names = { "city_outdoor", "woods", "noble_house", "typewriter_temple", "road_connectivity" };
        [Serializable] class Manifest { public Entry[] assets; }
        [Serializable] class Entry { public string name; public string path; public int[] size; }
        [Serializable] class Layout { public int[] canvas; public string origin; public Placement[] placements; }
        [Serializable] class Placement { public string asset; public int x; public int y; public int layer; public bool flipX; }

        [MenuItem("Tools/Typecaster/Build all modular environments")]
        public static void BuildAll()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                throw new InvalidOperationException("Exit Play Mode before building environments.");
            var manifest = JsonUtility.FromJson<Manifest>(File.ReadAllText(Source + "/manifest.json"));
            var entries = manifest.assets.ToDictionary(a => a.name);
            var layouts = Names.ToDictionary(n => n, n => JsonUtility.FromJson<Layout>(File.ReadAllText(Source + "/layouts/" + n + "/layout.json")));
            // Validate everything before changing import settings or creating output.
            foreach (var e in entries.Values)
            {
                if (e.path.Contains("..") || Path.IsPathRooted(e.path) || !File.Exists(Source + "/" + e.path))
                    throw new InvalidDataException("Missing/unsafe asset: " + e.path);
                if (e.size == null || e.size.Length != 2 || e.size.Any(s => s <= 0))
                    throw new InvalidDataException("Bad dimensions: " + e.name);
            }
            foreach (var l in layouts.Values)
                if (l.origin != "top-left" || l.canvas == null || l.canvas.Length != 2 || l.canvas.Any(s => s <= 0) || l.placements == null || l.placements.Any(p => !entries.ContainsKey(p.asset)))
                    throw new InvalidDataException("Invalid layout or missing asset reference.");
            foreach (var e in entries.Values)
            {
                var t = AssetImporter.GetAtPath(Source + "/" + e.path) as TextureImporter;
                if (t == null) throw new InvalidOperationException("Import package in Unity first: " + e.path);
                t.textureType = TextureImporterType.Sprite;
                t.spriteImportMode = SpriteImportMode.Single;
                t.filterMode = FilterMode.Point;
                t.textureCompression = TextureImporterCompression.Uncompressed;
                t.mipmapEnabled = false;
                t.alphaIsTransparency = true;
                t.npotScale = TextureImporterNPOTScale.None;
                t.wrapMode = TextureWrapMode.Clamp;
                t.spritePixelsPerUnit = 100;
                t.SaveAndReimport();
            }
            var sprites = entries.ToDictionary(k => k.Key, k => AssetDatabase.LoadAssetAtPath<Sprite>(Source + "/" + k.Value.path));
            if (sprites.Values.Any(s => s == null)) throw new InvalidOperationException("A sprite failed to import.");
            string output = AssetDatabase.GenerateUniqueAssetPath(Root + "/Generated");
            LastOutputDirectory = output;
            Directory.CreateDirectory(output);
            AssetDatabase.Refresh();
            var previous = SceneManager.GetActiveScene();
            foreach (string n in Names)
            {
                var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
                try
                {
                    SceneManager.SetActiveScene(scene);
                    var canvasObject = new GameObject("EnvironmentPreview", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler));
                    var canvas = canvasObject.GetComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvas.pixelPerfect = true;
                    var scaler = canvasObject.GetComponent<CanvasScaler>();
                    scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    scaler.referenceResolution = new Vector2(800, 600);
                    scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                    scaler.matchWidthOrHeight = 0.5f;
                    var layout = layouts[n];
                    var panel = new GameObject(n, typeof(RectTransform), typeof(RectMask2D));
                    panel.transform.SetParent(canvasObject.transform, false);
                    var rect = panel.GetComponent<RectTransform>();
                    rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);
                    rect.sizeDelta = new Vector2(layout.canvas[0], layout.canvas[1]);
                    rect.localScale = Vector3.one * 2f;
                    var layers = new Dictionary<int, RectTransform>();
                    int index = 0;
                    // OrderBy is stable: same-layer order exactly follows the original layout.
                    foreach (var p in layout.placements.OrderBy(p => p.layer))
                    {
                        RectTransform layer;
                        if (!layers.TryGetValue(p.layer, out layer))
                        {
                            layer = new GameObject("Layer_" + p.layer, typeof(RectTransform)).GetComponent<RectTransform>();
                            layer.SetParent(rect, false);
                            Place(layer, 0, 0, layout.canvas[0], layout.canvas[1]);
                            layers.Add(p.layer, layer);
                        }
                        var entry = entries[p.asset];
                        var go = new GameObject((index++).ToString("D4") + "_" + p.asset, typeof(RectTransform), typeof(Image));
                        go.transform.SetParent(layer, false);
                        var r = go.GetComponent<RectTransform>();
                        Place(r, p.x, p.y, entry.size[0], entry.size[1]);
                        var image = go.GetComponent<Image>();
                        image.sprite = sprites[p.asset];
                        image.raycastTarget = false;
                        if (p.flipX)
                        {
                            r.localScale = new Vector3(-1, 1, 1);
                            r.anchoredPosition += new Vector2(entry.size[0], 0);
                        }
                    }
                    // Prefab is a background panel only, with no Canvas, camera, scripts or input interception.
                    PrefabUtility.SaveAsPrefabAsset(panel, output + "/" + n + ".prefab");
                    if (!EditorSceneManager.SaveScene(scene, output + "/" + n + ".unity"))
                        throw new IOException("Could not save scene: " + n);
                }
                finally
                {
                    if (previous.IsValid() && previous.isLoaded) SceneManager.SetActiveScene(previous);
                    EditorSceneManager.CloseScene(scene, true);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Typecaster: built five background prefabs and five isolated preview scenes in " + output + ". Existing gameplay was not modified.");
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(output);
        }
        static void Place(RectTransform r, float x, float y, float width, float height)
        {
            r.anchorMin = r.anchorMax = r.pivot = new Vector2(0, 1);
            r.anchoredPosition = new Vector2(x, -y);
            r.sizeDelta = new Vector2(width, height);
        }
    }
}
#endif

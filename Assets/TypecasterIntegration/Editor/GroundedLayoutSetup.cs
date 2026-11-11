#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public static class GroundedLayoutSetup {
 public static void Build(){ReviewSkinBuilder.Open();Canvas.ForceUpdateCanvases();var p=Object.FindAnyObjectByType<Typecaster.Integration.CombatPresentation>();var tiles=Object.FindObjectsByType<Image>(FindObjectsSortMode.None).Where(i=>i.sprite&&i.sprite.name.StartsWith("floor_cobbles")).ToArray();if(tiles.Length==0)throw new System.Exception("Actual floor tiles not found");var tile=tiles.OrderByDescending(i=>Top(i.rectTransform)).First();var g=p.gameObject.GetComponent<GroundedActorLayout>()??p.gameObject.AddComponent<GroundedActorLayout>();g.floor=tile.rectTransform;g.hero=p.hero.rectTransform;g.enemy=p.enemy.rectTransform;g.Align();EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());}
 static float Top(RectTransform r){var c=new Vector3[4];r.GetWorldCorners(c);return c[1].y;}
}
#endif

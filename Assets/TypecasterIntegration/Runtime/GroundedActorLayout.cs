using UnityEngine;
// Keeps the visible feet (three transparent bottom pixels in both 32x48 source sprites)
// on the actual city floor tile, independent of viewport aspect ratio. Visuals only.
public class GroundedActorLayout : MonoBehaviour {
 public RectTransform floor,hero,enemy;
 void LateUpdate(){Align();}
 public void Align(){if(!floor)return;var f=new Vector3[4];floor.GetWorldCorners(f);float ground=f[1].y;foreach(var r in new[]{hero,enemy}){if(!r)continue;var c=new Vector3[4];r.GetWorldCorners(c);float visibleFoot=c[0].y+(c[1].y-c[0].y)*3f/48f;var p=r.position;p.y+=ground-visibleFoot;r.position=p;}}
}

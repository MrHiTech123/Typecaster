using UnityEngine;
using UnityEngine.UI;
// Mirrors the original text renderer's existing nonempty red error span. No input or game-state writes.
public class TypingErrorArt : MonoBehaviour {
 public Sprite[] frames;
 TypingTestBox box;Image border;
 public bool IsShowingError => border && border.enabled;
 void LateUpdate(){var current=Object.FindAnyObjectByType<TypingTestBox>();if(current!=box){Clear();box=current;if(box){var g=new GameObject("ExistingTypingErrorArt",typeof(RectTransform),typeof(Image));g.transform.SetParent(box.BoxForWhatPlayerTyped.transform,false);border=g.GetComponent<Image>();border.raycastTarget=false;var r=border.rectTransform;r.anchorMin=Vector2.zero;r.anchorMax=Vector2.one;r.offsetMin=new Vector2(-6,-3);r.offsetMax=new Vector2(6,3);}}
  if(!box||!border)return;
  var text=box.BoxForWhatPlayerTyped.text;const string open="<color=\"red\">";int begin=text.IndexOf(open,System.StringComparison.Ordinal);int end=begin<0?-1:text.IndexOf("</color>",begin,System.StringComparison.Ordinal);
  border.enabled=frames!=null&&frames.Length>0&&begin>=0&&end>begin+open.Length;
    if(border.enabled){border.sprite=frames[(int)(Time.unscaledTime*10)%frames.Length];var tmp=box.BoxForWhatPlayerTyped;tmp.ForceMeshUpdate();int index=begin+(end-begin-open.Length)-1;if(index>=0&&index<tmp.textInfo.characterCount){var ch=tmp.textInfo.characterInfo[index];var r=border.rectTransform;r.anchorMin=r.anchorMax=tmp.rectTransform.pivot;r.pivot=new Vector2(.5f,.5f);r.anchoredPosition=(ch.bottomLeft+ch.topRight)*.5f;r.sizeDelta=new Vector2(Mathf.Max(18,ch.topRight.x-ch.bottomLeft.x+8),Mathf.Max(20,ch.topRight.y-ch.bottomLeft.y+8));}}
 }
 void Clear(){if(border)Destroy(border.gameObject);border=null;}
 void OnDisable(){Clear();box=null;}
}


using UnityEngine;
using UnityEngine.UI;
using TMPro;
// Presentation only: skins the ORIGINAL live typing background and its two TMP text objects.
// No replacement input, event listeners, typed-string writes, timers or gameplay changes.
public class TypingArtFrame : MonoBehaviour {
 public Sprite book; public Sprite runes; // Legacy references retained for review, no longer instantiate decoration.
 public Sprite inputPanel;
 TypingTestBox shown; Image bg; Sprite oldSprite;Color oldBgColor;Image.Type oldType;RectState bgState;
 TextState promptState,typedState;
 class RectState {public Vector2 min,max,pivot,pos,size;public RectState(RectTransform r){min=r.anchorMin;max=r.anchorMax;pivot=r.pivot;pos=r.anchoredPosition;size=r.sizeDelta;}public void Restore(RectTransform r){r.anchorMin=min;r.anchorMax=max;r.pivot=pivot;r.anchoredPosition=pos;r.sizeDelta=size;}}
 class TextState {TMP_Text t;RectState r;float size,lineSpace;Color color;TextAlignmentOptions align;bool auto;public TextState(TMP_Text x){t=x;r=new RectState(t.rectTransform);size=t.fontSize;lineSpace=t.lineSpacing;color=t.color;align=t.alignment;auto=t.enableAutoSizing;}public void Restore(){if(!t)return;r.Restore(t.rectTransform);t.fontSize=size;t.lineSpacing=lineSpace;t.color=color;t.alignment=align;t.enableAutoSizing=auto;}}
 void LateUpdate(){var active=Object.FindAnyObjectByType<TypingTestBox>();if(active==shown)return;Restore();shown=active;if(!shown||!inputPanel)return;
  foreach(var i in shown.GetComponentsInChildren<Image>())if(i.name=="BackGround")bg=i;
  if(!bg)return;
  oldSprite=bg.sprite;oldBgColor=bg.color;oldType=bg.type;bgState=new RectState(bg.rectTransform);
  bg.sprite=inputPanel;bg.type=Image.Type.Sliced;bg.color=Color.white;Area(bg.rectTransform,new Vector2(.315f,.27f),new Vector2(.685f,.64f));
  promptState=new TextState(shown.BoxForWhatToType);typedState=new TextState(shown.BoxForWhatPlayerTyped);
  Style(shown.BoxForWhatToType,new Vector2(.335f,.455f),new Vector2(.665f,.615f),new Color(.19f,.13f,.18f));
  Style(shown.BoxForWhatPlayerTyped,new Vector2(.335f,.295f),new Vector2(.665f,.425f),new Color(.22f,.34f,.31f));
 }
 static void Area(RectTransform r,Vector2 min,Vector2 max){r.anchorMin=min;r.anchorMax=max;r.pivot=new Vector2(.5f,.5f);r.offsetMin=r.offsetMax=Vector2.zero;}
 static void Style(TMP_Text t,Vector2 size,Vector2 pos,Color color){Area(t.rectTransform,size,pos);t.enableAutoSizing=false;t.fontSize=14;t.lineSpacing=0;t.color=color;t.alignment=TextAlignmentOptions.TopLeft;}
 void Restore(){if(bg){bg.sprite=oldSprite;bg.color=oldBgColor;bg.type=oldType;bgState.Restore(bg.rectTransform);}promptState?.Restore();typedState?.Restore();bg=null;promptState=null;typedState=null;}
 void OnDisable(){Restore();shown=null;}
}




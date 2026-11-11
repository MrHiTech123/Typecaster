using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace Typecaster.Integration {
public sealed class CombatPresentation : MonoBehaviour {
 public Image hero, enemy, effect;
 public Sprite[] attack, cast, hurt, heal, projectile, impact, manaBlast, enemyHit;
 public float fps=8;
 IEnumerator Frames(Image image, Sprite[] frames, bool restore=true) {
  if(!image || frames==null || frames.Length==0) yield break;
  var original=image.sprite; var size=image.rectTransform.sizeDelta; bool visible=image.enabled;
  image.enabled=true;
  foreach(var frame in frames) { image.sprite=frame;
   if(image==hero && original) image.rectTransform.sizeDelta=new Vector2(size.x*frame.rect.width/original.rect.width,size.y*frame.rect.height/original.rect.height);
   yield return new WaitForSecondsRealtime(1/Mathf.Max(1,fps)); }
  if(restore) { image.sprite=original; image.rectTransform.sizeDelta=size; image.enabled=visible; }
 }
 public IEnumerator PlayManaBlast() { yield return Frames(hero,cast); if(effect && enemy) effect.rectTransform.position=enemy.rectTransform.position; yield return Frames(effect,manaBlast); }
 public IEnumerator PlayEnemyHit() { yield return Frames(enemy,enemyHit); }
 public IEnumerator PlayHeroAttack() { yield return Frames(hero,attack); }
 public IEnumerator PlayHeroHurt() { yield return Frames(hero,hurt); }
 public IEnumerator PlayHeal() { if(effect && hero) effect.rectTransform.position=hero.rectTransform.position; yield return Frames(effect,heal); }
 public IEnumerator PlayImpact() { if(effect && enemy) effect.rectTransform.position=enemy.rectTransform.position; yield return Frames(effect,impact); }
 public IEnumerator PlayFireball() {
  yield return Frames(hero,cast);
  if(!effect || !hero || !enemy || projectile.Length==0) yield break;
  effect.enabled=true;
  var from=hero.rectTransform.position; var to=enemy.rectTransform.position;
  for(int i=0;i<projectile.Length;i++) { effect.sprite=projectile[i]; effect.rectTransform.position=Vector3.Lerp(from,to,(i+1f)/projectile.Length); yield return new WaitForSecondsRealtime(1/Mathf.Max(1,fps)); }
  effect.enabled=false;
 }
 public IEnumerator PlayEnemyAttack() {
  // Preserve original training puppet, do not replace it with an unrelated enemy.
  if(!enemy) yield break;
  var p=enemy.rectTransform.anchoredPosition;
  enemy.rectTransform.anchoredPosition=p+Vector2.left*12;
  yield return new WaitForSecondsRealtime(.12f);
  enemy.rectTransform.anchoredPosition=p;
 }
}
}

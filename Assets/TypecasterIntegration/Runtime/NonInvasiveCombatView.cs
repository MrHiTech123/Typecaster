using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Typecaster.Integration {
// Presentation only: reads existing HP. Never writes gameplay state, calls an
// action, edits button listeners, changes timing, or intercepts input.
public sealed class NonInvasiveCombatView : MonoBehaviour {
 public CombatStatistics statistics;
 public CombatPresentation presentation;
 int player,enemy;bool initialized,running;
 readonly Queue<Vector2Int> changes=new Queue<Vector2Int>();
 void LateUpdate(){
  if(!statistics||!presentation)return;
  if(!initialized){player=statistics.playerHP;enemy=statistics.enemyHP;initialized=true;return;}
  var delta=new Vector2Int(player-statistics.playerHP,enemy-statistics.enemyHP);
  player=statistics.playerHP;enemy=statistics.enemyHP;
  if(delta!=Vector2Int.zero){changes.Enqueue(delta);if(!running)StartCoroutine(Feedback());}
 }
 IEnumerator Feedback(){running=true;
  while(changes.Count>0){var d=changes.Dequeue();
   if(d.y>0)yield return presentation.PlayEnemyHit();
   if(d.x>0)yield return presentation.PlayHeroHurt();else if(d.x<0)yield return presentation.PlayHeal();
  }running=false;
 }
 void OnDisable(){StopAllCoroutines();changes.Clear();running=false;initialized=false;}
}
}

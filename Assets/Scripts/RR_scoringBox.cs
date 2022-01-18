// Decompiled with JetBrains decompiler
// Type: RR_scoringBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class RR_scoringBox : MonoBehaviour
{
  public bool score_cubes = true;
  public bool score_balls = true;
  public List<gameElement> collisions = new List<gameElement>();

  private void Update()
  {
    float num = (float) (DateTime.Now.Ticks / 10000L % 2000L);
    if ((double) num > 1000.0)
      num = 2000f - num;
    foreach (gameElement collision in this.collisions)
    {
      Renderer component = collision.gameObject.GetComponent<Renderer>();
      Color color = component.material.color with
      {
        b = collision.type != ElementType.Cube ? (float) (0.300000011920929 * (double) num / 1000.0) : (float) (0.5 + 0.5 * (double) num / 1000.0)
      };
      component.material.color = color;
    }
  }

  private gameElement FindGameElement(Collider collision)
  {
    Transform transform = collision.transform;
    gameElement component;
    for (component = transform.GetComponent<gameElement>(); (UnityEngine.Object) component == (UnityEngine.Object) null && (UnityEngine.Object) transform.parent != (UnityEngine.Object) null; component = transform.GetComponent<gameElement>())
      transform = transform.parent;
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return (gameElement) null;
    return this.score_cubes && component.type == ElementType.Cube || this.score_balls && component.type == ElementType.Jewel ? component : (gameElement) null;
  }

  private void OnTriggerEnter(Collider collision)
  {
    gameElement gameElement = this.FindGameElement(collision);
    if ((UnityEngine.Object) gameElement == (UnityEngine.Object) null || this.collisions.Contains(gameElement))
      return;
    this.collisions.Add(gameElement);
  }

  private void OnTriggerExit(Collider collision)
  {
    gameElement gameElement = this.FindGameElement(collision);
    if ((UnityEngine.Object) gameElement == (UnityEngine.Object) null)
      return;
    if (this.collisions.Contains(gameElement))
      this.collisions.Remove(gameElement);
    gameElement.ResetColor();
  }

  public int GetElementCount() => this.collisions.Count;
}

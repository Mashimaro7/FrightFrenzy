// Decompiled with JetBrains decompiler
// Type: CubeScoring
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class CubeScoring : MonoBehaviour
{
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
    return component.type != ElementType.Cube && component.type != ElementType.CubeDark ? (gameElement) null : component;
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

  public bool IsLightCube()
  {
    foreach (gameElement collision in this.collisions)
    {
      if (collision.type == ElementType.Cube)
        return true;
    }
    return false;
  }

  public bool IsDarkCube()
  {
    foreach (gameElement collision in this.collisions)
    {
      if (collision.type == ElementType.CubeDark)
        return true;
    }
    return false;
  }

  public gameElement IsCube(gameElement previous_cube)
  {
    if (this.collisions.Count == 0)
      return (gameElement) null;
    if ((UnityEngine.Object) this.collisions[0] != (UnityEngine.Object) previous_cube)
      return this.collisions[0];
    return this.collisions.Count > 1 ? this.collisions[1] : (gameElement) null;
  }
}

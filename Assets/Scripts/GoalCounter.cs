// Decompiled with JetBrains decompiler
// Type: GoalCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GoalCounter : MonoBehaviour
{
  public List<GoalScorer> collisions = new List<GoalScorer>();
  public List<int> collisions_count = new List<int>();
  public bool isRed;
  public bool isBlue;

  private void Update()
  {
    int num = GLOBALS.CLIENT_MODE ? 1 : 0;
  }

  private GoalScorer FindGameElement(Collider collision)
  {
    Transform transform = collision.transform;
    GoalScorer component;
    for (component = transform.GetComponent<GoalScorer>(); (Object) component == (Object) null && (Object) transform.parent != (Object) null; component = transform.GetComponent<GoalScorer>())
      transform = transform.parent;
    return component;
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    GoalScorer gameElement = this.FindGameElement(collision);
    if (!(bool) (Object) gameElement || gameElement.isRed && this.isBlue || gameElement.isBlue && this.isRed)
      return;
    if (this.collisions.Contains(gameElement))
    {
      ++this.collisions_count[this.collisions.IndexOf(gameElement)];
    }
    else
    {
      this.collisions.Add(gameElement);
      this.collisions_count.Add(1);
    }
  }

  private void OnTriggerExit(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    GoalScorer gameElement = this.FindGameElement(collision);
    if (!(bool) (Object) gameElement || !this.collisions.Contains(gameElement))
      return;
    int index = this.collisions.IndexOf(gameElement);
    --this.collisions_count[index];
    if (this.collisions_count[index] > 0)
      return;
    this.collisions_count.RemoveAt(index);
    this.collisions.RemoveAt(index);
  }

  public int GetGoalCount() => this.collisions.Count;

  public bool IsGoalInside(GoalScorer item) => this.collisions.Contains(item);

  private void RemoveInvalidItems()
  {
    for (int index = this.collisions.Count - 1; index >= 0; --index)
    {
      if ((Object) this.collisions[index] == (Object) null)
      {
        this.collisions.RemoveAt(index);
        this.collisions_count.RemoveAt(index);
      }
    }
  }
}

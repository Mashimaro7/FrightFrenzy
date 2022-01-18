// Decompiled with JetBrains decompiler
// Type: CU_scoringBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class CU_scoringBox : MonoBehaviour
{
  private ChangeUp_Settings cu_settings;
  public Light our_light;
  public List<gameElement> collisions = new List<gameElement>();
  public List<int> collisions_count = new List<int>();

  private void Start()
  {
    if (!(bool) (Object) GameObject.Find("GameSettings"))
      return;
    this.cu_settings = GameObject.Find("GameSettings").GetComponent<ChangeUp_Settings>();
  }

  public void Reset()
  {
  }

  private void Update()
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    this.SetHighestBall(this.GetHighestBall());
  }

  private gameElement FindGameElement(Collider collision)
  {
    Transform transform = collision.transform;
    gameElement component;
    for (component = transform.GetComponent<gameElement>(); (Object) component == (Object) null && (Object) transform.parent != (Object) null; component = transform.GetComponent<gameElement>())
      transform = transform.parent;
    if ((Object) component == (Object) null)
      return (gameElement) null;
    return component.type == ElementType.Cube || component.type == ElementType.CubeDark ? component : (gameElement) null;
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    this.RemoveInvalidItems();
    gameElement gameElement = this.FindGameElement(collision);
    if ((Object) gameElement == (Object) null)
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
    gameElement gameElement = this.FindGameElement(collision);
    if ((Object) gameElement == (Object) null || !this.collisions.Contains(gameElement))
      return;
    int index = this.collisions.IndexOf(gameElement);
    --this.collisions_count[index];
    if (this.collisions_count[index] > 0)
      return;
    this.collisions_count.RemoveAt(index);
    this.collisions.RemoveAt(index);
  }

  public int GetElementCount() => this.collisions.Count;

  public bool IsGameElementInside(gameElement item) => this.collisions.Contains(item);

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

  public int GetHighestBall()
  {
    gameElement gameElement = (gameElement) null;
    foreach (gameElement collision in this.collisions)
    {
      if ((bool) (Object) collision.transform && (double) collision.transform.position.y <= 1.55999994277954)
      {
        if (!(bool) (Object) gameElement)
          gameElement = collision;
        else if ((bool) (Object) gameElement.transform && (double) collision.transform.position.y > (double) gameElement.transform.position.y)
          gameElement = collision;
      }
    }
    if (!(bool) (Object) gameElement)
      return 0;
    return gameElement.type == ElementType.CubeDark ? 1 : 2;
  }

  public void SetHighestBall(int top_element)
  {
    if ((Object) this.cu_settings == (Object) null)
    {
      this.cu_settings = GameObject.Find("GameSettings").GetComponent<ChangeUp_Settings>();
      if ((Object) this.cu_settings == (Object) null)
        return;
    }
    if (!(bool) (Object) this.our_light)
      return;
    if (!this.cu_settings.ENABLE_LIGHTS)
    {
      this.our_light.enabled = false;
    }
    else
    {
      this.our_light.enabled = true;
      if (top_element == 1)
        this.our_light.color = Color.red;
      else if (top_element == 2)
        this.our_light.color = Color.blue;
      else
        this.our_light.color = Color.grey;
    }
  }

  public bool IsTopElementBlue() => this.GetHighestBall() == 2;

  public bool IsTopElementRed() => this.GetHighestBall() == 1;

  public int GetBlueCount()
  {
    int blueCount = 0;
    foreach (gameElement collision in this.collisions)
    {
      if (collision.type == ElementType.Cube)
        ++blueCount;
    }
    return blueCount;
  }

  public int GetRedCount()
  {
    int redCount = 0;
    foreach (gameElement collision in this.collisions)
    {
      if (collision.type == ElementType.CubeDark)
        ++redCount;
    }
    return redCount;
  }
}

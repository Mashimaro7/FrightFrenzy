// Decompiled with JetBrains decompiler
// Type: DuckChecker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DuckChecker : MonoBehaviour
{
  public ElementType filter_element = ElementType.Off;
  public DuckStates.DuckPositions entry_state = DuckStates.DuckPositions.Placed;
  public DuckStates.DuckPositions exit_state = DuckStates.DuckPositions.Touched;
  public int processed_counter;

  private void Start()
  {
  }

  public void Reset() => this.processed_counter = 0;

  private void Update()
  {
    int num = GLOBALS.CLIENT_MODE ? 1 : 0;
  }

  private gameElement FindGameElement(Collider collision)
  {
    Transform transform = collision.transform;
    gameElement component;
    for (component = transform.GetComponent<gameElement>(); (Object) component == (Object) null && (Object) transform.parent != (Object) null; component = transform.GetComponent<gameElement>())
      transform = transform.parent;
    if ((Object) component == (Object) null)
      return (gameElement) null;
    return this.filter_element == ElementType.Off || component.type == this.filter_element ? component : (gameElement) null;
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    gameElement gameElement = this.FindGameElement(collision);
    if (!(bool) (Object) gameElement)
      return;
    DuckStates component = gameElement.GetComponent<DuckStates>();
    if (!(bool) (Object) component || component.mystate != this.entry_state)
      return;
    component.mystate = this.exit_state;
    ++this.processed_counter;
  }
}

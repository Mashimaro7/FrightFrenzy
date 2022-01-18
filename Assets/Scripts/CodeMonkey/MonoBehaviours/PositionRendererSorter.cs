// Decompiled with JetBrains decompiler
// Type: CodeMonkey.MonoBehaviours.PositionRendererSorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace CodeMonkey.MonoBehaviours
{
  public class PositionRendererSorter : MonoBehaviour
  {
    [SerializeField]
    private int sortingOrderBase = 5000;
    [SerializeField]
    private int offset;
    [SerializeField]
    private bool runOnlyOnce;
    private float timer;
    private float timerMax = 0.1f;
    private Renderer myRenderer;

    private void Awake() => this.myRenderer = this.gameObject.GetComponent<Renderer>();

    private void LateUpdate()
    {
      this.timer -= Time.deltaTime;
      if ((double) this.timer > 0.0)
        return;
      this.timer = this.timerMax;
      this.myRenderer.sortingOrder = (int) ((double) this.sortingOrderBase - (double) this.transform.position.y - (double) this.offset);
      if (!this.runOnlyOnce)
        return;
      Object.Destroy((Object) this);
    }

    public void SetOffset(int offset) => this.offset = offset;
  }
}

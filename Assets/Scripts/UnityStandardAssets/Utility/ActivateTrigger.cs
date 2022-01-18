// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.ActivateTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Utility
{
  public class ActivateTrigger : MonoBehaviour
  {
    public ActivateTrigger.Mode action = ActivateTrigger.Mode.Activate;
    public Object target;
    public GameObject source;
    public int triggerCount = 1;
    public bool repeatTrigger;

    private void DoActivateTrigger()
    {
      --this.triggerCount;
      if (this.triggerCount != 0 && !this.repeatTrigger)
        return;
      Object @object = this.target;
      if ((object) @object == null)
        @object = (Object) this.gameObject;
      Behaviour behaviour = @object as Behaviour;
      GameObject gameObject = @object as GameObject;
      if ((Object) behaviour != (Object) null)
        gameObject = behaviour.gameObject;
      switch (this.action)
      {
        case ActivateTrigger.Mode.Trigger:
          if (!((Object) gameObject != (Object) null))
            break;
          gameObject.BroadcastMessage(nameof (DoActivateTrigger));
          break;
        case ActivateTrigger.Mode.Replace:
          if (!((Object) this.source != (Object) null) || !((Object) gameObject != (Object) null))
            break;
          Object.Instantiate<GameObject>(this.source, gameObject.transform.position, gameObject.transform.rotation);
          Object.DestroyObject((Object) gameObject);
          break;
        case ActivateTrigger.Mode.Activate:
          if (!((Object) gameObject != (Object) null))
            break;
          gameObject.SetActive(true);
          break;
        case ActivateTrigger.Mode.Enable:
          if (!((Object) behaviour != (Object) null))
            break;
          behaviour.enabled = true;
          break;
        case ActivateTrigger.Mode.Animate:
          if (!((Object) gameObject != (Object) null))
            break;
          gameObject.GetComponent<Animation>().Play();
          break;
        case ActivateTrigger.Mode.Deactivate:
          if (!((Object) gameObject != (Object) null))
            break;
          gameObject.SetActive(false);
          break;
      }
    }

    private void OnTriggerEnter(Collider other) => this.DoActivateTrigger();

    public enum Mode
    {
      Trigger,
      Replace,
      Activate,
      Enable,
      Animate,
      Deactivate,
    }
  }
}

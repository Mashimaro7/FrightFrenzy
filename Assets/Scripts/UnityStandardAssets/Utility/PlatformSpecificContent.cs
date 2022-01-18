// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.PlatformSpecificContent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Utility
{
  public class PlatformSpecificContent : MonoBehaviour
  {
    [SerializeField]
    private PlatformSpecificContent.BuildTargetGroup m_BuildTargetGroup;
    [SerializeField]
    private GameObject[] m_Content = new GameObject[0];
    [SerializeField]
    private MonoBehaviour[] m_MonoBehaviours = new MonoBehaviour[0];
    [SerializeField]
    private bool m_ChildrenOfThisObject;

    private void OnEnable() => this.CheckEnableContent();

    private void CheckEnableContent()
    {
      if (this.m_BuildTargetGroup == PlatformSpecificContent.BuildTargetGroup.Mobile)
        this.EnableContent(false);
      else
        this.EnableContent(true);
    }

    private void EnableContent(bool enabled)
    {
      if (this.m_Content.Length != 0)
      {
        foreach (GameObject gameObject in this.m_Content)
        {
          if ((Object) gameObject != (Object) null)
            gameObject.SetActive(enabled);
        }
      }
      if (this.m_ChildrenOfThisObject)
      {
        foreach (Component component in this.transform)
          component.gameObject.SetActive(enabled);
      }
      if (this.m_MonoBehaviours.Length == 0)
        return;
      foreach (Behaviour monoBehaviour in this.m_MonoBehaviours)
        monoBehaviour.enabled = enabled;
    }

    private enum BuildTargetGroup
    {
      Standalone,
      Mobile,
    }
  }
}

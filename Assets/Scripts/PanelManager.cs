// Decompiled with JetBrains decompiler
// Type: PanelManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
  public AudioManager myAudioManager;
  public Animator initiallyOpen;
  private int m_OpenParameterId;
  private Animator m_Open;
  private GameObject m_PreviouslySelected;
  private const string k_OpenTransitionName = "Open";
  private const string k_ClosedStateName = "Closed";

  public void OnEnable()
  {
    this.m_OpenParameterId = Animator.StringToHash("Open");
    if ((Object) this.initiallyOpen == (Object) null)
      return;
    this.OpenPanel(this.initiallyOpen);
  }

  public void OpenPanel(Animator anim)
  {
    try
    {
      this.myAudioManager.GetComponent<AudioManager>().Play("click");
    }
    catch
    {
    }
    if ((Object) this.m_Open == (Object) anim)
      return;
    anim.gameObject.SetActive(true);
    GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;
    anim.transform.SetAsLastSibling();
    this.CloseCurrent();
    this.m_PreviouslySelected = selectedGameObject;
    this.m_Open = anim;
    this.m_Open.SetBool(this.m_OpenParameterId, true);
    this.SetSelected(PanelManager.FindFirstEnabledSelectable(anim.gameObject));
  }

  private static GameObject FindFirstEnabledSelectable(GameObject gameObject)
  {
    GameObject enabledSelectable = (GameObject) null;
    foreach (Selectable componentsInChild in gameObject.GetComponentsInChildren<Selectable>(true))
    {
      if (componentsInChild.IsActive() && componentsInChild.IsInteractable())
      {
        enabledSelectable = componentsInChild.gameObject;
        break;
      }
    }
    return enabledSelectable;
  }

  public void CloseCurrent()
  {
    try
    {
      this.myAudioManager.GetComponent<AudioManager>().Play("click");
    }
    catch
    {
    }
    if ((Object) this.m_Open == (Object) null)
      return;
    this.m_Open.SetBool(this.m_OpenParameterId, false);
    this.SetSelected(this.m_PreviouslySelected);
    this.StartCoroutine(this.DisablePanelDeleyed(this.m_Open));
    this.m_Open = (Animator) null;
  }

  private IEnumerator DisablePanelDeleyed(Animator anim)
  {
    bool closedStateReached = false;
    bool wantToClose = true;
    while (!closedStateReached & wantToClose)
    {
      if (!anim.IsInTransition(0))
        closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName("Closed");
      wantToClose = !anim.GetBool(this.m_OpenParameterId);
      yield return (object) new WaitForEndOfFrame();
    }
    if (wantToClose)
      anim.gameObject.SetActive(false);
  }

  private void SetSelected(GameObject go)
  {
    try
    {
      this.myAudioManager.GetComponent<AudioManager>().Play("click");
    }
    catch
    {
    }
    EventSystem.current.SetSelectedGameObject(go);
  }
}

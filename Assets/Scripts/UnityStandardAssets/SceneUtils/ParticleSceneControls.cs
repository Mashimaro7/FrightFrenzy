// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.SceneUtils.ParticleSceneControls
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.Effects;

namespace UnityStandardAssets.SceneUtils
{
  public class ParticleSceneControls : MonoBehaviour
  {
    public ParticleSceneControls.DemoParticleSystemList demoParticles;
    public float spawnOffset = 0.5f;
    public float multiply = 1f;
    public bool clearOnChange;
    public Text titleText;
    public Transform sceneCamera;
    public Text instructionText;
    public Button previousButton;
    public Button nextButton;
    public GraphicRaycaster graphicRaycaster;
    public EventSystem eventSystem;
    private ParticleSystemMultiplier m_ParticleMultiplier;
    private List<Transform> m_CurrentParticleList = new List<Transform>();
    private Transform m_Instance;
    private static int s_SelectedIndex;
    private Vector3 m_CamOffsetVelocity = Vector3.zero;
    private Vector3 m_LastPos;
    private static ParticleSceneControls.DemoParticleSystem s_Selected;

    private void Awake()
    {
      this.Select(ParticleSceneControls.s_SelectedIndex);
      this.previousButton.onClick.AddListener(new UnityAction(this.Previous));
      this.nextButton.onClick.AddListener(new UnityAction(this.Next));
    }

    private void OnDisable()
    {
      this.previousButton.onClick.RemoveListener(new UnityAction(this.Previous));
      this.nextButton.onClick.RemoveListener(new UnityAction(this.Next));
    }

    private void Previous()
    {
      --ParticleSceneControls.s_SelectedIndex;
      if (ParticleSceneControls.s_SelectedIndex == -1)
        ParticleSceneControls.s_SelectedIndex = this.demoParticles.items.Length - 1;
      this.Select(ParticleSceneControls.s_SelectedIndex);
    }

    public void Next()
    {
      ++ParticleSceneControls.s_SelectedIndex;
      if (ParticleSceneControls.s_SelectedIndex == this.demoParticles.items.Length)
        ParticleSceneControls.s_SelectedIndex = 0;
      this.Select(ParticleSceneControls.s_SelectedIndex);
    }

    private void Update()
    {
      this.KeyboardInput();
      this.sceneCamera.localPosition = Vector3.SmoothDamp(this.sceneCamera.localPosition, Vector3.forward * (float) -ParticleSceneControls.s_Selected.camOffset, ref this.m_CamOffsetVelocity, 1f);
      RaycastHit hitInfo;
      if (ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Activate || this.CheckForGuiCollision() || ((!Input.GetMouseButtonDown(0) ? 0 : (ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Instantiate ? 1 : 0)) | (!Input.GetMouseButton(0) ? (false ? 1 : 0) : (ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Trail ? 1 : 0))) == 0 || !Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
        return;
      Quaternion rotation = Quaternion.LookRotation(hitInfo.normal);
      if (ParticleSceneControls.s_Selected.align == ParticleSceneControls.AlignMode.Up)
        rotation = Quaternion.identity;
      Vector3 position = hitInfo.point + hitInfo.normal * this.spawnOffset;
      if ((double) (position - this.m_LastPos).magnitude <= (double) ParticleSceneControls.s_Selected.minDist)
        return;
      if (ParticleSceneControls.s_Selected.mode != ParticleSceneControls.Mode.Trail || (UnityEngine.Object) this.m_Instance == (UnityEngine.Object) null)
      {
        this.m_Instance = UnityEngine.Object.Instantiate<Transform>(ParticleSceneControls.s_Selected.transform, position, rotation);
        if ((UnityEngine.Object) this.m_ParticleMultiplier != (UnityEngine.Object) null)
          this.m_Instance.GetComponent<ParticleSystemMultiplier>().multiplier = this.multiply;
        this.m_CurrentParticleList.Add(this.m_Instance);
        if (ParticleSceneControls.s_Selected.maxCount > 0 && this.m_CurrentParticleList.Count > ParticleSceneControls.s_Selected.maxCount)
        {
          if ((UnityEngine.Object) this.m_CurrentParticleList[0] != (UnityEngine.Object) null)
            UnityEngine.Object.Destroy((UnityEngine.Object) this.m_CurrentParticleList[0].gameObject);
          this.m_CurrentParticleList.RemoveAt(0);
        }
      }
      else
      {
        this.m_Instance.position = position;
        this.m_Instance.rotation = rotation;
      }
      if (ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Trail)
      {
        this.m_Instance.transform.GetComponent<ParticleSystem>().emission.enabled = false;
        this.m_Instance.transform.GetComponent<ParticleSystem>().Emit(1);
      }
      this.m_Instance.parent = hitInfo.transform;
      this.m_LastPos = position;
    }

    private void KeyboardInput()
    {
      if (Input.GetKeyDown(KeyCode.LeftArrow))
        this.Previous();
      if (!Input.GetKeyDown(KeyCode.RightArrow))
        return;
      this.Next();
    }

    private bool CheckForGuiCollision()
    {
      PointerEventData eventData = new PointerEventData(this.eventSystem);
      eventData.pressPosition = (Vector2) Input.mousePosition;
      eventData.position = (Vector2) Input.mousePosition;
      List<RaycastResult> resultAppendList = new List<RaycastResult>();
      this.graphicRaycaster.Raycast(eventData, resultAppendList);
      return resultAppendList.Count > 0;
    }

    private void Select(int i)
    {
      ParticleSceneControls.s_Selected = this.demoParticles.items[i];
      this.m_Instance = (Transform) null;
      foreach (ParticleSceneControls.DemoParticleSystem demoParticleSystem in this.demoParticles.items)
      {
        if (demoParticleSystem != ParticleSceneControls.s_Selected && demoParticleSystem.mode == ParticleSceneControls.Mode.Activate)
          demoParticleSystem.transform.gameObject.SetActive(false);
      }
      if (ParticleSceneControls.s_Selected.mode == ParticleSceneControls.Mode.Activate)
        ParticleSceneControls.s_Selected.transform.gameObject.SetActive(true);
      this.m_ParticleMultiplier = ParticleSceneControls.s_Selected.transform.GetComponent<ParticleSystemMultiplier>();
      this.multiply = 1f;
      if (this.clearOnChange)
      {
        while (this.m_CurrentParticleList.Count > 0)
        {
          UnityEngine.Object.Destroy((UnityEngine.Object) this.m_CurrentParticleList[0].gameObject);
          this.m_CurrentParticleList.RemoveAt(0);
        }
      }
      this.instructionText.text = ParticleSceneControls.s_Selected.instructionText;
      this.titleText.text = ParticleSceneControls.s_Selected.transform.name;
    }

    public enum Mode
    {
      Activate,
      Instantiate,
      Trail,
    }

    public enum AlignMode
    {
      Normal,
      Up,
    }

    [Serializable]
    public class DemoParticleSystem
    {
      public Transform transform;
      public ParticleSceneControls.Mode mode;
      public ParticleSceneControls.AlignMode align;
      public int maxCount;
      public float minDist;
      public int camOffset = 15;
      public string instructionText;
    }

    [Serializable]
    public class DemoParticleSystemList
    {
      public ParticleSceneControls.DemoParticleSystem[] items;
    }
  }
}

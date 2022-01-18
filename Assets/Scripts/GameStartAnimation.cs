// Decompiled with JetBrains decompiler
// Type: GameStartAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class GameStartAnimation : MonoBehaviour
{
  public Transform[] robot_red;
  public Transform[] robot_blue;
  public TextMeshPro[] text_r;
  public TextMeshPro[] text_b;
  public GameObject canvas_text;
  public Animator myanimator;

  private void Awake() => this.myanimator = this.GetComponent<Animator>();

  public void OnEnable()
  {
    if (!this.gameObject.activeSelf)
      return;
    this.OnDisable();
    int index1 = 0;
    int index2 = 0;
    foreach (ClientLow.Player player in GLOBALS.topclient.players.Values)
    {
      if (!player.isSpectator)
      {
        if (player.isRed && index1 < this.robot_red.Length)
        {
          GameObject gameObject = MyUtils.InstantiateRobot(player.model, this.robot_red[index1].transform.position, this.robot_red[index1].transform.rotation, player.skins, player.robot_skins);
          foreach (BandwidthHelper componentsInChild in gameObject.GetComponentsInChildren<BandwidthHelper>())
            componentsInChild.pauseUpdates = true;
          RobotInterface3D component = gameObject.GetComponent<RobotInterface3D>();
          component.SetName(player.playerName);
          component.SetKinematic();
          component.isSpectator = true;
          component.SetColorFromPosition(player.position);
          gameObject.transform.SetParent(this.robot_red[index1].parent);
          this.text_r[index1].gameObject.SetActive(true);
          this.text_r[index1].text = player.playerName;
          ++index1;
        }
        if (!player.isRed && index2 < this.robot_blue.Length)
        {
          GameObject gameObject = MyUtils.InstantiateRobot(player.model, this.robot_blue[index2].transform.position, this.robot_blue[index2].transform.rotation, player.skins, player.robot_skins);
          foreach (BandwidthHelper componentsInChild in gameObject.GetComponentsInChildren<BandwidthHelper>())
            componentsInChild.pauseUpdates = true;
          RobotInterface3D component = gameObject.GetComponent<RobotInterface3D>();
          component.SetName(player.playerName);
          component.SetKinematic();
          component.isSpectator = true;
          component.SetColorFromPosition(player.position);
          gameObject.transform.SetParent(this.robot_blue[index2].parent);
          this.text_b[index2].gameObject.SetActive(true);
          this.text_b[index2].text = player.playerName;
          ++index2;
        }
      }
    }
    if (!(bool) (Object) this.canvas_text)
      return;
    this.canvas_text.SetActive(true);
  }

  private void OnDisable()
  {
    foreach (Component componentsInChild in this.GetComponentsInChildren<RobotInterface3D>())
      Object.Destroy((Object) componentsInChild.gameObject);
    foreach (Component component in this.text_r)
      component.gameObject.SetActive(false);
    foreach (Component component in this.text_b)
      component.gameObject.SetActive(false);
    if (!(bool) (Object) this.canvas_text)
      return;
    this.canvas_text.SetActive(false);
  }

  public void TurnOff() => this.gameObject.SetActive(false);

  public void EndAnimation()
  {
    if (!(bool) (Object) this.myanimator || !this.myanimator.enabled || QualitySettings.GetQualityLevel() < 1)
    {
      this.TurnOff();
    }
    else
    {
      if (this.myanimator.GetCurrentAnimatorStateInfo(0).IsName("Close"))
        return;
      this.myanimator.Play("Close");
    }
  }

  public void ShowNoAnimation(bool state)
  {
    if (!state)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      if (this.gameObject.activeSelf)
        return;
      this.gameObject.SetActive(true);
      this.myanimator.Play("OpenNoAnimation");
    }
  }
}

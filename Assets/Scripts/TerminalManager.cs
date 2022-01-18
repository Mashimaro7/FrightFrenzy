// Decompiled with JetBrains decompiler
// Type: TerminalManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class TerminalManager : MonoBehaviour
{
  public List<GenericFieldTracker> robot_detectors = new List<GenericFieldTracker>();
  public List<GenericFieldTracker> ball_outs = new List<GenericFieldTracker>();
  public GenericFieldTracker inside_detector;

  private void Start()
  {
  }

  private void Update()
  {
    if (this.inside_detector.GetGameElementCount() < 1)
      return;
    for (int index = 0; index < this.robot_detectors.Count; ++index)
    {
      if (this.robot_detectors[index].IsAnyRobotInside())
        this.DeployBall(index);
    }
  }

  private void DeployBall(int index)
  {
    if (this.robot_detectors[index].robots.Count <= 0 || this.ball_outs[index].IsAnyGameElementInside())
      return;
    RobotID robot = this.robot_detectors[index].robots[0];
    gameElement gameElement1 = (gameElement) null;
    foreach (gameElement gameElement2 in this.inside_detector.game_elements)
    {
      if (gameElement2.type == ElementType.Blue1 && !robot.is_red || gameElement2.type == ElementType.Red1 && robot.is_red)
      {
        gameElement1 = gameElement2;
        break;
      }
    }
    if ((Object) gameElement1 == (Object) null)
      return;
    gameElement1.transform.position = this.ball_outs[index].transform.position;
    if (!(bool) (Object) gameElement1.GetComponent<Rigidbody>())
      return;
    Rigidbody component = gameElement1.GetComponent<Rigidbody>();
    component.velocity = Vector3.zero;
    component.angularVelocity = Vector3.zero;
  }
}

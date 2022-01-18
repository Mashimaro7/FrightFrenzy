// Decompiled with JetBrains decompiler
// Type: FTC_Simulator.Assets.Scripts.WheelsDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace FTC_Simulator.Assets.Scripts
{
  public class WheelsDisplay : RobotSkin
  {
    public Transform WheelTL;
    public Transform WheelTR;
    public Transform WheelBL;
    public Transform WheelBR;
    public Transform TWheelTL;
    public Transform TWheelTR;
    public Transform TWheelBL;
    public Transform TWheelBR;
    public Transform TWheelML;
    public Transform TWheelMR;
    private string my_DriveTrain = "";
    private float lastBodyAngleRad;
    private Vector3 lastBodyPosition;
    public float deltaTL;
    public float deltaTR;
    public float deltaBL;
    public float deltaBR;

    public override void InitSkin()
    {
      base.InitSkin();
      this.WheelTL = MyUtils.FindHierarchy(this.transform, "MECWheelTL");
      this.WheelTR = MyUtils.FindHierarchy(this.transform, "MECWheelTR");
      this.WheelBL = MyUtils.FindHierarchy(this.transform, "MECWheelBL");
      this.WheelBR = MyUtils.FindHierarchy(this.transform, "MECWheelBR");
      this.TWheelTL = MyUtils.FindHierarchy(this.transform, "TANKWheelTL");
      this.TWheelTR = MyUtils.FindHierarchy(this.transform, "TANKWheelTR");
      this.TWheelBL = MyUtils.FindHierarchy(this.transform, "TANKWheelBL");
      this.TWheelBR = MyUtils.FindHierarchy(this.transform, "TANKWheelBR");
      this.TWheelML = MyUtils.FindHierarchy(this.transform, "TANKWheelML");
      this.TWheelMR = MyUtils.FindHierarchy(this.transform, "TANKWheelMR");
      this.UpdateDriveTrain("Tank");
    }

    private void UpdateDriveTrain(string new_DriveTrain)
    {
      if (new_DriveTrain == this.my_DriveTrain)
        return;
      this.my_DriveTrain = new_DriveTrain;
      if (new_DriveTrain == "6-Wheel Tank")
      {
        if ((bool) (UnityEngine.Object) this.WheelBL)
          this.WheelBL.gameObject.SetActive(false);
        if ((bool) (UnityEngine.Object) this.WheelBR)
          this.WheelBR.gameObject.SetActive(false);
        if ((bool) (UnityEngine.Object) this.WheelTL)
          this.WheelTL.gameObject.SetActive(false);
        if ((bool) (UnityEngine.Object) this.WheelTR)
          this.WheelTR.gameObject.SetActive(false);
        if ((bool) (UnityEngine.Object) this.TWheelBL)
          this.TWheelBL.gameObject.SetActive(true);
        else
          this.WheelBL.gameObject.SetActive(true);
        if ((bool) (UnityEngine.Object) this.TWheelBR)
          this.TWheelBR.gameObject.SetActive(true);
        else
          this.WheelBR.gameObject.SetActive(true);
        if ((bool) (UnityEngine.Object) this.TWheelTL)
          this.TWheelTL.gameObject.SetActive(true);
        else
          this.WheelTL.gameObject.SetActive(true);
        if ((bool) (UnityEngine.Object) this.TWheelTR)
          this.TWheelTR.gameObject.SetActive(true);
        else
          this.WheelTR.gameObject.SetActive(true);
        if ((bool) (UnityEngine.Object) this.TWheelML)
          this.TWheelML.gameObject.SetActive(true);
        if (!(bool) (UnityEngine.Object) this.TWheelMR)
          return;
        this.TWheelMR.gameObject.SetActive(true);
      }
      else
      {
        if ((bool) (UnityEngine.Object) this.WheelBL)
          this.WheelBL.gameObject.SetActive(true);
        if ((bool) (UnityEngine.Object) this.WheelBR)
          this.WheelBR.gameObject.SetActive(true);
        if ((bool) (UnityEngine.Object) this.WheelTL)
          this.WheelTL.gameObject.SetActive(true);
        if ((bool) (UnityEngine.Object) this.WheelTR)
          this.WheelTR.gameObject.SetActive(true);
        if ((bool) (UnityEngine.Object) this.TWheelBL)
          this.TWheelBL.gameObject.SetActive(false);
        if ((bool) (UnityEngine.Object) this.TWheelBR)
          this.TWheelBR.gameObject.SetActive(false);
        if ((bool) (UnityEngine.Object) this.TWheelTL)
          this.TWheelTL.gameObject.SetActive(false);
        if ((bool) (UnityEngine.Object) this.TWheelTR)
          this.TWheelTR.gameObject.SetActive(false);
        if ((bool) (UnityEngine.Object) this.TWheelML)
          this.TWheelML.gameObject.SetActive(false);
        if (!(bool) (UnityEngine.Object) this.TWheelMR)
          return;
        this.TWheelMR.gameObject.SetActive(false);
      }
    }

    public void Update()
    {
      if (!(bool) (UnityEngine.Object) this.ri3d.rb_body || !(bool) (UnityEngine.Object) this.WheelBL || !(bool) (UnityEngine.Object) this.WheelBR || !(bool) (UnityEngine.Object) this.WheelTL || !(bool) (UnityEngine.Object) this.WheelTR)
        return;
      this.UpdateDriveTrain(this.ri3d.DriveTrain);
      Vector3 position = this.ri3d.rb_body.transform.position;
      float num1 = (float) (-(double) WheelsDisplay.AngleWrapDeg(this.ri3d.rb_body.transform.rotation.eulerAngles.y) * (Math.PI / 180.0));
      this.currDeltaBodyRotationRad = WheelsDisplay.AngleWrapRad(num1 - this.lastBodyAngleRad);
      float num2 = Vector3.Distance(position, this.lastBodyPosition);
      float x = position.x - this.lastBodyPosition.x;
      float num3 = (float) Math.Atan2((double) position.z - (double) this.lastBodyPosition.z, (double) x) - num1;
      this.currDeltaBodyForwards = (float) Math.Cos((double) num3) * num2;
      this.currDeltaBodyStraif = (float) Math.Sin((double) num3) * num2;
      float num4 = this.currDeltaBodyForwards * 280f;
      float num5 = this.currDeltaBodyStraif * 200f;
      float num6 = this.currDeltaBodyRotationRad * 120f;
      if (this.my_DriveTrain == "6-Wheel Tank")
        num5 = 0.0f;
      this.deltaTL = num4 - num5 - num6;
      this.deltaTR = num4 + num5 + num6;
      this.deltaBL = num4 + num5 - num6;
      this.deltaBR = num4 - num5 + num6;
      if ((bool) (UnityEngine.Object) this.WheelTL)
        this.WheelTL.transform.Rotate(new Vector3(0.0f, 1f, 0.0f), this.deltaTL);
      if ((bool) (UnityEngine.Object) this.WheelTR)
        this.WheelTR.transform.Rotate(new Vector3(0.0f, 1f, 0.0f), this.deltaTR);
      if ((bool) (UnityEngine.Object) this.WheelBL)
        this.WheelBL.transform.Rotate(new Vector3(0.0f, 1f, 0.0f), this.deltaBL);
      if ((bool) (UnityEngine.Object) this.WheelBR)
        this.WheelBR.transform.Rotate(new Vector3(0.0f, 1f, 0.0f), this.deltaBR);
      if ((bool) (UnityEngine.Object) this.TWheelTL)
        this.TWheelTL.transform.Rotate(new Vector3(0.0f, 1f, 0.0f), this.deltaTL);
      if ((bool) (UnityEngine.Object) this.TWheelTR)
        this.TWheelTR.transform.Rotate(new Vector3(0.0f, 1f, 0.0f), this.deltaTR);
      if ((bool) (UnityEngine.Object) this.TWheelBL)
        this.TWheelBL.transform.Rotate(new Vector3(0.0f, 1f, 0.0f), this.deltaBL);
      if ((bool) (UnityEngine.Object) this.TWheelBR)
        this.TWheelBR.transform.Rotate(new Vector3(0.0f, 1f, 0.0f), this.deltaBR);
      if ((bool) (UnityEngine.Object) this.TWheelML)
        this.TWheelML.transform.Rotate(new Vector3(0.0f, 1f, 0.0f), this.deltaBL);
      if ((bool) (UnityEngine.Object) this.TWheelMR)
        this.TWheelMR.transform.Rotate(new Vector3(0.0f, 1f, 0.0f), this.deltaBR);
      this.lastBodyAngleRad = num1;
      this.lastBodyPosition = position;
    }

    public float currDeltaBodyForwards { get; private set; }

    public float currDeltaBodyStraif { get; private set; }

    public float currDeltaBodyRotationRad { get; private set; }

    public float currentForwardsSpeed => this.currDeltaBodyForwards / Time.deltaTime;

    public float currentStraifSpeed => this.currDeltaBodyStraif / Time.deltaTime;

    public float currentRotationSpeedRad => this.currDeltaBodyRotationRad / Time.deltaTime;

    public static float AngleWrapDeg(float degrees)
    {
      float num = degrees % 360f;
      if ((double) num > 180.0)
        num -= 360f;
      return num;
    }

    public static float AngleWrapRad(float rad)
    {
      float num = rad % 6.283185f;
      if ((double) num > 3.14159274101257)
        num -= 6.283185f;
      return num;
    }

    private Quaternion GetRelativeToBody(Transform myTransform) => Quaternion.FromToRotation(myTransform.transform.right, this.ri3d.rb_body.transform.forward);
  }
}

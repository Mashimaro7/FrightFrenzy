// Decompiled with JetBrains decompiler
// Type: RotationDriver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class RotationDriver : MonoBehaviour
{
  public RotationDriver.AnimationParameter myDriver;
  public List<RotationDriver.AnimationParameter> myTargets;
  public List<RotationDriver.KeyFrame> myKeyFrames;
  public float driver_value;
  public float driver_percentage;
  public Transform topparent_ineditor;
  public int EditingKeyframe;

  public void SetInitialPos() => this.myTargets.ForEach((Action<RotationDriver.AnimationParameter>) (target => target.SetInitialPos()));

  private float getPercentOnDriver(float value) => !this.myDriver.isAnAngle ? (float) (((double) value - (double) this.minimumDriverValue) / ((double) this.maximumDriverValue - (double) this.minimumDriverValue)) : (value - this.minimumDriverValue) / this.getTotalDriverRange();

  private float getTotalDriverRange()
  {
    if (!this.myDriver.isAnAngle)
      return this.maximumDriverValue - this.minimumDriverValue;
    float totalDriverRange = 0.0f;
    for (int index = 1; index < this.myKeyFrames.Count; ++index)
      totalDriverRange += RotationDriver.AngleWrap(this.myKeyFrames[index].driverValue - this.myKeyFrames[index - 1].driverValue);
    return totalDriverRange;
  }

  private void Update()
  {
    int index1 = 0;
    this.driver_value = RotationDriver.AngleWrap(this.myDriver.currentValue);
    float num1 = this.getPercentOnDriver(this.driver_value);
    this.driver_percentage = num1;
    if ((double) num1 > 0.999000012874603)
      num1 = 0.999f;
    if ((double) num1 < 9.99999974737875E-05)
      num1 = 0.0001f;
    while (index1 < this.myKeyFrames.Count - 2 && (double) this.getPercentOnDriver(this.myKeyFrames[index1 + 1].driverValue) < (double) num1)
      ++index1;
    RotationDriver.KeyFrame keyFrame1 = this.myKeyFrames[index1];
    RotationDriver.KeyFrame keyFrame2 = this.myKeyFrames[index1 + 1];
    float num2 = this.myDriver.isAnAngle ? RotationDriver.AngleWrap(this.myDriver.currentValue - keyFrame1.driverValue) / RotationDriver.AngleWrap(keyFrame2.driverValue - keyFrame1.driverValue) : (float) (((double) this.myDriver.currentValue - (double) keyFrame1.driverValue) / ((double) keyFrame2.driverValue - (double) keyFrame1.driverValue));
    if ((double) num2 > 0.999000012874603)
      num2 = 0.999f;
    if ((double) num2 < 9.99999974737875E-05)
      num2 = 0.0001f;
    for (int index2 = 0; index2 < this.myTargets.Count; ++index2)
      this.myTargets[index2].currentValue = this.myTargets[index2].isAnAngle ? RotationDriver.AngleWrap(num2 * RotationDriver.AngleWrap(keyFrame2.targetValues[index2] - keyFrame1.targetValues[index2]) + keyFrame1.targetValues[index2]) : num2 * (keyFrame2.targetValues[index2] - keyFrame1.targetValues[index2]) + keyFrame1.targetValues[index2];
  }

  private void Start()
  {
    RobotInterface3D componentInParent = this.GetComponentInParent<RobotInterface3D>();
    Transform topparent = (UnityEngine.Object) componentInParent == (UnityEngine.Object) null ? (Transform) null : componentInParent.transform;
    foreach (RotationDriver.AnimationParameter target in this.myTargets)
      target.Initialize(topparent);
    this.myDriver.Initialize(topparent);
  }

  public void InitializeInEditor()
  {
    foreach (RotationDriver.AnimationParameter target in this.myTargets)
      target.Initialize(this.topparent_ineditor);
    this.myDriver.Initialize(this.topparent_ineditor);
  }

  public void AddKeyframe()
  {
    if (this.myKeyFrames == null)
      this.myKeyFrames = new List<RotationDriver.KeyFrame>();
    RotationDriver.KeyFrame newKeyFrame = new RotationDriver.KeyFrame();
    newKeyFrame.driverValue = this.myDriver.currentValue;
    this.myTargets.ForEach((Action<RotationDriver.AnimationParameter>) (ap => newKeyFrame.targetValues.Add(ap.currentValue)));
    this.myKeyFrames.Add(newKeyFrame);
  }

  public void SetKeyframe()
  {
    RotationDriver.KeyFrame keyFrame = this.myKeyFrames[this.EditingKeyframe];
    keyFrame.driverValue = this.myDriver.currentValue;
    for (int index = 0; index < this.myTargets.Count; ++index)
      keyFrame.targetValues[index] = this.myTargets[index].currentValue;
  }

  private float minimumDriverValue => this.myKeyFrames[0].driverValue;

  private float maximumDriverValue => this.myKeyFrames[this.myKeyFrames.Count - 1].driverValue;

  public void RemoveKeyframe() => this.myKeyFrames.RemoveAt(this.myKeyFrames.Count - 1);

  public static float AngleWrap(float angle)
  {
    while ((double) angle > 180.0)
      angle -= 360f;
    while ((double) angle < -180.0)
      angle += 360f;
    return angle;
  }

  public float AngleWrapLight(float angle)
  {
    while ((double) angle > 360.0)
      angle -= 360f;
    while ((double) angle < -360.0)
      angle += 360f;
    return angle;
  }

  public void RestoreKeyframe()
  {
    for (int index = 0; index < this.myTargets.Count; ++index)
      this.myTargets[index].currentValue = this.myKeyFrames[this.EditingKeyframe].targetValues[index];
    this.myDriver.currentValue = this.myKeyFrames[this.EditingKeyframe].driverValue;
  }

  public enum TransformParameter
  {
    PositionX,
    PositionY,
    PositionZ,
    RotationX,
    RotationY,
    RotationZ,
  }

  [Serializable]
  public class AnimationParameter
  {
    public string myTransformName;
    public Transform myTransform;
    public string myReferenceName;
    public Transform myReference;
    public RotationDriver.TransformParameter myParameter;
    private bool initialized;
    public Vector3 initialPos;
    public Vector3 initialRot;
    public float lastValueAbs;

    public bool isAnAngle => this.myParameter == RotationDriver.TransformParameter.RotationX || this.myParameter == RotationDriver.TransformParameter.RotationY || this.myParameter == RotationDriver.TransformParameter.RotationZ;

    public void Initialize(Transform topparent = null)
    {
      if ((UnityEngine.Object) topparent != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) this.myTransform == (UnityEngine.Object) null && this.myTransformName.Length > 0)
          this.myTransform = MyUtils.FindHierarchy(topparent, this.myTransformName);
        if ((UnityEngine.Object) this.myReference == (UnityEngine.Object) null && this.myReferenceName.Length > 0)
          this.myReference = MyUtils.FindHierarchy(topparent, this.myReferenceName);
      }
      this.lastValueAbs = this.currentValueAbs;
      this.initialized = true;
    }

    public void SetInitialPos()
    {
      this.initialPos = this.myReference.worldToLocalMatrix.MultiplyPoint(this.myTransform.position);
      this.initialRot = (Quaternion.Inverse(this.myReference.rotation) * this.myTransform.rotation).eulerAngles;
      this.Initialize();
    }

    public float currentValue
    {
      get => this.currentValueAbs;
      set => this.currentValueAbs = value;
    }

    public float currentValueAbs
    {
      get
      {
        Vector3 vector3 = this.myReference.worldToLocalMatrix.MultiplyPoint(this.myTransform.position);
        Vector3 eulerAngles = (Quaternion.Inverse(this.myReference.rotation) * this.myTransform.rotation).eulerAngles;
        if (this.myParameter == RotationDriver.TransformParameter.PositionX)
          return vector3.x;
        if (this.myParameter == RotationDriver.TransformParameter.PositionY)
          return vector3.y;
        if (this.myParameter == RotationDriver.TransformParameter.PositionZ)
          return vector3.z;
        if (this.myParameter == RotationDriver.TransformParameter.RotationX)
          return eulerAngles.x;
        if (this.myParameter == RotationDriver.TransformParameter.RotationY)
          return eulerAngles.y;
        return this.myParameter == RotationDriver.TransformParameter.RotationZ ? eulerAngles.z : -1f;
      }
      set
      {
        this.myReference.worldToLocalMatrix.MultiplyPoint(this.myTransform.position);
        Vector3 eulerAngles = (Quaternion.Inverse(this.myReference.rotation) * this.myTransform.rotation).eulerAngles;
        Vector3 initialPos = this.initialPos;
        if (this.myParameter == RotationDriver.TransformParameter.PositionX)
        {
          initialPos.x = value;
          initialPos.y = this.initialPos.y;
          initialPos.z = this.initialPos.z;
        }
        if (this.myParameter == RotationDriver.TransformParameter.PositionY)
        {
          initialPos.x = this.initialPos.x;
          initialPos.y = value;
          initialPos.z = this.initialPos.z;
        }
        if (this.myParameter == RotationDriver.TransformParameter.PositionZ)
        {
          initialPos.x = this.initialPos.x;
          initialPos.y = this.initialPos.y;
          initialPos.z = value;
        }
        Vector3 initialRot = this.initialRot;
        if (this.myParameter == RotationDriver.TransformParameter.RotationX)
        {
          initialRot.x = value;
          initialRot.y = this.initialRot.y;
          initialRot.z = this.initialRot.z;
        }
        if (this.myParameter == RotationDriver.TransformParameter.RotationY)
        {
          initialRot.x = this.initialRot.x;
          initialRot.y = value;
          initialRot.z = this.initialRot.z;
        }
        if (this.myParameter == RotationDriver.TransformParameter.RotationZ)
        {
          initialRot.x = this.initialRot.x;
          initialRot.y = this.initialRot.y;
          initialRot.z = value;
        }
        this.myTransform.rotation = this.myReference.rotation * Quaternion.Euler(initialRot);
        this.myTransform.position = this.myReference.localToWorldMatrix.MultiplyPoint(initialPos);
      }
    }
  }

  [Serializable]
  public class KeyFrame
  {
    public float driverValue;
    public List<float> targetValues;

    public KeyFrame() => this.targetValues = new List<float>();
  }
}

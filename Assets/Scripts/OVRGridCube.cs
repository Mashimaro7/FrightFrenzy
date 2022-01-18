// Decompiled with JetBrains decompiler
// Type: OVRGridCube
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

public class OVRGridCube : MonoBehaviour
{
  public KeyCode GridKey = KeyCode.G;
  private GameObject CubeGrid;
  private bool CubeGridOn;
  private bool CubeSwitchColorOld;
  private bool CubeSwitchColor;
  private int gridSizeX = 6;
  private int gridSizeY = 4;
  private int gridSizeZ = 6;
  private float gridScale = 0.3f;
  private float cubeScale = 0.03f;
  private OVRCameraRig CameraController;

  private void Update() => this.UpdateCubeGrid();

  public void SetOVRCameraController(ref OVRCameraRig cameraController) => this.CameraController = cameraController;

  private void UpdateCubeGrid()
  {
    if (Input.GetKeyDown(this.GridKey))
    {
      if (!this.CubeGridOn)
      {
        this.CubeGridOn = true;
        Debug.LogWarning((object) "CubeGrid ON");
        if ((Object) this.CubeGrid != (Object) null)
          this.CubeGrid.SetActive(true);
        else
          this.CreateCubeGrid();
      }
      else
      {
        this.CubeGridOn = false;
        Debug.LogWarning((object) "CubeGrid OFF");
        if ((Object) this.CubeGrid != (Object) null)
          this.CubeGrid.SetActive(false);
      }
    }
    if (!((Object) this.CubeGrid != (Object) null))
      return;
    this.CubeSwitchColor = !OVRManager.tracker.isPositionTracked;
    if (this.CubeSwitchColor != this.CubeSwitchColorOld)
      this.CubeGridSwitchColor(this.CubeSwitchColor);
    this.CubeSwitchColorOld = this.CubeSwitchColor;
  }

  private void CreateCubeGrid()
  {
    Debug.LogWarning((object) "Create CubeGrid");
    this.CubeGrid = new GameObject("CubeGrid");
    this.CubeGrid.layer = this.CameraController.gameObject.layer;
    for (int index1 = -this.gridSizeX; index1 <= this.gridSizeX; ++index1)
    {
      for (int index2 = -this.gridSizeY; index2 <= this.gridSizeY; ++index2)
      {
        for (int index3 = -this.gridSizeZ; index3 <= this.gridSizeZ; ++index3)
        {
          int num1 = 0;
          if (index1 == 0 && index2 == 0 || index1 == 0 && index3 == 0 || index2 == 0 && index3 == 0)
            num1 = index1 != 0 || index2 != 0 || index3 != 0 ? 1 : 2;
          GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
          primitive.GetComponent<BoxCollider>().enabled = false;
          primitive.layer = this.CameraController.gameObject.layer;
          Renderer component = primitive.GetComponent<Renderer>();
          component.shadowCastingMode = ShadowCastingMode.Off;
          component.receiveShadows = false;
          switch (num1)
          {
            case 0:
              component.material.color = Color.red;
              break;
            case 1:
              component.material.color = Color.white;
              break;
            default:
              component.material.color = Color.yellow;
              break;
          }
          primitive.transform.position = new Vector3((float) index1 * this.gridScale, (float) index2 * this.gridScale, (float) index3 * this.gridScale);
          float num2 = 0.7f;
          if (num1 == 1)
            num2 = 1f;
          if (num1 == 2)
            num2 = 2f;
          primitive.transform.localScale = new Vector3(this.cubeScale * num2, this.cubeScale * num2, this.cubeScale * num2);
          primitive.transform.parent = this.CubeGrid.transform;
        }
      }
    }
  }

  private void CubeGridSwitchColor(bool CubeSwitchColor)
  {
    Color color = Color.red;
    if (CubeSwitchColor)
      color = Color.blue;
    foreach (Component component in this.CubeGrid.transform)
    {
      Material material = component.GetComponent<Renderer>().material;
      if (material.color == Color.red || material.color == Color.blue)
        material.color = color;
    }
  }
}

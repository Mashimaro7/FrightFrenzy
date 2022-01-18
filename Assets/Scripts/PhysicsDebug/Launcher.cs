// Decompiled with JetBrains decompiler
// Type: PhysicsDebug.Launcher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace PhysicsDebug
{
  public class Launcher : MonoBehaviour
  {
    public GameObject[] objsToLaunch;
    public Vector2 forceRange = new Vector2(100f, 200f);
    private GameObject launchObjParent;
    private int launchIndex;

    private void Start() => Time.fixedDeltaTime = 0.01f;

    private void Update()
    {
      if (!Input.GetKeyDown(KeyCode.T))
        return;
      this.Launch();
    }

    private void Launch()
    {
      if (!(bool) (Object) this.launchObjParent)
      {
        this.launchObjParent = new GameObject();
        this.launchObjParent.name = "Launched Objects";
      }
      GameObject gameObject = Object.Instantiate<GameObject>(this.objsToLaunch[this.launchIndex]);
      ++this.launchIndex;
      if (this.launchIndex >= this.objsToLaunch.Length)
        this.launchIndex = 0;
      gameObject.transform.SetParent(this.launchObjParent.transform);
      Rigidbody component1 = gameObject.GetComponent<Rigidbody>();
      gameObject.transform.position = this.transform.Find("LP").position;
      gameObject.transform.rotation = this.transform.Find("LP").rotation;
      component1.AddRelativeForce(new Vector3(Random.Range(0.0f, 10f), Random.Range(0.0f, 10f), Random.Range(this.forceRange.x, this.forceRange.y)));
      Renderer component2 = gameObject.GetComponent<Renderer>();
      component2.material = Object.Instantiate<Material>(component2.material);
      component2.material.color = this.RandomColor();
      gameObject.GetComponent<DelayDestroy>().StartUp();
    }

    private Color RandomColor()
    {
      double r = (double) Random.Range(0.0f, 1f);
      float num1 = Random.Range(0.0f, 1f);
      float num2 = Random.Range(0.0f, 1f);
      double g = (double) num1;
      double b = (double) num2;
      return new Color((float) r, (float) g, (float) b);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: OVRScreenFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

public class OVRScreenFade : MonoBehaviour
{
  [Tooltip("Fade duration")]
  public float fadeTime = 2f;
  [Tooltip("Screen color at maximum fade")]
  public Color fadeColor = new Color(0.01f, 0.01f, 0.01f, 1f);
  public bool fadeOnStart = true;
  public int renderQueue = 5000;
  private float uiFadeAlpha;
  private MeshRenderer fadeRenderer;
  private MeshFilter fadeMesh;
  private Material fadeMaterial;
  private bool isFading;

  public float currentAlpha { get; private set; }

  private void Awake()
  {
    this.fadeMaterial = new Material(Shader.Find("Oculus/Unlit Transparent Color"));
    this.fadeMesh = this.gameObject.AddComponent<MeshFilter>();
    this.fadeRenderer = this.gameObject.AddComponent<MeshRenderer>();
    Mesh mesh = new Mesh();
    this.fadeMesh.mesh = mesh;
    Vector3[] vector3Array1 = new Vector3[4];
    float x = 2f;
    float y = 2f;
    float z = 1f;
    vector3Array1[0] = new Vector3(-x, -y, z);
    vector3Array1[1] = new Vector3(x, -y, z);
    vector3Array1[2] = new Vector3(-x, y, z);
    vector3Array1[3] = new Vector3(x, y, z);
    mesh.vertices = vector3Array1;
    int[] numArray = new int[6]{ 0, 2, 1, 2, 3, 1 };
    mesh.triangles = numArray;
    Vector3[] vector3Array2 = new Vector3[4]
    {
      -Vector3.forward,
      -Vector3.forward,
      -Vector3.forward,
      -Vector3.forward
    };
    mesh.normals = vector3Array2;
    Vector2[] vector2Array = new Vector2[4]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(1f, 0.0f),
      new Vector2(0.0f, 1f),
      new Vector2(1f, 1f)
    };
    mesh.uv = vector2Array;
    this.SetFadeLevel(0.0f);
  }

  public void FadeOut() => this.StartCoroutine(this.Fade(0.0f, 1f));

  private void OnLevelFinishedLoading(int level) => this.StartCoroutine(this.Fade(1f, 0.0f));

  private void Start()
  {
    if (!this.fadeOnStart)
      return;
    this.StartCoroutine(this.Fade(1f, 0.0f));
  }

  private void OnEnable()
  {
    if (this.fadeOnStart)
      return;
    this.SetFadeLevel(0.0f);
  }

  private void OnDestroy()
  {
    if ((Object) this.fadeRenderer != (Object) null)
      Object.Destroy((Object) this.fadeRenderer);
    if ((Object) this.fadeMaterial != (Object) null)
      Object.Destroy((Object) this.fadeMaterial);
    if (!((Object) this.fadeMesh != (Object) null))
      return;
    Object.Destroy((Object) this.fadeMesh);
  }

  public void SetUIFade(float level)
  {
    this.uiFadeAlpha = Mathf.Clamp01(level);
    this.SetMaterialAlpha();
  }

  public void SetFadeLevel(float level)
  {
    this.currentAlpha = level;
    this.SetMaterialAlpha();
  }

  private IEnumerator Fade(float startAlpha, float endAlpha)
  {
    float elapsedTime = 0.0f;
    while ((double) elapsedTime < (double) this.fadeTime)
    {
      elapsedTime += Time.deltaTime;
      this.currentAlpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(elapsedTime / this.fadeTime));
      this.SetMaterialAlpha();
      yield return (object) new WaitForEndOfFrame();
    }
  }

  private void SetMaterialAlpha()
  {
    Color fadeColor = this.fadeColor with
    {
      a = Mathf.Max(this.currentAlpha, this.uiFadeAlpha)
    };
    this.isFading = (double) fadeColor.a > 0.0;
    if (!((Object) this.fadeMaterial != (Object) null))
      return;
    this.fadeMaterial.color = fadeColor;
    this.fadeMaterial.renderQueue = this.renderQueue;
    this.fadeRenderer.material = this.fadeMaterial;
    this.fadeRenderer.enabled = this.isFading;
  }
}

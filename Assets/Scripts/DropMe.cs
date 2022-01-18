// Decompiled with JetBrains decompiler
// Type: DropMe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropMe : 
  MonoBehaviour,
  IDropHandler,
  IEventSystemHandler,
  IPointerEnterHandler,
  IPointerExitHandler
{
  public Image containerImage;
  public Image receivingImage;
  private Color normalColor;
  public Color highlightColor = Color.yellow;

  public void OnEnable()
  {
    if (!((Object) this.containerImage != (Object) null))
      return;
    this.normalColor = this.containerImage.color;
  }

  public void OnDrop(PointerEventData data)
  {
    this.containerImage.color = this.normalColor;
    if ((Object) this.receivingImage == (Object) null)
      return;
    Sprite dropSprite = this.GetDropSprite(data);
    if (!((Object) dropSprite != (Object) null))
      return;
    this.receivingImage.overrideSprite = dropSprite;
  }

  public void OnPointerEnter(PointerEventData data)
  {
    if ((Object) this.containerImage == (Object) null || !((Object) this.GetDropSprite(data) != (Object) null))
      return;
    this.containerImage.color = this.highlightColor;
  }

  public void OnPointerExit(PointerEventData data)
  {
    if ((Object) this.containerImage == (Object) null)
      return;
    this.containerImage.color = this.normalColor;
  }

  private Sprite GetDropSprite(PointerEventData data)
  {
    GameObject pointerDrag = data.pointerDrag;
    if ((Object) pointerDrag == (Object) null)
      return (Sprite) null;
    if ((Object) pointerDrag.GetComponent<DragMe>() == (Object) null)
      return (Sprite) null;
    Image component = pointerDrag.GetComponent<Image>();
    return (Object) component == (Object) null ? (Sprite) null : component.sprite;
  }
}

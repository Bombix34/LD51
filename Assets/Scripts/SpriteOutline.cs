using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour {
    public Color outlineColor = Color.white;

    [SerializeField] private List<Color> rarityColors;
    [SerializeField] private Color badDishColor;

    [Range(0, 32)]
    public int outlineSize = 1;

    private SpriteRenderer spriteRenderer;

    private void OnEnable() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateOutline(true);
    }

    private void OnDisable() 
    {
        UpdateOutline(false);
    }

    private void Update() 
    {
        UpdateOutline(true);
    }

    private void UpdateOutline(bool outline) 
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", outlineColor);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);
    }

    public void SetupSprite(Sprite newSprite, Rarity ingredientRarity)
    {
        spriteRenderer.sprite = newSprite;
        outlineColor = rarityColors[(int)ingredientRarity];
        /*
        switch(ingredientRarity)
        {
            case Rarity.COMMON:
                outlineColor = Color.white;
                break;
            case Rarity.UNCOMMON:
                outlineColor = Color.green;
                break;
            case Rarity.RARE:
                outlineColor = Color.blue;
                break;
            case Rarity.EPIC:
                outlineColor = new Color(186/255,85/255,211/255);
                break;
            case Rarity.LEGENDARY:
                outlineColor = new Color(1f,69/255f,0f);
                break;
        }
        */
    }

    public void SetupBadDish(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
        outlineColor = badDishColor;
    }
}

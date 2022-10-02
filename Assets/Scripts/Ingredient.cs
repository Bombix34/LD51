using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [field: SerializeField]
    public IngredientScriptableObject IngredientSo { get; set; }

    public void DestroyImmediate()
    {
        DestroyImmediate(gameObject);
    }
}

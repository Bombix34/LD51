using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUI : MonoBehaviour
{
    private const string INGREDIENTS_PATH = "ScriptableObjects/Ingredients/";
    [field: SerializeField]
    public GameObject CollectionImagePrefab { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        var ingredients = Resources.LoadAll<IngredientScriptableObject>(INGREDIENTS_PATH).OrderBy(q => q.Rarity).ThenBy(q => q.Name);
        foreach (var ingredient in ingredients)
        {
            var ingredientImageGameobject = Instantiate(CollectionImagePrefab, transform);
            var ingredientImage = ingredientImageGameobject.GetComponent<Image>();
            ingredientImage.sprite = ingredient.Sprite;

            if (!PlayerPrefs.HasKey(ingredient.Name))
            {
                ingredientImage.color = new Color(0, 0, 0, 0.5f);
            }
        }
    }
}

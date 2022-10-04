using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollectionUI : MonoBehaviour
{
    private const string INGREDIENTS_PATH = "ScriptableObjects/Ingredients/";
    [field: SerializeField]
    public GameObject CollectionImagePrefab { get; set; }

    [SerializeField] private GridLayoutGroup gridLayout;

    private void Start()
    {
        float cellSize = ((Screen.width/Screen.height)*70)/(2960/1440);
       // gridLayout.cellSize = Vector2.one * cellSize;

        var ingredients = Resources.LoadAll<IngredientScriptableObject>(INGREDIENTS_PATH).OrderBy(q => q.Rarity).ThenBy(q => q.Name);
        foreach (var ingredient in ingredients)
        {
            var ingredientImageGameobject = Instantiate(CollectionImagePrefab, transform);
            var ingredientImage = ingredientImageGameobject.GetComponentInChildren<Image>();
            ingredientImage.sprite = ingredient.Sprite;

            if (!PlayerPrefs.HasKey(ingredient.Name))
            {
                ingredientImage.color = new Color(0, 0, 0, 0.5f);
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientDraw
{
    public List<IngredientScriptableObject> DrawIngredients()
    {
        var scoreBoard = ScoreBoard.Instance;
        var drawedIngredients = new List<IngredientScriptableObject>();
        var newlyAddedIngredients = scoreBoard.GetNewlyAdded();

        for (int i = 0; i < scoreBoard.IngredientDrawSize; i++)
        {
            if (newlyAddedIngredients.Count > 0)
            {
                var ingredient = newlyAddedIngredients.First();
                drawedIngredients.Add(ingredient);
                newlyAddedIngredients.Remove(ingredient);
                continue;
            }
            drawedIngredients.Add(scoreBoard.UnlockedIngredients[Random.Range(0, scoreBoard.UnlockedIngredients.Count)]);
        }

        return drawedIngredients;
    }
}

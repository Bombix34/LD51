internal struct RecipeWithPriority
{
    public Recipe recipe;
    public int priority;

    public RecipeWithPriority(Recipe recipe, int priority)
    {
        this.recipe = recipe;
        this.priority = priority;
    }

    public void Deconstruct(out Recipe recipe, out int priority)
    {
        recipe = this.recipe;
        priority = this.priority;
    }

    public static implicit operator (Recipe recipe, int priority)(RecipeWithPriority value)
    {
        return (value.recipe, value.priority);
    }

    public static implicit operator RecipeWithPriority((Recipe recipe, int priority) value)
    {
        return new RecipeWithPriority(value.recipe, value.priority);
    }
}
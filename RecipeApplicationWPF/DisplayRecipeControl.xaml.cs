using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RecipeApplicationWPF
{
    public partial class DisplayRecipeControl : UserControl
    {
        // Constructor for DisplayRecipeControl, takes a Recipe object as a parameter
        public DisplayRecipeControl(Recipe recipe)
        {
            InitializeComponent();
            try
            {
                // Check if the recipe is null and throw an exception if it is
                if (recipe == null)
                {
                    throw new ArgumentNullException(nameof(recipe), "Recipe cannot be null");
                }

                // Call the method to display the recipe details
                DisplayRecipe(recipe);
            }
            catch (Exception ex)
            {
                // Show a message box if an error occurs while displaying the recipe
                MessageBox.Show($"An error occurred while displaying the recipe: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method to display the recipe details on the UI
        private void DisplayRecipe(Recipe recipe)
        {
            // Set the text of the RecipeNameTextBlock to the recipe's name
            RecipeNameTextBlock.Text = recipe.Name;

            // Create a formatted string for each ingredient and set it as the ItemsSource of IngredientsItemsControl
            var ingredientTexts = recipe.Ingredients.Select(ingredient =>
                $"- {ingredient.Quantity} {ingredient.Unit} of {ingredient.Name} (Calories: {ingredient.Calories}, Food Group: {ingredient.FoodGroup})");
            IngredientsItemsControl.ItemsSource = ingredientTexts;

            // Set the steps of the recipe as the ItemsSource of StepsItemsControl
            StepsItemsControl.ItemsSource = recipe.Steps;

            // Calculate the total calories of the recipe
            int totalCalories = recipe.CalculateTotalCalories();

            // Set the text of TotalCaloriesTextBlock to the total calories
            TotalCaloriesTextBlock.Text = $"Total Calories: {totalCalories}";

            // Change the text color based on the total calories and show a warning message if it exceeds 300 calories
            if (totalCalories > 300)
            {
                // Set text color to red if total calories exceed 300
                TotalCaloriesTextBlock.Foreground = Brushes.Red;
                MessageBox.Show($"Warning: This recipe has {totalCalories} calories, which exceeds the recommended limit of 300 calories.", "High Calorie Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                // Set text color to green if total calories are within the limit
                TotalCaloriesTextBlock.Foreground = Brushes.Green;
            }
        }
    }
}

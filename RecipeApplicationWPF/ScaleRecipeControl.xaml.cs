using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace RecipeApplicationWPF
{
    public partial class ScaleRecipeControl : UserControl
    {
        // Constructor for the ScaleRecipeControl UserControl
        public ScaleRecipeControl()
        {
            InitializeComponent();
            PopulateRecipeListBox();  // Populate the ListBox with recipes on initialization
        }

        // Method to populate the RecipeListBox with available recipes
        private void PopulateRecipeListBox()
        {
            RecipeListBox.Items.Clear();  // Clear any existing items in the ListBox
            if (MainWindow.Recipes.Count == 0)  // Check if there are no recipes available
            {
                // Show a message box if no recipes are available
                MessageBox.Show("There are no available recipes to scale.");
            }
            else
            {
                // Add each recipe name to the RecipeListBox
                foreach (var recipe in MainWindow.Recipes)
                {
                    RecipeListBox.Items.Add(recipe.Name);
                }
            }
        }

        // Helper method to display the scaling result message
        private void ShowScaleResult(string message)
        {
            ScaleResultTextBlock.Text = message;
        }

        // Event handler for the "Scale by 0.5" button click
        private void ScaleBy05_Click(object sender, RoutedEventArgs e)
        {
            ScaleSelectedRecipe(0.5);
        }

        // Event handler for the "Scale by 2" button click
        private void ScaleBy2_Click(object sender, RoutedEventArgs e)
        {
            ScaleSelectedRecipe(2);
        }

        // Event handler for the "Scale by 3" button click
        private void ScaleBy3_Click(object sender, RoutedEventArgs e)
        {
            ScaleSelectedRecipe(3);
        }

        // Method to scale the selected recipe by a given factor
        private void ScaleSelectedRecipe(double scalingFactor)
        {
            if (RecipeListBox.SelectedItem != null)  // Check if a recipe is selected
            {
                // Get the name of the selected recipe
                var selectedRecipeName = RecipeListBox.SelectedItem.ToString();
                // Find the recipe in the MainWindow's list of recipes
                var selectedRecipe = MainWindow.Recipes.FirstOrDefault(recipe => recipe.Name == selectedRecipeName);
                if (selectedRecipe != null)
                {
                    // Scale the recipe by the specified factor
                    selectedRecipe.Scale(scalingFactor);
                    // Show a success message
                    ShowScaleResult($"Recipe '{selectedRecipeName}' scaled by {scalingFactor} successfully!");
                }
            }
            else
            {
                // Show a message if no recipe is selected
                ShowScaleResult("Please select a recipe to scale.");
            }
        }
    }
}

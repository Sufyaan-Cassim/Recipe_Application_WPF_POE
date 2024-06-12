using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RecipeApplicationWPF
{
    public partial class EnterRecipeControl : UserControl
    {
        // Lists to store the ingredients and steps for the recipe
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<Step> Steps { get; set; } = new List<Step>();

        // Variables to track the current index and total number of ingredients and steps
        private int currentIngredientIndex = 0;
        private int totalIngredients = 0;
        private int currentStepIndex = 0;
        private int totalSteps = 0;

        // Constructor for EnterRecipeControl
        public EnterRecipeControl()
        {
            InitializeComponent();
        }

        // Event handler for the button to show ingredient details
        private void ShowIngredientDetails_Click(object sender, RoutedEventArgs e)
        {
            // Validate the number of ingredients input
            if (!int.TryParse(NumIngredientsTextBox.Text, out totalIngredients) || totalIngredients <= 0)
            {
                MessageBox.Show("Please enter a valid number of ingredients.");
                return;
            }

            // Update UI visibility and reset the current ingredient index
            RecipeDetailsPanel.Visibility = Visibility.Collapsed;
            IngredientDetailsPanel.Visibility = Visibility.Visible;
            currentIngredientIndex = 0;
            ShowNextIngredientInput();
        }

        // Event handler for the button to show step details
        private void ShowStepDetails_Click(object sender, RoutedEventArgs e)
        {
            // Validate the number of steps input
            if (!int.TryParse(NumStepsTextBox.Text, out totalSteps) || totalSteps <= 0)
            {
                MessageBox.Show("Please enter a valid number of steps.");
                return;
            }

            // Update UI visibility and reset the current step index
            RecipeDetailsPanel.Visibility = Visibility.Collapsed;
            StepDetailsPanel.Visibility = Visibility.Visible;
            currentStepIndex = 0;
            ShowNextStepInput();
        }

        // Method to show input fields for the next ingredient
        private void ShowNextIngredientInput()
        {
            // Update the title for the ingredient details section
            IngredientDetailsTitle.Text = $"Ingredient {currentIngredientIndex + 1} Details";

            if (currentIngredientIndex < totalIngredients)
            {
                // Clear previous input fields
                IngredientsInputStackPanel.Children.Clear();

                // Create input fields for ingredient details
                var ingredientNameTextBox = new TextBox { Width = 300, Margin = new Thickness(0, 5, 0, 5), TextWrapping = TextWrapping.Wrap };
                IngredientsInputStackPanel.Children.Add(CreateLabeledControl("Ingredient Name", ingredientNameTextBox));

                var quantityTextBox = new TextBox { Width = 300, Margin = new Thickness(0, 5, 0, 5), TextWrapping = TextWrapping.Wrap };
                IngredientsInputStackPanel.Children.Add(CreateLabeledControl("Quantity", quantityTextBox));

                var unitTextBox = new TextBox { Width = 300, Margin = new Thickness(0, 5, 0, 5), TextWrapping = TextWrapping.Wrap };
                IngredientsInputStackPanel.Children.Add(CreateLabeledControl("Unit of measurement", unitTextBox));

                var caloriesTextBox = new TextBox { Width = 300, Margin = new Thickness(0, 5, 0, 5), TextWrapping = TextWrapping.Wrap };
                IngredientsInputStackPanel.Children.Add(CreateLabeledControl("Calories", caloriesTextBox));

                var foodGroupComboBox = new ComboBox { Width = 300, Margin = new Thickness(0, 5, 0, 5) };
                foodGroupComboBox.Items.Add("Starchy foods");
                foodGroupComboBox.Items.Add("Vegetables and fruits");
                foodGroupComboBox.Items.Add("Dry beans, peas, lentils and soya");
                foodGroupComboBox.Items.Add("Chicken, fish, meat and eggs");
                foodGroupComboBox.Items.Add("Milk and dairy products");
                foodGroupComboBox.Items.Add("Fats and oil");
                foodGroupComboBox.Items.Add("Water");
                IngredientsInputStackPanel.Children.Add(CreateLabeledControl("Food Group", foodGroupComboBox));

                // Update button visibility based on the current ingredient index
                NextIngredientButton.Visibility = currentIngredientIndex == totalIngredients - 1 ? Visibility.Collapsed : Visibility.Visible;
                SaveIngredientsButton.Visibility = currentIngredientIndex == totalIngredients - 1 ? Visibility.Visible : Visibility.Collapsed;
                BackToRecipeDetailsButton.Visibility = currentIngredientIndex == totalIngredients - 1 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        // Event handler for the button to add ingredients
        private void AddIngredientsButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve input fields for ingredient details
            var ingredientNameTextBox = (TextBox)((StackPanel)IngredientsInputStackPanel.Children[0]).Children[1];
            var quantityTextBox = (TextBox)((StackPanel)IngredientsInputStackPanel.Children[1]).Children[1];
            var unitTextBox = (TextBox)((StackPanel)IngredientsInputStackPanel.Children[2]).Children[1];
            var caloriesTextBox = (TextBox)((StackPanel)IngredientsInputStackPanel.Children[3]).Children[1];
            var foodGroupComboBox = (ComboBox)((StackPanel)IngredientsInputStackPanel.Children[4]).Children[1];

            // Validate the input fields
            if (string.IsNullOrWhiteSpace(ingredientNameTextBox.Text) ||
                !double.TryParse(quantityTextBox.Text, out double quantity) ||
                string.IsNullOrWhiteSpace(unitTextBox.Text) ||
                !int.TryParse(caloriesTextBox.Text, out int calories) ||
                foodGroupComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill out all ingredient details.");
                return;
            }

            // Add the ingredient to the list
            Ingredients.Add(new Ingredient(
                ingredientNameTextBox.Text,
                quantity,
                unitTextBox.Text,
                calories,
                foodGroupComboBox.SelectedItem.ToString()
            ));

            // Show the next ingredient input or save ingredients if all inputs are complete
            if (currentIngredientIndex < totalIngredients - 1)
            {
                currentIngredientIndex++;
                ShowNextIngredientInput();
            }
            else
            {
                SaveIngredients();
            }
        }

        // Method to save the ingredients and update UI visibility
        private void SaveIngredients()
        {
            IngredientDetailsPanel.Visibility = Visibility.Collapsed;
            RecipeDetailsPanel.Visibility = Visibility.Visible;
        }

        // Event handler for the button to add steps
        private void AddStepsButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve input fields for step description
            var stepDescriptionTextBox = (TextBox)((StackPanel)StepsInputStackPanel.Children[0]).Children[1];
            stepDescriptionTextBox.TextWrapping = TextWrapping.Wrap;

            // Validate the input field
            if (string.IsNullOrWhiteSpace(stepDescriptionTextBox.Text))
            {
                MessageBox.Show("Please fill out the step description.");
                return;
            }

            // Add the step to the list
            Steps.Add(new Step(stepDescriptionTextBox.Text));

            // Show the next step input or save steps if all inputs are complete
            if (currentStepIndex < totalSteps - 1)
            {
                currentStepIndex++;
                ShowNextStepInput();
            }
            else
            {
                SaveSteps();
            }
        }

        // Method to save the steps and update UI visibility
        private void SaveSteps()
        {
            StepDetailsPanel.Visibility = Visibility.Collapsed;
            RecipeDetailsPanel.Visibility = Visibility.Visible;
        }

        // Method to show input fields for the next step
        private void ShowNextStepInput()
        {
            // Update the title for the step details section
            StepDetailsTitle.Text = $"Step {currentStepIndex + 1} Details";

            if (currentStepIndex < totalSteps)
            {
                // Clear previous input fields
                StepsInputStackPanel.Children.Clear();

                // Create input fields for step description
                var stepDescriptionTextBox = new TextBox { Width = 300, Margin = new Thickness(0, 5, 0, 5), TextWrapping = TextWrapping.Wrap };
                StepsInputStackPanel.Children.Add(CreateLabeledControl("Step Description", stepDescriptionTextBox));

                // Update button visibility based on the current step index
                NextStepButton.Visibility = currentStepIndex == totalSteps - 1 ? Visibility.Collapsed : Visibility.Visible;
                SaveStepsButton.Visibility = currentStepIndex == totalSteps - 1 ? Visibility.Visible : Visibility.Collapsed;
                BackToRecipeDetailsFromStepsButton.Visibility = currentStepIndex == totalSteps - 1 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        // Event handler for the button to go back to recipe details from ingredient details
        private void BackToRecipeDetails_Click(object sender, RoutedEventArgs e)
        {
            BackToRecipeDetails();
        }

        // Method to update UI visibility and reset input fields
        private void BackToRecipeDetails()
        {
            RecipeDetailsPanel.Visibility = Visibility.Visible;
            IngredientDetailsPanel.Visibility = Visibility.Collapsed;
            StepDetailsPanel.Visibility = Visibility.Collapsed;
            IngredientsInputStackPanel.Children.Clear();
            StepsInputStackPanel.Children.Clear();
            currentIngredientIndex = 0;
            currentStepIndex = 0;
        }

        // Event handler for the button to save ingredients and show a confirmation message
        private void SaveIngredientsButton_Click(object sender, RoutedEventArgs e)
        {
            AddIngredientsButton_Click(sender, e);
            MessageBox.Show("Ingredients saved successfully!");
        }

        // Event handler for the button to save steps and show a confirmation message
        private void SaveStepsButton_Click(object sender, RoutedEventArgs e)
        {
            AddStepsButton_Click(sender, e);
            MessageBox.Show("Steps saved successfully!");
        }

        // Event handler for the button to save the recipe
        private void SaveRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve and validate the recipe name
            var recipeName = RecipeNameTextBox.Text;

            if (string.IsNullOrWhiteSpace(recipeName) || Ingredients.Count == 0 || Steps.Count == 0)
            {
                MessageBox.Show("Please enter a recipe name, at least one ingredient, and at least one step.");
                return;
            }

            // Show a confirmation dialog before saving the recipe
            var dialogResult = MessageBox.Show("Are you sure you want to save this recipe?", "Save Recipe", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                // Create a new recipe and add it to the main window's recipe list
                var recipe = new Recipe(recipeName, "", 0)
                {
                    Ingredients = new List<Ingredient>(Ingredients),
                    Steps = new List<Step>(Steps)
                };

                MainWindow.Recipes.Add(recipe);

                // Clear input fields and reset variables
                RecipeNameTextBox.Clear();
                NumIngredientsTextBox.Clear();
                NumStepsTextBox.Clear();
                Ingredients.Clear();
                Steps.Clear();
                currentIngredientIndex = 0;
                currentStepIndex = 0;

                MessageBox.Show("Recipe saved successfully!");
            }
            else
            {
                MessageBox.Show("Saving recipe canceled.");
            }
        }

        // Method to create a labeled control (label + input control)
        private StackPanel CreateLabeledControl(string label, Control control)
        {
            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };
            var textBlock = new TextBlock
            {
                Text = label,
                Width = 150,
                VerticalAlignment = VerticalAlignment.Center,
                TextWrapping = TextWrapping.Wrap
            };
            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(control);
            return stackPanel;
        }
    }
}

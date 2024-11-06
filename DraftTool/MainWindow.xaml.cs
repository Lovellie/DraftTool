using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System;
using System.Linq; // Make sure this is included

namespace DraftTool
{
    public partial class MainWindow : Window
    {
        private List<string> maps;
        private string lastCaptain1 = null;
        private string lastCaptain2 = null;
        private List<string> team1 = new List<string>();
        private List<string> team2 = new List<string>();
        private bool isCaptain1Turn = true;
        private int draftRound = 1;  // Track the current draft round

        private List<string> allNames = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeMaps();
        }

        private void InitializeMaps()
        {
            maps = new List<string>()
        {
            "Bind", "Haven", "Split", "Ascent", "Icebox", "Breeze",
            "Fracture", "Pearl", "Lotus", "Sunset", "Abyss"
        };

            TextBlock mapTextBlock = mainGrid.FindName("Map") as TextBlock;
            if (mapTextBlock != null)
            {
                mapTextBlock.Text = "Awaiting Map";
            }
        }

        private void SetUpDraftTool()
        {
            TextBlock mapTextBlock = mainGrid.FindName("Map") as TextBlock;

            if (mapTextBlock != null)
            {
                if (maps.Count > 0)
                {
                    Random random = new Random();
                    int randomIndex = random.Next(maps.Count);
                    string randomMap = maps[randomIndex];

                    mapTextBlock.Text = randomMap;
                    maps.RemoveAt(randomIndex);
                }
                else
                {
                    MessageBox.Show("All maps have been selected! Resetting pool", "Draft Tool", MessageBoxButton.OK, MessageBoxImage.Information);
                    InitializeMaps();
                }
            }
        }

        private void RandomMapButton_Click(object sender, RoutedEventArgs e) => SetUpDraftTool();

        private void ResetButton_Click(object sender, RoutedEventArgs e) => InitializeMaps();

        private void SubmitNamesButton_Click(object sender, RoutedEventArgs e)
        {
            string namesInput = NamesInputTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(namesInput))
            {
                // Split the input by newlines or commas (you can choose one depending on the format)
                string[] names = namesInput.Split(new[] { "\r\n", "\n", "," }, StringSplitOptions.RemoveEmptyEntries);

                // Add each name to the list, checking for duplicates
                foreach (var name in names)
                {
                    string trimmedName = name.Trim();
                    if (!allNames.Contains(trimmedName) && !string.IsNullOrEmpty(trimmedName))
                    {
                        allNames.Add(trimmedName);
                        NameListBox.Items.Add(trimmedName); // Add name to the NameListBox
                    }
                }

                // Clear the TextBox after submission
                NamesInputTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Please enter at least one name.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void SelectCaptainsButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameListBox.Items.Count < 2)
            {
                MessageBox.Show("Not enough names to select two captains.", "Insufficient Names", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<string> names = NameListBox.Items.Cast<string>().ToList();
            Random random = new Random();

            // Randomly select Captain 1
            int captain1Index = random.Next(names.Count);
            string newCaptain1 = names[captain1Index];
            names.RemoveAt(captain1Index);

            // Randomly select Captain 2
            int captain2Index = random.Next(names.Count);
            string newCaptain2 = names[captain2Index];

            // Remove the old captains from the teams if they exist
            if (lastCaptain1 != null && team1.Contains(lastCaptain1))
            {
                team1.Remove(lastCaptain1);
                Team1ListBox.Items.Remove(lastCaptain1);
                NameListBox.Items.Add(lastCaptain1);  // Re-add the old Captain 1 back to the NameListBox
            }

            if (lastCaptain2 != null && team2.Contains(lastCaptain2))
            {
                team2.Remove(lastCaptain2);
                Team2ListBox.Items.Remove(lastCaptain2);
                NameListBox.Items.Add(lastCaptain2);  // Re-add the old Captain 2 back to the NameListBox
            }

            // Update the TextBlocks showing Team Captains' names
            TextBlock team1TextBlock = mainGrid.FindName("Team1TextBlock") as TextBlock;
            TextBlock team2TextBlock = mainGrid.FindName("Team2TextBlock") as TextBlock;

            if (team1TextBlock != null && team2TextBlock != null)
            {
                team1TextBlock.Text = $"Team {newCaptain1}"; // Set Team Captain 1 to the name of the first captain
                team2TextBlock.Text = $"Team {newCaptain2}"; // Set Team Captain 2 to the name of the second captain
            }

            // Remove the new captains from the NameListBox
            NameListBox.Items.Remove(newCaptain1);
            NameListBox.Items.Remove(newCaptain2);

            // Add new captains to their respective teams
            team1.Add(newCaptain1);  // Add new Captain 1 to team 1
            Team1ListBox.Items.Add(newCaptain1);

            team2.Add(newCaptain2);  // Add new Captain 2 to team 2
            Team2ListBox.Items.Add(newCaptain2);

            // Update the last captain references
            lastCaptain1 = newCaptain1;
            lastCaptain2 = newCaptain2;
        }
        private void ResetCaptainsButton_Click(object sender, RoutedEventArgs e)
        {
            if (lastCaptain1 != null && lastCaptain2 != null)
            {
                // Remove captains from the teams
                team1.Remove(lastCaptain1);
                team2.Remove(lastCaptain2);

                // Remove the captains from the Team list boxes
                Team1ListBox.Items.Remove(lastCaptain1);
                Team2ListBox.Items.Remove(lastCaptain2);

                // Add the captains back to the NameListBox
                NameListBox.Items.Add(lastCaptain1);
                NameListBox.Items.Add(lastCaptain2);

                // Reset the captains' display text
                CaptainsDisplay.Text = "Captains have been reset!";

                // Reset the team labels back to "Team 1" and "Team 2"
                TextBlock team1TextBlock = mainGrid.FindName("Team1TextBlock") as TextBlock;
                TextBlock team2TextBlock = mainGrid.FindName("Team2TextBlock") as TextBlock;

                if (team1TextBlock != null && team2TextBlock != null)
                {
                    team1TextBlock.Text = "Team Captain 1"; // Reset to "Team 1"
                    team2TextBlock.Text = "Team Captain 2"; // Reset to "Team 2"
                }

                // Reset the captains' variables to null
                lastCaptain1 = null;
                lastCaptain2 = null;
            }
            else
            {
                MessageBox.Show("No captains to reset.", "Reset Captains", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void StartDraftButton_Click(object sender, RoutedEventArgs e)
        {
            if (lastCaptain1 == null || lastCaptain2 == null)
            {
                MessageBox.Show("Please select captains first!", "Draft Tool", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Add captains to their respective teams first
            team1.Add(lastCaptain1);  // Add Captain 1 to team 1
            Team1ListBox.Items.Add(lastCaptain1);

            team2.Add(lastCaptain2);  // Add Captain 2 to team 2
            Team2ListBox.Items.Add(lastCaptain2);

            // Start drafting
            isCaptain1Turn = true; // Start with Captain 1

            // Set the display to show whose turn it is
            CaptainsDisplay.Text = $"{lastCaptain1}'s turn to pick!"; // Show that Captain 1 goes first

            PickPlayerButton.IsEnabled = true; // Enable the pick button when the draft starts

            MessageBox.Show($"{lastCaptain1} and {lastCaptain2}, start drafting!", "Draft Tool", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void PickPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a player to draft.", "Draft Tool", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string selectedPlayer = NameListBox.SelectedItem.ToString();

            // Add the selected player to the appropriate team based on whose turn it is
            if (isCaptain1Turn)
            {
                team1.Add(selectedPlayer);
                Team1ListBox.Items.Add(selectedPlayer);
                CaptainsDisplay.Text = $"{lastCaptain2}'s turn to pick!"; // Update to next captain's turn
            }
            else
            {
                team2.Add(selectedPlayer);
                Team2ListBox.Items.Add(selectedPlayer);
                CaptainsDisplay.Text = $"{lastCaptain1}'s turn to pick!"; // Update to next captain's turn
            }

            // Remove the drafted player from the available list
            NameListBox.Items.Remove(selectedPlayer);

            // Toggle the turn after each pick
            isCaptain1Turn = !isCaptain1Turn;

            // Check if the draft is completed (after 10 picks)
            int totalPlayersDrafted = team1.Count + team2.Count;
            if (totalPlayersDrafted >= 10)
            {
                PickPlayerButton.IsEnabled = false; // Disable pick button after 10 players have been selected
                MessageBox.Show("The draft is complete! 10 players have been selected.", "Draft Tool", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // After both captains have picked once, switch the pick order (snake draft logic)
            if ((team1.Count + team2.Count) % 2 == 0) // After each round (both captains pick)
            {
                isCaptain1Turn = !isCaptain1Turn; // Reverse the turn order for the next round
            }

            // Update the CaptainsDisplay based on who is next to pick after each round
            if (isCaptain1Turn)
            {
                CaptainsDisplay.Text = $"{lastCaptain1}'s turn to pick!";
            }
            else
            {
                CaptainsDisplay.Text = $"{lastCaptain2}'s turn to pick!";
            }
        }

        private void ResetDraftButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset team selections
            team1.Clear();
            team2.Clear();

            // Clear team list boxes
            Team1ListBox.Items.Clear();
            Team2ListBox.Items.Clear();

            // Reset captain selections
            lastCaptain1 = null;
            lastCaptain2 = null;
            CaptainsDisplay.Text = "Draft has been reset!";

            // Re-enable relevant buttons
            PickPlayerButton.IsEnabled = false;
            SelectCaptainsButton.IsEnabled = true;

            // Add all names back to the NameListBox
            NameListBox.Items.Clear(); // Clear the existing names in the list
            foreach (var name in allNames) // Re-add all names from the tracked list
            {
                NameListBox.Items.Add(name);
            }

            // Reset the draft round counter
            draftRound = 1;
        }
    }
}
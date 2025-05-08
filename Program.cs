//using System;
//using System.Collections.Generic;
//using System.Linq;
using HabitTracker.Models;
using HabitTracker.Managers;

namespace HabitTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the singleton instance
            HabitManager habitManager = HabitManager.Instance;

            Console.WriteLine("Welcome to the Habit Tracker!");
            Console.WriteLine("----------------------------------------");

            bool running = true;
            while (running)
            {
                Console.WriteLine("\nPlease select an option:");
                Console.WriteLine("1. Create a new habit");
                Console.WriteLine("2. Mark a habit as completed");
                Console.WriteLine("3. View habit progress");
                Console.WriteLine("4. Switch habit type (Weekly/Monthly)");
                Console.WriteLine("5. View total points");
                Console.WriteLine("6. Exit");
                Console.Write("\nYour choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateHabit(habitManager);
                        break;
                    case "2":
                        MarkHabitAsCompleted(habitManager);
                        break;
                    case "3":
                        ViewProgress(habitManager);
                        break;
                    case "4":
                        SwitchHabitType(habitManager);
                        break;
                    case "5":
                        ViewPoints(habitManager);
                        break;
                    case "6":
                        running = false;
                        Console.WriteLine("Thank you for using Habit Tracker. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void CreateHabit(HabitManager manager)
        {
            Console.WriteLine("\n=== Create a New Habit ===");

            Console.Write("Enter habit title: ");
            string title = Console.ReadLine();

            Console.Write("Enter habit description: ");
            string description = Console.ReadLine();

            Console.Write("Enter point value for completing this habit: ");
            int points;
            //Validate that the point value given is a positive integer
            if (!int.TryParse(Console.ReadLine(), out points) || points < 1)
            {
                points = 1; 
                Console.WriteLine("Invalid point value. Setting to default (1 point).");
            }

            Console.Write("Is this a Weekly habit? (y/n): ");
            string typeChoice = Console.ReadLine().ToLower();
            bool isWeekly = (typeChoice == "y" || typeChoice == "yes");

            string habitType = isWeekly ? "Weekly" : "Monthly";

            Habit newHabit = manager.CreateHabit(title, description, points, isWeekly);
            Console.WriteLine($"Habit '{title}' ({habitType}) created successfully with {points} points!");
        }

        static void MarkHabitAsCompleted(HabitManager manager)
        {
            if (manager.Habits.Count == 0)
            {
                Console.WriteLine("\nYou don't have any habits yet. Create one first!");
                return;
            }

            Console.WriteLine("\n=== Mark a Habit as Completed ===");
            Console.WriteLine("Select a habit to mark as completed:");

            // Display list of habits
            for (int i = 0; i < manager.Habits.Count; i++)
            {
                Habit habit = manager.Habits[i];
                string habitType = habit is WeeklyHabit ? "Weekly" : "Monthly";
                Console.WriteLine($"{i + 1}. {habit.Title} ({habitType}) - {habit.PointValue} points");
            }

            Console.Write("\nEnter habit number: ");
            int habitIndex;
            if (!int.TryParse(Console.ReadLine(), out habitIndex) || habitIndex < 1 || habitIndex > manager.Habits.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            Habit selectedHabit = manager.Habits[habitIndex - 1];


            // Prevent double-counting completions on the same day
            if (selectedHabit.IsCompletedToday())
            {
                Console.WriteLine($"You have already completed '{selectedHabit.Title}' today!");
                return;
            }

            
            manager.CompleteHabit(selectedHabit);
            Console.WriteLine($"Habit '{selectedHabit.Title}' marked as completed!");
            Console.WriteLine($"You earned {selectedHabit.PointValue} points!");
        }

        static void ViewProgress(HabitManager manager)
        {
            if (manager.Habits.Count == 0)
            {
                Console.WriteLine("\nYou don't have any habits yet. Create one first!");
                return;
            }

            Console.WriteLine("\n=== Your Habit Progress ===");

            foreach (Habit habit in manager.Habits)
            {
                string habitType = habit is WeeklyHabit ? "Weekly" : "Monthly";
                int completedCount = habit.CompletionDates.Count;

                Console.WriteLine($"Habit: {habit.Title} ({habitType})");
                Console.WriteLine($"Description: {habit.Description}");
                Console.WriteLine($"Point Value: {habit.PointValue}");
                Console.WriteLine($"Total Completions: {completedCount}");

                // Show specific progress based on habit type
                if (habit is WeeklyHabit weeklyHabit)
                {
                    int weeklyProgress = weeklyHabit.GetWeeklyProgress();
                    Console.WriteLine($"Completions this week: {weeklyProgress}");
                }
                else if (habit is MonthlyHabit monthlyHabit)
                {
                    int monthlyProgress = monthlyHabit.GetMonthlyProgress();
                    Console.WriteLine($"Completions this month: {monthlyProgress}");
                }

                // Show recent completion dates (up to 5)
                if (completedCount > 0)
                {
                    Console.WriteLine("Recent completions:");

                        var recentDates = habit.CompletionDates
                        .OrderByDescending(date => date)
                        .Take(5);

                    foreach (DateTime date in recentDates)
                    {
                        Console.WriteLine($"  - {date.ToShortDateString()}");
                    }
                }

                Console.WriteLine(); 
            }
        }

        static void SwitchHabitType(HabitManager manager)
        {
            if (manager.Habits.Count == 0)
            {
                Console.WriteLine("\nYou don't have any habits yet. Create one first!");
                return;
            }

            Console.WriteLine("\n=== Switch Habit Type ===");
            Console.WriteLine("Select a habit to change its type (Weekly <-> Monthly):");

            // Display list of habits with their current type
            for (int i = 0; i < manager.Habits.Count; i++)
            {
                Habit habit = manager.Habits[i];
                string habitType = habit is WeeklyHabit ? "Weekly" : "Monthly";
                Console.WriteLine($"{i + 1}. {habit.Title} (Currently: {habitType})");
            }

            Console.Write("\nEnter habit number: ");
            int habitIndex;

            // Validate that the input is a valid integer within the range of existing habits
            if (!int.TryParse(Console.ReadLine(), out habitIndex) || habitIndex < 1 || habitIndex > manager.Habits.Count)
            {
                Console.WriteLine("Invalid selection. Returning to main menu.");
                return;
            }

            Habit selectedHabit = manager.Habits[habitIndex - 1];
            string oldType = selectedHabit is WeeklyHabit ? "Weekly" : "Monthly";

            // Change habit type and return new instance
            Habit newHabit = manager.ChangeHabitType(selectedHabit);

            string newType = newHabit is WeeklyHabit ? "Weekly" : "Monthly";
            Console.WriteLine($"Habit '{newHabit.Title}' changed from {oldType} to {newType}.");
        }

        static void ViewPoints(HabitManager manager)
        {
            Console.WriteLine($"\nTotal Points: {manager.CurrentUser.TotalPoints}");

            if (manager.CurrentUser.TotalPoints == 0)
            {
                Console.WriteLine("You haven't earned any points. Enter a habit to get started.");
            }
            else if (manager.CurrentUser.TotalPoints < 50)
            {
                Console.WriteLine("Keep going! You're doing great!");
            }
            else if (manager.CurrentUser.TotalPoints < 100)
            {
                Console.WriteLine("Impressive progress! You're building good habits!");
            }
            else
            {
                Console.WriteLine("Outstanding achievement! You're a habit master!");
            }
        }
    }
}
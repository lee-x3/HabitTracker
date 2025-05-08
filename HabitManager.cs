using System;
using System.Collections.Generic;
using HabitTracker.Models;

namespace HabitTracker.Managers
{
    
    public class HabitManager
    {
        // Static instance for the singleton pattern
        private static HabitManager _instance;

        
        public static HabitManager Instance
        {
            get
            {
                // Create the instance when first accessed
                if (_instance == null)
                {
                    _instance = new HabitManager();
                }
                return _instance;
            }
        }

        
        public List<Habit> Habits { get; private set; }

        
        public User CurrentUser { get; private set; }

        // Private constructor prevents direct instantiation
        private HabitManager()
        {
            Habits = new List<Habit>();
            CurrentUser = new User("Default User");
        }

        
        public Habit CreateHabit(string title, string description, int points, bool isWeekly)
        {
            Habit newHabit;

            if (isWeekly)
            {
                newHabit = new WeeklyHabit(title, description, points);
            }
            else
            {
                newHabit = new MonthlyHabit(title, description, points);
            }

            Habits.Add(newHabit);
            return newHabit;
        }

        
        public void CompleteHabit(Habit habit)
        {
            if (!habit.IsCompletedToday())
            {
                habit.MarkAsCompleted();
                CurrentUser.AddPoints(habit.PointValue);
            }
        }

        // Changes a habit from weekly to monthly or vice versa
        public Habit ChangeHabitType(Habit habit)
        {
            Habit newHabit;

            if (habit is WeeklyHabit)
            {
                // Convert to Monthly
                newHabit = new MonthlyHabit(habit.Title, habit.Description, habit.PointValue);
            }
            else
            {
                // Convert to Weekly
                newHabit = new WeeklyHabit(habit.Title, habit.Description, habit.PointValue);
            }

            // Copy completion dates to maintain history
            foreach (DateTime date in habit.CompletionDates)
            {
                newHabit.CompletionDates.Add(date);
            }

            // Replace in collection
            int index = Habits.IndexOf(habit);
            if (index >= 0)
            {
                Habits[index] = newHabit;
            }

            return newHabit;
        }
    }
}
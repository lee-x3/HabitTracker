using System;

namespace HabitTracker.Models
{
    
    public class MonthlyHabit : Habit
    {
        public MonthlyHabit(string title, string description, int pointValue)
            : base(title, description, pointValue)
        {
        }

        // Returns the completed date when a habit has been marked completed
        public int GetMonthlyProgress()
        {
            int currentMonth = DateTime.Today.Month;
            int currentYear = DateTime.Today.Year;
            int count = 0;

            foreach (DateTime date in CompletionDates)
            {
                if (date.Month == currentMonth && date.Year == currentYear)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
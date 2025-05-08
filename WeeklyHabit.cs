using System;

namespace HabitTracker.Models
{
    
    public class WeeklyHabit : Habit
    {
        public WeeklyHabit(string title, string description, int pointValue)
            : base(title, description, pointValue)
        {
        }

        
        public int GetWeeklyProgress()
        {
            // Get the start of the current week (Sunday)
            DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            int count = 0;

            foreach (DateTime date in CompletionDates)
            {
                if (date >= startOfWeek && date < startOfWeek.AddDays(7))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
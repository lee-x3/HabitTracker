using System;
using System.Collections.Generic;

namespace HabitTracker.Models
{
    
    public class Habit
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int PointValue { get; set; }
        public List<DateTime> CompletionDates { get; private set; }

        public Habit(string title, string description, int pointValue)
        {
            Title = title;
            Description = description;
            PointValue = pointValue;
            CompletionDates = new List<DateTime>();
        }

        public void MarkAsCompleted()
        {
            CompletionDates.Add(DateTime.Today);
        }

        public bool IsCompletedToday()
        {
            return CompletionDates.Exists(date => date.Date == DateTime.Today);
        }
    }
}
namespace HabitTracker.Models
{
    
    public class User
    {
        public string Name { get; set; }
        public int TotalPoints { get; private set; }

        public User(string name)
        {
            Name = name;
            TotalPoints = 0;
        }

        public void AddPoints(int points)
        {
            TotalPoints += points;
        }
    }
}
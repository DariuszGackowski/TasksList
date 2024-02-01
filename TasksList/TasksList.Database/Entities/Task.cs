namespace TasksList.Database
{
    public class Task
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime AlarmDate { get; set; }
        public bool IsAlarmed { get; set; }
        public bool IsMuted { get; set; }
        public bool IsDone { get; set; }
    }
}

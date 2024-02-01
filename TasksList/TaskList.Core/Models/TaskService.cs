using System.Collections.ObjectModel;
using System.Diagnostics;
using TasksList.Core.Helpers;
using TaskDB = TasksList.Database.Task;

namespace TasksList.Core.ViewModels
{
    public class TaskService
    {
        public static ObservableCollection<TaskModel> TasksList { get; set; } = [];

        //public ICommand AddNewTaskCommand { get; set; }
        //public ICommand DeleteSelectedTaskCommand { get; set; }

        public TaskService()
        {
            //AddNewTaskCommand = new RelayCommand(AddNewTask);
            //RemoveTaskCommand = new RelayCommand(RemoveTask);
        }
        public static void AddNewTask(string description, DateTime selectedDate, int alarmHour, int alarmMinute)
        {
            DateTime combineDate = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, alarmHour, alarmMinute, 0);

            var newTaskDb = new TaskDB
            {
                Description = description,
                AlarmDate = combineDate,
                IsAlarmed = combineDate > DateTime.Now ? false : true,
                IsMuted = false,
                IsDone = false
            };
            DatabaseLocator.Database.Tasks.Add(newTaskDb);
            DatabaseLocator.Database.SaveChanges();

            var newTask = new TaskModel
            {
                Id = newTaskDb.Id,
                Description = newTaskDb.Description,
                AlarmDate = newTaskDb.AlarmDate,
                IsAlarmed = combineDate > DateTime.Now ? false : true,
                IsMuted = newTaskDb.IsMuted,
                IsDone = newTaskDb.IsDone
            };
            TasksList.Add(newTask);
        }
        public static void RemoveTask(int taskId)
        {
            var taskToRemove = TasksList.FirstOrDefault(item => item.Id == taskId);
            if (taskToRemove != null)
            {
                TasksList.Remove(taskToRemove);
                var foundEntity = DatabaseLocator.Database.Tasks.FirstOrDefault(x => x.Id == taskId);
                if (foundEntity != null)
                {
                    DatabaseLocator.Database.Tasks.Remove(foundEntity);
                    DatabaseLocator.Database.SaveChanges();
                }
            }
        }
        public static void EditTask(int taskId, string newDescription, DateTime newSelectedDate, int newAlarmHour, int newAlarmMinute)
        {
            DateTime newCombineDate = new DateTime(newSelectedDate.Year, newSelectedDate.Month, newSelectedDate.Day, newAlarmHour, newAlarmMinute, 0);
            var taskToEdit = TasksList.FirstOrDefault(item => item.Id == taskId);

            if (taskToEdit != null)
            {
                taskToEdit.Description = newDescription;
                taskToEdit.AlarmDate = newCombineDate;
                taskToEdit.IsAlarmed = newCombineDate > DateTime.Now ? false : true;
                taskToEdit.IsMuted = false;
                taskToEdit.IsDone = false;

                var foundEntity = DatabaseLocator.Database.Tasks.FirstOrDefault(x => x.Id == taskId);
                if (foundEntity != null)
                {
                    foundEntity.Description = newDescription;
                    foundEntity.AlarmDate = newCombineDate;
                    foundEntity.IsAlarmed = newCombineDate > DateTime.Now ? false : true;
                    foundEntity.IsMuted = false;
                    foundEntity.IsDone = false;

                    DatabaseLocator.Database.SaveChanges();
                }
            }
        }
        public static void LoadTasks()
        {
            TasksList.Clear();

            foreach (var task in DatabaseLocator.Database.Tasks.ToList())
            {
                TasksList.Add(new TaskModel
                {
                    Id = task.Id,
                    Description = task.Description,
                    AlarmDate = task.AlarmDate,
                    IsMuted = task.IsMuted,
                    IsDone = task.IsDone,
                    IsAlarmed = task.IsAlarmed
                });
            }
        }
        public static void MuteTask(int taskId, bool isMuted)
        {
            var taskToEdit = TasksList.FirstOrDefault(item => item.Id == taskId);

            if (taskToEdit != null)
            {
                taskToEdit.IsMuted = isMuted;

                var foundEntity = DatabaseLocator.Database.Tasks.FirstOrDefault(x => x.Id == taskId);
                if (foundEntity != null)
                {
                    foundEntity.IsMuted = isMuted;

                    DatabaseLocator.Database.SaveChanges();
                }
            }
        }
        public static void DoneTask(int taskId, bool isDone)
        {
            var taskToEdit = TasksList.FirstOrDefault(item => item.Id == taskId);

            if (taskToEdit != null)
            {
                taskToEdit.IsDone = isDone;
                taskToEdit.IsMuted = isDone;
                taskToEdit.IsAlarmed = isDone ? isDone : taskToEdit.IsAlarmed;

                var foundEntity = DatabaseLocator.Database.Tasks.FirstOrDefault(x => x.Id == taskId);
                if (foundEntity != null)
                {
                    foundEntity.IsDone = isDone;
                    foundEntity.IsMuted = isDone;
                    foundEntity.IsAlarmed = isDone ? isDone : taskToEdit.IsAlarmed;

                    DatabaseLocator.Database.SaveChanges();
                }
            }
        }
        public static void SetAlarmedTask(int taskId, bool isAlarmed)
        {
            var taskToEdit = TasksList.FirstOrDefault(item => item.Id == taskId);

            if (taskToEdit != null)
            {
                taskToEdit.IsAlarmed = isAlarmed;

                var foundEntity = DatabaseLocator.Database.Tasks.FirstOrDefault(x => x.Id == taskId);
                if (foundEntity != null)
                {
                    foundEntity.IsAlarmed = isAlarmed;

                    DatabaseLocator.Database.SaveChanges();
                }
            }
        }
    }
}


using System.Windows.Media;
using TasksList.Controls;
using TasksList.Core.ViewModels;

namespace TasksList.ViewModels
{
    public class TaskItemViewModel
    {
        #region Variables
        private bool _isMuted;
        private bool _isDone;
        #endregion

        #region Task methods
        public void RemoveTask(int taskId, TaskItem taskItem)
        {
            TaskService.RemoveTask(taskId);
            MainViewModel.NotesButtonsPanel.Children.Remove(taskItem);
        }
        public void MarkTask(int taskId, TaskItem taskItem)
        {
            FontAwesome.WPF.FontAwesomeIcon _icon = _isDone != true ? FontAwesome.WPF.FontAwesomeIcon.CheckCircle : FontAwesome.WPF.FontAwesomeIcon.CircleThin;
            taskItem.SetValue(TaskItem.IconProperty, _icon);

            FontAwesome.WPF.FontAwesomeIcon _iconBell = _isDone != true ? FontAwesome.WPF.FontAwesomeIcon.BellSlash : FontAwesome.WPF.FontAwesomeIcon.Bell;
            taskItem.SetValue(TaskItem.IconBellProperty, _iconBell);

            Color noteColor = _isDone != true ? (Color)ColorConverter.ConvertFromString(MainViewModel.DoneNoteColor) : (Color)ColorConverter.ConvertFromString(MainViewModel.StandardNoteColor);
            SolidColorBrush _color = new SolidColorBrush(noteColor);
            taskItem.SetValue(TaskItem.ColorProperty, _color);

            _isDone = !_isDone;
            _isMuted = _isDone;
            TaskService.DoneTask(taskId, _isDone);
        }
        public void MuteTask(int taskId, TaskItem taskItem)
        {
            if (_isDone) return;

            FontAwesome.WPF.FontAwesomeIcon _iconBell = _isMuted != true ? FontAwesome.WPF.FontAwesomeIcon.BellSlash : FontAwesome.WPF.FontAwesomeIcon.Bell;
            taskItem.SetValue(TaskItem.IconBellProperty, _iconBell);

            _isMuted ^= true;
            TaskService.MuteTask(taskId, _isMuted);
        }

        public void EditTask(int taskId, string description, string time, TaskItem taskItem)
        {
            MainViewModel.NoteIsEdited = true;
            MainViewModel.DescriptionNoteTextBox.Text = description;
            string[] hoursMinutes = time.Split(':');
            MainViewModel.HoursTextBox.Text = hoursMinutes[0];
            MainViewModel.MinutesTextBox.Text = hoursMinutes[1];
            MainViewModel.EditedId = taskId;

            MainViewModel.NotesButtonsPanel.Children.Remove(taskItem);
        }
        #endregion
    }
}

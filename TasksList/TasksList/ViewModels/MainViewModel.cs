using Hardcodet.Wpf.TaskbarNotification;
using System.Drawing;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using TasksList.Controls;
using TasksList.Core.ViewModels;
using TasksList.Windows;
using ColorConverter = System.Windows.Media.ColorConverter;
using Color = System.Windows.Media.Color;
using TasksList.Views;
using Calendar = System.Windows.Controls.Calendar;

namespace TasksList.ViewModels
{
    internal class MainViewModel
    {
        #region Variables
        public static bool NoteIsEdited;
        public static int EditedId;
        public static string StandardNoteColor = "#f1f1f1";
        public static string DoneNoteColor = "#7ED6D9";

        private const string AddNoteText = "Add Note";
        private const string DoubleUnderscore = "__";
        private const string DoubleZero = "00";

        private readonly List<string> _months = ["None", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        private readonly List<string> _days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

        private int _currentYear;
        private int _currentMonth;
        private int _currentDay;

        private Style _buttonsStyle;
        private Style _chosenButtonsStyle;
        private Style _monthButtonsStyle;
        private Style _chosenMonthButtonsStyle;
        private Style _angleStyle;

        private StackPanel _yearsButtonsPanel;
        private StackPanel _monthsButtonsPanel;
        private static StackPanel _notesButtonsPanel;
        private static TextBox _hoursTextBox;
        private static TextBox _minutesTextBox;
        private static TextBox _descriptionNoteTextBox;
        private Calendar _Calendar;
        private TextBlock _monthsTextBlock;
        private TextBlock _dayNumberTextBlock;
        private TextBlock _monthNameTextBlock;
        private TextBlock _dayNameTextBlock;

        private MainPage _mainPage;

        private TaskbarIcon _taskbarIcon;
        #endregion

        #region Properties 
        public List<string> Months
        {
            get => _months;
        }
        public List<string> Days
        {
            get => _days;
        }
        public MainPage MainPage
        {
            get => _mainPage;
            set => _mainPage = value;
        }
        public Calendar Calendar
        {
            get => _Calendar;
            set => _Calendar = value;
        }
        public TextBlock MonthsTextBlock
        {
            get => _monthsTextBlock;
            set => _monthsTextBlock = value;
        }
        public TextBlock DayNumberTextBlock
        {
            get => _dayNumberTextBlock;
            set => _dayNumberTextBlock = value;
        }
        public TextBlock DayNameTextBlock
        {
            get => _dayNameTextBlock;
            set => _dayNameTextBlock = value;
        }
        public TextBlock MonthNameTextBlock
        {
            get => _monthNameTextBlock;
            set => _monthNameTextBlock = value;
        }
        public static StackPanel NotesButtonsPanel
        {
            get => _notesButtonsPanel;
            set => _notesButtonsPanel = value;
        }
        public StackPanel YearsButtonsPanel
        {
            get => _yearsButtonsPanel;
            set => _yearsButtonsPanel = value;
        }
        public StackPanel MonthsButtonsPanel
        {
            get => _monthsButtonsPanel;
            set => _monthsButtonsPanel = value;
        }
        public static TextBox HoursTextBox
        {
            get => _hoursTextBox;
            set => _hoursTextBox = value;
        }
        public static TextBox MinutesTextBox
        {
            get => _minutesTextBox;
            set => _minutesTextBox = value;
        }
        public static TextBox DescriptionNoteTextBox
        {
            get => _descriptionNoteTextBox;
            set => _descriptionNoteTextBox = value;
        }
        public Style MonthButtonsStyle
        {
            get => _monthButtonsStyle;
            set => _monthButtonsStyle = value;
        }
        public Style ChosenMonthButtonsStyle
        {
            get => _chosenMonthButtonsStyle;
            set => _chosenMonthButtonsStyle = value;
        }
        public Style ButtonStyle
        {
            get => _buttonsStyle;
            set => _buttonsStyle = value;
        }
        public Style ChosenButtonStyle
        {
            get => _chosenButtonsStyle;
            set => _chosenButtonsStyle = value;
        }
        public Style AngleStyle
        {
            get => _angleStyle;
            set => _angleStyle = value;
        }
        #endregion

        public MainViewModel(MainPage mainPage, TextBlock monthsTextBlock, TextBlock dayNumberTextBlock, TextBlock monthNameTextBlock, TextBlock dayNameTextBlock, Calendar calendar, StackPanel yearsButtonsPanel, StackPanel monthsButtonsPanel,
            Style monthButton, Style chosenMonthButton, Style button, Style chosenButton, Style angle, TextBox hoursTextBox, TextBox minutesTextBox, TextBox descriptionNoteTextBox, StackPanel notesButtonsPanel)
        {
            MainPage = mainPage;
            MonthsTextBlock = monthsTextBlock;
            DayNumberTextBlock = dayNumberTextBlock;
            MonthNameTextBlock = monthNameTextBlock;
            DayNameTextBlock = dayNameTextBlock;
            Calendar = calendar;
            YearsButtonsPanel = yearsButtonsPanel;
            MonthsButtonsPanel = monthsButtonsPanel;
            MonthButtonsStyle = monthButton;
            ChosenMonthButtonsStyle = chosenMonthButton;
            ButtonStyle = button;
            ChosenButtonStyle = chosenButton;
            AngleStyle = angle;
            HoursTextBox = hoursTextBox;
            MinutesTextBox = minutesTextBox;
            DescriptionNoteTextBox = descriptionNoteTextBox;
            NotesButtonsPanel = notesButtonsPanel;

            InitializeCalendar();
            InitializeNotifications();
            InitializeTimer();
        }

        #region Timer
        private void InitializeTimer()
        {
            DispatcherTimer timer = new()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += TimerTick;
            timer.Start();
        }
        private void TimerTick(object? sender, EventArgs e)
        {
            var selectedByDayTaskList = TaskService.TasksList.Where(task => task.AlarmDate.Date == DateTime.Now.Date && !task.IsAlarmed && !task.IsMuted);

            foreach (var task in selectedByDayTaskList)
            {
                if (task.AlarmDate.Hour == DateTime.Now.Hour && task.AlarmDate.Minute == DateTime.Now.Minute)
                {
                    ShowNotification("TasksList", task.Description);
                    TaskService.SetAlarmedTask(task.Id, isAlarmed: true);
                }
            }
        }
        #endregion

        #region Notifications
        private void InitializeNotifications()
        {
            _taskbarIcon = new TaskbarIcon();

            byte[] iconBytes = Properties.Resources.icon;
            using (MemoryStream ms = new(iconBytes))
            {
                Icon icon = new(ms);
                _taskbarIcon.Icon = icon;
            }
            _taskbarIcon.Visibility = Visibility.Visible;
        }
        private void ShowNotification(string title, string text)
        {
            _taskbarIcon.ShowBalloonTip(title, text, BalloonIcon.Info);
        }
        #endregion

        #region Tasks
        private void SpawnTaskByDate(DateTime selectedDate)
        {
            TaskService.LoadTasks();

            NotesButtonsPanel.Children.Clear();
            var sortedTaskList = TaskService.TasksList.OrderBy(task => task.AlarmDate).ToList();

            foreach (var task in sortedTaskList)
            {
                if (task.AlarmDate.Date != selectedDate.Date) continue;

                Color noteColor = task.IsDone == true ? (Color)ColorConverter.ConvertFromString(DoneNoteColor) : (Color)ColorConverter.ConvertFromString(StandardNoteColor);
                TaskItem taskItem = new()
                {
                    TaskId = task.Id,
                    Description = task.Description,
                    Time = $"{task.AlarmDate.Hour:D2}:{task.AlarmDate.Minute:D2}",
                    Color = new SolidColorBrush(noteColor),
                    Icon = task.IsDone == true ? FontAwesome.WPF.FontAwesomeIcon.CheckCircle : FontAwesome.WPF.FontAwesomeIcon.CircleThin,
                    IconBell = task.IsMuted == true ? FontAwesome.WPF.FontAwesomeIcon.BellSlash : FontAwesome.WPF.FontAwesomeIcon.Bell
                };
                NotesButtonsPanel.Children.Add(taskItem);
            }
        }
        #endregion

        #region Calendar
        public void CalendarDisplayDate(CalendarDateChangedEventArgs e)
        {
            DateTime? newDisplayDate = e.AddedDate;

            int selectedYear = newDisplayDate.Value.Year;
            ClearYearsButtons();
            GenerateYearsButtons(selectedYear);

            int selectedMonth = newDisplayDate.Value.Month;
            ClearMonthsButtons();
            GenerateMonthsButtons(selectedMonth);
            UpdateChosenMonth(selectedMonth);
        }
        private void InitializeCalendar()
        {
            GenerateYearsButtons();
            GenerateMonthsButtons();
            UpdateChosenMonth();
            UpdateSelectedDay();
        }
        private void SetNewYear(int year)
        {
            DateTime currentDate = Calendar.DisplayDate;
            DateTime newData = new(year, currentDate.Month, currentDate.Day);

            Calendar.SelectedDate = Calendar.DisplayDate = newData;

            SpawnTaskByDate(Calendar.SelectedDate.Value);
        }
        private void SetNewMonth(int month)
        {
            DateTime currentDate = Calendar.DisplayDate;
            DateTime newData = new(currentDate.Year, month, currentDate.Day);

            Calendar.SelectedDate = Calendar.DisplayDate = newData;

            SpawnTaskByDate(Calendar.SelectedDate.Value);
        }
        #region Calendar - Years
        public void ChangeYearButton(object sender)
        {
            Button button = (Button)sender;

            int selectedYear = Convert.ToInt32(button.Content);
            if (selectedYear.Equals(_currentYear)) return;

            ClearYearsButtons();
            GenerateYearsButtons(selectedYear);

            SetNewYear(selectedYear);
        }
        public void ChangeYearLeftButton()
        {
            int yearToSetup = --_currentYear;

            ClearYearsButtons();
            GenerateYearsButtons(yearToSetup);

            SetNewYear(yearToSetup);
        }
        public void ChangeYearRightButton()
        {
            int yearToSetup = ++_currentYear;

            ClearYearsButtons();
            GenerateYearsButtons(yearToSetup);

            SetNewYear(yearToSetup);
        }
        private void GenerateYearsButtons(int? currentYear = null)
        {
            DateTime currentDate = DateTime.Today;
            currentYear ??= currentDate.Year;
            int startYear = (int)currentYear - 2;
            int endYear = (int)currentYear + 2;

            for (int year = startYear; year <= endYear; year++)
            {
                Button button;
                if (year != currentYear)
                {
                    button = new Button
                    {
                        Content = year.ToString(),
                        Style = ButtonStyle
                    };
                }
                else
                {
                    button = new Button
                    {
                        Content = year.ToString(),
                        Style = ChosenButtonStyle
                    };
                }
                button.Click += MainPage.YearButtonClick;
                YearsButtonsPanel.Children.Add(button);

                _currentYear = (int)currentYear;
            }
        }
        private void ClearYearsButtons()
        {
            YearsButtonsPanel.Children.Clear();
        }
        #endregion

        #region Calendar - Months
        public void ChangeMonthButton(object sender)
        {
            Button button = (Button)sender;

            int selectedMonth = Convert.ToInt32(button.Content);
            if (selectedMonth.Equals(_currentMonth)) return;

            ClearMonthsButtons();
            GenerateMonthsButtons(selectedMonth);
            UpdateChosenMonth(selectedMonth);
            SetNewMonth(selectedMonth);
        }
        private void GenerateMonthsButtons(int? currentMonth = null)
        {
            DateTime currentDate = DateTime.Today;
            currentMonth ??= currentDate.Month;

            for (int month = 1; month <= 12; month++)
            {
                Button button;
                if (month != currentMonth)
                {
                    button = new Button
                    {
                        Content = month.ToString(),
                        Style = MonthButtonsStyle
                    };
                }
                else
                {
                    button = new Button
                    {
                        Content = month.ToString(),
                        Style = ChosenMonthButtonsStyle
                    };
                }
                button.Click += MainPage.MonthButtonClick;
                MonthsButtonsPanel.Children.Add(button);

                _currentMonth = (int)currentMonth;
            }
        }
        private void ClearMonthsButtons()
        {
            MonthsButtonsPanel.Children.Clear();
        }
        private void UpdateChosenMonth(int? selectedMonth = null)
        {
            DateTime currentDate = DateTime.Today;
            selectedMonth ??= currentDate.Month;

            string month = Months[(int)selectedMonth];
            MonthsTextBlock.Text = month;
        }
        #endregion

        #region Calendar - Day
        public void ChangeDayLeftButton()
        {
            int dayToSetup = --_currentDay;
            DateTime selectedDay = Calendar.SelectedDate ?? DateTime.Today;
            if (dayToSetup != 0)
            {
                Calendar.SelectedDate = new DateTime(selectedDay.Year, selectedDay.Month, dayToSetup);
            }
            else
            {
                int newMonth = selectedDay.Month - 1;
                if (newMonth != 0)
                {
                    _currentYear = selectedDay.Year;
                    _currentMonth = newMonth;
                }
                else
                {
                    _currentYear = selectedDay.Year - 1;
                    _currentMonth = 12;
                }
                _currentDay = DateTime.DaysInMonth(_currentYear, _currentMonth);

                Calendar.SelectedDate = new DateTime(_currentYear, _currentMonth, _currentDay);
            }
            Calendar.DisplayDate = Calendar.SelectedDate.Value;

            SpawnTaskByDate(Calendar.SelectedDate.Value);
        }
        public void ChangeDayRightButton()
        {
            int dayToSetup = ++_currentDay;
            DateTime selectedDay = Calendar.SelectedDate ?? DateTime.Today;
            int maxDaysInSelectedMonth = DateTime.DaysInMonth(selectedDay.Year, selectedDay.Month);

            if (dayToSetup <= maxDaysInSelectedMonth)
            {
                Calendar.SelectedDate = new DateTime(selectedDay.Year, selectedDay.Month, dayToSetup);
            }
            else
            {
                int newMonth = selectedDay.Month + 1;
                if (newMonth <= 12)
                {
                    _currentYear = selectedDay.Year;
                    _currentMonth = newMonth;
                }
                else
                {
                    _currentYear = selectedDay.Year + 1;
                    _currentMonth = 1;
                }
                _currentDay = 1;

                Calendar.SelectedDate = new DateTime(_currentYear, _currentMonth, _currentDay);
            }
            Calendar.DisplayDate = Calendar.SelectedDate.Value;

            SpawnTaskByDate(Calendar.SelectedDate.Value);
        }
        public void UpdateSelectedDay()
        {
            DateTime selectedDay = Calendar.SelectedDate ?? DateTime.Today;

            DayNumberTextBlock.Text = selectedDay.Day.ToString();
            string month = Months[selectedDay.Month];
            MonthNameTextBlock.Text = month;
            string day = Days[(int)selectedDay.DayOfWeek];
            DayNameTextBlock.Text = day;
            _currentDay = selectedDay.Day;

            SpawnTaskByDate(selectedDay);
        }
        #endregion

        #endregion

        #region Message box
        private void ShowCustomMessageBox(string message)
        {
            CustomMessageBox customMessageBox = new()
            {
                Message = message
            };
            customMessageBox.ShowDialog();
        }
        #endregion

        #region Note

        #region Note - time
        public bool ContainsOnlyDigits(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }
        public void CheckTimeTextBox(object sender)
        {
            TextBox textBox = (TextBox)sender;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = DoubleUnderscore;
            }
            else
            {
                if (int.TryParse(textBox.Text, out int value))
                {
                    if (textBox == HoursTextBox)
                    {
                        if (value < 0 || value > 23)
                        {
                            ShowCustomMessageBox("Enter correct hour (0-23).");
                            textBox.Text = DoubleZero;
                        }
                    }
                    else if (textBox == MinutesTextBox)
                    {
                        if (value < 0 || value > 59)
                        {
                            ShowCustomMessageBox("Enter correct minutes (0-59).");
                            textBox.Text = DoubleZero;
                        }
                    }
                }
                else
                {
                    ShowCustomMessageBox("Enter correct number.");
                    textBox.Text = DoubleZero;
                }
            }
        }
        public void SetEmptyTextToTimeTextBox(object sender)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == DoubleUnderscore)
            {
                textBox.Text = string.Empty;
            }
        }
        #endregion

        #region Note - description
        public void SetEmptyTextToDescription(object sender)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == AddNoteText)
            {
                textBox.Text = string.Empty;
            }
        }

        public void SetAddNoteTextToDescription(object sender)
        {
            TextBox textBox = (TextBox)sender;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = AddNoteText;
            }
        }
        #endregion

        #region Note - creating
        public void AddNote()
        {
            if (!int.TryParse(HoursTextBox.Text, out int hours))
            {
                ShowCustomMessageBox("Enter correct hour (0-23).");
                return;
            }

            if (!int.TryParse(MinutesTextBox.Text, out int minutes))
            {
                ShowCustomMessageBox("Enter correct minutes (0-59).");
                return;
            }
            DateTime selectedDate = Calendar.SelectedDate == null ? DateTime.Now.Date : Calendar.SelectedDate.Value;
            string description = DescriptionNoteTextBox.Text;

            if (!NoteIsEdited)
            {
                TaskService.AddNewTask(description, selectedDate, hours, minutes);
            }
            else
            {
                TaskService.EditTask(EditedId, description, selectedDate, hours, minutes);
                NoteIsEdited = false;
            }

            SpawnTaskByDate(selectedDate);

            HoursTextBox.Text = DoubleUnderscore;
            MinutesTextBox.Text = DoubleUnderscore;
            DescriptionNoteTextBox.Text = AddNoteText;
        }
        #endregion

        #endregion
    }
}

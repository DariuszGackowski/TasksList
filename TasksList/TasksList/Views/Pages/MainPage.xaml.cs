using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TasksList.ViewModels;

namespace TasksList.Views
{
    /// <summary>
    /// Logika interakcji dla klasy MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private readonly MainViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();

            _viewModel = new(this, monthsTextBlock, dayNumberTextBlock, monthNameTextBlock, dayNameTextBlock, calendar, yearsButtonsPanel, monthsButtonsPanel,
                (Style)FindResource("monthButton"), (Style)FindResource("chosenMonthButton"), (Style)FindResource("button"), (Style)FindResource("chosenButton"), (Style)FindResource("imageAwesome"),
                hoursTextBox, minutesTextBox, descriptionNoteTextBox, notesButtonsPanel);

            DataContext = _viewModel;
        }

        #region Main page methods
        public void MonthButtonClick(object sender, RoutedEventArgs e)
        {
            _viewModel.ChangeMonthButton(sender);
        }
        public void YearButtonClick(object sender, RoutedEventArgs e)
        {
            _viewModel.ChangeYearButton(sender);
        }
        private void AddNoteButtonClick(object sender, RoutedEventArgs e)
        {
            _viewModel.AddNote();
        }
        private void DescriptionTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.SetEmptyTextToDescription(sender);
        }
        private void DescriptionTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Application.Current.MainWindow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                Keyboard.ClearFocus();
                e.Handled = true;
            }
        }
        private void DescriptionTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.SetAddNoteTextToDescription(sender);
        }
        private void TimeTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Application.Current.MainWindow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                Keyboard.ClearFocus();
                e.Handled = true;
            }
        }
        private void TimeTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.SetEmptyTextToTimeTextBox(sender);
        }
        private void TimeTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.CheckTimeTextBox(sender);
        }
        private void TextBoxPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_viewModel.ContainsOnlyDigits(e.Text);
        }
        private void BorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                Application.Current.MainWindow.DragMove();
        }
        private void MinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
        private void MainWindowPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            Keyboard.ClearFocus();
        }
        private void ChangeDayLeftButtonClick(object sender, RoutedEventArgs e)
        {
            _viewModel.ChangeDayLeftButton();
        }
        private void ChangeDayRightButtonClick(object sender, RoutedEventArgs e)
        {
            _viewModel.ChangeDayRightButton();
        }
        private void CalendarSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.UpdateSelectedDay();

            Mouse.Capture(null);
        }
        private void ChangeYearLeftButtonClick(object sender, RoutedEventArgs e)
        {
            _viewModel.ChangeYearLeftButton();
        }
        private void ChangeYearRightButtonClick(object sender, RoutedEventArgs e)
        {
            _viewModel.ChangeYearRightButton();
        }
        private void CalendarDisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            _viewModel.CalendarDisplayDate(e);

            Mouse.Capture(null);
        }
        #endregion
    }
}

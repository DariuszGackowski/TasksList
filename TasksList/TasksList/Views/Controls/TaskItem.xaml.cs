using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TasksList.ViewModels;

namespace TasksList.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy TaskItem.xaml
    /// </summary>
    public partial class TaskItem : UserControl
    {
        private TaskItemViewModel _viewModel;

        public TaskItem()
        {
            InitializeComponent();

            _viewModel = new();
            DataContext = _viewModel;
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            _viewModel.RemoveTask(TaskId,this);
        }
        private void CheckButtonClick(object sender, RoutedEventArgs e)
        {
            _viewModel.MarkTask(TaskId, this);
        }
        private void MuteButtonClick(object sender, RoutedEventArgs e)
        {
            _viewModel.MuteTask(TaskId, this);
        }
        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            _viewModel.EditTask(TaskId, Description, Time, this);
        }


        #region DependencyProperties
        public int TaskId
        {
            get
            {
                return (int)GetValue(TaskIdProperty);
            }
            set
            {
                SetValue(TaskIdProperty, value);
            }
        }
        public static readonly DependencyProperty TaskIdProperty = DependencyProperty.Register("TaskId", typeof(int), typeof(TaskItem));
        public string Description
        {
            get
            {
                return (string)GetValue(DescriptionProperty);
            }
            set
            {
                SetValue(DescriptionProperty, value);
            }
        }
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(TaskItem));

        public string Time
        {
            get
            {
                return (string)GetValue(TimeProperty);
            }
            set
            {
                SetValue(TimeProperty, value);
            }
        }
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(string), typeof(TaskItem));

        public SolidColorBrush Color
        {
            get
            {
                return (SolidColorBrush)GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
            }
        }
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(SolidColorBrush), typeof(TaskItem));

        public FontAwesome.WPF.FontAwesomeIcon Icon
        {
            get
            {
                return (FontAwesome.WPF.FontAwesomeIcon)GetValue(IconProperty);
            }
            set
            {
                SetValue(IconProperty, value);
            }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(FontAwesome.WPF.FontAwesomeIcon), typeof(TaskItem));

        public FontAwesome.WPF.FontAwesomeIcon IconBell
        {
            get
            {
                return (FontAwesome.WPF.FontAwesomeIcon)GetValue(IconBellProperty);
            }
            set
            {
                SetValue(IconBellProperty, value);
            }
        }
        public static readonly DependencyProperty IconBellProperty = DependencyProperty.Register("IconBell", typeof(FontAwesome.WPF.FontAwesomeIcon), typeof(TaskItem));
        #endregion
    }
}

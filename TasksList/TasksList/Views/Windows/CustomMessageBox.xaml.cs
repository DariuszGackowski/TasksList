using System.Windows;

namespace TasksList.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public string Message
        {
            get 
            { 
                return (string)GetValue(MessageProperty); 
            }
            set 
            { 
                SetValue(MessageProperty, value); 
            }
        }
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(CustomMessageBox));

        public CustomMessageBox()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void OKButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

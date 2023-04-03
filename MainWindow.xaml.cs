using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;


namespace NotesApp
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Task> tasks;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadTasks();

        }

        public class TaskGroup
        {
            public string GroupName { get; set; }
            public ObservableCollection<Task> Tasks { get; set; }
        }

        public ObservableCollection<Task> Tasks
        {
            get { return tasks; }
        }

        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTask.Text)) // проверка на пустой текст в TextBox
            {
                tasks.Add(new Task { Name = txtTask.Text });
                txtTask.Text = "";
            }
        }

        private void BtnRemoveCompleted_Click(object sender, RoutedEventArgs e)
        {
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                if (tasks[i].IsCompleted)
                {
                    tasks.RemoveAt(i);
                }
            }
        }

        private void BtnSaveTasks_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Task>));
            using (TextWriter writer = new StreamWriter("tasks.xml"))
            {
                serializer.Serialize(writer, tasks);
            }
            MessageBox.Show("Сохранено");
        }

        private void LoadTasks()
        {
            if (File.Exists("tasks.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Task>));
                using (TextReader reader = new StreamReader("tasks.xml"))
                {
                    tasks = (ObservableCollection<Task>)serializer.Deserialize(reader);
                }
            }
        }

        private void txtTask_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                BtnAddTask_Click(null, null); // вызов метода BtnAddTask_Click с имитацией нажатия кнопки
            }

        }
    }

    public class Task
    {
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
    }

}



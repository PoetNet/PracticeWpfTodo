using System.ComponentModel;
using System.Windows;
using WpfApp.Models;
using WpfApp.Serivces;

namespace WpfApp;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly string Path = $"{Environment.CurrentDirectory}\\todoDataList.json";
    private BindingList<TodoModel> _todoDataList;
    private FileIOService _fileIOService;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _fileIOService = new FileIOService(Path);

        try
        {
            _todoDataList = _fileIOService.LoadData() ?? new();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            Close();
        }

        dgTodoList.ItemsSource = _todoDataList;
        _todoDataList.ListChanged += _todoDataList_ListChanged;
    }

    private void _todoDataList_ListChanged(object sender, ListChangedEventArgs e)
    {
        if (e.ListChangedType == ListChangedType.ItemAdded ||
            e.ListChangedType == ListChangedType.ItemDeleted ||
            e.ListChangedType == ListChangedType.ItemChanged)
        {
            try
            {
                _fileIOService.SaveData(sender);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }
    }

    private void AddTodo_Click(object sender, RoutedEventArgs e)
    {
        var newTodoText = tbNewTodoText.Text.Trim();
        if (!string.IsNullOrWhiteSpace(newTodoText))
        {
            _todoDataList.Add(new TodoModel(newTodoText));
            tbNewTodoText.Clear();
        }
        else
        {
            MessageBox.Show("Please enter a valid todo text.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}

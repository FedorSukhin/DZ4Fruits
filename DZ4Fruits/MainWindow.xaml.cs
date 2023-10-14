using DZ4Fruits.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DZ4Fruits
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DBFruitClient dbfruitClient;
        private FruitsVegetable fvtemp;
        private int qweryItem;
        public MainWindow()
        {
            dbfruitClient = new DBFruitClient(new DbConnectionProvider());
            InitializeComponent();
            updateFVList();
            fvtemp = new FruitsVegetable();
        }

        // обнавление листu
        private void updateFVList()
        {
            try
            {
                // обновление списка объектов (можно придумать рациональный способ)
                List<FruitsVegetable> frutveg = dbfruitClient.SelectAll();
                frutsListBox.Items.Clear();
                frutveg.ForEach(frutveget => frutsListBox.Items.Add(frutveget));
                maxCalorie();
                minCalorie();
                avgCalorie();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during select object list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //Menu
        //1 проверка соединения 
        private void testDbConnection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // проверка соединения с БД
                DbConnectionProvider provider = new DbConnectionProvider();
                using (SqlConnection connection = provider.OpenDbConnection())
                {
                    MessageBox.Show("Connection is ok", "Connected", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //2 обработка команды отобразить все
        private void viewAllFV_Click(object sender, RoutedEventArgs e)
        {
            updateFVList();
        }
        //3 обработка команды вывести все названия
        private void viewAllName1(object sender, RoutedEventArgs e)
        {
            try
            {
                List<NameFV> frutveg = dbfruitClient.AllName();
                frutsListBox.Items.Clear();
                frutveg.ForEach(frutveget => frutsListBox.Items.Add(frutveget));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during select object list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //4 вывести все цвета
        private void viewAllColor_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                List<NameFV> frutveg = dbfruitClient.AllColor();
                frutsListBox.Items.Clear();
                frutveg.ForEach(frutveget => frutsListBox.Items.Add(frutveget));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during select object list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // отображение калорийности

        private void maxCalorie()
        {
            try
            {
                MaxTextBox.Text = Convert.ToString(dbfruitClient.MaxCalorie());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during select object list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void minCalorie()
        {
            try
            {
                MinTextBox.Text = Convert.ToString(dbfruitClient.MinCalorie());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during select object list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void avgCalorie()
        {
            try
            {
                AvgTextBox.Text = Convert.ToString(dbfruitClient.AVGCalorie());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during select object list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // отработка кнопок
        // выделение одного объекта листа
        private void frutsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var selectedItem = frutsListBox.SelectedItem as FruitsVegetable;
                // Проверяем, что элемент не пустой
                if (selectedItem != null)
                {
                    addNameTextBox.Text = selectedItem.Name;
                    addTypeTextBox.Text = selectedItem.Type;
                    addColorTextBox.Text = selectedItem.Color;
                    addCalorieTextBox.Text = selectedItem.Calorie.ToString();
                    fvtemp = selectedItem;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during select object list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // отработка кнопки добавить
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = addNameTextBox.Text;
                string type = addTypeTextBox.Text;
                string color = addColorTextBox.Text;
                int calorie = Convert.ToInt32(addCalorieTextBox.Text);
                FruitsVegetable fruitsVegetable = new FruitsVegetable() { Name = name, Type = type, Color = color, Calorie = calorie };
                dbfruitClient.AddFruitVegetable(fruitsVegetable);
                updateFVList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during select object list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //отработка кнопки удалить
        private void delButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dbfruitClient.DelFruitVegetable(fvtemp.Id);
                updateFVList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during select object list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //обработка кнопки обнавить
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dbfruitClient.Update(fvtemp.Id, addNameTextBox.Text, addTypeTextBox.Text, addColorTextBox.Text, Convert.ToInt32(addCalorieTextBox.Text));
                updateFVList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during select object list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void resultButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string vegcount = "Veg count";
                string fruitcount = "Fruit count";
                string colorFV = "Color FV";
                if (qweryItem ==0) 
                {
                    qweryResult.Text=Convert.ToString(dbfruitClient.CountVegetable());
                }
                if (qweryItem == 1)
                {
                    qweryResult.Text = Convert.ToString(dbfruitClient.CountFruit());
                }
                if (qweryItem == 2)
                {
                    qweryResult.Text = Convert.ToString(dbfruitClient.CountColor(qweryParam.Text));
                }
            }
            catch (Exception ex )
            {
                MessageBox.Show($"Error during select object list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void qweryChois_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            qweryItem = qweryChois.SelectedIndex;
        }
    }
}

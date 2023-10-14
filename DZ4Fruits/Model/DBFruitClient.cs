using DZ4Fruits.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace DZ4Fruits.Model
{
    internal class DBFruitClient
    {
        // провайдер подключения к БД
        private DbConnectionProvider connectionProvider;

        // конструктор
        public DBFruitClient(DbConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }
        // 1. получить все записи
        public List<FruitsVegetable> SelectAll()
        {
            using (SqlConnection connection = connectionProvider.OpenDbConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Fruits_Vegetables_t order by id;", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // считать строки результат в List<FruitsVegetable>
                    return ReadSelectResult(reader);
                }
            }
        }
        // 2. добавить запись
        public void AddFruitVegetable(FruitsVegetable fruitsVegetable)
        {
            using (SqlConnection connection = connectionProvider.OpenDbConnection())
            {
                SqlCommand command = new SqlCommand(
                     " INSERT INTO Fruits_Vegetables_t (Name_f, Type_f, Color_f, Calorie_f) " +
                     "VALUES (@Name_f, @Type_f, @Color_f, @Calorie_f);",
                    connection);
                command.Parameters.Add("@Name_f", System.Data.SqlDbType.NVarChar).Value = fruitsVegetable.Name;
                command.Parameters.Add("@Type_f", System.Data.SqlDbType.NVarChar).Value = fruitsVegetable.Type;
                command.Parameters.Add("@Color_f", System.Data.SqlDbType.NVarChar).Value = fruitsVegetable.Color;
                command.Parameters.Add("@Calorie_f", System.Data.SqlDbType.Int).Value = fruitsVegetable.Calorie;
                int rowsAffect = command.ExecuteNonQuery();
                if (rowsAffect != 1)
                {
                    throw new Exception($"rowsAffect {rowsAffect} !=1");
                }
            }
        }
        // 3. Удалить запись

        public void DelFruitVegetable(int index)
        {
            using (SqlConnection connection = connectionProvider.OpenDbConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "delete from Fruits_Vegetables_t where id=@id;",
                    connection);
                cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = index;
                int rowsAffect = cmd.ExecuteNonQuery();
                if (rowsAffect != 1)
                {
                    throw new Exception($"rowsAffect {rowsAffect} !=1");
                }
            }
        }
        //4. Обновить данные

        public void Update(int id, string name, string type, string color, int calorie)
        {
            using (SqlConnection connection = connectionProvider.OpenDbConnection())
            {
                SqlCommand command = new SqlCommand(
                     "update Fruits_Vegetables_t set Name_f = @Name_f, Type_f = @Type_f, Color_f = @Color_f, Calorie_f = @Calorie_f  where id = @id;",
                    connection);
                command.Parameters.Add("@id", System.Data.SqlDbType.NVarChar).Value = id;
                command.Parameters.Add("@Name_f", System.Data.SqlDbType.NVarChar).Value = name;
                command.Parameters.Add("@Type_f", System.Data.SqlDbType.NVarChar).Value = type;
                command.Parameters.Add("@Color_f", System.Data.SqlDbType.NVarChar).Value = color;
                command.Parameters.Add("@Calorie_f", System.Data.SqlDbType.Int).Value = calorie;
                int rowsAffect = command.ExecuteNonQuery();
                if (rowsAffect != 1)
                {
                    throw new Exception($"rowsAffect {rowsAffect} !=1");
                }
            }
        }
        // 5. вывести все названия 
        public List<NameFV> AllName()
        {
            using (SqlConnection connection = connectionProvider.OpenDbConnection())
            {
                SqlCommand command = new SqlCommand(
                     " select DISTINCT Name_f from Fruits_Vegetables_t;",
                    connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // считать строки результат в List<NameFV>
                    return ReadSelectNameResult(reader);
                }
            }
        }
        // 6. вывести все цвета

        public List<NameFV> AllColor()
        {
            using (SqlConnection connection = connectionProvider.OpenDbConnection())
            {
                SqlCommand command = new SqlCommand(
                     " select DISTINCT Color_f from Fruits_Vegetables_t;",
                    connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // считать строки результат в List<NameFV>
                    return ReadSelectColorResult(reader);
                }
            }
        }
        // 7. максимальная калорийность
        public int MaxCalorie()
        {
            using (SqlConnection connection = connectionProvider.OpenDbConnection())
            {
                SqlCommand cmd = new SqlCommand(
                     "select max(Calorie_f) from Fruits_Vegetables_t;",
                    connection);
                return (int)cmd.ExecuteScalar();
            }
        }
        // 8. минимальная калорийность
        public int MinCalorie()
        {
            using (SqlConnection connection = connectionProvider.OpenDbConnection())
            {
                SqlCommand cmd = new SqlCommand(
                     "select min(Calorie_f) from Fruits_Vegetables_t;",
                    connection);
                return (int)cmd.ExecuteScalar();
            }
        }
        // 9. средняя калорийность
        public int AVGCalorie()
        {
            using (SqlConnection connection = connectionProvider.OpenDbConnection())
            {
                SqlCommand cmd = new SqlCommand(
                     "select avg(Calorie_f) from Fruits_Vegetables_t;",
                    connection);
                return (int)cmd.ExecuteScalar();
            }
        }
        // 10. колличество овощей

        public int CountVegetable()
        {
            using (SqlConnection connection = connectionProvider.OpenDbConnection())
            {
                SqlCommand cmd = new SqlCommand(
                     "select count(*) from Fruits_Vegetables_t where Type_f like 'vegetable' or Type_f like 'Vegetable';",
                    connection);
                return (int)cmd.ExecuteScalar();
            }
        }
        // 11. колличество фруктов

        public int CountFruit()
        {
            using (SqlConnection connection = connectionProvider.OpenDbConnection())
            {
                SqlCommand cmd = new SqlCommand(
                     "select count(*) from Fruits_Vegetables_t where Type_f like 'Fruit' or Type_f like 'fruit';",
                    connection);
                return (int)cmd.ExecuteScalar();
            }
        }
        // 12. показать колличество овощей и фруктов заданного цвета

        public int CountColor(string color)
        {
            using (SqlConnection connection = connectionProvider.OpenDbConnection())
            {
                SqlCommand cmd = new SqlCommand(
                     "select count(*) from Fruits_Vegetables_t where Color_f like @color;",
                    connection);
                cmd.Parameters.Add("@color", System.Data.SqlDbType.NVarChar).Value = color;
                return (int)cmd.ExecuteScalar();
            }
        }
        // ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ

        // 1. конвертация строки результата в объект FruitsVegetable
        private FruitsVegetable ConvertReaderRow(SqlDataReader reader)
        {
            int id = (int)reader["id"];
            string name = (string)reader["name_f"];
            string type = (string)reader["type_f"];
            string color = (string)reader["color_f"];
            int calorie = (int)reader["calorie_f"];
            return new FruitsVegetable() { Id = id, Name = name, Type = type, Color = color, Calorie = calorie };

        }
        // 2. чтение табличного результата в список обектов FruitsVegetable
        private List<FruitsVegetable> ReadSelectResult(SqlDataReader reader)
        {
            List<FruitsVegetable> fruitvegetables = new List<FruitsVegetable>();
            while (reader.Read())
            {
                FruitsVegetable frutvegetable = ConvertReaderRow(reader);
                fruitvegetables.Add(frutvegetable);
            }
            return fruitvegetables;
        }
        // 3. конвертация строки результата в объект  NameFV для названий
        private NameFV ConvertReaderNameRow(SqlDataReader reader)
        {
            string name = (string)reader["name_f"];
            return new NameFV() { Name = name };
        }
        // 4. чтение табличного результата в список обектов NameFV для названий
        private List<NameFV> ReadSelectNameResult(SqlDataReader reader)
        {
            List<NameFV> fruitvegetables = new List<NameFV>();
            while (reader.Read())
            {
                NameFV frutvegetable = ConvertReaderNameRow(reader);
                fruitvegetables.Add(frutvegetable);
            }
            return fruitvegetables;
        }
        // 5. конвертация строки результата в объект  NameFV для цветов
        private NameFV ConvertReaderColorRow(SqlDataReader reader)
        {
            string name = (string)reader["color_f"];
            return new NameFV() { Name = name };
        }
        // 6. чтение табличного результата в список обектов NameFV для цветов
        private List<NameFV> ReadSelectColorResult(SqlDataReader reader)
        {
            List<NameFV> fruitvegetables = new List<NameFV>();
            while (reader.Read())
            {
                NameFV frutvegetable = ConvertReaderColorRow(reader);
                fruitvegetables.Add(frutvegetable);
            }
            return fruitvegetables;
        }
    }
}

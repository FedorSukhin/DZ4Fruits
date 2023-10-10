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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Fruits_Vegetables_t;", connection);
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
        public void AllName()
        {
            using (SqlConnection connection = connectionProvider.OpenDbConnection())
            {
                SqlCommand command = new SqlCommand(
                     " select DISTINCT Name_f from Fruits_Vegetables_t",
                    connection);
                
                int rowsAffect = command.ExecuteNonQuery();
                if (rowsAffect != 1)
                {
                    throw new Exception($"rowsAffect {rowsAffect} !=1");
                }
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
        // 2. чтение табличного результата в список обектов
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
    }
}

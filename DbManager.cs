using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Collections;

namespace ESOperatorTaxi
{
    class DbManager
    {
        private MySqlConnection dbConnection;

        private ObservableCollection<Client> clients = new ObservableCollection<Client>();
        private ObservableCollection<Car> cars = new ObservableCollection<Car>();
        private ObservableCollection<Driver> drivers = new ObservableCollection<Driver>();
        private ObservableCollection<Order> orders = new ObservableCollection<Order>();

        public DbManager() 
        {
            dbConnection = new MySqlConnection("server=s2.kts.tu-bryansk.ru;port=3306;user=IAS18.ZHivII;database=IAS18_ZHivII;password=3q%Md=Q2/4;");

            Clients = new ReadOnlyObservableCollection<Client>(clients);
            Cars = new ReadOnlyObservableCollection<Car>(cars);
            Drivers = new ReadOnlyObservableCollection<Driver>(drivers);
            Orders = new ReadOnlyObservableCollection<Order>(orders);
        }

        public ReadOnlyObservableCollection<Client> Clients { get; private set; }
        public ReadOnlyObservableCollection<Car> Cars { get; private set; }
        public ReadOnlyObservableCollection<Driver> Drivers { get; private set; }
        public ReadOnlyObservableCollection<Order> Orders { get; private set; }

        /// <summary>
        /// Найти коллекцию сущностей по типу
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <returns>Найденный список сущностей</returns>
        private IList FindCollectionByType<T>() where T: Entity 
        {
            // NOTE: Для замены switch/case попробовать реализовать поиск через рефлексию
            Type type = typeof(T);
            switch (type.Name)
            {
                case nameof(Client):
                    return clients;
                case nameof(Car):
                    return cars;
                case nameof(Driver):
                    return drivers;
                case nameof(Order):
                    return orders;
                // Добавить остальные коллекции сущностей
                default:
                    return null;
            }
        }

        /// <summary>
        /// Загрузить сущности из таблицы базы данных по типу
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <returns>Коллекция сущностей</returns>
        public ICollection<T> LoadEntities<T>() where T : Entity, new()
        {
            try
            {
                Type typeEntity = typeof(T);
                var result = new List<T>();
                if (typeEntity.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() is TableAttribute tableAttr)
                {
                    dbConnection.Open();

                    string sql = $"SELECT * FROM {tableAttr.Name}";
                    MySqlCommand sqlCommand = new MySqlCommand(sql, dbConnection);
                    DataTable dt = new DataTable();
                    using (MySqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        dt.Load(reader);
                    }

                    var columnAttrs = new Dictionary<PropertyInfo, ColumnAttribute>();
                    foreach (var prop in typeEntity.GetProperties())
                    {
                        var attr = prop.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault() as ColumnAttribute;
                        if (attr != null)
                            columnAttrs.Add(prop, attr);
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        T item = new T();
                        foreach (var prop in columnAttrs.Keys)
                        {
                            var value = row[columnAttrs[prop].Name];
                            prop.SetValue(item, value);
                        }
                        result.Add(item);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbConnection.State != ConnectionState.Closed)
                    dbConnection.Close();
            }
        }

        /// <summary>
        /// Загрузить все сущности из базы данных в локальные коллекции
        /// </summary>
        public void Load() 
        {
            var loadedСlients = LoadEntities<Client>();
            clients.ClearAndRange(loadedСlients);

            var loadedCars = LoadEntities<Car>();
            cars.ClearAndRange(loadedCars);

            // Загрузка и маппинг через внешние ключи остальных сущностей
        }

        public void Add<T>(T entity) where T : Entity 
        {
            try
            {
                Type typeEntity = typeof(T);
                if (typeEntity.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() is TableAttribute tableAttr)
                {
                    // Поиск названия столбца идентификатора
                    var idColumnAttr = typeEntity.GetProperty(nameof(entity.Id)).GetCustomAttribute<ColumnAttribute>();

                    // Подготовка аргументов для Sql-команды
                    var arguments = new Dictionary<string, object>();
                    foreach (var prop in typeEntity.GetProperties())
                    {
                        var attr = prop.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault() as ColumnAttribute;
                        if (attr != null && attr != idColumnAttr)
                            arguments.Add(attr.Name, prop.GetValue(entity));
                    }
                    
                    // Создание Sql-комаынды
                    StringBuilder sqlColumns = new StringBuilder($"INSERT {tableAttr.Name} (");
                    StringBuilder sqlValues = new StringBuilder("VALUES (");
                    foreach (var arg in arguments)
                    {
                        sqlColumns.Append($"{arg.Key},");
                        sqlValues.Append($"@{arg.Key},");
                    }
                    sqlColumns.Remove(sqlColumns.Length - 1, 1);
                    sqlColumns.Append(")");
                    sqlValues.Remove(sqlValues.Length - 1, 1);
                    sqlValues.Append(")");
                    string sql = sqlColumns.Append(" " + sqlValues.ToString() + "; SELECT LAST_INSERT_ID();").ToString();

                    // Создание и выполнение Sql-команды
                    dbConnection.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand(sql.ToString(), dbConnection)) 
                    {
                        // Добавление значений параметров
                        foreach (var arg in arguments)
                            sqlCommand.Parameters.AddWithValue($"@{arg.Key}", arg.Value);

                        var result = sqlCommand.ExecuteScalar();
                        int.TryParse(result.ToString(), out int id);
                        entity.Id = id;
                    }

                    // Добавление сущности в локальное хранилище
                    var collection = FindCollectionByType<T>();
                    collection?.Add(entity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbConnection.State != ConnectionState.Closed)
                    dbConnection.Close();
            }
        }

        public void Update<T>(T entity) where T : Entity 
        {
            try
            {
                Type typeEntity = typeof(T);
                if (typeEntity.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() is TableAttribute tableAttr)
                {
                    // Поиск названия столбца идентификатора
                    var idColumnAttr = typeEntity.GetProperty(nameof(entity.Id)).GetCustomAttribute<ColumnAttribute>();

                    // Подготовка аргументов для Sql-команды
                    var arguments = new Dictionary<string, object>();
                    foreach (var prop in typeEntity.GetProperties())
                    {
                        var attr = prop.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault() as ColumnAttribute;
                        if (attr != null && attr != idColumnAttr)
                            arguments.Add(attr.Name, prop.GetValue(entity));
                    }

                    // Создание Sql-комаынды
                    StringBuilder sql = new StringBuilder($"UPDATE {tableAttr.Name} SET ");
                    foreach (var arg in arguments)
                        sql.Append($"{arg.Key} = @{arg.Key},");

                    sql.Remove(sql.Length - 1, 1);
                    sql.Append($" WHERE ID = {entity.Id}");

                    // Создание и выполнение Sql-команды
                    dbConnection.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand(sql.ToString(), dbConnection)) 
                    {
                        // Добавление значений параметров
                        foreach (var arg in arguments)
                            sqlCommand.Parameters.AddWithValue($"@{arg.Key}", arg.Value);

                        var result = sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
                if (dbConnection.State != ConnectionState.Closed)
                    dbConnection.Close();
            }
        }

        public void Delete<T>(T entity) where T : Entity 
        {
            try
            {
                Type typeEntity = typeof(T);
                if (typeEntity.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() is TableAttribute tableAttr)
                {
                    // Поиск названия столбца идентификатора
                    var idColumnAttr = typeEntity.GetProperty(nameof(entity.Id)).GetCustomAttribute<ColumnAttribute>();
                    string idColumnName = idColumnAttr != null ? idColumnAttr.Name : "ID";

                    // Создание и выполнение Sql-команды
                    string sql = $"DELETE FROM {tableAttr.Name} WHERE {idColumnName} = {entity.Id}";
                    dbConnection.Open();
                    using (MySqlCommand sqlCommand = new MySqlCommand(sql.ToString(), dbConnection))
                    {
                        int rows = sqlCommand.ExecuteNonQuery();
                        if (rows > 0) 
                        {
                            // Удаление сущности из локальной коллекции
                            var collection = FindCollectionByType<T>();
                            collection?.Remove(entity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbConnection.State != ConnectionState.Closed)
                    dbConnection.Close();
            }
        }
    }
}

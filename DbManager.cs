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
        private ObservableCollection<Review> reviews = new ObservableCollection<Review>();
        private ObservableCollection<TripTicket> tripTickets = new ObservableCollection<TripTicket>();
        private ObservableCollection<DriverRating> driverRatings = new ObservableCollection<DriverRating>();
        private ObservableCollection<City> cities = new ObservableCollection<City>();
        private ObservableCollection<Street> streets = new ObservableCollection<Street>();
        private ObservableCollection<DriverSelectionRule> driverSelectionRules = new ObservableCollection<DriverSelectionRule>();
        private ObservableCollection<PriceRule> priceRules = new ObservableCollection<PriceRule>();

        public DbManager() 
        {
            //dbConnection = new MySqlConnection("server=s2.kts.tu-bryansk.ru;port=3306;user=IAS18.ZHivII;database=IAS18_ZHivII;password=3q%Md=Q2/4;");
            dbConnection = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=dgiva4444;database=taxi");

            Clients = new ReadOnlyObservableCollection<Client>(clients);
            Cars = new ReadOnlyObservableCollection<Car>(cars);
            Drivers = new ReadOnlyObservableCollection<Driver>(drivers);
            Orders = new ReadOnlyObservableCollection<Order>(orders);
            Reviews = new ReadOnlyObservableCollection<Review>(reviews);
            TripTickets = new ReadOnlyObservableCollection<TripTicket>(tripTickets);
            DriverRatings = new ReadOnlyObservableCollection<DriverRating>(driverRatings);
            Cities = new ReadOnlyObservableCollection<City>(cities);
            Streets = new ReadOnlyObservableCollection<Street>(streets);
            DriverSelectionRules = new ReadOnlyObservableCollection<DriverSelectionRule>(driverSelectionRules);
            PriceRules = new ReadOnlyObservableCollection<PriceRule>(priceRules);
        }

        public ReadOnlyObservableCollection<Client> Clients { get; private set; }
        public ReadOnlyObservableCollection<Car> Cars { get; private set; }
        public ReadOnlyObservableCollection<Driver> Drivers { get; private set; }
        public ReadOnlyObservableCollection<Order> Orders { get; private set; }
        public ReadOnlyObservableCollection<Review> Reviews { get; private set; }
        public ReadOnlyObservableCollection<TripTicket> TripTickets { get; private set; }
        public ReadOnlyObservableCollection<DriverRating> DriverRatings { get; private set; }
        public ReadOnlyObservableCollection<City> Cities { get; private set; }
        public ReadOnlyObservableCollection<Street> Streets { get; private set; }
        public ReadOnlyObservableCollection<DriverSelectionRule> DriverSelectionRules { get; private set; }
        public ReadOnlyObservableCollection<PriceRule> PriceRules { get; private set; }

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
                case nameof(Review):
                    return reviews;
                case nameof(TripTicket):
                    return tripTickets;
                case nameof(City):
                    return cities;
                case nameof(Street):
                    return streets;
                case nameof(DriverSelectionRule):
                    return driverSelectionRules;
                case nameof(PriceRule):
                    return priceRules;
                case nameof(DriverRating):
                    return DriverRatings;
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
                            
                            if (prop.PropertyType == typeof(Boolean))
                            {
                                prop.SetValue(item, Convert.ToBoolean(value));
                                continue;
                            }
                            else if (prop.PropertyType == typeof(int) && value.ToString() == "")
                            {
                                value = null;
                            }
                            else if(value.ToString() == "")
                            {
                                value = "";
                            }
                            
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
            clients.ClearAndAddRange(loadedСlients);

            var loadedCars = LoadEntities<Car>();
            cars.ClearAndAddRange(loadedCars);

            var loadedDrivers = LoadEntities<Driver>();
            drivers.ClearAndAddRange(loadedDrivers);
            foreach (var driver in drivers)
            {
                driver.Car = cars.FirstOrDefault(c => c.Id == driver.CarId);
            }

            var loadedTripTickets = LoadEntities<TripTicket>();
            tripTickets.ClearAndAddRange(loadedTripTickets);
            foreach (var tripTicket in tripTickets)
            {
                tripTicket.Driver = drivers.FirstOrDefault(c => c.Id == tripTicket.DriverId);
            }

            var loadedDriverRatings = LoadEntities<DriverRating>();
            driverRatings.ClearAndAddRange(loadedDriverRatings);
            foreach (var driverRating in driverRatings)
            {
                driverRating.Driver = drivers.FirstOrDefault(c => c.Id == driverRating.IdDriver);
            }

            var loadedCities = LoadEntities<City>();
            cities.ClearAndAddRange(loadedCities);

            var loadedStreets = LoadEntities<Street>();
            streets.ClearAndAddRange(loadedStreets);
            foreach (var street in streets)
            {
                street.City = cities.FirstOrDefault(c => c.Id == street.CityId);
            }

            var loadedOrders = LoadEntities<Order>();
            orders.ClearAndAddRange(loadedOrders);
            foreach (var order in orders)
            {
                order.Client = clients.FirstOrDefault(c => c.Id == order.ClientId);
                order.Driver = drivers.FirstOrDefault(c => c.Id == order.DriverId);
                order.StartStreet = streets.FirstOrDefault(c => c.Id == order.StartStreetId);
                order.FinishStreet = streets.FirstOrDefault(c => c.Id == order.FinishStreetId);
            }

            var loadedReviews = LoadEntities<Review>();
            reviews.ClearAndAddRange(loadedReviews);
            foreach (var review in reviews)
            {
                review.Order = orders.FirstOrDefault(c => c.Id == review.OrderId);
            }

            var loadedDriverRules = LoadEntities<DriverSelectionRule>();
            driverSelectionRules.ClearAndAddRange(loadedDriverRules);

            var loadedPriceRules = LoadEntities<PriceRule>();
            priceRules.ClearAndAddRange(loadedPriceRules);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace ApiCase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressInformationController : ControllerBase
    {
        /// <summary>
        /// Function for getting all of the Addresses.
        /// </summary>
        ///
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /
        ///
        /// </remarks>
        /// 
        /// <returns>List of all Addresses</returns>
        [HttpGet]
        public IEnumerable<AddressInformation> Get()
        {
            using (var connection = new SqliteConnection("Data Source=apicase.db"))
            {
                List<AddressInformation> addresses = new List<AddressInformation>();

                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                        SELECT *
                        FROM addresses
                     ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var address = new AddressInformation
                        {
                            AddressId = reader.GetInt32(0),
                            Street = reader.GetString(1),
                            HouseNumber = reader.GetString(2),
                            PostalCode = reader.GetString(3),
                            City = reader.GetString(4),
                            Country = reader.GetString(5)
                        };
                        addresses.Add(address);
                    }

                    connection.Close();

                }

                return addresses;
            }

        }

        /// <summary>
        /// Function for getting an address by id
        /// </summary>
        ///
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /1
        ///
        /// </remarks>
        /// 
        /// <returns>One address with the right id</returns>
        [HttpGet("{id}")]
        public AddressInformation GetById(int id)
        {
            using (var connection = new SqliteConnection("Data Source=apicase.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                        SELECT *
                        FROM addresses
                        WHERE AddressId = $id
                     ";
                command.Parameters.AddWithValue("$id", id);

                using (var reader = command.ExecuteReader())
                {
                    reader.Read();

                    var address = new AddressInformation
                    {
                        AddressId = reader.GetInt32(0),
                        Street = reader.GetString(1),
                        HouseNumber = reader.GetString(2),
                        PostalCode = reader.GetString(3),
                        City = reader.GetString(4),
                        Country = reader.GetString(5)
                    };

                    connection.Close();
                    return address;
                }
            }

        }

        /// <summary>
        /// Function for updating an address.
        /// </summary>
        ///
        /// <remarks>
        /// Sample request:
        ///
        ///     Put /1
        ///     {
        ///         "street": "De Iep",
        ///         "houseNumber": "28",
        ///         "postalCode": "3224TG",
        ///         "city": "Hellevoetsluis",
        ///         "country": "Nederland"
        ///     }
        ///
        /// </remarks>
        /// 
        /// <returns>A message when it is finished</returns>
        [HttpPut("{id}")]
        public string Put(AddressInformation addressInformation, int id)
        {
            using (var connection = new SqliteConnection("Data Source=apicase.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                        UPDATE addresses
                        SET Street = $street,
                        HouseNumber = $housenumber,
                        PostalCode = $postalcode,
                        City = $city,
                        Country = $country
                        WHERE AddressId = $id
                     ";
                command.Parameters.AddWithValue("$street", addressInformation.Street);
                command.Parameters.AddWithValue("$housenumber", addressInformation.HouseNumber);
                command.Parameters.AddWithValue("$postalcode", addressInformation.PostalCode);
                command.Parameters.AddWithValue("$city", addressInformation.City);
                command.Parameters.AddWithValue("$country", addressInformation.Country);
                command.Parameters.AddWithValue("$id", id);

                command.ExecuteReader();

                connection.Close();

                return "Finished Put";
            }
        }

        /// <summary>
        /// Function for creating an address.
        /// </summary>
        ///
        /// <remarks>
        /// Sample request:
        ///
        ///     Post /
        ///     {
        ///         "street": "De Iep",
        ///         "houseNumber": "28",
        ///         "postalCode": "3224TG",
        ///         "city": "Hellevoetsluis",
        ///         "country": "Nederland"
        ///     }
        ///
        /// </remarks>
        /// 
        /// <returns>A message when it is finished</returns>
        [HttpPost]
        public string Post(AddressInformation addressInformation)
        {
            using (var connection = new SqliteConnection("Data Source=apicase.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                        INSERT INTO addresses(Street, HouseNumber, PostalCode, City, Country)
                        VALUES
                        ($street, $housenumber, $postalcode, $city, $country);
                     ";
                command.Parameters.AddWithValue("$street", addressInformation.Street);
                command.Parameters.AddWithValue("$housenumber", addressInformation.HouseNumber);
                command.Parameters.AddWithValue("$postalcode", addressInformation.PostalCode);
                command.Parameters.AddWithValue("$city", addressInformation.City);
                command.Parameters.AddWithValue("$country", addressInformation.Country);

                command.ExecuteReader();

                connection.Close();

                return "Finished Post";
            }
        }

        /// <summary>
        /// Function for deleting a address.
        /// </summary>
        ///
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /1
        ///
        /// </remarks>
        /// 
        /// <returns>A message when it is finished</returns>
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            using (var connection = new SqliteConnection("Data Source=apicase.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                        DELETE FROM addresses
                        WHERE AddressId = $id;
                     ";
                command.Parameters.AddWithValue("$id", id);

                command.ExecuteReader();

                connection.Close();

                return "Finished Delete";
            }
        }

        /// <summary>
        /// Function for searching a specific address with a search term.
        /// </summary>
        ///
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /city/Hellevoetsluis
        ///
        /// </remarks>
        /// 
        /// <returns>List of addresses or address</returns>
        [HttpGet("{column}/{searchterm}")]
        public IEnumerable<AddressInformation> Search(string column, string searchterm)
        {
            using (var connection = new SqliteConnection("Data Source=apicase.db"))
            {
                List<AddressInformation> addresses = new List<AddressInformation>();

                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    $@"
                        SELECT *
                        FROM addresses
                        WHERE [{column}] = $searchterm
                     ";

                Console.WriteLine(command);
                command.Parameters.AddWithValue("$searchterm", searchterm);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var address = new AddressInformation
                        {
                            AddressId = reader.GetInt32(0),
                            Street = reader.GetString(1),
                            HouseNumber = reader.GetString(2),
                            PostalCode = reader.GetString(3),
                            City = reader.GetString(4),
                            Country = reader.GetString(5)
                        };
                        addresses.Add(address);
                    }

                    connection.Close();
                }

                return addresses;
            }
        }

        /// <summary>
        /// Function for filter addresses.
        /// </summary>
        ///
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /filter/street
        ///
        /// </remarks>
        /// 
        /// <returns>List of filtered addresses</returns>
        [HttpGet("filter/{column}")]
        public IEnumerable<AddressInformation> Filter(string column)
        {
            using (var connection = new SqliteConnection("Data Source=apicase.db"))
            {
                List<AddressInformation> addresses = new List<AddressInformation>();

                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    $@"
                        SELECT *
                        FROM addresses
                        ORDER BY [{column}]
                     ";

                Console.WriteLine(command);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var address = new AddressInformation
                        {
                            AddressId = reader.GetInt32(0),
                            Street = reader.GetString(1),
                            HouseNumber = reader.GetString(2),
                            PostalCode = reader.GetString(3),
                            City = reader.GetString(4),
                            Country = reader.GetString(5)
                        };
                        addresses.Add(address);
                    }

                    connection.Close();
                }

                return addresses;
            }
        }

        /// <summary>
        /// Function for distance between to addresses.
        /// </summary>
        ///
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /distance/2/3
        ///
        /// </remarks>
        /// 
        /// <returns>A message with the distance</returns>
        [HttpGet("distance/{id}/{idTwo}")]
        public Task<string> GetDistanceInKm(int id, int idTwo)
        {
            using (var connection = new SqliteConnection("Data Source=apicase.db"))
            {
                List<AddressInformation> addresses = new List<AddressInformation>();

                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    $@"
                        SELECT *
                        FROM addresses
                        WHERE AddressId IN ($id,$idTwo);
                     ";

                command.Parameters.AddWithValue("$id", id);
                command.Parameters.AddWithValue("$idTwo", idTwo);

                Console.WriteLine(command);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var address = new AddressInformation
                        {
                            AddressId = reader.GetInt32(0),
                            Street = reader.GetString(1),
                            HouseNumber = reader.GetString(2),
                            PostalCode = reader.GetString(3),
                            City = reader.GetString(4),
                            Country = reader.GetString(5)
                        };
                        addresses.Add(address);
                    }

                    connection.Close();
                }
                return Helper.GetDistanceAsync(addresses[0].Street + " " + addresses[0].HouseNumber + ", " + addresses[0].City, addresses[1].Street + " " + addresses[1].HouseNumber + ", " + addresses[1].City);
            }
        }
    }
}

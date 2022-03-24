using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Images.Data
{
    public class ImagesRepository
    {

        private string _connectionString;


        public ImagesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public Image GetImageById(int id)
        {


            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();


            cmd.CommandText = @"SELECT * FROM Images Where Id=@id ";

            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            //Image image = new();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();


            Image image = (new Image
            {
                Id = (int)reader["Id"],
                NumberOfVeiws = (int)reader["NumberOfVeiws"],
                Password = (string)reader["Password"],
                ImageLocation = (string)reader["ImageLocation"]


            });




            return image;

        }

        public int AddImage(Image image)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO images (NumberOfVeiws, Password, ImageLocation) " +
                "VALUES (@numOfViews, @password, @location) SELECT SCOPE_IDENTITY()";

            cmd.Parameters.AddWithValue("@numOfViews", image.NumberOfVeiws);
            cmd.Parameters.AddWithValue("@password", image.Password);
            cmd.Parameters.AddWithValue("@location", image.ImageLocation);
            connection.Open();
           return (int) (decimal)cmd.ExecuteScalar();
        }

        public void AddVeiw(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"Update images set NumberOfVeiws=NumberOfVeiws+1
                Where Id=@id";

            cmd.Parameters.AddWithValue("@id", id);
          
            connection.Open();
          cmd.ExecuteNonQuery();
        }
    }
}

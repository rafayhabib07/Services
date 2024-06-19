using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MvvmDemo.Models
{
    public class EmployeeService


    {
        SqlConnection ObjSqlConnection;
        SqlCommand ObjSqlCommand;


        public EmployeeService()
        {

            ObjSqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EMSConnection"].ConnectionString);
            ObjSqlCommand = new SqlCommand();
            ObjSqlCommand.Connection = ObjSqlConnection;
            ObjSqlCommand.CommandType = CommandType.StoredProcedure;


        }



        public List<Employee> GetAll()
        {
            List<Employee> ObjEmployeesList = new List<Employee>();
            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "upd_SelectAllEmploy";
                ObjSqlConnection.Open();
                var ObjSqlDataReader = ObjSqlCommand.ExecuteReader();
                if (ObjSqlDataReader.HasRows)
                {
                    Employee ObjEmployee = null;
                    while (ObjSqlDataReader.Read())
                    {
                        ObjEmployee = new Employee();
                        ObjEmployee.Id = ObjSqlDataReader.GetInt32(0);
                        ObjEmployee.Name = ObjSqlDataReader.GetString(1);
                        ObjEmployee.Price = ObjSqlDataReader.GetInt32(2);

                        ObjEmployeesList.Add(ObjEmployee);
                    }
                }
                ObjSqlDataReader.Close();
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            finally
            {
                ObjSqlConnection.Close();
            }
            return ObjEmployeesList;
        }

        public bool Add(Employee objNewEmployee)

        {
            bool IsAdded = false;

            if (objNewEmployee.Price < 1 || objNewEmployee.Price > 580000)
                throw new ArgumentException("Invalid price limit for employee");
            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "upd_InsertEmploy";
                ObjSqlCommand.Parameters.AddWithValue("@Id", objNewEmployee.Id);
                ObjSqlCommand.Parameters.AddWithValue("@Name", objNewEmployee.Name);
                ObjSqlCommand.Parameters.AddWithValue("@Price", objNewEmployee.Price);

                ObjSqlConnection.Open();
                int NoOfRowsAffected = ObjSqlCommand.ExecuteNonQuery();
                IsAdded = NoOfRowsAffected > 0;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            finally
            {
                ObjSqlConnection.Close();
            }

            return IsAdded;
        }


        public bool Update(Employee objEmployeeToUpdate)
        {
            bool IsUpdated = false;

            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "upd_UpdateEmploy";
                ObjSqlCommand.Parameters.AddWithValue("@Id", objEmployeeToUpdate.Id);
                ObjSqlCommand.Parameters.AddWithValue("@Name", objEmployeeToUpdate.Name);
                ObjSqlCommand.Parameters.AddWithValue("@Price", objEmployeeToUpdate.Price);

                ObjSqlConnection.Open();
                int NoOfRowsAffected = ObjSqlCommand.ExecuteNonQuery();
                IsUpdated = NoOfRowsAffected > 0;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            finally
            {
                ObjSqlConnection.Close();
            }

            return IsUpdated;
        }

        public bool Delete(int id)
        {
            bool IsDeleted = false;
            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "upd_DeleteEmploy";
                ObjSqlCommand.Parameters.AddWithValue("@Id", id);


                ObjSqlConnection.Open();
                int NoOfRowsAffected = ObjSqlCommand.ExecuteNonQuery();
                IsDeleted = NoOfRowsAffected > 0;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            finally
            {
                ObjSqlConnection.Close();
            }


            return IsDeleted;
        }

        public Employee Search(int id)
        {
            Employee ObjEmployee = null;


            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "upd_SelectEmployById";
                ObjSqlCommand.Parameters.AddWithValue("@Id", id);

                ObjSqlConnection.Open();
                var ObjSqlDataReader = ObjSqlCommand.ExecuteReader();
                if (ObjSqlDataReader.HasRows)
                {
                    ObjSqlDataReader.Read();
                    ObjEmployee = new Employee();
                    ObjEmployee.Id = ObjSqlDataReader.GetInt32(0);
                    ObjEmployee.Name = ObjSqlDataReader.GetString(1);
                    ObjEmployee.Price = ObjSqlDataReader.GetInt32(2);
                }
                ObjSqlDataReader.Close();
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            finally
            {
                ObjSqlConnection.Close();
            }

            return ObjEmployee;
        }
    }
}
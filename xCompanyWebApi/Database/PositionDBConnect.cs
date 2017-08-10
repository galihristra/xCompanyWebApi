using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using xCompanyWebApi.Models;
using xCompanyWebApi.Models.ViewModels;
using System.Data;

namespace xCompanyWebApi.Database
{
    public class PositionDBConnect
    {
        static readonly xcompanyEntities db = new xcompanyEntities();
        static readonly string connectionString = ConfigurationManager.ConnectionStrings["xCompanySql"].ConnectionString;

        #region Validation
        public bool InsertPositionIsValid(string posname)
        {
            bool isValid = false;

            int count = (from s in db.positions
                         where s.posName.ToUpper() == posname.ToUpper()
                         select s).Count();

            if (count == 0)
                isValid = true;

            return isValid;
        }
        #endregion

        #region Insert
        public string AddPosition(string position)
        {
            string errMsg = String.Empty;
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                if (sqlConn.State == ConnectionState.Closed)
                {
                    sqlConn.Open();
                }

                if (this.InsertPositionIsValid(position))
                {
                    try
                    {
                        string query = @"
                            INSERT INTO positions 
                            (
	                            posName
                            ) values (
	                            @position
                            );";

                        SqlCommand cmd = new SqlCommand(query, sqlConn);
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("position", position));

                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        errMsg = ex.Message;
                    }
                    finally
                    {
                        sqlConn.Close();
                    }
                }
                else
                {
                    errMsg = "Position already exists.";
                }
            }

            return errMsg;
        }
        #endregion

        public string DeletePosition(string posId)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                string exp = string.Empty;
                if (sqlConn.State == ConnectionState.Closed)
                {
                    sqlConn.Open();
                }

                Guid guid = new Guid(posId);

                try
                {
                    string query = @"DELETE FROM POSITIONS WHERE POSID = @GUID;";

                    SqlCommand cmd = new SqlCommand(query, sqlConn);
                    cmd.CommandTimeout = 300;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("GUID", guid));

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    exp = ex.Message;
                }
                finally
                {
                    sqlConn.Close();
                }

                return exp;
            }
        }
    }
}
using System;
using System.Data;
using System.Data.SqlClient;

namespace ejemplo.netcore.Entity
{
    public class db
    {
        private SqlConnection con =
            new SqlConnection(
                "Server=localhost;Database=Northwind;Trusted_Connection=True;Integrated Security=false;User Id=sa;Password=Josericardo3");

        public DataSet OrderGet(Order ord,out string msg)
        {
            msg = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                SqlCommand com = new SqlCommand("USP_SEL_ORDERDETAILS", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@OrderID", ord.OrderID);
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(ds);
                msg = "SUCCESS";
                return ds;
            }
            catch (Exception e)
            {
                msg = e.Message;
            }

            return ds;
        }
        public DataSet OrderListGet(Order ord,out string msg)
        {
            msg = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                SqlCommand com = new SqlCommand("USP_SEL_ORDERSNOCONFIRMED", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(ds);
                msg = "SUCCESS";
                return ds;
            }
            catch (Exception e)
            {
                msg = e.Message;
            }

            return ds;
        }
    }
}
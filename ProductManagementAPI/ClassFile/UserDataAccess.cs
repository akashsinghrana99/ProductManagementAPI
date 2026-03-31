using ProductManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ProductManagementAPI.ClassFile
{
    public class UserDataAccess
    {
        SqlConnection con = new SqlConnection(ClsConnection.str);
        public DataTable UserLogin(ModUser obj)
        {


            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                SqlCommand cmd = new SqlCommand("SP_UserLogin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user", obj.username);
                cmd.Parameters.AddWithValue("@password", obj.password);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                throw;
            }


        }
        public bool UserRegister(ModUser obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("Sp_UserRegister", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", obj.username);
                cmd.Parameters.AddWithValue("@email", obj.email);
                cmd.Parameters.AddWithValue("@phone", obj.phone);
                cmd.Parameters.AddWithValue("@userid", obj.userid);
                cmd.Parameters.AddWithValue("@password", obj.password);
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateUser(ModUser obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_UpdateUserLogin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", obj.id);
                cmd.Parameters.AddWithValue("@username", obj.username);
                cmd.Parameters.AddWithValue("@email", obj.email);
                cmd.Parameters.AddWithValue("@phone", obj.phone);
                cmd.Parameters.AddWithValue("@userid", obj.userid);
                cmd.Parameters.AddWithValue("@password", obj.password);
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool DeleteUser(ModUser obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_DeleteUserLogin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", obj.id);

                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public string ChangePassword(ModUser obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_UserchangePassword", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@password", obj.password);
                cmd.Parameters.AddWithValue("@oldpassword", obj.OldPassword);
                cmd.Parameters.AddWithValue("@id", obj.id);
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return "SUCCESS";
                }
                else
                {
                    return "OLD_PASSWORD_MISMATCH";
                }

            }
            catch (Exception ex)
            {
                return "Fail To Change Password";
            }
        }
        public DataTable GetProduct()
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                SqlCommand cmd = new SqlCommand("SP_GetProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetProductById(int id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetProductById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool AddToCart(string userId, int productId, int quantity, string image)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_AddToCart", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@Image", image);
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public DataTable GetCart(string userid)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetCartByUserid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", userid);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;

            }
        }
        public bool UpdateDeleteCart(int id, int qty)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_UpdateDeleteCart", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@qty", qty);

                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public DataTable GetAddress()
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                SqlCommand cmd = new SqlCommand("SP_GetAddress", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool AddAddress(ModAddress obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_AddAddress", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pincode", obj.pincode);
                cmd.Parameters.AddWithValue("@lanmark", obj.lanmark);
                cmd.Parameters.AddWithValue("@country", obj.country);
                cmd.Parameters.AddWithValue("@state", obj.state);
                cmd.Parameters.AddWithValue("@city", obj.city);
                cmd.Parameters.AddWithValue("@address", obj.address);
                cmd.Parameters.AddWithValue("@bulding_no", obj.bulding_no);
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateAddress(ModAddress obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_UpdateAddress", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", obj.id);
                cmd.Parameters.AddWithValue("@pincode", obj.pincode);
                cmd.Parameters.AddWithValue("@lanmark", obj.lanmark);
                cmd.Parameters.AddWithValue("@country", obj.country);
                cmd.Parameters.AddWithValue("@state", obj.state);
                cmd.Parameters.AddWithValue("@city", obj.city);
                cmd.Parameters.AddWithValue("@address", obj.address);
                cmd.Parameters.AddWithValue("@bulding_no", obj.bulding_no);
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool DeleteAddress(ModAddress obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_DeleteAddress", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", obj.id);
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool PlaceOrder(ModOrderLog obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_PlaceOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@order_id", obj.order_id);
                cmd.Parameters.AddWithValue("@product_id", obj.product_id);
                cmd.Parameters.AddWithValue("@quantity", obj.quantity);
                cmd.Parameters.AddWithValue("@product_name", obj.product_name);
                cmd.Parameters.AddWithValue("@unit", obj.unit);
                cmd.Parameters.AddWithValue("@product_code", obj.product_code);
                cmd.Parameters.AddWithValue("@txt_type", obj.txt_type);
                cmd.Parameters.AddWithValue("@no_of_item", obj.no_of_item);
                cmd.Parameters.AddWithValue("@price", obj.price);
                cmd.Parameters.AddWithValue("@gst_per", obj.gst_per);
                cmd.Parameters.AddWithValue("@gst_amount", obj.gst_amount);
                cmd.Parameters.AddWithValue("@sell_price", obj.sell_price);
                cmd.Parameters.AddWithValue("@mrp_prise", obj.mrp_prise);
                cmd.Parameters.AddWithValue("@discount", obj.discount);
                cmd.Parameters.AddWithValue("@total_amount", obj.total_amount);
                cmd.Parameters.AddWithValue("@userid", obj.userid);
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdatePlaceOrder(ModOrderLog obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_UpdatePlaceOrder", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@order_id", obj.order_id);
                cmd.Parameters.AddWithValue("@product_id", obj.product_id);
                cmd.Parameters.AddWithValue("@quantity", obj.quantity);
                cmd.Parameters.AddWithValue("@product_name", obj.product_name);
                cmd.Parameters.AddWithValue("@unit", obj.unit);
                cmd.Parameters.AddWithValue("@product_code", obj.product_code);
                cmd.Parameters.AddWithValue("@txt_type", obj.txt_type);
                cmd.Parameters.AddWithValue("@no_of_item", obj.no_of_item);
                cmd.Parameters.AddWithValue("@price", obj.price);
                cmd.Parameters.AddWithValue("@gst_per", obj.gst_per);
                cmd.Parameters.AddWithValue("@gst_amount", obj.gst_amount);
                cmd.Parameters.AddWithValue("@sell_price", obj.sell_price);
                cmd.Parameters.AddWithValue("@mrp_prise", obj.mrp_prise);
                cmd.Parameters.AddWithValue("@discount", obj.discount);
                cmd.Parameters.AddWithValue("@total_amount", obj.total_amount);
                cmd.Parameters.AddWithValue("@userid", obj.userid);
                cmd.Parameters.AddWithValue("@id", obj.Id);


                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool DelPlaceOrder(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_DeletePlaceORder", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public DataTable UserOrderStatus(string userid)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                SqlCommand cmd = new SqlCommand("SP_UserOrderStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", userid);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable UserProfile(string userid)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                SqlCommand cmd = new SqlCommand("SP_UserProfile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", userid);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool UpdateProfile(ModUser obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_UpdateProfile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", obj.username);
                cmd.Parameters.AddWithValue("@email", obj.email);
                cmd.Parameters.AddWithValue("@phone", obj.phone);
                cmd.Parameters.AddWithValue("@userid", obj.userid);
                cmd.Parameters.AddWithValue("@id", obj.id);
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
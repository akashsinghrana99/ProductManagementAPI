using ProductManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace ProductManagementAPI.ClassFile
{
    public class DataAccess
    {
        SqlConnection con = new SqlConnection(ClsConnection.str);

        public int AdminLogin(ModAdmin obj)
        {
            int id = 0;
            try
            {
                SqlConnection con=new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_AdminLogin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@password", obj.password);
                SqlDataAdapter sda=new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    id= Convert.ToInt32(dt.Rows[0]["id"]);
                    return id;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public bool changepassword(ModAdmin obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_ChangeAdminPassword", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@password", obj.password);
                cmd.Parameters.AddWithValue("@oldpassword", obj.oldpassword);
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
            catch(Exception ex)
            {
                return false;
            }
        }
       
        public DataTable GetCategory()
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                SqlCommand cmd = new SqlCommand("GetCategory", con);
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
        public bool AddCategory(ModCategory obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("AddCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@image", obj.image);
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
        public bool UpdateCategory(ModCategory obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("UpdateCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", obj.id);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@image", obj.image);
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
        public bool DeleteCategory(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("DeleteCategory", con);
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
        public DataTable GetSubCat()
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                SqlCommand cmd = new SqlCommand("sp_GetSubCat", con);
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
        public bool AddSubCat(ModSubCat obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("sp_AddSubCat", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@image", obj.image);
                cmd.Parameters.AddWithValue("@cat_id", obj.cat_id);
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
        public bool UpdateSubCat(ModSubCat obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("sp_UpdateSubCat", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cat_id", obj.cat_id);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@image", obj.image);
                cmd.Parameters.AddWithValue("@id", obj.id);
                int i = cmd.ExecuteNonQuery();
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
        public bool DeleteSubCat(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("sp_DeleteSubCat", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                int i = cmd.ExecuteNonQuery();
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
        public DataTable GetUnit()
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                SqlCommand cmd = new SqlCommand("SP_GetUnit", con);
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
        public bool AddUnit(ModUnit obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_AddUnit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@unit_name", obj.unit_name);
                cmd.Parameters.AddWithValue("@unit", obj.unit);
                cmd.Parameters.AddWithValue("@catid", obj.catid);
                cmd.Parameters.AddWithValue("@subcatid", obj.subcatid);
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
        public bool UpdateUnit(ModUnit obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_UpdateUnit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@unit_name", obj.unit_name);
                cmd.Parameters.AddWithValue("@unit", obj.unit);
                cmd.Parameters.AddWithValue("@catid", obj.catid);
                cmd.Parameters.AddWithValue("@subcatid", obj.subcatid);
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
        public bool DeleteUnit(ModUnit obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_DeleteUnit", con);
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
        public bool AddProduct(ModProduct obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_AddProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cat_id", obj.catId);
                cmd.Parameters.AddWithValue("@subcat_id", obj.subcat_id);
                cmd.Parameters.AddWithValue("@product_name", obj.product_name);
                cmd.Parameters.AddWithValue("@unit", obj.unit);
                cmd.Parameters.AddWithValue("@hsn_code", obj.hsn_code);
                cmd.Parameters.AddWithValue("@product_code", obj.product_code);
                cmd.Parameters.AddWithValue("@quantity", obj.quantity);
                cmd.Parameters.AddWithValue("@txt_type", obj.txt_type);
                cmd.Parameters.AddWithValue("@no_of_item", obj.no_of_item);
                cmd.Parameters.AddWithValue("@price", obj.price);
                cmd.Parameters.AddWithValue("@gst_per", obj.gst_per);
                cmd.Parameters.AddWithValue("@gst_amount", obj.gst_amount);
                cmd.Parameters.AddWithValue("@amount_with_gst", obj.amount_with_gst);
                cmd.Parameters.AddWithValue("@sell_price", obj.sell_price);
                cmd.Parameters.AddWithValue("@mrp_price", obj.mrp_price);
                cmd.Parameters.AddWithValue("@discount", obj.discount);
                cmd.Parameters.AddWithValue("@image", obj.image);
                cmd.Parameters.AddWithValue("@product_description", obj.product_description);
                cmd.Parameters.AddWithValue("@total_amount", obj.total_amount);
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
        public bool UpdateProduct(ModProduct obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_UpdateProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", obj.id);
                cmd.Parameters.AddWithValue("@cat_id", obj.catId);
                cmd.Parameters.AddWithValue("@subcat_id", obj.subcat_id);
                cmd.Parameters.AddWithValue("@product_name", obj.product_name);
                cmd.Parameters.AddWithValue("@unit", obj.unit);
                cmd.Parameters.AddWithValue("@hsn_code", obj.hsn_code);
                cmd.Parameters.AddWithValue("@product_code", obj.product_code);
                cmd.Parameters.AddWithValue("@quantity", obj.quantity);
                cmd.Parameters.AddWithValue("@txt_type", obj.txt_type);
                cmd.Parameters.AddWithValue("@no_of_item", obj.no_of_item);
                cmd.Parameters.AddWithValue("@price", obj.price);
                cmd.Parameters.AddWithValue("@gst_per", obj.gst_per);
                cmd.Parameters.AddWithValue("@gst_amount", obj.gst_amount);
                cmd.Parameters.AddWithValue("@amount_with_gst", obj.amount_with_gst);
                cmd.Parameters.AddWithValue("@sell_price", obj.sell_price);
                cmd.Parameters.AddWithValue("@mrp_price", obj.mrp_price);
                cmd.Parameters.AddWithValue("@discount", obj.discount);
                cmd.Parameters.AddWithValue("@image", obj.image);
                cmd.Parameters.AddWithValue("@product_description", obj.product_description);
                cmd.Parameters.AddWithValue("@total_amount", obj.total_amount);
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
        public bool DeleteProduct(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_DeleteProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                int i=cmd.ExecuteNonQuery();
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
        public DataTable GetSlider()
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                SqlCommand cmd = new SqlCommand("SP_GetSlider", con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if(dt!=null && dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool AddSlider(ModSlider obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_AddSlider", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cat_id",obj.cat_id);
                cmd.Parameters.AddWithValue("@slider_name", obj.slider_name);
                cmd.Parameters.AddWithValue("@image",obj.image);
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
        public bool UpdateSlider(ModSlider obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_UpdateSlider", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id",obj.id);
                cmd.Parameters.AddWithValue("@cat_id",obj.cat_id);
                cmd.Parameters.AddWithValue("@slider_name", obj.slider_name);
                cmd.Parameters.AddWithValue("@image",obj.image);
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
        public bool DeleteSlider(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_DeleteSlider", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id",id);
                
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
        public DataTable GetDeliveryBoy()
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                SqlCommand cmd=new SqlCommand("SP_GetDeliveryBoy",con);
                cmd.CommandType=CommandType.StoredProcedure;
                SqlDataAdapter sda=new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if(dt!=null && dt.Rows.Count > 0)
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
        public bool AddDeliveryBoy(ModDeliveryBoy obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_AddDeliveryBoy", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name",obj.name);
                cmd.Parameters.AddWithValue("@mobile_no", obj.mobile_no);
                cmd.Parameters.AddWithValue("@email", obj.email);
                cmd.Parameters.AddWithValue("@dob", obj.dob);
                cmd.Parameters.AddWithValue("@country", obj.country);
                cmd.Parameters.AddWithValue("@state", obj.state);
                cmd.Parameters.AddWithValue("@city", obj.city);
                cmd.Parameters.AddWithValue("@address", obj.address);
                cmd.Parameters.AddWithValue("@pin", obj.pin);
                cmd.Parameters.AddWithValue("@identity_proof", obj.identity_proof);
                cmd.Parameters.AddWithValue("@image", obj.image);
                int i=cmd.ExecuteNonQuery();
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
            catch(Exception ex)
            {
                return false;
            }
        }
        public bool UpdateDeliveryBoy(ModDeliveryBoy obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_UpdateDeliveryBoy", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@mobile_no", obj.mobile_no);
                cmd.Parameters.AddWithValue("@email", obj.email);
                cmd.Parameters.AddWithValue("@dob", obj.dob);
                cmd.Parameters.AddWithValue("@country", obj.country);
                cmd.Parameters.AddWithValue("@state", obj.state);
                cmd.Parameters.AddWithValue("@city", obj.city);
                cmd.Parameters.AddWithValue("@address", obj.address);
                cmd.Parameters.AddWithValue("@pin", obj.pin);
                cmd.Parameters.AddWithValue("@identity_proof", obj.identity_proof);
                cmd.Parameters.AddWithValue("@image", obj.image);
                cmd.Parameters.AddWithValue("@id", obj.id);
                int i=cmd.ExecuteNonQuery();
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
            catch(Exception ex)
            {
                return false;
            }
        }
        public bool DeleteDeliveryBoy(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SP_DeleteDeliveryBoy", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                int i=cmd.ExecuteNonQuery();
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
            catch(Exception ex)
            {
                return false;
            }
        }
        public DataTable OrderStatus(int status)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_OrderLogStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@status", status);
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
        public bool changeOrderStatus(int id, int status)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_UpdateOrderLogStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@status", status);
                con.Open();
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
        public DataTable GetProfile(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ClsConnection.str);
                SqlCommand cmd = new SqlCommand("SP_GetAdminProfile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if(dt!=null && dt.Rows.Count > 0)
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
    }
}
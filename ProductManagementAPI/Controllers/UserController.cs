using ProductManagementAPI.ClassFile;
using ProductManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;

namespace ProductManagementAPI.Controllers
{
    [RoutePrefix("Api/User")]
    public class UserController : ApiController
    {
        SqlConnection con = new SqlConnection(ClsConnection.str);
        UserDataAccess uda=new UserDataAccess();
        public string checkvalidate_user()
        {

            string username = "";
            System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
            string userId = "";
            if (headers.Contains("X-Auth-Token"))
            {
                username = TokenManager.ValidateToken(userId = headers.GetValues("X-Auth-Token").First());
            }
            return username;
        }
        [HttpPost]
        [Route("Register")]
        public CommonResponse Register(ModUser obj)
        {
            CommonResponse res=new CommonResponse();
            try
            {
                if (string.IsNullOrEmpty(obj.username) || string.IsNullOrEmpty(obj.email)|| string.IsNullOrEmpty(obj.phone)||
                    string.IsNullOrEmpty(obj.password))
                {

                    res.Status = "0";
                    res.Message = "all Field is required";
                    res.Payload = null;
                    return res;

                }
                bool result = uda.UserRegister(obj);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "User Register SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "User Register Fail";
                }

            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = "0";
                res.Payload = null;
            }
            return res;
        }
        [HttpPost]
        [Route("login")]
        public IHttpActionResult login(ModUser obj)
        {
            CommonResponse res = new CommonResponse();

            if (obj == null || string.IsNullOrEmpty(obj.username) || string.IsNullOrEmpty(obj.password))
            {
                res.Status = "0";
                res.Message = "Username and Password required";
                return Ok(res);
            }

            DataTable dt = uda.UserLogin(obj);

            if (dt != null && dt.Rows.Count > 0)
            {
                string userId = dt.Rows[0]["userid"].ToString();

                res.Status = "1";
                res.Payload = new
                {
                    Token = TokenManager.GenerateToken(userId),
                    Username = dt.Rows[0]["userid"].ToString()
                };
                res.Message = "User Login Successfully";
            }
            else
            {
                res.Status = "0";
                res.Message = "Invalid Username or Password";
            }

            return Ok(res);
        }
        [HttpPut]
        [Route("UpdateUser")]
        public CommonResponse UpdateUser(ModUser obj)
        {
            CommonResponse res = new CommonResponse();
            try
            {
                bool result = uda.UpdateUser(obj);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "User Data Updated SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "User Data Update Fail";
                }

            }
            catch(Exception ex)
            {
                res.Message = ex.Message;
                res.Status = "0";
                res.Payload = null;
            }
            return res;
        }
        [HttpDelete]
        [Route("DeleteUser")]
        public CommonResponse DeleteUser(ModUser obj)
        {
            CommonResponse res = new CommonResponse();
            try
            {
                bool result = uda.DeleteUser(obj);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "User Data Deleted SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "User Data Deleted Fail";
                }

            }
            catch(Exception ex)
            {
                res.Message = ex.Message;
                res.Status = "0";
                res.Payload = null;
            }
            return res;
        }
        [HttpPut]
        [Route("change_password")]
        public CommonResponse change_password(ModUser obj)
        {
            CommonResponse res = new CommonResponse();

            if (obj == null)
            {
                res.Status = "0";
                res.Message = "Invalid data";
                res.Payload = null;
                return res;
            }

            string result = uda.ChangePassword(obj);

            if (result == "SUCCESS")
            {
                res.Status = "1";
                res.Payload = true;
                res.Message = "User Password Changed Successfully";
            }
            else if (result == "OLD_PASSWORD_MISMATCH")
            {
                res.Status = "0";
                res.Payload = null;
                res.Message = "Old password does not match";
            }
            else
            {
                res.Status = "0";
                res.Payload = null;
                res.Message = "User Password Change Failed";
            }

            return res;
        }
        [HttpGet]
        [Route("Product")]
        public CommonResponse Product()
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Message = "Access Denied";
                    return res;
                }
                DataTable dt = new DataTable();
                dt = uda.GetProduct();
                if(dt!=null && dt.Rows.Count > 0)
                {
                    res.Status = "1";
                    res.Payload = dt;
                    res.Message = "Fetch Product Data SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Fetch Product Data Fail";
                }

            }
            catch(Exception ex)
            {
                res.Message = ex.Message;
                res.Status = "0";
                res.Payload = null;
            }
            return res;
        }
        [HttpGet]
        [Route("ProductDetails")]
        public CommonResponse ProductDetails(int id)
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Message = "Access Denied";
                    return res;
                }
                DataTable dt = new DataTable();
                dt = uda.GetProductById(id);
                if(dt!=null && dt.Rows.Count > 0)
                {
                    res.Status = "1";
                    res.Payload = dt;
                    res.Message = "Fetch Product Data SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Fetch Product Data SuccessFully";
                }

            }
            catch(Exception ex)
            {
                res.Message = ex.Message;
                res.Status = "0";
                res.Payload = null;
            }
            return res;
        }
        [HttpPost]
        [Route("AddCart")]
        public IHttpActionResult AddCart()
        {
            CommonResponse res = new CommonResponse();

            try
            {
                
                string userid = checkvalidate_user();
                
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Message = "Access Denied";
                    return Ok(res);
                }

                var httpRequest = HttpContext.Current?.Request;
                if (httpRequest == null)
                {
                    res.Status = "0";
                    res.Message = "No HTTP Request Available";
                    return Ok(res);
                }

                // Read form data
                string productId = httpRequest.Form["productId"];
                string quantity = httpRequest.Form["quantity"];
                string userids = httpRequest.Form["UserId"];
                //obj.id = Convert.ToInt32(userid);
                if (string.IsNullOrEmpty(productId))
                {
                    res.Status = "0";
                    res.Message = "ProductId is required";
                    return Ok(res);
                }

                int qty = 1;
                if (!string.IsNullOrEmpty(quantity))
                {
                    int.TryParse(quantity, out qty);
                }

                // Image upload
                string fileName = "";
                HttpPostedFile file = httpRequest.Files["image"];

                if (file != null && file.ContentLength > 0)
                {
                    string extension = Path.GetExtension(file.FileName).ToLower();

                    string[] allowed = { ".jpg", ".jpeg", ".png", ".gif" };
                    if (!allowed.Contains(extension))
                    {
                        res.Status = "0";
                        res.Message = "Invalid image format";
                        return Ok(res);
                    }

                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + extension;

                    string folderPath = HttpContext.Current.Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fullPath = Path.Combine(folderPath, fileName);
                    file.SaveAs(fullPath);
                }

                // Save to DB
                bool added = uda.AddToCart(
                    userids,
                    Convert.ToInt32(productId),
                    qty,
                    fileName
                );

                if (added)
                {
                    res.Status = "1";
                    res.Message = "Add To Cart Successfully";
                    res.Payload = new
                    {
                        UserId = userids,
                        ProductId = productId,
                        Quantity = qty,
                        Image = fileName
                    };
                }
                else
                {
                    res.Status = "0";
                    res.Message = "Add To Cart Failed";
                    res.Payload = null;
                }
            }
            catch (Exception ex)
            {
                res.Status = "0";
                res.Message = ex.Message;
                res.Payload = null;
            }

            return Ok(res);
        }
        [HttpGet]
        [Route("Carts")]
        public CommonResponse Carts()
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Message = "Access Denied";
                    return (res);
                }
                DataTable dt = new DataTable();
                dt = uda.GetCart(userid);
                if(dt!=null && dt.Rows.Count > 0)
                {
                    res.Status = "1";
                    res.Payload = dt;
                    res.Message = "Fetch Cart Data SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "No Data Found";
                }

            }
            catch(Exception ex)
            {
                res.Status = "0";
                res.Message = ex.Message;
                res.Payload = null;
            }
            return res;
        }
        [HttpPost]
        [Route("UpdateDeleteCart")]
        public IHttpActionResult UpdateDeleteCart()
        {
            CommonResponse res = new CommonResponse();

            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Message = "Access Denied";
                    return Ok(res);
                }

                var httpRequest = HttpContext.Current?.Request;

                if (httpRequest == null)
                {
                    res.Status = "0";
                    res.Message = "Invalid Request";
                    return Ok(res);
                }

                string id = httpRequest.Form["id"];
                string qty = httpRequest.Form["qty"];

                if (string.IsNullOrEmpty(id))
                {
                    res.Status = "0";
                    res.Message = "Cart Id is required";
                    return Ok(res);
                }

                int cartId = Convert.ToInt32(id);
                int quantity = 0;

                if (!string.IsNullOrEmpty(qty))
                {
                    int.TryParse(qty, out quantity);
                }

                bool result = uda.UpdateDeleteCart(cartId, quantity);

                if (result)
                {
                    res.Status = "1";
                    res.Message = quantity == 0
                        ? "Item removed from cart"
                        : "Cart updated successfully";

                    res.Payload = new
                    {
                        Id = cartId,
                        Quantity = quantity
                    };
                }
                else
                {
                    res.Status = "0";
                    res.Message = "Operation failed";
                }
            }
            catch (Exception ex)
            {
                res.Status = "0";
                res.Message = ex.Message;
                res.Payload = null;
            }

            return Ok(res);
        }
        [HttpGet]
        [Route("GetAddress")]
        public CommonResponse GetAddress()
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Message = "Access Denied";
                    return (res);
                }
                DataTable dt = new DataTable();
                dt = uda.GetAddress();
                if(dt!=null && dt.Rows.Count > 0)
                {
                    res.Status = "1";
                    res.Payload= dt;
                    res.Message = "Fetch Address Data SuccessFully";
                }
                else
                {
                    res.Status="0";
                    res.Payload = null;
                    res.Message = "Data Not Found";
                }

            }
            catch (Exception ex)
            {
                res.Status = "0";
                res.Message = ex.Message;
                res.Payload = null;

            }
            return res;

        }
        [HttpPost]
        [Route("AddAddress")]
        public CommonResponse AddAddress(ModAddress obj)
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Message = "Access Denied";
                    return (res);
                }
                bool result=uda.AddAddress(obj);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Add Addrress Data SuccessFully";
                }
                else
                {
                    res.Status="0";
                    res.Payload = null;
                    res.Message = "Add Address Data Fai";
                }

            }
            catch (Exception ex)
            {
                res.Status = "0";
                res.Message = ex.Message;
                res.Payload= null;
            }
            return res;
        }
        [HttpPut]
        [Route("UpdateAddress")]
        public CommonResponse UpdateAddress(ModAddress obj)
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Message = "Access Denied";
                    return (res);
                }
                bool result=uda.UpdateAddress(obj);
                if (result)
                {
                    res.Status="1";
                    res.Payload = result;
                    res.Message = "Address Data Updated SuccessFully";
                }
                else
                {
                    res.Status="0";
                    res.Payload = null;
                    res.Message = "Address Data Updated Faill";
                }

            }
            catch (Exception ex)
            {
                res.Status = "0";
                res.Message = ex.Message;
                res.Payload = null;
            }
            return res;
        }
        [HttpDelete]
        [Route("DeleteAddress")]
        public CommonResponse DeleteAddress(ModAddress obj)
        {
            CommonResponse res= new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Message = "Access Denied";
                    return (res);
                }
                bool result=uda.DeleteAddress(obj);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Address Data Deleted SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Address Data Deleted Fail";
                }

            }
            catch(Exception ex)
            {
                res.Status = "0";
                res.Message = ex.Message;
                res.Payload = null;
            }
            return res;
        }
        [HttpPost]
        [Route("PlaceOrder")]
        public CommonResponse PlaceOrder(ModOrderLog obj)
        {
            CommonResponse res= new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Message = "Access Denied";
                    return (res);
                }
                bool result=uda.PlaceOrder(obj);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Order Placed SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Order Placed Fail";
                }

            }
            catch(Exception ex)
            {
                res.Message=ex.Message;
                res.Payload = null;
                res.Status = "0";
            }
            return res;
        }
        [HttpPut]
        [Route("UpdatePlaceOrder")]
        public CommonResponse UpdatePlaceOrder(ModOrderLog obj)
        {
            CommonResponse res= new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Message = "Access Denied";
                    return (res);
                }
                bool result=uda.UpdatePlaceOrder(obj);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Update Order Placed SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Update Order Placed Fail";
                }

            }
            catch(Exception ex)
            {
                res.Message=ex.Message;
                res.Payload = null;
                res.Status = "0";
            }
            return res;
        }
        [HttpDelete]
        [Route("DelPlaceOrder")]
        public CommonResponse DelPlaceOrder(ModOrderLog obj)
        {
            CommonResponse res= new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Message = "Access Denied";
                    return (res);
                }
                bool result=uda.DelPlaceOrder(obj.Id);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Order Placed Remove SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Order Placed Remove Fail";
                }

            }
            catch(Exception ex)
            {
                res.Message=ex.Message;
                res.Payload = null;
                res.Status = "0";
            }
            return res;
        }
        [HttpGet]
        [Route("UserOrderStatus")]
        public CommonResponse UserOrderStatus()
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();

                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Message = "Access Denied";
                    return (res);
                }
                DataTable dt = new DataTable();
                dt = uda.UserOrderStatus(userid);
                if(dt!=null && dt.Rows.Count > 0)
                {
                    res.Status = "1";
                    res.Payload = dt;
                    res.Message = "Fetch Order Status SuccessFully";
                }
                else
                {
                    res.Status = "1";
                    res.Payload = null;
                    res.Message = "Fetch Order Status Fail";
                }

            }
            catch(Exception ex)
            {
                res.Message = ex.Message;
                res.Payload = null;
                res.Status = "0";
            }
            return res;
        }
        [HttpGet]
        [Route("UserProfile")]
        public CommonResponse UserProfile()
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();

                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Message = "Access Denied";
                    return (res);
                }
                DataTable dt = new DataTable();
                dt = uda.UserProfile(userid);
                if(dt!=null && dt.Rows.Count > 0)
                {
                    res.Status = "1";
                    res.Payload = dt;
                    res.Message = "Fetch User Profile SuccessFully";
                }
                else
                {
                    res.Status = "1";
                    res.Payload = null;
                    res.Message = "Fetch User Profile Fail";
                }

            }
            catch(Exception ex)
            {
                res.Message = ex.Message;
                res.Payload = null;
                res.Status = "0";
            }
            return res;
        }
        [HttpPut]
        [Route("UpdateProfile")]
        public CommonResponse UpdateProfile(ModUser obj)
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();

                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Message = "Access Denied";
                    return (res);
                }
                bool result = uda.UpdateProfile(obj);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "User Profile Updated SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "User Profile Update Fail";
                }
            }
            catch(Exception ex)
            {
                res.Message = ex.Message;
                res.Payload = null;
                res.Status = "0";
            }
            return res;
        }
    }
}

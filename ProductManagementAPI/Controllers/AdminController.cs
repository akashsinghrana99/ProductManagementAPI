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
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;

namespace ProductManagementAPI.Controllers
{
    [RoutePrefix("Api/Admin")]
    public class AdminController : ApiController
    {
        SqlConnection conn = new SqlConnection(ClsConnection.str);
        DataAccess da = new DataAccess();
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
        [Route("login")]
        public IHttpActionResult login(ModAdmin obj)
        {
            CommonResponse res = new CommonResponse();
            if (obj == null)
            {
                res.Status = "0";
                res.Message = "Invalid data";
                res.Payload = null;
                return Ok(res);
            }

            int id = da.AdminLogin(obj);
            if (id > 0)
            {

                res.Status = "1";
                res.Payload = new
                {
                    Token = TokenManager.GenerateToken(Convert.ToString(id))
                };
                res.Message = "Admin Login SuccessFully";
            }
            else
            {
                res.Status = "0";
                res.Payload = null;
                res.Message = "Invalid username or password";
            }
            return Ok(res);
        }
        [HttpPost]
        [Route("change-password")]
        public CommonResponse ChangePassword(ModAdmin obj)
        {
            CommonResponse res = new CommonResponse();
            string userid = checkvalidate_user();
            if (string.IsNullOrEmpty(userid))
            {
                res.Status = "2";
                res.Payload = null;
                res.Message = "Access Denied";
                return res;
            }
            obj.id = Convert.ToInt32(userid);
            if (obj == null)
            {
                res.Status = "0";
                res.Message = "Invalid data";
                res.Payload = null;
                return res;
            }
            bool result = da.changepassword(obj);
            if (result)
            {
                res.Status = "1";
                res.Payload = result;
                res.Message = "Admin Password Change SuccessFully";
            }
            else
            {
                res.Status = "0";
                res.Payload = null;
                res.Message = " Faill Admin Password Change ";
            }
            return res;
        }

        [HttpGet]
        [Route("GetCategory")]
        public CommonResponse GetCategory()
        {
            CommonResponse res = new CommonResponse();

            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return res;
                }

                DataTable dt = da.GetCategory();
                if (dt != null && dt.Rows.Count > 0)
                {
                    res.Status = "1";
                    res.Payload = dt;
                    res.Message = "Fetch Category Data SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Category Data Not Found";
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
        [Route("AddCategory")]
        public IHttpActionResult AddCategory()
        {
            CommonResponse res = new CommonResponse();

            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return Ok(res);
                }

                var httpRequest = HttpContext.Current?.Request;

                if (httpRequest == null)
                {
                    return Ok(new CommonResponse
                    {
                        Status = "0",
                        Message = "No HTTP Request Available",
                        Payload = null
                    });
                }

                // ✅ Validate Name
                string name = httpRequest.Form["name"];
                if (string.IsNullOrEmpty(name))
                {
                    return Ok(new CommonResponse
                    {
                        Status = "0",
                        Message = "Category name is required",
                        Payload = null
                    });
                }

                ModCategory obj = new ModCategory();
                obj.name = name;
                
                // ✅ File Handling
                HttpPostedFile file = httpRequest.Files["image"];

                if (file != null && file.ContentLength > 0)
                {
                    string extension = Path.GetExtension(file.FileName).ToLower();

                    // ✅ Validate file type
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                    if (!allowedExtensions.Contains(extension))
                    {
                        return Ok(new CommonResponse
                        {
                            Status = "0",
                            Message = "Invalid file format",
                            Payload = null
                        });
                    }

                    // ✅ Unique filename
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + extension;

                    string folderPath = HttpContext.Current.Server.MapPath("~/Uploads/");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fullPath = Path.Combine(folderPath, fileName);
                    file.SaveAs(fullPath);

                    obj.image = fileName;
                }

                // ✅ Save to DB
                bool result = da.AddCategory(obj);

                if (result)
                {
                    res.Status = "1";
                    res.Message = "Category Added Successfully";
                    res.Payload = obj; // return object instead of bool
                }
                else
                {
                    res.Status = "0";
                    res.Message = "Category Add Failed";
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
        [HttpPut]
        [Route("UpdateCategory")]
        public IHttpActionResult UpdateCategory()
        {
            CommonResponse res = new CommonResponse();

            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return Ok(res);
                }

                var httpRequest = HttpContext.Current.Request;
                if (httpRequest == null)
                {
                    res.Status = "0";
                    res.Message = "No Http Request Avalable";
                    res.Payload = null;
                    return Ok(res);
                }
                var obj = new ModCategory();
                int id;
                
                var idValue = httpRequest.Form["Id"];
                if (!string.IsNullOrEmpty(idValue) && int.TryParse(idValue, out id))
                {
                    obj.id = id;
                }

                obj.name = httpRequest.Form["name"];

                HttpPostedFile file = null;
                if (httpRequest.Files.Count > 0)
                {
                    file = httpRequest.Files["image"] ?? httpRequest.Files[0];
                }
                if (file != null && file.ContentLength > 0)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string filename = DateTime.Now.ToString("ddMMyyyyhhmmss") + extension;
                    string folderpath = HttpContext.Current.Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(folderpath))
                    {
                        Directory.CreateDirectory(folderpath);
                    }
                    string fullPath = Path.Combine(folderpath, filename);
                    file.SaveAs(fullPath);
                    obj.image = filename;
                }
                bool result = da.UpdateCategory(obj);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Category Updated SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Category Updated Fail";
                }


            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = "0";
                res.Payload = null;
            }
            return Ok(res);
        }
        [HttpDelete]
        [Route("DeleteCategory")]
        public CommonResponse DeleteCategory(ModCategory obj)
        {
            CommonResponse res = new CommonResponse();
            try
            {
                //string userid = checkvalidate_user();
                //if (string.IsNullOrEmpty(userid))
                //{
                //    res.Status = "2";
                //    res.Payload = null;
                //    res.Message = "Access Denied";
                //    return res;
                //}
                //obj.id = Convert.ToInt32(userid);
                bool result = da.DeleteCategory(obj.id);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Category Deleted SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Category Deleted Fail";
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
        [HttpGet]
        [Route("GetSubCat")]
        public CommonResponse GetSubCat()
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return res;
                }

                DataTable dt = da.GetSubCat();
                if (dt != null && dt.Rows.Count > 0)
                {
                    res.Status = "1";
                    res.Payload = dt;
                    res.Message = "Fetch SubCategory Data SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "SubCategory Data Not Found";
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
        [Route("AddSubCat")]
        public IHttpActionResult AddSubCat()
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return Ok(res);
                }
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest == null)
                {
                    res.Status = "0";
                    res.Message = "No Http Request Avalable";
                    res.Payload = null;
                    return Ok(res);
                }
                string name = httpRequest.Form["name"];
                string catId = httpRequest.Form["cat_id"];
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(catId))
                {
                    res.Status = "0";
                    res.Message = "Name and Category ID are required";
                    res.Payload = null;
                    return Ok(res);
                }

                ModSubCat obj = new ModSubCat();
                obj.id = Convert.ToInt32(userid);

                obj.name = name;
                obj.cat_id = int.Parse(catId);
                HttpPostedFile file = httpRequest.Files["image"];
                if (file != null && file.ContentLength > 0)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    string folderpath = HttpContext.Current.Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(folderpath))
                    {
                        Directory.CreateDirectory(folderpath);
                    }
                    string fullPath = Path.Combine(folderpath, filename);
                    file.SaveAs(fullPath);
                    obj.image = filename;
                }
                bool result = da.AddSubCat(obj);
                if (result)
                {
                    res.Message = "SubCategory Added SuccessFully";
                    res.Status = "1";
                    res.Payload = result;
                }
                else
                {
                    res.Message = "SubCategory Added Fail";
                    res.Status = "0";
                    res.Payload = null;
                }


            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = "0";
                res.Payload = null;

            }
            return Ok(res);
        }

        [HttpPut]
        [Route("UpdateSubCat")]
        public IHttpActionResult UpdateSubCat()
        {
            CommonResponse res = new CommonResponse();

            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    return Ok(new CommonResponse
                    {
                        Status = "2",
                        Message = "Access Denied",
                        Payload = null
                    });
                }

                var httpRequest = HttpContext.Current.Request;
                if (httpRequest == null)
                {
                    return Ok(new CommonResponse
                    {
                        Status = "0",
                        Message = "No Http Request Available",
                        Payload = null
                    });
                }

                // Validate ID
                if (!int.TryParse(httpRequest.Form["id"], out int id))
                {
                    return Ok(new CommonResponse
                    {
                        Status = "0",
                        Message = "Valid id is required"
                    });
                }

                // Validate Category ID
                if (!int.TryParse(httpRequest.Form["cat_id"], out int cat_id))
                {
                    return Ok(new CommonResponse
                    {
                        Status = "0",
                        Message = "Valid Category ID is required"
                    });
                }

                string name = httpRequest.Form["name"];
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Ok(new CommonResponse
                    {
                        Status = "0",
                        Message = "Name is required"
                    });
                }

                ModSubCat obj = new ModSubCat
                {
                    id = id,              // ✅ Correct ID
                    cat_id = cat_id,
                    name = name
                };

                // File Upload
                HttpPostedFile file = httpRequest.Files["image"];
                if (file != null && file.ContentLength > 0)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    string folderPath = HttpContext.Current.Server.MapPath("~/Uploads/");

                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string fullPath = Path.Combine(folderPath, fileName);
                    file.SaveAs(fullPath);

                    obj.image = fileName;
                }
                else
                {
                    obj.image = null; // optional (handle in SP)
                }

                bool result = da.UpdateSubCat(obj);

                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Update SubCategory Success";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Update SubCategory Fail";
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = "0";
                res.Payload = null;
            }
            return Ok(res);
        }
        [HttpDelete]
        [Route("DeleteSubCat")]
        public CommonResponse DeleteSubCat(ModSubCat obj)
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return res;
                }
                //obj.id = Convert.ToInt32(userid);

                bool result = da.DeleteSubCat(obj.id);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Delete SubCategory Success";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Subcat Delete Fail";
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
        [HttpGet]
        [Route("GetUnit")]
        public CommonResponse GetUnit()
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return res;
                }
                DataTable dt = da.GetUnit();
                if (dt != null && dt.Rows.Count > 0)
                {
                    res.Status = "1";
                    res.Payload = dt;
                    res.Message = "Fetch Unit Data SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Unit Data Not Found";
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
        [Route("AddUnit")]
        public CommonResponse AddUnit(ModUnit obj)
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return res;
                }
                //obj.id = Convert.ToInt32(userid);
                bool result = da.AddUnit(obj);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Unit Data Added SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Unit data Added Fail";
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
        [HttpPut]
        [Route("UpdateUnit")]
        public CommonResponse UpdateUnit(ModUnit obj)
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return res;
                }
                
                bool result = da.UpdateUnit(obj);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Unit Data Updated SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Unit data Updated Fail";
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
        [HttpDelete]
        [Route("DeleteUnit")]
        public CommonResponse DeleteUnit(ModUnit obj)
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return res;
                }
                
                bool result = da.DeleteUnit(obj);

                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Unit Data Deleted SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Unit data Deleted Fail";
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
        [HttpGet]
        [Route("GetPtoduct")]
        public CommonResponse GetProduct()
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return res;
                }
                DataTable dt = da.GetProduct();
                if (dt != null && dt.Rows.Count > 0)
                {
                    res.Status = "1";
                    res.Payload = dt;
                    res.Message = "Product Data Fetch SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Product Data Not Found";
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
        [Route("AddProduct")]
        public IHttpActionResult AddProduct()
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return Ok(res);
                }

                var httpRequest = HttpContext.Current.Request;
                if (httpRequest == null)
                {
                    res.Status = "0";
                    res.Message = "No Http Request Avalable";
                    res.Payload = null;
                    return Ok(res);
                }
                string catid = httpRequest.Form["cat_id"];
                string subcat = httpRequest.Form["subcat_id"];
                string product = httpRequest.Form["product_name"];
                string unit = httpRequest.Form["unit"];
                string hsn = httpRequest.Form["hsn_code"];
                string product_code = httpRequest.Form["product_code"];
                string quantity = httpRequest.Form["quantity"];
                string txttype = httpRequest.Form["txt_type"];
                string no_item = httpRequest.Form["no_of_item"];
                string price = httpRequest.Form["price"];
                string gstper = httpRequest.Form["gst_per"];
                string gstamount = httpRequest.Form["gst_amount"];
                string amount_gst = httpRequest.Form["amount_with_gst"];
                string sellprice = httpRequest.Form["sell_price"];
                string mrpprice = httpRequest.Form["mrp_price"];
                string discount = httpRequest.Form["discount"];
                string totalamount = httpRequest.Form["total_amount"];
                string discription = httpRequest.Form["product_description"];
                if (string.IsNullOrEmpty(catid) || !int.TryParse(catid, out int cat_id) || string.IsNullOrEmpty(subcat) || !int.TryParse(subcat, out int sub_cat) || string.IsNullOrEmpty(product) || string.IsNullOrEmpty(unit) || !int.TryParse(unit, out int units) || string.IsNullOrEmpty(hsn) ||
                     string.IsNullOrEmpty(product_code) || string.IsNullOrEmpty(quantity) || string.IsNullOrEmpty(txttype) || string.IsNullOrEmpty(no_item) || string.IsNullOrEmpty(price) ||
                     string.IsNullOrEmpty(gstper) || string.IsNullOrEmpty(gstamount) || string.IsNullOrEmpty(amount_gst) || string.IsNullOrEmpty(sellprice) || string.IsNullOrEmpty(mrpprice) ||
                     string.IsNullOrEmpty(discount) || string.IsNullOrEmpty(totalamount))
                {
                    res.Status = "0";
                    res.Message = "All Fields are required";
                    res.Payload = null;
                    return Ok(res);
                }

                ModProduct obj = new ModProduct
                {
                    catId = cat_id,
                    subcat_id = sub_cat,
                    product_name = product,
                    unit = units,
                    hsn_code = hsn,
                    product_code = product_code,
                    quantity = quantity,
                    txt_type = txttype,
                    no_of_item = no_item,
                    price = price,
                    gst_per = gstper,
                    gst_amount = gstamount,
                    amount_with_gst = amount_gst,
                    sell_price = sellprice,
                    mrp_price = mrpprice,
                    discount = discount,
                    total_amount = totalamount,
                    product_description = discription,

                };
                
                HttpPostedFile file = null;
                if (httpRequest.Files.Count > 0)
                {
                    file = httpRequest.Files["image"] ?? httpRequest.Files[0];
                }
                if (file != null && file.ContentLength > 0)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string filename = DateTime.Now.ToString("ddMMyyyyhhmmss") + extension;
                    string folderPath = HttpContext.Current.Server.MapPath("~/Uploads/");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);

                    }
                    string fullPath = Path.Combine(folderPath, filename);
                    file.SaveAs(fullPath);
                    obj.image = filename;
                }
                bool result = da.AddProduct(obj);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Product Data Added SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Product Data Add Faill";
                }


            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = "0";
                res.Payload = null;
            }
            return Ok(res);
        }
        [HttpPut]
        [Route("UpdateProduct")]
        public IHttpActionResult UpdateProduct()
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return Ok(res);
                }
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest == null)
                {
                    res.Status = "0";
                    res.Message = "No Http Request Available";
                    res.Payload = null;
                    return Ok(res);
                }
                var idValue = httpRequest.Form["id"];
                string catId = httpRequest.Form["cat_id"];
                string SubcatId = httpRequest.Form["subcat_id"];
                string unitId = httpRequest.Form["unit"];
                if (string.IsNullOrWhiteSpace(idValue) || !int.TryParse(idValue, out int id) || string.IsNullOrWhiteSpace(catId) || !int.TryParse(catId, out int cat_id) || string.IsNullOrWhiteSpace(SubcatId) || !int.TryParse(SubcatId, out int Sub_catId)
                    || string.IsNullOrWhiteSpace(unitId) || !int.TryParse(unitId, out int units))
                {
                    res.Status = "0";
                    res.Message = "Valid id is required";
                    res.Payload = null;
                    return Ok(res);
                }
                string product = httpRequest.Form["product_name"];
                string hsn = httpRequest.Form["hsn_code"];
                string product_code = httpRequest.Form["product_code"];
                string quantity = httpRequest.Form["quantity"];
                string txttype = httpRequest.Form["txt_type"];
                string no_item = httpRequest.Form["no_of_item"];
                string price = httpRequest.Form["price"];
                string gstper = httpRequest.Form["gst_per"];
                string gstamount = httpRequest.Form["gst_amount"];
                string amount_gst = httpRequest.Form["amount_with_gst"];
                string sellprice = httpRequest.Form["sell_price"];
                string mrpprice = httpRequest.Form["mrp_price"];
                string discount = httpRequest.Form["discount"];
                string totalamount = httpRequest.Form["total_amount"];
                string discription = httpRequest.Form["product_description"];
                if (string.IsNullOrWhiteSpace(product) || string.IsNullOrWhiteSpace(hsn) || string.IsNullOrWhiteSpace(product_code) || string.IsNullOrWhiteSpace(quantity) || string.IsNullOrWhiteSpace(hsn) || string.IsNullOrWhiteSpace(txttype)
                    || string.IsNullOrWhiteSpace(no_item) || string.IsNullOrWhiteSpace(price) || string.IsNullOrWhiteSpace(gstper) || string.IsNullOrWhiteSpace(gstamount) || string.IsNullOrWhiteSpace(amount_gst) || string.IsNullOrWhiteSpace(sellprice)
                    || string.IsNullOrWhiteSpace(mrpprice) || string.IsNullOrWhiteSpace(discount) || string.IsNullOrWhiteSpace(totalamount))
                {
                    res.Status = "0";
                    res.Message = "all Field is required";
                    res.Payload = null;
                    return Ok(res);
                }
                ModProduct obj = new ModProduct
                {
                    id = id,
                    catId = cat_id,
                    subcat_id = Sub_catId,
                    product_name = product,
                    unit = units,
                    hsn_code = hsn,
                    product_code = product_code,
                    quantity = quantity,
                    txt_type = txttype,
                    no_of_item = no_item,
                    price = price,
                    gst_per = gstper,
                    gst_amount = gstamount,
                    amount_with_gst = amount_gst,
                    sell_price = sellprice,
                    mrp_price = mrpprice,
                    discount = discount,
                    total_amount = totalamount,
                    product_description = discription,
                };
                

                HttpPostedFile file = null;
                if (httpRequest.Files.Count > 0)
                {
                    file = httpRequest.Files["image"] ?? httpRequest.Files[0];
                }
                if (file != null && file.ContentLength > 0)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string filename = DateTime.Now.ToString("ddMMyyyyhhmmss") + extension;
                    string folderPath = HttpContext.Current.Server.MapPath("~/Uploads/");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);

                    }
                    string fullPath = Path.Combine(folderPath, filename);
                    file.SaveAs(fullPath);
                    obj.image = filename;
                }
                bool result = da.UpdateProduct(obj);
                if (result)
                {
                    res.Status = "0";
                    res.Payload = result;
                    res.Message = "Product Data Updated SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Product Data Updated fail";
                }


            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = "0";
                res.Payload = null;
            }
            return Ok(res);
        }
        [HttpDelete]
        [Route("DeleteProduct")]
        public CommonResponse DeleteProduct(ModProduct obj)
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return res;
                }
                
                bool result = da.DeleteProduct(obj.id);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Product Data Deleted Success";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Product Data Deleted Fail";
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
        [HttpGet]
        [Route("GetSlider")]
        public CommonResponse GetSlider()
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return res;
                }
                DataTable dt = da.GetSlider();
                if (dt != null && dt.Rows.Count > 0)
                {
                    res.Status = "1";
                    res.Payload = dt;
                    res.Message = "Slider Data Fetch SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Slider Data Not Found";
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
        [Route("AddSlider")]
        public IHttpActionResult AddSlider()
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return Ok(res);
                }
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest == null)
                {
                    res.Status = "0";
                    res.Message = "No Http Request Available";
                    res.Payload = null;
                    return Ok(res);
                }
                string catid = httpRequest.Form["cat_id"];
                string name = httpRequest.Form["slider_name"];
                if (string.IsNullOrEmpty(catid) || !int.TryParse(catid, out int cat_id) || string.IsNullOrEmpty(name))
                {
                    res.Message = "all Filds Are Required";
                    res.Status = "0";
                    res.Payload = null;
                    return Ok(res);
                }
                ModSlider obj = new ModSlider
                {

                    cat_id = cat_id,
                    slider_name = name,

                };
                

                HttpPostedFile file = null;
                if (httpRequest.Files.Count > 0)
                {
                    file = httpRequest.Files["image"] ?? httpRequest.Files[0];
                }
                if (file != null && file.ContentLength > 0)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string filename = DateTime.Now.ToString("ddMMyyyyhhmmss") + extension;
                    string folderPath = HttpContext.Current.Server.MapPath("~/Uploads/");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);

                    }
                    string fullPath = Path.Combine(folderPath, filename);
                    file.SaveAs(fullPath);
                    obj.image = filename;
                }

                bool result = da.AddSlider(obj);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Slider Added Success";
                }
                else
                {
                    res.Status = "1";
                    res.Payload = null;
                    res.Message = "Slider Addrd Fail";
                }

            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = "0";
                res.Payload = null;
            }
            return Ok(res);
        }
        [HttpPut]
        [Route("UpdateSlider")]
        public IHttpActionResult UpdateSlider()
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return Ok(res);
                }
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest == null)
                {
                    res.Status = "0";
                    res.Message = "No Http Request Available";
                    res.Payload = null;
                    return Ok(res);
                }
                var idValue = httpRequest.Form["id"];
                string catId = httpRequest.Form["cat_id"];

                if (string.IsNullOrWhiteSpace(idValue) || !int.TryParse(idValue, out int id) || string.IsNullOrWhiteSpace(catId) || !int.TryParse(catId, out int cat_id))
                {
                    res.Status = "0";
                    res.Message = "Valid id is required";
                    res.Payload = null;
                    return Ok(res);
                }
                string slider = httpRequest.Form["slider_name"];

                if (string.IsNullOrWhiteSpace(slider))
                {
                    res.Status = "0";
                    res.Message = "all Field is required";
                    res.Payload = null;
                    return Ok(res);
                }
                ModSlider obj = new ModSlider
                {
                    id = id,
                    cat_id = cat_id,
                    slider_name = slider,

                };
                //obj.id = Convert.ToInt32(userid);

                HttpPostedFile file = null;
                if (httpRequest.Files.Count > 0)
                {
                    file = httpRequest.Files["image"] ?? httpRequest.Files[0];
                }
                if (file != null && file.ContentLength > 0)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string filename = DateTime.Now.ToString("ddMMyyyyhhmmss") + extension;
                    string folderPath = HttpContext.Current.Server.MapPath("~/Uploads/");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);

                    }
                    string fullPath = Path.Combine(folderPath, filename);
                    file.SaveAs(fullPath);
                    obj.image = filename;
                }
                bool result = da.UpdateSlider(obj);
                if (result)
                {
                    res.Status = "0";
                    res.Payload = result;
                    res.Message = "Slider Data Updated SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Slider Data Updated fail";
                }


            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = "0";
                res.Payload = null;
            }
            return Ok(res);
        }
        [HttpDelete]
        [Route("DeleteSlider")]
        public CommonResponse DeleteSlider(ModSlider obj)
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return res;
                }
                
                bool result = da.DeleteSlider(obj.id);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Slider Data Deleted SuccessFully";

                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Slider Data Deleted Fail";
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
        [HttpGet]
        [Route("GetDeliveryBoy")]
        public CommonResponse GetDeliveryBoy()
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return res;
                }
                DataTable dt = da.GetDeliveryBoy();
                if (dt != null && dt.Rows.Count > 0)
                {
                    res.Status = "1";
                    res.Payload = dt;
                    res.Message = "Delevery Boy Data Fetch SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Delevery Boy Data Fetch Fail";
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
        [Route("AddDeliveryBoy")]
        public IHttpActionResult AddDeliveryBoy()
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return Ok(res);
                }
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest == null)
                {
                    res.Status = "0";
                    res.Message = "No Http Request Available";
                    res.Payload = null;
                    return Ok(res);
                }
                string name = httpRequest.Form["name"];
                string mobile_no = httpRequest.Form["mobile_no"];
                string email = httpRequest.Form["email"];
                string dobStr = httpRequest.Form["dob"];

                string country = httpRequest.Form["country"];
                string state = httpRequest.Form["state"];
                string city = httpRequest.Form["city"];
                string address = httpRequest.Form["address"];
                string pin = httpRequest.Form["pin"];

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(mobile_no) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(country) ||
                   string.IsNullOrEmpty(state) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(pin))
                {
                    res.Message = "all Filds Are Required";
                    res.Status = "0";
                    res.Payload = null;
                    return Ok(res);
                }
                // ----------- DOB CONVERSION -----------
                DateTime dob;
                if (!DateTime.TryParse(dobStr, out dob))
                {
                    return Ok(new CommonResponse
                    {
                        Status = "0",
                        Message = "Invalid DOB format",
                        Payload = null
                    });
                }
                ModDeliveryBoy obj = new ModDeliveryBoy
                {

                    name = name,
                    mobile_no = mobile_no,
                    email = email,
                    dob = dob,
                    country = country,
                    state = state,
                    city = city,
                    address = address,
                    pin = pin

                };
                

                // ----------- FILES -----------
                HttpPostedFile imageFile = httpRequest.Files["image"];
                HttpPostedFile identityFile = httpRequest.Files["identity_proof"];

                if ((imageFile == null || imageFile.ContentLength == 0) ||
                    (identityFile == null || identityFile.ContentLength == 0))
                {
                    return Ok(new CommonResponse
                    {
                        Status = "0",
                        Message = "Both Image and Identity Proof are required",
                        Payload = null
                    });
                }

                string folderPath = HttpContext.Current.Server.MapPath("~/Uploads/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // ----------- SAVE IMAGE -----------
                string imgExt = Path.GetExtension(imageFile.FileName);
                string imgName = "IMG_" + DateTime.Now.ToString("yyyyMMddHHmmss") + imgExt;
                imageFile.SaveAs(Path.Combine(folderPath, imgName));
                obj.image = imgName;
                // ----------- SAVE IDENTITY -----------
                string idExt = Path.GetExtension(identityFile.FileName);
                string idName = "ID_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + idExt;
                identityFile.SaveAs(Path.Combine(folderPath, idName));
                obj.identity_proof = idName;

                bool result = da.AddDeliveryBoy(obj);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Delivery Boy Added Success";
                }
                else
                {
                    res.Status = "1";
                    res.Payload = null;
                    res.Message = "Delevery Addd Fail";
                }

            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = "0";
                res.Payload = null;
            }
            return Ok(res);
        }
        [HttpPut]
        [Route("UpdateDeliveryBoy")]
        public IHttpActionResult UpdateDeliveryBoy()
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return Ok(res);
                }
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest == null)
                {
                    res.Status = "0";
                    res.Message = "No Http Request Available";
                    res.Payload = null;
                    return Ok(res);
                }
                var idValue = httpRequest.Form["id"];
                if (string.IsNullOrEmpty(idValue) || !int.TryParse(idValue, out int id))
                {
                    res.Status = "0";
                    res.Message = "Valid Id Is Required";
                    res.Payload = null;
                    return Ok(res);
                }
                string name = httpRequest.Form["name"];
                string mobile_no = httpRequest.Form["mobile_no"];
                string email = httpRequest.Form["email"];
                string dobStr = httpRequest.Form["dob"];

                string country = httpRequest.Form["country"];
                string state = httpRequest.Form["state"];
                string city = httpRequest.Form["city"];
                string address = httpRequest.Form["address"];
                string pin = httpRequest.Form["pin"];
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(mobile_no) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(country) ||
                   string.IsNullOrEmpty(state) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(pin))
                {
                    res.Message = "all Filds Are Required";
                    res.Status = "0";
                    res.Payload = null;
                    return Ok(res);
                }
                // ----------- DOB CONVERSION -----------
                DateTime dob;
                if (!DateTime.TryParse(dobStr, out dob))
                {
                    return Ok(new CommonResponse
                    {
                        Status = "0",
                        Message = "Invalid DOB format",
                        Payload = null
                    });
                }
                ModDeliveryBoy obj = new ModDeliveryBoy
                {
                    name = name,
                    mobile_no = mobile_no,
                    email = email,
                    dob = dob,
                    country = country,
                    state = state,
                    city = city,
                    address = address,
                    pin = pin,
                    id = id
                };
                

                // ----------- FILES -----------
                HttpPostedFile imageFile = httpRequest.Files["image"];
                HttpPostedFile identityFile = httpRequest.Files["identity_proof"];

                if ((imageFile == null || imageFile.ContentLength == 0) ||
                    (identityFile == null || identityFile.ContentLength == 0))
                {
                    return Ok(new CommonResponse
                    {
                        Status = "0",
                        Message = "Both Image and Identity Proof are required",
                        Payload = null
                    });
                }

                string folderPath = HttpContext.Current.Server.MapPath("~/Uploads/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // ----------- SAVE IMAGE -----------
                string imgExt = Path.GetExtension(imageFile.FileName);
                string imgName = "IMG_" + DateTime.Now.ToString("yyyyMMddHHmmss") + imgExt;
                imageFile.SaveAs(Path.Combine(folderPath, imgName));
                obj.image = imgName;
                // ----------- SAVE IDENTITY -----------
                string idExt = Path.GetExtension(identityFile.FileName);
                string idName = "ID_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + idExt;
                identityFile.SaveAs(Path.Combine(folderPath, idName));
                obj.identity_proof = idName;

                bool result = da.UpdateDeliveryBoy(obj);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "DeliveryBoy Data Updated SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "DeliveryBoy Data Updated Fail";
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = "0";
                res.Payload = null;
            }
            return Ok(res);
        }
        [HttpDelete]
        [Route("DeltDeliveryBoy")]
        public CommonResponse DeltDeliveryBoy(ModDeliveryBoy obj)
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return res;
                }

                
                bool result = da.DeleteDeliveryBoy(obj.id);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Delivery Data Deleted SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Delivery Data Deleted Fail";
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
        [HttpGet]
        [Route("OrderStatus")]
        public CommonResponse OrderStatus(int status)
        {
            CommonResponse res = new CommonResponse();
            try
            {
                //string userid = checkvalidate_user();
                //if (string.IsNullOrEmpty(userid))
                //{
                //    res.Status = "2";
                //    res.Payload = null;
                //    res.Message = "Access Denied";
                //    return res;
                //}

                DataTable dt = da.OrderStatus(status);
                if (dt != null && dt.Rows.Count > 0)
                {
                    res.Status = "1";
                    res.Payload = dt;
                    res.Message = "Order Status Fetch SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Order Status Fetch Fail";
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
        [HttpPut]
        [Route("changeOrderStatus")]
        public CommonResponse changeOrderStatus(int id, int status)
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return res;
                }

                //id = Convert.ToInt32(userid);
                bool result = da.changeOrderStatus(id, status);
                if (result)
                {
                    res.Status = "1";
                    res.Payload = result;
                    res.Message = "Change Order Status SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Change Order Status Fail";
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
        [HttpGet]
        [Route("GetProfile")]
        public CommonResponse GetProfile(int id)
        {
            CommonResponse res = new CommonResponse();
            try
            {
                string userid = checkvalidate_user();
                if (string.IsNullOrEmpty(userid))
                {
                    res.Status = "2";
                    res.Payload = null;
                    res.Message = "Access Denied";
                    return res;
                }


                DataTable dt = da.GetProfile(id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    res.Status = "1";
                    res.Payload = dt;
                    res.Message = "Profile Data Fetch SuccessFully";
                }
                else
                {
                    res.Status = "0";
                    res.Payload = null;
                    res.Message = "Profile Data Fetch Fail";
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
    }
}

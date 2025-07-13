using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SchoolErp.Config;
using SchoolErp.Models.Admin;
using System.Data.SqlClient;
using System.Data;



namespace SchoolErp.Controllers.Admin
{
    public class StaffController : Controller
    {       

        // GET: Staff
        public ActionResult Staff(string SortOrder, string SortBy, int PageNumber = 1)
        {
            List<Staff> staff = new List<Staff>();           

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetStaff", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        staff.Add(
                            new Staff
                            {
                                StaffID = Convert.ToInt32(row["StaffID"].ToString()),
                                Staffname = row["FullName"].ToString(),
                                Designation = row["Designation"].ToString(),
                                Gender = row["Gender"].ToString(),
                                Nationality = row["Nationality"].ToString(),
                                DateTime =  Convert.ToDateTime(row["DateTime"].ToString())
                            }
                            );
                    }
                }
                con.Close();
                ViewBag.SortOrder = SortOrder;

                ViewBag.SortBy = SortBy;
                
            }
           
            ViewBag.TotalPages = Math.Ceiling(staff.Count() / 10.0);
            ViewBag.PageNumber = PageNumber;
            staff = staff.Skip((PageNumber - 1) * 10).Take(10).ToList();            
            return View(staff);
        }

        public ActionResult CreateStaff()
        {
            ViewBag.Title = "Create Staff";
            ViewBag.Designation = PopulateDesignation();
            ViewBag.CountryName = PopulateNationality();
            Staff staff = new Staff();

            char ciyID = staff.CountryID.ToString().FirstOrDefault();
            ViewBag.CityName = PopulateCity(1);
            return View();
        }

       
        [HttpGet]
        public ActionResult GetCityList(int CountryID)
        {
            List<City> city = new List<City>();
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetCities", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CountryID", CountryID);
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            city.Add(
                                new City
                                {
                                    CityID = Convert.ToInt32(sdr["CityID"]),
                                    CityCode = sdr["CityCode"].ToString(),
                                    CityName = sdr["CityName"].ToString()
                                });
                        }
                    }
                }
                con.Close();
                return Json(city);
            }
        }

        [HttpPost]
        public ActionResult CreateStaff(Staff staff)
        {           
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("CreateStaff", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@StaffType", staff.StaffType);
                    cmd.Parameters.AddWithValue("@Fullname", staff.Staffname);
                    cmd.Parameters.AddWithValue("@Gender", staff.Gender);
                    cmd.Parameters.AddWithValue("@Designation", staff.DesignationID);
                    cmd.Parameters.AddWithValue("@Phone1", staff.Phone1);
                    cmd.Parameters.AddWithValue("@Phone2", staff.phone2);
                    cmd.Parameters.AddWithValue("@Email", staff.Email);
                    cmd.Parameters.AddWithValue("@StaffAddress", staff.StaffAddress);
                    cmd.Parameters.AddWithValue("@dateOfAppointment", staff.DateOfAppointment);
                    cmd.Parameters.AddWithValue("@Nationality", staff.CountryID);
                    cmd.Parameters.AddWithValue("@YOE", staff.YOE);
                    cmd.Parameters.AddWithValue("@PreviousOrg", staff.previouseOrg);
                    cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Resume","r");
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("Staff");
            
        }


        public ActionResult EditStaff(int id)
        {
            ViewBag.Designation = PopulateDesignation();
            ViewBag.CountryName = PopulateNationality();
            Staff stafff = new Staff();

            DataTable dtsession = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "Select StaffID, FullName, Gender, Designation, Phone1, Phone2, Email, DateOfAppointment, Nationality, YearOfExperience, PreviousOrg, StaffAddress from StaffTbl where StaffID = @ID";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@ID", id);
                sqlDA.Fill(dtsession);
                con.Close();
            }

            if (dtsession.Rows.Count == 1)
            {
                stafff.StaffID = Convert.ToInt32(dtsession.Rows[0][0].ToString());
                stafff.Staffname = dtsession.Rows[0][1].ToString();
                stafff.Gender = dtsession.Rows[0][2].ToString();
                stafff.DesignationID = Convert.ToInt32(dtsession.Rows[0][3].ToString());
                stafff.Phone1 = dtsession.Rows[0][4].ToString();
                stafff.phone2 = dtsession.Rows[0][5].ToString();
                stafff.Email = dtsession.Rows[0][6].ToString();
                stafff.DateOfAppointment = dtsession.Rows[0][7].ToString();
                stafff.CountryID = Convert.ToInt32(dtsession.Rows[0][8].ToString());
                stafff.YOE = Convert.ToInt32(dtsession.Rows[0][9]);
                stafff.previouseOrg = dtsession.Rows[0][10].ToString();
                stafff.StaffAddress = dtsession.Rows[0][11].ToString();
                return View(stafff);
            }

            else


                return View("stafff");


        }

        [HttpPost]
        public ActionResult EditStaff(int id, Staff staff)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("EditStaff", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StaffID", id);
                    cmd.Parameters.AddWithValue("@Fullname", staff.Staffname);
                    cmd.Parameters.AddWithValue("@Gender", staff.Gender);
                    cmd.Parameters.AddWithValue("@Designation", staff.DesignationID);
                    cmd.Parameters.AddWithValue("@Phone1", staff.Phone1);
                    cmd.Parameters.AddWithValue("@Phone2", staff.phone2);
                    cmd.Parameters.AddWithValue("@Email", staff.Email);
                    cmd.Parameters.AddWithValue("@StaffAddress", staff.StaffAddress);
                    cmd.Parameters.AddWithValue("@dateOfAppointment", staff.DateOfAppointment);
                    cmd.Parameters.AddWithValue("@Nationality", staff.CountryID);
                    cmd.Parameters.AddWithValue("@YOE", staff.YOE);
                    cmd.Parameters.AddWithValue("@PreviousOrg", staff.previouseOrg);
                    cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Updated Successfully";
            return RedirectToAction("Staff");
        }


        [HttpPost]
        public ActionResult Staffs(string searchtxt)
        {
            List<Staff> staff = new List<Staff>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("SearchStaff", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Staffname", searchtxt);
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        staff.Add(
                            new Staff
                            {
                                StaffID = Convert.ToInt32(row["StaffID"].ToString()),
                                Staffname = row["FullName"].ToString(),
                                Designation = row["Designation"].ToString(),
                                Gender = row["Gender"].ToString(),
                                Nationality = row["Nationality"].ToString(),
                                AccountStatus = row["AccountStatus"].ToString(),
                                DateTime = Convert.ToDateTime(row["DateTime"].ToString())
                            });
                    }
                }
            }
            ViewBag.TotalPages = Math.Ceiling(staff.Count() / 10.0);
            return View(staff);
        }

        //[HttpPost]
        //public ActionResult Staff(string searchtxt)
        //{
        //    List<Staff> staff = new List<Staff>();

        //    using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("SearchStaff", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@Staffname", searchtxt);
        //            if (con.State != System.Data.ConnectionState.Open)

        //                con.Open();

        //            SqlDataReader sdr = cmd.ExecuteReader();

        //            DataTable dtTable = new DataTable();

        //            dtTable.Load(sdr);

        //            foreach (DataRow row in dtTable.Rows)
        //            {
        //                staff.Add(
        //                    new Staff
        //                    {
        //                        StaffID = Convert.ToInt32(row["StaffID"].ToString()),
        //                        Staffname = row["FullName"].ToString(),
        //                        Designation = row["Designation"].ToString(),
        //                        Gender = row["Gender"].ToString(),
        //                        Nationality = row["Nationality"].ToString(),
        //                        AccountStatus = row["AccountStatus"].ToString(),
        //                        DateTime = Convert.ToDateTime(row["DateTime"].ToString())
        //                    });
        //            }
        //        }
        //    }
        //    ViewBag.TotalPages = Math.Ceiling(staff.Count() / 10.0);
        //    return View(staff);
        //}

        /// <summary>
        /// get the City Value frm Selected Country
        /// </summary>
        /// <param name="countryid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Staff(int countryid)
        {
            List<City> city = new List<City>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetCities", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CountryID", countryid);
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        city.Add(
                            new City
                            {
                                CityID = Convert.ToInt32(row["CityID"].ToString()),
                                CityCode = row["CityCode"].ToString(),
                                CityName = row["CityName"].ToString()


                            }

                            );
                    }
                }
            }
           
            return View(city);
        }

        public ActionResult StaffDetails(int id)
        {

            Staff stf = new Staff();

            DataTable dtTable = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "Select s.StaffID, s.FullName, s.Gender, dl.Designation, s.Phone1, s.Phone2, s.Email,s.DateOfAppointment,c.CountryName as Nationality, " +
                    "s.YearOfExperience, s.PreviousOrg, s.AccountStatus, s.StaffAddress  from StaffTbl s inner join Designation dl on s.Designation = dl.DesignationID " +
                    "inner join Country c on s.Nationality = c.CountryID   where s.StaffID = @Id";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@Id", id);
                sqlDA.Fill(dtTable);
                con.Close();
            }

            if (dtTable.Rows.Count == 1)
            {
                stf.StaffID = Convert.ToInt32(dtTable.Rows[0][0].ToString());
                stf.Staffname = dtTable.Rows[0][1].ToString();
                stf.Gender = dtTable.Rows[0][2].ToString();
                stf.Designation = dtTable.Rows[0][3].ToString();
                stf.Phone1 = dtTable.Rows[0][4].ToString();
                stf.phone2 = dtTable.Rows[0][5].ToString();
                stf.Email = dtTable.Rows[0][6].ToString();
                stf.DateOfAppointment = dtTable.Rows[0][7].ToString();
                stf.Nationality = dtTable.Rows[0][8].ToString();
                stf.YOE = Convert.ToInt32(dtTable.Rows[0][9].ToString());
                stf.previouseOrg = dtTable.Rows[0][10].ToString();
                stf.AccountStatus = dtTable.Rows[0][11].ToString();
                stf.StaffAddress = dtTable.Rows[0][12].ToString();

                return View(stf);
            }

            else


                return View("Student");
        }

        public ActionResult DeleteStaff(int id)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteStaff", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Deleted Successfully";
            return RedirectToAction("Staff");
        }       

        public ActionResult StaffAccountStatus(string SortOrder, string SortBy, int PageNumber = 1)
        {
            List<Staff> staff = new List<Staff>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetStaff", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        staff.Add(
                            new Staff
                            {
                                StaffID = Convert.ToInt32(row["StaffID"].ToString()),
                                Staffname = row["FullName"].ToString(),
                                Designation = row["Designation"].ToString(),
                                Gender = row["Gender"].ToString(),
                                AccountStatus = row["AccountStatus"].ToString(),
                                DateTime = Convert.ToDateTime(row["DateTime"].ToString())

                            }

                            );
                    }
                }
                con.Close();
                ViewBag.SortOrder = SortOrder;

                ViewBag.SortBy = SortBy;

            }

            ViewBag.TotalPages = Math.Ceiling(staff.Count() / 5.0);
            ViewBag.PageNumber = PageNumber;
            staff = staff.Skip((PageNumber - 1) * 5).Take(5).ToList();
            return View(staff);

        }

        public ActionResult ActivateStaffAccount(int id)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("Update StaffTbl set AccountStatus='Active' where StaffID ='" + id + "'", con))
                {
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    cmd.ExecuteNonQuery();
                    TempData["SuccessMessage"] = "Record Updated Successfully";
                    return RedirectToAction("StaffAccountStatus");
                }
                con.Close();
            }
        }

        public ActionResult DeactivateStaffAccount(int id)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("Update StaffTbl set AccountStatus='In-Active' where StaffID ='" + id + "'", con))
                {
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    cmd.ExecuteNonQuery();
                    TempData["SuccessMessage"] = "Record Updated Successfully";
                    return RedirectToAction("StaffAccountStatus");
                }
                con.Close();
            }
        }

        public ActionResult Role(string SortOrder, string SortBy, int PageNumber = 1)
        {
            List<Role> role = new List<Role>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetRole", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        role.Add(
                            new Role
                            {
                                RoleID = Convert.ToInt32(row["RoleID"].ToString()),
                                Rolename = row["Role"].ToString(),
                                StaffID = Convert.ToInt32(row["StaffID"].ToString()),
                                Staffname = row["Fullname"].ToString(),
                                Username = row["Username"].ToString(),
                            }

                            );
                    }
                }
                con.Close();
                ViewBag.SortOrder = SortOrder;

                ViewBag.SortBy = SortBy;

            }

            ViewBag.TotalPages = Math.Ceiling(role.Count() / 5.0);
            ViewBag.PageNumber = PageNumber;
            role = role.Skip((PageNumber - 1) * 5).Take(5).ToList();
            return View(role);
        }

        public ActionResult CreateRole(string SortOrder, string SortBy, int PageNumber = 1)
        {
            List<Staff> staff = new List<Staff>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("getActiveStaffs", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        staff.Add(
                            new Staff
                            {
                                StaffID = Convert.ToInt32(row["StaffID"].ToString()),
                                Staffname = row["FullName"].ToString(),
                                Designation = row["Designation"].ToString(),
                                Gender = row["Gender"].ToString(),
                                AccountStatus = row["AccountStatus"].ToString()


                            }

                            );
                    }

                }
                con.Close();
                ViewBag.SortOrder = SortOrder;

                ViewBag.SortBy = SortBy;

            }

            ViewBag.TotalPages = Math.Ceiling(staff.Count() / 5.0);
            ViewBag.PageNumber = PageNumber;
            staff = staff.Skip((PageNumber - 1) * 5).Take(5).ToList();
            return View(staff);

        }

        public ActionResult AddRole(int id)
        {
            Role role = new Role();

            DataTable dtsession = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "Select * from StaffTbl where StaffID = @ID";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@ID", id);

                sqlDA.Fill(dtsession);
                con.Close();
                con.Close();
            }

            if (dtsession.Rows.Count == 1)
            {
                role.StaffID = Convert.ToInt32(dtsession.Rows[0][0].ToString());
                role.Staffname = dtsession.Rows[0][1].ToString();
                return View(role);
            }

            else


                return View("CreateRole");


        }

        [HttpPost]
        public ActionResult Addrole(Role role)
        {

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM RoleTbl WHERE Username = @uName", con))
                {
                    cmd.Parameters.AddWithValue("@uName", role.Username);

                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();

                    int count = (int)cmd.ExecuteScalar();
                    string sd; int df = 0;
                    if (role.StaffID > 0)
                    {
                        df = role.StaffID;

                    }
                    con.Close();
                    sd = Convert.ToString(df);
                    string vw = "AddRole/" + sd;
                    if (count > 0)
                    {
                        TempData["SuccessMessage"] = "User name already exists, Please use another username";
                        return RedirectToAction(vw);

                    }
                }
                con.Close();
            }
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("insertRole", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RoleName", role.Rolename);
                    cmd.Parameters.AddWithValue("@StaffID", role.StaffID);

                    cmd.Parameters.AddWithValue("@Username", role.Username);
                    cmd.Parameters.AddWithValue("@Password", role.Password);
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("Role");
        }

        //Delete the role
        public ActionResult DeleteRole(int id)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteRole", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RoleID", id);

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Deleted Successfully";
            return RedirectToAction("Role");
        }


        //As
        public ActionResult AssignedClass()
        {
            List<AssignClass> assign = new List<AssignClass>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {             
                
                using (SqlCommand cmd = new SqlCommand("getAssignClass", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        assign.Add(
                            new AssignClass
                            {
                                ID = Convert.ToInt32(row["ID"].ToString()),
                                Staffname = row["FullName"].ToString(),
                                ClassLevelName = row["ClassLevel"].ToString(),
                                Prefix = row["Prefix"].ToString()

                            }

                            );
                    }

                }
                con.Close();
            }
            return View(assign);
        }

        public ActionResult AssignClass()
        {
            ViewBag.Staff = PopulateStaff();
            ViewBag.ClassLevel = PopulateClassLevel();
            return View();
        }

        [HttpPost]
        public ActionResult AssignClass(AssignClass assign)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM AssignClass WHERE StaffID = @StaffID", con))
                {
                    cmd.Parameters.AddWithValue("@StaffID", assign.StaffID);
                    con.Open();
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        TempData["SuccessMessage"] = "Staff already exists, Please use another Other";
                        return RedirectToAction("AssignClass");                        
                    }
                    con.Close();
                }
                using (SqlCommand cmd = new SqlCommand("AssignClasses", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StaffID", assign.StaffID);
                    cmd.Parameters.AddWithValue("@ClassLevelID", assign.ClassLevelID);
                    cmd.Parameters.AddWithValue("@Prefix", assign.Prefix);
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("AssignedClass");
        }

        public ActionResult EditAssignClass(int id)
        {
            ViewBag.Staff = PopulateStaff();
            ViewBag.ClassLevel = PopulateClassLevel();
            AssignClass asgnclass = new AssignClass();

            DataTable dtTable = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "Select cstb.ID, sttb.StaffID, cstb.ClassLevelID, cstb.Prefix   from AssignClass cstb INNER JOIN StaffTbl sttb ON cstb.StaffID = sttb.StaffID INNER JOIN ClassLevel cltb ON cltb.ID = cstb.ClassLevelID where cstb.ID = @Id";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@Id", id);
                sqlDA.Fill(dtTable);

            }
            if (dtTable.Rows.Count == 1)
            {
                asgnclass.ID = Convert.ToInt32(dtTable.Rows[0][0].ToString());
                asgnclass.StaffID = Convert.ToInt32(dtTable.Rows[0][1].ToString());
                asgnclass.ClassLevelID = Convert.ToInt32(dtTable.Rows[0][2].ToString());
                asgnclass.Prefix = dtTable.Rows[0][3].ToString();

                return View(asgnclass);
            }
            else
                return View("AssignedClass");
        }

        [HttpPost]
        public ActionResult EditAssignClass(int id, AssignClass assign)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateAssignClass", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@StaffID", assign.StaffID);
                    cmd.Parameters.AddWithValue("@ClassLevelID", assign.ClassLevelID);
                    cmd.Parameters.AddWithValue("@Prefix", assign.Prefix);
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();
                    cmd.ExecuteNonQuery();

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }

            return RedirectToAction("AssignedClass");
        }
        //End Assign Classes
        public ActionResult DeleteAssignedClasses(int id)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteAssignedClasses", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Deleted Successfully";
            return RedirectToAction("AssignedClass");
        }

        public ActionResult AssignedSubject()
        {

            List<AssignSubject> assign = new List<AssignSubject>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("getAssignedSubject", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        assign.Add(
                            new AssignSubject
                            {
                                ID = Convert.ToInt32(row["ID"].ToString()),
                                staffname = row["FullName"].ToString(),
                                Subject = row["Subject"].ToString(),
                                ClassLevelname = row["ClassLevel"].ToString(),
                                Prefix = row["Prefix"].ToString(),


                            }

                            );
                    }

                }
                con.Close();
            }
            return View(assign);
        }

        public ActionResult AssignSubject()
        {
            ViewBag.Subject = PopulateSubject();
            ViewBag.Staff = PopulateStaff();
            ViewBag.ClassLevel = PopulateClassLevel();
            return View();
        }

        [HttpPost]
        public ActionResult AssignSubject(AssignSubject assign)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("AssignSubjects", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StaffID", assign.StaffID);
                    cmd.Parameters.AddWithValue("@SubjectID", assign.SubjectID);
                    cmd.Parameters.AddWithValue("@LevelID", assign.ClassLevelID);
                    cmd.Parameters.AddWithValue("@Prefix", assign.Prefix);
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("AssignedSubject");
        }

        public ActionResult EditAssignSubject(int id)
        {
            ViewBag.Subject = PopulateSubject();
            ViewBag.Staff = PopulateStaff();
            ViewBag.ClassLevel = PopulateClassLevel();

            AssignSubject assignSubject = new AssignSubject();

            DataTable dtassignSubject = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "Select * from AssignSubject where ID = @ID";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@ID", id);
                sqlDA.Fill(dtassignSubject);
                con.Close();
            }

            if (dtassignSubject.Rows.Count == 1)
            {
                assignSubject.ID = Convert.ToInt32(dtassignSubject.Rows[0][0].ToString());
                assignSubject.StaffID = Convert.ToInt32(dtassignSubject.Rows[0][1].ToString());
                assignSubject.SubjectID = Convert.ToInt32(dtassignSubject.Rows[0][2].ToString());
                assignSubject.ClassLevelID = Convert.ToInt32(dtassignSubject.Rows[0][3].ToString());
                assignSubject.Prefix = dtassignSubject.Rows[0][4].ToString();

                return View(assignSubject);
            }

            else


                return View("AssignedSubject");


        }

        [HttpPost]
        public ActionResult EditAssignSubject(int id, AssignSubject assignSubject)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("EditAssignSubject", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@StaffID", assignSubject.StaffID);
                    cmd.Parameters.AddWithValue("@SubjectID", assignSubject.SubjectID);
                    cmd.Parameters.AddWithValue("@LevelID", assignSubject.ClassLevelID);
                    cmd.Parameters.AddWithValue("@Prefix", assignSubject.Prefix);

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Updated Successfully";
            return RedirectToAction("AssignedSubject");
        }

        public ActionResult DeleteAssignedSubject(int id)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteAssignedSubject", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Deleted Successfully";
            return RedirectToAction("AssignedSubject");
        }

        public ActionResult StaffFinance()
        {
            List<StaffFinance> staff = new List<StaffFinance>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetStaffFinance", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        staff.Add(
                            new StaffFinance
                            {
                                ID = Convert.ToInt32(row["ID"].ToString()),
                                staffID = Convert.ToInt32(row["StaffID"].ToString()),
                                StaffName = row["FullName"].ToString(),
                                TotalPay = Convert.ToInt32(row["TotalPay"].ToString())


                            }

                            );
                    }

                }
                con.Close();
            }

            return View(staff);
        }


        public ActionResult StaffListForSalary()
        {
            List<Staff> staff = new List<Staff>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetStaff", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        staff.Add(
                            new Staff
                            {
                                StaffID = Convert.ToInt32(row["StaffID"].ToString()),
                                Staffname = row["FullName"].ToString(),
                                Designation = row["Designation"].ToString(),
                                Gender = row["Gender"].ToString()


                            }

                            );
                    }
                }
                con.Close();
            }

            return View(staff);
        }

        public ActionResult CreateStaffFinance(int id)
        {
            StaffFinance finance = new StaffFinance();

            DataTable dtTable = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "Select * from StaffTbl where StaffID = @ID";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@ID", id);

                sqlDA.Fill(dtTable);

            }
            if (dtTable.Rows.Count == 1)
            {
                finance.staffID = Convert.ToInt32(dtTable.Rows[0][0].ToString());
                finance.StaffName = dtTable.Rows[0][1].ToString();
                return View(finance);
            }

            else


                return View("StaffListForSalary");



        }

        [HttpPost]
        public ActionResult CreateStaffFinance(StaffFinance finance)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("InsertStaffFinance", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StaffID", finance.staffID);
                    cmd.Parameters.AddWithValue("@BasicSalary", finance.BasicSalary);
                    cmd.Parameters.AddWithValue("@HouseAllowance", finance.HouseAllowance);
                    cmd.Parameters.AddWithValue("@TransportAllowance", finance.TransportAllowance);
                    cmd.Parameters.AddWithValue("@LateComingFee", finance.LateComingFee);
                    cmd.Parameters.AddWithValue("@Tax", finance.Tax);
                    cmd.Parameters.AddWithValue("@Vat", finance.Vat);
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("StaffFinance");
        }

        public ActionResult GeneratePayrollSLip(int id)
        {

            StaffFinance finance = new StaffFinance();

            DataTable dtsession = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                //string query = "select * from StaffFinance sf inner join StaffTbl s on sf.StaffID = s.StaffID  where sf.StaffID = @ID";
                string query = "select s.StaffID, BasicSalary, HouseAllowance,TransportAllowance, Tax, Vat, TotalPay, FullName, sf.ID   from StaffFinance sf inner join StaffTbl s on sf.StaffID = s.StaffID  where sf.ID = " + id;
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                //sqlDA.SelectCommand.Parameters.AddWithValue("@ID", id);

                sqlDA.Fill(dtsession);

            }
            if (dtsession.Rows.Count == 1)
            {
                finance.staffID = Convert.ToInt32(dtsession.Rows[0][0].ToString());

                finance.BasicSalary = Convert.ToDecimal(dtsession.Rows[0][1].ToString());
                finance.HouseAllowance = Convert.ToDecimal(dtsession.Rows[0][2].ToString());
                finance.TransportAllowance = Convert.ToDecimal(dtsession.Rows[0][3].ToString());
                finance.Tax = Convert.ToDecimal(dtsession.Rows[0][4].ToString());
                finance.Vat = Convert.ToDecimal(dtsession.Rows[0][5].ToString());
                finance.TotalPay = Convert.ToDecimal(dtsession.Rows[0][6].ToString());
                finance.StaffName = dtsession.Rows[0][7].ToString();
                finance.ID = Convert.ToInt32(dtsession.Rows[0][8].ToString());
                return View(finance);
            }

            else


                return View("StaffFinance");

        }

        public ActionResult DeleteStaffFinance(int id)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteStaffFinance", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Deleted Successfully";
            return RedirectToAction("StaffFinance");
        }

        public ActionResult StaffAttendance()
        {
            List<StaffAttendance> staff = new List<StaffAttendance>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("getStaffAttendance", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        staff.Add(
                            new StaffAttendance
                            {
                                ID = Convert.ToInt32(row["ID"].ToString()),
                                StaffID = Convert.ToInt32(row["StaffID"].ToString()),
                                Staffname = row["FullName"].ToString(),
                                Date = row["Date"].ToString(),
                                Remark = row["Remarks"].ToString()



                            }

                            );
                    }
                }
            }


            return View(staff);
        }

        public ActionResult EditStaffAttendance(int id)
        {

            ViewBag.Staff = PopulateAllStaff();

            StaffAttendance staffAttendance = new StaffAttendance();

            DataTable dtassignSubject = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "Select * from StaffAttendance where ID = @ID";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@ID", id);
                sqlDA.Fill(dtassignSubject);
                con.Close();
            }

            if (dtassignSubject.Rows.Count == 1)
            {
                staffAttendance.ID = Convert.ToInt32(dtassignSubject.Rows[0][0].ToString());
                staffAttendance.StaffID = Convert.ToInt32(dtassignSubject.Rows[0][1].ToString());
                staffAttendance.Date = dtassignSubject.Rows[0][2].ToString();
                staffAttendance.Remark = dtassignSubject.Rows[0][3].ToString();


                return View(staffAttendance);
            }

            else


                return View("staffAttendance");


        }

        [HttpPost]
        public ActionResult EditStaffAttendance(int id, StaffAttendance staffAttendance)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("EditStaffAttendance", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@StaffID", staffAttendance.StaffID);
                    cmd.Parameters.AddWithValue("@Date", staffAttendance.Date);
                    cmd.Parameters.AddWithValue("@Remark", staffAttendance.Remark);

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Updated Successfully";
            return RedirectToAction("staffAttendance");
        }

        public ActionResult DeleteStaffAttendance(int id)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteStaffAttendance", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Deleted Successfully";
            return RedirectToAction("StaffAttendance");
        }

        private static List<Staff> PopulateAllStaff()
        {
            List<Staff> staff = new List<Staff>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {

                using (SqlCommand cmd = new SqlCommand("select  * from StaffTbl  Order by FullName ASC ", con))
                {
                    cmd.Connection = con;

                    con.Open();

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            staff.Add(
                                new Staff
                                {
                                    StaffID = Convert.ToInt32(sdr["StaffID"]),
                                    Staffname = sdr["FullName"].ToString()
                                }

                                );
                        }
                        con.Close();
                    }


                }

                return staff;

            }
        }

        public ActionResult CreateStaffAttendance()
        {
            ViewBag.Staff = PopulateAllStaff();

            return View();
        }

        [HttpPost]
        public ActionResult CreateStaffAttendance(StaffAttendance staff)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("insertStaffAttendance", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StaffID", staff.StaffID);
                    cmd.Parameters.AddWithValue("@Date", staff.Date);
                    cmd.Parameters.AddWithValue("@Remark", staff.Remark);
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }

            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("StaffAttendance");
        }


        private static List<ClassLevel> PopulateClassLevel()
        {
            List<ClassLevel> level = new List<ClassLevel>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {

                using (SqlCommand cmd = new SqlCommand("Select  * from ClassLevel ", con))
                {
                    cmd.Connection = con;

                    con.Open();

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            level.Add(
                                new ClassLevel
                                {
                                    LevelID = Convert.ToInt32(sdr["ID"]),
                                    Levelname = sdr["ClassLevel"].ToString()
                                }

                                );
                        }

                    }


                }
                con.Close();
                return level;

            }
        }

        private static List<Staff> PopulateStaff()
        {
            List<Staff> staff = new List<Staff>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {

                using (SqlCommand cmd = new SqlCommand("select  * from StaffTbl s inner join Designation d  on s.Designation = d.DesignationID where  AccountStatus = 'Active' Order by FullName ASC", con))
                {
                    cmd.Connection = con;

                    con.Open();

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            staff.Add(
                                new Staff
                                {
                                    StaffID = Convert.ToInt32(sdr["StaffID"]),
                                    Staffname = sdr["FullName"].ToString()
                                }

                                );
                        }

                    }


                }
                con.Close();
                return staff;

            }
        }

        private static List<Subject> PopulateSubject()
        {
            List<Subject> subject = new List<Subject>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {

                using (SqlCommand cmd = new SqlCommand("select  * from Subject ", con))
                {
                    cmd.Connection = con;

                    con.Open();

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            subject.Add(
                                new Subject
                                {
                                    SubjectID = Convert.ToInt32(sdr["ID"]),
                                    Subjectname = sdr["Subject"].ToString()
                                }

                                );
                        }

                    }


                }
                con.Close();
                return subject;

            }
        }

        private static List<Designation> PopulateDesignation()
        {
            List<Designation> designation = new List<Designation>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("Select  * from Designation", con))
                {
                    cmd.Connection = con;

                    con.Open();

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            designation.Add(
                                new Designation
                                {
                                    DesignationID = Convert.ToInt32(sdr["DesignationID"]),
                                    DesignationName = sdr["Designation"].ToString()
                                } );
                        }
                        con.Close();
                    }
                }
                con.Close();
                return designation;
            }
        }

        private static List<Nationality> PopulateNationality()
        {
            List<Nationality> country = new List<Nationality>();
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("Select  * from Country ", con))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            country.Add(
                                new Nationality
                                {
                                    CountryID = Convert.ToInt32(sdr["CountryID"]),
                                    CountryCode = sdr["CountryCode"].ToString(),
                                    NationalityName = sdr["CountryName"].ToString()
                                }  );
                        }
                    }
                }
                con.Close();
                return country;
            }
        }


        private static List<City> PopulateCity(int countryID)
        {
            List<City> city = new List<City>();
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("Select CityID, CityCode, CityName from City", con))
                {
                    cmd.Connection = con;
                    con.Open();
                   
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            city.Add(
                                new City
                                {
                                    CityID = Convert.ToInt32(sdr["CityID"]),
                                    CityCode = sdr["CityCode"].ToString(),
                                    CityName = sdr["CityName"].ToString()
                                });
                        }
                    }
                }
                con.Close();
                return city;
            }
        }
    }




}


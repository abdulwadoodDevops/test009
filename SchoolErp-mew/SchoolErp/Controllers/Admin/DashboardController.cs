using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolErp.Config;
using SchoolErp.Models.Admin;
using System.Data.SqlClient;
using System.Data;

namespace SchoolErp.Controllers.Admin
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index(int PageNumber = 1)
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
                                AccountStatus = row["AccountStatus"].ToString(),
                                DateTime = Convert.ToDateTime(row["DateTime"].ToString())
                            }

                            );
                    }
                }
            }
            ViewBag.TotalPages = Math.Ceiling(staff.Count() / 5.0);
            ViewBag.PageNumber = PageNumber;
            staff = staff.Skip((PageNumber - 1) * 5).Take(5).ToList();
            return View(staff);
        }

        [HttpPost]
        public ActionResult Index(string searchtxt)
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
            ViewBag.TotalPages = Math.Ceiling(staff.Count() / 5.0);
            return View(staff);
        }

        public ActionResult Student(string SortOrder, string SortBy, int PageNumber = 1)
        {
            List<Student> student = new List<Student>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetStudentList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtSessions = new DataTable();

                    dtSessions.Load(sdr);

                    foreach (DataRow row in dtSessions.Rows)
                    {
                        student.Add(
                            new Student
                            {
                                AdmissionNo = Convert.ToInt32(row["AdmissionNo"].ToString()),
                                StudentName = row["FullName"].ToString(),
                                SessionName = row["Session"].ToString(),
                                ClassLevel = row["ClassLevel"].ToString(),
                                ClassAphabet = row["ClassAlphabet"].ToString(),
                                Gender = row["Gender"].ToString(),
                                DateTime = Convert.ToDateTime(row["DateTime"].ToString())
                            });
                    }
                }

                ViewBag.SortOrder = SortOrder;

                ViewBag.SortBy = SortBy;

            }
            ViewBag.TotalPages = Math.Ceiling(student.Count() / 5.0);
            ViewBag.PageNumber = PageNumber;
            student = student.Skip((PageNumber - 1) * 5).Take(10).ToList();
            return View(student);
        }

        [HttpPost]
        public ActionResult Student(string searchtxt)
        {          
            List<Student> student = new List<Student>();
            string q1 = "SELECT std.AdmissionNo, std.Fullname, sstb.Session, clstb.ClassLevel, std.Gender FROM Student std INNER JOIN SessionTble sstb ON std.Session = sstb.ID INNER JOIN ClassLevel clstb ON std.ClassLevel = clstb.ID where std.Fullname LIKE '%' + @student + '%'";
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand(q1, con))
                {
                    cmd.Parameters.AddWithValue("@student", searchtxt);
                   
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtclass = new DataTable();

                    dtclass.Load(sdr);

                    foreach (DataRow row in dtclass.Rows)
                    {
                        student.Add(
                            new Student
                            {
                                AdmissionNo = Convert.ToInt32(row["AdmissionNo"].ToString()),
                                StudentName = row["FullName"].ToString(),
                                SessionName = row["Session"].ToString(),
                                ClassLevel = row["ClassLevel"].ToString(),                               
                                Gender = row["Gender"].ToString()
                            });
                    }
                }
                con.Close();
            }
            ViewBag.TotalPages = Math.Ceiling(student.Count() / 5.0);
            return View(student);
        }
        
        public ActionResult Subject(string SortOrder, string SortBy, int PageNumber = 1)
        {
            int pageSize = 4; // Number of items to display per page
            List<Subject> sbj = new List<Subject>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetSubject", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dtSessions = new DataTable();
                    dtSessions.Load(sdr);

                    foreach (DataRow row in dtSessions.Rows)
                    {
                        sbj.Add(new Subject
                        {
                            SubjectID = Convert.ToInt32(row["ID"].ToString()),
                            Subjectname = row["Subject"].ToString(),
                            SectionName = row["ClassType"].ToString()
                        });
                    }
                }

                // Apply sorting
                // Here you can add the logic for sorting the sbj list based on SortOrder and SortBy parameters

                // Pagination logic
                int totalItems = sbj.Count;
                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                ViewBag.TotalPages = totalPages;
                ViewBag.PageNumber = PageNumber;

                sbj = sbj.Skip((PageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortBy = SortBy;
            return View(sbj);
        }

        [HttpPost]
        public ActionResult Subject(string searchtxt)
        {
           
            List<Subject> sbj = new List<Subject>();
            string q1 = "SELECT sbj.ID, sbj.Subject, sctbl.ClassType FROM Subject sbj INNER JOIN Sections sctbl ON sbj.SectionID = sctbl.ID WHERE sbj.Subject LIKE '%' + @Subject + '%'";
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand(q1, con))
                {

                    cmd.Parameters.AddWithValue("@Subject", searchtxt);
                    
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtSubject = new DataTable();

                    dtSubject.Load(sdr);

                    foreach (DataRow row in dtSubject.Rows)
                    {
                        sbj.Add(
                            new Subject
                            {
                                SubjectID = Convert.ToInt32(row["ID"].ToString()),
                                Subjectname = row["Subject"].ToString(),
                                SectionName = row["ClassType"].ToString()

                            });
                    }
                }
                con.Close();
            }
            ViewBag.TotalPages = Math.Ceiling(sbj.Count() / 4.0);
            return View(sbj);
        }

        public ActionResult Class(string SortOrder, string SortBy, int PageNumber = 1)
        {
            List<ClassLevel> sbj = new List<ClassLevel>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetClassLevel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtclass = new DataTable();

                    dtclass.Load(sdr);

                    foreach (DataRow row in dtclass.Rows)
                    {
                        sbj.Add(
                            new ClassLevel
                            {
                                LevelID = Convert.ToInt32(row["ID"].ToString()),
                                Levelname = row["ClassLevel"].ToString(),
                                Section = row["ClassType"].ToString()
                            });
                    }
                }
                con.Close();
                ViewBag.SortOrder = SortOrder;
                ViewBag.SortBy = SortBy;
            }
            
            ViewBag.TotalPages = Math.Ceiling(sbj.Count() / 5.0);
            ViewBag.PageNumber = PageNumber;
            sbj = sbj.Skip((PageNumber - 1) * 5).Take(5).ToList();
            return View(sbj);
        }

        [HttpPost]
        public ActionResult Class(string searchtxt)
        {
            List<ClassLevel> clevel = new List<ClassLevel>();
            string q1 = "SELECT clevel.ID, clevel.ClassLevel, sctbl.ClassType FROM ClassLevel clevel INNER JOIN Sections sctbl ON" + 
                        "clevel.SectionID = sctbl.ID WHERE clevel.ClassLevel LIKE '%' + @ClassLevel + '%'";
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand(q1, con))
                {
                   
                    cmd.Parameters.AddWithValue("@ClassLevel", searchtxt);
                    //SqlDataAdapter sqlDA = new SqlDataAdapter(q1, con);
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtclass = new DataTable();

                    dtclass.Load(sdr);

                    foreach (DataRow row in dtclass.Rows)
                    {
                        clevel.Add(
                            new ClassLevel
                            {
                                LevelID = Convert.ToInt32(row["ID"].ToString()),
                                Levelname = row["ClassLevel"].ToString(),
                                Section = row["ClassType"].ToString()

                            });
                    }
                }
                con.Close();
            }
            ViewBag.TotalPages = Math.Ceiling(clevel.Count() / 5.0);
            return View(clevel);
        }
    }
}
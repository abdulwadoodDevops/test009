using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolErp.Config;
using SchoolErp.Models.Admin;
using System.Data.SqlClient;
using SchoolErp.Models.Teacher;
using System.Data;
namespace SchoolErp.Controllers.Teacher
{
    public class TeacherDashboardController : Controller
    {
        // GET: TeacherDashboard
        public ActionResult Index(int PageNumber = 1)
        {
            string Username = (string)Session["Username"];
            List<SubjectMark> student = new List<SubjectMark>();
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                string smd = "Select sm.ID, sm.AdmissionNo, st.Session, sb.Subject, cl.ClassLevel, sm.Total, s.FullName from SubjectMark sm INNER JOIN Student s ON sm.AdmissionNo = s.AdmissionNo INNER JOIN ClassLevel cl ON cl.ID = sm.ClassID INNER JOIN Subject sb ON sb.ID = sm.SubjectID INNER JOIN SessionTble st ON st.ID = sm.SessionID";
                using (SqlCommand cmd = new SqlCommand(smd, con))
                {
                   
                    //cmd.Parameters.AddWithValue("@Username", Username);
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        student.Add(
                            new SubjectMark
                            {
                                AdmissionNo = Convert.ToInt32(row["AdmissionNo"].ToString()),
                                StudentName = row["FullName"].ToString(),
                                SessionName = row["Session"].ToString(),
                                Subjectname = row["Subject"].ToString(),
                                ClassLevelname = row["ClassLevel"].ToString(),
                                Total = Convert.ToInt32(row["Total"].ToString())

                            }

                            );
                    }
                }
            }
            ViewBag.TotalPages = Math.Ceiling(student.Count() / 10.0);
            ViewBag.PageNumber = PageNumber;
            student = student.Skip((PageNumber - 1) * 10).Take(10).ToList();
            return View(student);
        }


        [HttpPost]
        public ActionResult Index(string searchtxt)
        {
            string Username = (string)Session["Username"];
            List<Student> student = new List<Student>();
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("SearchAssignedStudent", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Studentname", searchtxt);
                    cmd.Parameters.AddWithValue("@Username", Username);
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        student.Add(
                            new Student
                            {
                                AdmissionNo = Convert.ToInt32(row["AdmissionNo"].ToString()),
                                StudentName = row["FullName"].ToString(),
                                ClassLevel = row["ClassLevel"].ToString(),
                                ClassAphabet = row["ClassAlphabet"].ToString(),
                                Gender = row["Gender"].ToString()

                            }

                            );
                    }
                }
            }
            ViewBag.TotalPages = Math.Ceiling(student.Count() / 10.0);
            return View(student);

        }
    }
}





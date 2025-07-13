using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using SchoolErp.Config;
using SchoolErp.Models.Admin;
using System.Data.SqlClient;
using System.Data;
namespace SchoolErp.Controllers.Admin
{
    public class StudentController : Controller
    {
        // GET: Student

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
                            } );
                    }
                }

                ViewBag.SortOrder = SortOrder;

                ViewBag.SortBy = SortBy;
            }

            ViewBag.TotalPages = Math.Ceiling(student.Count() / 10.0);
            ViewBag.PageNumber = PageNumber;
            student = student.Skip((PageNumber - 1) * 10).Take(10).ToList();
            return View(student);
        }

        [HttpPost]
        public ActionResult Student(string searchtxt)
        {
            List<Student> student = new List<Student>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("SearchStudent", con))
                {
                    cmd.Parameters.AddWithValue("@Studentname", searchtxt);
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
            }
            ViewBag.TotalPages = Math.Ceiling(student.Count() / 10.0);
            return View(student);
        }


        public ActionResult CreateStudent()
        {
            ViewBag.Session = PopulateSession();
            ViewBag.ClassLevel = PopulateClassLevel();
            ViewBag.CountryName = PopulateNationality();
            return View();
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
                                });
                        }
                    }
                }
                con.Close();
                return country;
            }
        }

        [HttpPost]
        public ActionResult CreateStudent(Student student)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Student WHERE FullName =  @sname", con))
                {
                    cmd.Parameters.AddWithValue("@sname", student.StudentName);

                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();

                    int count = (int)cmd.ExecuteScalar();
                    con.Close();
                    if (count > 0)
                    {
                        TempData["SuccessMessage"] = "Student already exists, Please Add New Student";
                        return RedirectToAction("CreateStudent");

                    }
                }
                con.Close();
            }
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("CreateStudent", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Fullname", student.StudentName);
                    cmd.Parameters.AddWithValue("@DOB", student.DOB);
                    cmd.Parameters.AddWithValue("@Nationality", student.CountryID);
                    cmd.Parameters.AddWithValue("@Gender", student.Gender);
                    cmd.Parameters.AddWithValue("@Religion", student.Religion);
                    cmd.Parameters.AddWithValue("@Session", student.SessionID);
                    cmd.Parameters.AddWithValue("@Term", student.Term);
                    cmd.Parameters.AddWithValue("@ClassLevelID", student.ClassLevelID);
                    cmd.Parameters.AddWithValue("@Alphabet", student.ClassAphabet);
                    cmd.Parameters.AddWithValue("@AdmissionDate", student.AdmissionDate);
                    cmd.Parameters.AddWithValue("@Fathername", student.Fathername);
                    cmd.Parameters.AddWithValue("@Mothername", student.Mothername);
                    cmd.Parameters.AddWithValue("@FatherContact", student.FatherPhone);
                    cmd.Parameters.AddWithValue("@MotherContact", student.MotherPhone);
                    cmd.Parameters.AddWithValue("@Datetime", DateTime.Now);

                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("Student");
        }

        private static List<Session> PopulateSession()
        {
            List<Session> session = new List<Session>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {

                using (SqlCommand cmd = new SqlCommand("select * from  SessionTble", con))
                {
                    cmd.Connection = con;

                    con.Open();

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            session.Add(
                                new Session
                                {
                                    SessionID = Convert.ToInt32(sdr["ID"]),
                                    SessionName = sdr["Session"].ToString()
                                });
                        }
                        con.Close();
                    }
                }
                return session;
            }
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
                                });
                        }
                        con.Close();
                    }
                }
                return level;
            }
        }

        public JsonResult GetStudentDetails(int id)
        {
            var student = new Student();
            var dtTable = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "Select S.AdmissionNo, s.FullName, s.DOB, s.Nationality, ct.CountryName, s.Gender, s.Religion, s.Session, se.Session,s.Term," +
                            "cl.ClassLevel, s.ClassAlphabet, s.AdmissionDate, s.Fathername, s.Mothername, s.FatherContact, s.MotherContact, s.DateTime, " +
                             "cl.ID, cl.ClassLevel, se.ID, se.Session, se.Term, se.IsActive, se.DateTime from Student s inner join ClassLevel cl on s.ClassLevel = cl.ID inner join Country ct on s.Nationality = ct.CountryID  inner join SessionTble se on s.Session = se.ID where s.AdmissionNo = @Id";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@Id", id);
                sqlDA.Fill(dtTable);
            }

            if (dtTable.Rows.Count == 1)
            {
                DataRow row = dtTable.Rows[0];
                student.AdmissionNo = Convert.ToInt32(row[0]);
                student.StudentName = row[1].ToString();
                student.DOB = row[2].ToString();
                student.CountryID = Convert.ToInt32(row[3]);
                student.CountryName = row[4].ToString();
                student.Gender = row[5].ToString();
                student.Religion = row[6].ToString();
                student.SessionID = Convert.ToInt32(row[7]);
                student.SessionName = row[8].ToString();
                student.Term = row[9].ToString();
                student.ClassLevel = row[10].ToString();
                student.ClassAphabet = row[11].ToString();
                student.AdmissionDate = row[12].ToString();
                student.Fathername = row[13].ToString();
                student.Mothername = row[14].ToString();
                student.FatherPhone = row[15].ToString();
                student.MotherPhone = row[16].ToString();
            }

            return Json(student, JsonRequestBehavior.AllowGet); // JSON output enabled
        }


        public ActionResult StudentDetails(int id)
        {

            Student student = new Student();

            DataTable dtTable = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "Select * from Student s inner join ClassLevel cl on s.ClassLevel = cl.ID inner join SessionTble se on  s.Session = se.ID  where s.AdmissionNo = @Id";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@Id", id);
                sqlDA.Fill(dtTable);

            }
            if (dtTable.Rows.Count == 1)
            {
                student.AdmissionNo = Convert.ToInt32(dtTable.Rows[0][0].ToString());
                student.StudentName = dtTable.Rows[0][1].ToString();
                student.DOB = dtTable.Rows[0][2].ToString();
                student.CountryID = Convert.ToInt32(dtTable.Rows[0][3]);
                student.Gender = dtTable.Rows[0][4].ToString();
                student.Religion = dtTable.Rows[0][5].ToString();
                student.SessionName = dtTable.Rows[0][19].ToString();
                student.Term = dtTable.Rows[0][7].ToString();
                student.ClassLevel = dtTable.Rows[0][16].ToString();
                student.ClassAphabet = dtTable.Rows[0][9].ToString();
                student.AdmissionDate = dtTable.Rows[0][10].ToString();
                student.Fathername = dtTable.Rows[0][11].ToString();
                student.Mothername = dtTable.Rows[0][12].ToString();
                student.FatherPhone = dtTable.Rows[0][13].ToString();
                student.MotherPhone = dtTable.Rows[0][14].ToString();
                return View(student);
            }
            else
                return View("Student");
        }

        


        public ActionResult EditStudent(int id)
        {
            ViewBag.Session = PopulateSession();
            ViewBag.ClassLevel = PopulateClassLevel();
            Student student = new Student();
            ViewBag.CountryName = PopulateNationality();
            DataTable dtTable = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                //Update by mohammed
                con.Open();
                string query = "select * from Student s inner join ClassLevel cl on s.ClassLevel = cl.ID inner join SessionTble se on  s.Session = se.ID  where s.AdmissionNo = @Id";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@Id", id);
                sqlDA.Fill(dtTable);

            }
            if (dtTable.Rows.Count == 1)
            {
                student.AdmissionNo = Convert.ToInt32(dtTable.Rows[0][0].ToString());
                student.StudentName = dtTable.Rows[0][1].ToString();
                student.DOB = dtTable.Rows[0][2].ToString();
                student.CountryID = Convert.ToInt32(dtTable.Rows[0][3]);
                student.Gender = dtTable.Rows[0][4].ToString();
                student.Religion = dtTable.Rows[0][5].ToString();
                student.SessionID = Convert.ToInt32(dtTable.Rows[0][6].ToString());
                student.Term = dtTable.Rows[0][7].ToString();
                student.ClassLevelID = Convert.ToInt32(dtTable.Rows[0][8].ToString());
                student.ClassAphabet = dtTable.Rows[0][9].ToString();
                student.AdmissionDate = dtTable.Rows[0][10].ToString();
                student.Fathername = dtTable.Rows[0][11].ToString();
                student.Mothername = dtTable.Rows[0][12].ToString();
                student.FatherPhone = dtTable.Rows[0][13].ToString();
                student.MotherPhone = dtTable.Rows[0][14].ToString();
                return View(student);
            }

            else


                return View("Student");
        }

        [HttpPost]
        public ActionResult EditStudent(Student student)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateStudent", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AdmissionNo", student.AdmissionNo);
                    cmd.Parameters.AddWithValue("@Fullname", student.StudentName);
                    cmd.Parameters.AddWithValue("@DOB", student.DOB);
                    cmd.Parameters.AddWithValue("@Nationality", student.CountryID);
                    cmd.Parameters.AddWithValue("@Gender", student.Gender);
                    cmd.Parameters.AddWithValue("@Religion", student.Religion);
                    cmd.Parameters.AddWithValue("@Session", student.SessionID);
                    cmd.Parameters.AddWithValue("@Term", student.Term);
                    cmd.Parameters.AddWithValue("@ClassLevelID", student.ClassLevelID);
                    cmd.Parameters.AddWithValue("@Alphabet", student.ClassAphabet);
                    cmd.Parameters.AddWithValue("@AdmissionDate", student.AdmissionDate);
                    cmd.Parameters.AddWithValue("@Fathername", student.Fathername);
                    cmd.Parameters.AddWithValue("@Mothername", student.Mothername);
                    cmd.Parameters.AddWithValue("@FatherContact", student.FatherPhone);
                    cmd.Parameters.AddWithValue("@MotherContact", student.MotherPhone);
                    cmd.Parameters.AddWithValue("@Datetime", DateTime.Now);

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }

            return RedirectToAction("Student");
        }


        public ActionResult CheckStudent(string StudentName)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Student WHERE FullName =  @sname", con))
                {
                    cmd.Parameters.AddWithValue("@sname", StudentName);

                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();

                    int count = (int)cmd.ExecuteScalar();
                    con.Close();
                    if (count > 0)
                    {
                        TempData["SuccessMessage"] = "Student already exists, Please Add New Student";
                        return RedirectToAction("CreateStudent");

                    }
                }
                con.Close();
            }
            return RedirectToAction("CreateStudent");
        }

        public ActionResult DeleteStudent(int id)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteStudent", con))
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
            return RedirectToAction("Student");
        }
    }
}
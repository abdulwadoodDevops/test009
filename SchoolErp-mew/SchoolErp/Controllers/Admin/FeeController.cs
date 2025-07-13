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
    public class FeeController : Controller
    {
        // GET: Fee
        public ActionResult CollectedFee(string SortOrder, string SortBy, int PageNumber = 1)
        {
            List<CollectFee> fee = new List<CollectFee>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("getCollectedFee", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        fee.Add(
                            new CollectFee
                            {
                                Id = Convert.ToInt32(row["ID"].ToString()),
                                AdmissionNo = Convert.ToInt32(row["AdmissionNo"].ToString()),
                                StudentName = row["FullName"].ToString(),
                                Session = row["Session"].ToString(),
                                Amount = Convert.ToInt32(row["Amount"].ToString()),
                                Bank = row["Bank"].ToString(),
                                TellerNo = row["TellerNo"].ToString()
                            }
                            );
                    }
                }
                con.Close();
                ViewBag.SortOrder = SortOrder;

                ViewBag.SortBy = SortBy;
            }

            ViewBag.TotalPages = Math.Ceiling(fee.Count() / 5.0);
            ViewBag.PageNumber = PageNumber;
            fee = fee.Skip((PageNumber - 1) * 5).Take(5).ToList();
            return View(fee);
        }

        public ActionResult StudentCollectFee()
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
                                ClassLevel = row["ClassLevel"].ToString(),
                                ClassAphabet = row["ClassAlphabet"].ToString(),
                                Gender = row["Gender"].ToString()

                            }
                            );
                    }
                }
            }
            return View(student);
        }

        public ActionResult CollectFees(int id)
        {

            ViewBag.Session = PopulateSession();

            CollectFee fee = new CollectFee();

            DataTable dtsession = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "Select * from Student where AdmissionNo = @ID";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@ID", id);

                sqlDA.Fill(dtsession);

            }
            if (dtsession.Rows.Count == 1)
            {
                fee.AdmissionNo = Convert.ToInt32(dtsession.Rows[0][0].ToString());
                fee.StudentName = dtsession.Rows[0][1].ToString();
                fee.ClassLevelID = Convert.ToInt32(dtsession.Rows[0][8].ToString());
                return View(fee);
            }
            else
                return View("StudentCollectedFee");
        }

        [HttpPost]
        public ActionResult CollectFees(CollectFee fee)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("InsertFeeDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("AddmissionNo", fee.AdmissionNo);
                    cmd.Parameters.AddWithValue("@ClassLevelID", fee.ClassLevelID);
                    cmd.Parameters.AddWithValue("@Session", fee.SessionID);
                    cmd.Parameters.AddWithValue("@Amount", fee.Amount);
                    cmd.Parameters.AddWithValue("@TellerNo", fee.TellerNo);
                    cmd.Parameters.AddWithValue("@Bank", fee.Bank);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                TempData["SuccessMessage"] = "Record Saved Successfully";
                return RedirectToAction("CollectedFee");
            }
        }

        private static List<Session> PopulateSession()
        {
            List<Session> session = new List<Session>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {

                using (SqlCommand cmd = new SqlCommand("select ID, Session from  SessionTble", con))
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
                                }

                                );
                        }
                        con.Close();
                    }
                }

                return session;

            }
        }

        public ActionResult PrintFeeReciept(int id)
        {

            CollectFee fee = new CollectFee();

            DataTable dtsession = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "select f.ID, s.AdmissionNo, s.FullName,f.TellerNo, f.Amount, cl.ClassLevel, sst.Session  from FeePayment f inner join Student s on f.AdmissionNo = s.AdmissionNo INNER JOIN SessionTble sst ON f.Session = sst.ID inner join ClassLevel cl on f.ClassLevelID = cl.ID where f.ID = @ID";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@ID", id);

                sqlDA.Fill(dtsession);

            }
            if (dtsession.Rows.Count == 1)
            {
                fee.Id = Convert.ToInt32(dtsession.Rows[0][0].ToString());
                fee.AdmissionNo = Convert.ToInt32(dtsession.Rows[0][1].ToString());
                fee.StudentName = dtsession.Rows[0][2].ToString();
                fee.TellerNo = dtsession.Rows[0][3].ToString();
                fee.Amount = Convert.ToDecimal(dtsession.Rows[0][4].ToString());
                fee.ClassLevelname = dtsession.Rows[0][5].ToString();
                fee.Session = dtsession.Rows[0][6].ToString();
                return View(fee);
            }

            else


                return View("CollectedFee");

        }

        public ActionResult DeleteFeePayment(int id)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteFeePayment", con))
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
            return RedirectToAction("CollectedFee");
        }

    }
}
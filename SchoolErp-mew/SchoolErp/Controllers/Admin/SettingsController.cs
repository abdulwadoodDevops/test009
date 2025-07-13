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
    public class SettingsController : Controller
    {
     
         // GET: Sessions
        public new ActionResult Session(int PageNumber = 1)
        {
            List<Session> session = new List<Session>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetSession", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtSessions = new DataTable();

                    dtSessions.Load(sdr);

                    foreach (DataRow row in dtSessions.Rows)
                    {
                        session.Add(
                            new Session
                            {
                                SessionID = Convert.ToInt32(row["ID"].ToString()),
                                SessionName = row["Session"].ToString(),
                                Term = row["Term"].ToString(),
                                IsActive = row["IsActive"].ToString()

                            }

                            );
                    }
                }
                con.Close();
            }
            ViewBag.TotalPages = Math.Ceiling(session.Count() / 10.0);
            ViewBag.PageNumber = PageNumber;
            session = session.Skip((PageNumber - 1) * 10).Take(10).ToList();
            return View(session);
        }

        [HttpPost]
        public new ActionResult Session(string searchtxt)
        {
            List<Session> session = new List<Session>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("SearchSessionorTerm", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Session", searchtxt);
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtSessions = new DataTable();

                    dtSessions.Load(sdr);

                    foreach (DataRow row in dtSessions.Rows)
                    {
                        session.Add(
                            new Session
                            {
                                SessionID = Convert.ToInt32(row["ID"].ToString()),
                                SessionName = row["Session"].ToString(),
                                Term = row["Term"].ToString(),
                                IsActive = row["IsActive"].ToString()

                            }

                            );
                    }
                }
                con.Close();
            }
            ViewBag.TotalPages = Math.Ceiling(session.Count() / 5.0);
            return View(session);
        }

        public ActionResult AddSession()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSession(Session session)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM SessionTble WHERE Session = @sname", con))
                {
                    cmd.Parameters.AddWithValue("@sname", session.SessionName);

                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();

                    int count = (int)cmd.ExecuteScalar();
                    con.Close();
                    if (count > 0)
                    {
                        TempData["SuccessMessage"] = "Session already exists, Please Add New Session";
                        return RedirectToAction("AddSession");

                    }
                }
                con.Close();
            }
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("addsession", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SessionName", session.SessionName);
                    cmd.Parameters.AddWithValue("@Term", session.Term);
                    cmd.Parameters.AddWithValue("@Isactive", session.IsActive);

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }

            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("Session");
        }

        public ActionResult EditSession(int id)
        {
            Session session = new Session();

            DataTable dtsession = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "Select * from SessionTble where ID = @ID";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@ID", id);
                sqlDA.Fill(dtsession);

            }
            if (dtsession.Rows.Count == 1)
            {
                session.SessionID = Convert.ToInt32(dtsession.Rows[0][0].ToString());
                session.SessionName = dtsession.Rows[0][2].ToString();
                session.Term = dtsession.Rows[0][3].ToString();
                session.IsActive = dtsession.Rows[0][4].ToString();
                return View(session);
            }

            else


                return View("Session");


        }

        [HttpPost]
        public ActionResult EditSession(int id, Session session)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM SessionTble WHERE Session = @sname", con))
                {
                    cmd.Parameters.AddWithValue("@sname", session.SessionName);

                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();

                    int count = (int)cmd.ExecuteScalar();
                    con.Close();
                    int s = session.SessionID;
                    string se = "EditSession/";
                    string rs = se + Convert.ToInt32(s);
                    if (count > 0)
                    {
                        TempData["SuccessMessage"] = "Session already exists, Please Add New Session";
                        return RedirectToAction(rs);

                    }
                }
                con.Close();
            }
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("EditSession", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@SessionName", session.SessionName);
                    cmd.Parameters.AddWithValue("@Term", session.Term);
                    cmd.Parameters.AddWithValue("@Isactive", session.IsActive);

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Updated Successfully";
            return RedirectToAction("Session");
        }

        public ActionResult DeleteSession(int id)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("Deletesession", con))
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
            return RedirectToAction("Session");
        }
        


       

        public ActionResult FeeType()
        {
            List<FeeType> fee = new List<FeeType>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetFeeType", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtFeeType = new DataTable();

                    dtFeeType.Load(sdr);

                    foreach (DataRow row in dtFeeType.Rows)
                    {
                        fee.Add(
                            new FeeType
                            {
                                FeeTypeID = Convert.ToInt32(row["FeeID"].ToString()),
                                FeeName = row["Fee"].ToString()

                            }

                            );
                    }
                }
                con.Close();
            }
            return View(fee);
        }

        public ActionResult CreateFeeType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateFeeType(FeeType fee)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("CreateFeeType", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FeeName ", fee.FeeName);


                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("FeeType");


        }

        public ActionResult EditFeeType(int id)
        {

            FeeType fee = new FeeType();

            DataTable dtFeeType = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "Select * from FeeType where FeeID = @ID";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@ID", id);
                sqlDA.Fill(dtFeeType);
                con.Close();
            }

            if (dtFeeType.Rows.Count == 1)
            {
                fee.FeeTypeID = Convert.ToInt32(dtFeeType.Rows[0][0].ToString());
                fee.FeeName = dtFeeType.Rows[0][1].ToString();
                return View(fee);
            }

            else


                return View("FeeType");
        }

        [HttpPost]
        public ActionResult EditFeeType(int id, FeeType fee)
        {

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateFeeType", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FeeID", id);
                    cmd.Parameters.AddWithValue("@FeeName", fee.FeeName);


                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("FeeType");
        }

        public ActionResult DeleteFeeType(int id)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteFeeType", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FeeID", id);

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Deleted Successfully";
            return RedirectToAction("FeeType");
        }

        public ActionResult Sections(string SortOrder, string SortBy, int PageNumber = 1)
        {
            List<Sections> section = new List<Sections>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetSections", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtSections = new DataTable();

                    dtSections.Load(sdr);

                    foreach (DataRow row in dtSections.Rows)
                    {
                        section.Add(
                            new Sections
                            {
                                SectionID = Convert.ToInt32(row["ID"].ToString()),
                                SectionName = row["ClassType"].ToString(),
                            }

                            );
                    }
                }
                con.Close();
                ViewBag.SortOrder = SortOrder;

                ViewBag.SortBy = SortBy;

            }

            ViewBag.TotalPages = Math.Ceiling(section.Count() / 5.0);
            ViewBag.PageNumber = PageNumber;
            section = section.Skip((PageNumber - 1) * 5).Take(5).ToList();
            return View(section);
        }

        public ActionResult CreateSections()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateSections(Sections section)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("InsertSections", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@section", section.SectionName);
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("Sections");
        }

        public ActionResult EditSections(int id)
        {
            Sections section = new Sections();

            DataTable dtsession = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "Select * from Sections where ID = @ID";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@ID", id);
                sqlDA.Fill(dtsession);
                con.Close();
            }

            if (dtsession.Rows.Count == 1)
            {
                section.SectionID = Convert.ToInt32(dtsession.Rows[0][0].ToString());
                section.SectionName = dtsession.Rows[0][1].ToString();

                return View(section);
            }

            else


                return View("Session");


        }


        [HttpPost]
        public ActionResult EditSections(int id, Sections secs)
        {

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateSections", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@ClassType", secs.SectionName);


                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("Sections");
        }



        //Delete the Section
        public ActionResult DeleteSections(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
                {
                    using (SqlCommand cmd = new SqlCommand("DeleteSections", con))
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
            }
            catch(Exception ex)
            {
                TempData["SuccessMessage"] = "Record cannot be Deleted";
            }
            
            return RedirectToAction("Sections");
        }

        //End the Sections


        //Create new Class Level
        public ActionResult ClassLevel(string SortOrder, string SortBy, int PageNumber = 1)
        {
            List<ClassLevel> level = new List<ClassLevel>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetClassLevel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        level.Add(
                            new ClassLevel
                            {
                                LevelID = Convert.ToInt32(row["ID"].ToString()),
                                Levelname = row["ClassLevel"].ToString(),
                                Section = row["ClassType"].ToString()
                            }

                            );
                    }
                }
                con.Close();
                ViewBag.SortOrder = SortOrder;
                ViewBag.SortBy = SortBy;
            }

            ViewBag.TotalPages = Math.Ceiling(level.Count() / 5.0);
            ViewBag.PageNumber = PageNumber;
            level = level.Skip((PageNumber - 1) * 5).Take(5).ToList();
            return View(level);
        }

        public ActionResult CreateClassLevel()
        {

            ViewBag.Section = PopulateSection();

            return View();

        }

        [HttpPost]
        public ActionResult CreateClassLevel(ClassLevel level)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("CreateClassLevel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClassLevel", level.Levelname);
                    cmd.Parameters.AddWithValue("@SectionId", level.SectionID);
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("ClassLevel");
        }


        //Edit the ClassLevel
        public ActionResult EditClassLevel(int id)
        {
            ViewBag.Section = PopulateSection();
            ClassLevel clevel = new ClassLevel();

            DataTable dtclevel = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();

                string query = "SELECT clevel.ID as LevelID, clevel.ClassLevel as Levelname, sctbl.ID as SectionID FROM ClassLevel clevel INNER JOIN Sections sctbl ON clevel.SectionID = sctbl.ID where clevel.ID = @ID";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@ID", id);
                sqlDA.Fill(dtclevel);
                con.Close();
            }

            if (dtclevel.Rows.Count == 1)
            {
                clevel.LevelID = Convert.ToInt32(dtclevel.Rows[0][0].ToString());
                clevel.Levelname = dtclevel.Rows[0][1].ToString();
                clevel.SectionID = Convert.ToInt32(dtclevel.Rows[0][2].ToString());

                return View(clevel);
            }

            else


                return View("ClassLevel");


        }

        [HttpPost]
        public ActionResult EditClassLevel(int id, ClassLevel level)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("EditClassLevel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@ClassLevel", level.Levelname);
                    cmd.Parameters.AddWithValue("@SectionId", level.SectionID);

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Updated Successfully";
            return RedirectToAction("ClassLevel");
        }

        //Delete the Class level
        public ActionResult DeleteClassLevel(int id)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteClassLevel", con))
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
            return RedirectToAction("ClassLevel");
        }

        //End Class Level
        private static List<Sections> PopulateSection()
        {
            List<Sections> section = new List<Sections>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {

                using (SqlCommand cmd = new SqlCommand("Select  * from Sections", con))
                {
                    cmd.Connection = con;

                    con.Open();

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            section.Add(
                                new Sections
                                {
                                    SectionID = Convert.ToInt32(sdr["ID"]),
                                    SectionName = sdr["ClassType"].ToString()
                                }

                                );
                        }
                        con.Close();
                    }


                }

                return section;

            }
        }


        private static List<FeeType> PopulateFeeType()
        {
            List<FeeType> fee = new List<FeeType>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {

                using (SqlCommand cmd = new SqlCommand("Select  * from FeeType", con))
                {
                    cmd.Connection = con;

                    con.Open();

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            fee.Add(
                                new FeeType
                                {
                                    FeeTypeID = Convert.ToInt32(sdr["FeeID"]),
                                    FeeName = sdr["Fee"].ToString()
                                }

                                );
                        }
                        con.Close();
                    }


                }

                return fee;
            }
        }

        public ActionResult FeeStructure()
        {
            List<FeeStructure> fee = new List<FeeStructure>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("getFeeStructure", con))
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
                            new FeeStructure
                            {
                                FeeStructureID = Convert.ToInt32(row["ID"].ToString()),
                                FeeTypename = row["Fee"].ToString(),
                                SectionName = row["ClassType"].ToString(),
                                Amount = Convert.ToDecimal(row["Amount"].ToString())


                            }

                            );
                    }
                }
                con.Close();
            }
            return View(fee);
        }

        public ActionResult CreateFeeStructure()
        {
            ViewBag.Section = PopulateSection();

            ViewBag.FeeType = PopulateFeeType();


            return View();
        }

        [HttpPost]
        public ActionResult CreateFeeStructure(FeeStructure fee)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("insertFeeStructure", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FeeTypeID", fee.FeeTypeID);
                    cmd.Parameters.AddWithValue("@SecionID", fee.SectionID);
                    cmd.Parameters.AddWithValue("@Amount", fee.Amount);
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("FeeStructure");
        }

        public ActionResult EditFeeStructure(int id)
        {
            ViewBag.Section = PopulateSection();
            ViewBag.FeeType = PopulateFeeType();
            FeeStructure feeStructure = new FeeStructure();
            DataTable dtsubject = new DataTable();
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "Select ID as FeeStructureID, FeeTypeID, SectionID, Amount from FeeStructure where ID = @ID";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@ID", id);
                sqlDA.Fill(dtsubject);
                con.Close();
            }

            if (dtsubject.Rows.Count == 1)
            {
                feeStructure.FeeStructureID = Convert.ToInt32(dtsubject.Rows[0][0].ToString());
                feeStructure.FeeTypeID = Convert.ToInt32(dtsubject.Rows[0][1].ToString());
                feeStructure.SectionID = Convert.ToInt32(dtsubject.Rows[0][2].ToString());
                feeStructure.Amount = Convert.ToInt32(dtsubject.Rows[0][3].ToString());

                return View(feeStructure);
            }

            else
            {
                return View(feeStructure);
            }
        }



        [HttpPost]
        public ActionResult EditFeeStructure(int id, FeeStructure feeStructure)
        {

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateFeeStructure", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@FeeTypeID", feeStructure.FeeTypeID);
                    cmd.Parameters.AddWithValue("@SectionID", feeStructure.SectionID);
                    cmd.Parameters.AddWithValue("@Amount", feeStructure.Amount);

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Update record Successfully";
            return RedirectToAction("FeeStructure");
        }

        //Delete the Subject
        public ActionResult DeleteFeeStructure(int id)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteFeeStructure", con))
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
            return RedirectToAction("FeeStructure");
        }


        //End the Fees Structure

        //Create the Subject

        public ActionResult Subject(string SortOrder, string SortBy, int PageNumber = 1)
        {
            List<Subject> subject = new List<Subject>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetSubject", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        subject.Add(
                            new Subject
                            {
                                SubjectID = Convert.ToInt32(row["ID"].ToString()),
                                Subjectname = row["Subject"].ToString(),
                                SectionName = row["ClassType"].ToString()


                            }

                            );
                    }
                }
                con.Close();
                ViewBag.SortOrder = SortOrder;

                ViewBag.SortBy = SortBy;

            }
            ViewBag.TotalPages = Math.Ceiling(subject.Count() / 5.0);
            ViewBag.PageNumber = PageNumber;
            subject = subject.Skip((PageNumber - 1) * 5).Take(5).ToList();
            return View(subject);
        }

        public ActionResult CreateSubject()
        {
            ViewBag.Section = PopulateSection();

            return View();
        }

        [HttpPost]
        public ActionResult CreateSubject(Subject subject)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("insertSubject", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Subject", subject.Subjectname);
                    cmd.Parameters.AddWithValue("@SectionID", subject.SectionID);
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("Subject");
        }


        //Update the Subject


        public ActionResult EditSubject(int id)
        {
            ViewBag.Section = PopulateSection();
            Subject subject = new Subject();
            DataTable dtsubject = new DataTable();
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "Select * from Subject where ID = @ID";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@ID", id);
                sqlDA.Fill(dtsubject);
                con.Close();
            }

            if (dtsubject.Rows.Count == 1)
            {
                subject.SubjectID = Convert.ToInt32(dtsubject.Rows[0][0].ToString());
                subject.Subjectname = dtsubject.Rows[0][1].ToString();
                subject.SectionID = Convert.ToInt32(dtsubject.Rows[0][2].ToString());

                return View(subject);
            }

            else
            {
                return View(subject);
            }


        }




        [HttpPost]
        public ActionResult EditSubject(int id, Subject sbjs)
        {
            ViewBag.Section = PopulateSection();
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("EditSubject", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Subject", sbjs.Subjectname);
                    cmd.Parameters.AddWithValue("@SectionID", sbjs.SectionID);


                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("Subject");
        }



        //Delete the Subject
        public ActionResult DeleteSubject(int id)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteSubject", con))
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
            return RedirectToAction("Subject");
        }

        //End the Subject

        //Start the Designation

        public ActionResult Designation()
        {
            List<Designation> designation = new List<Designation>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetDesignation", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        designation.Add(
                            new Designation
                            {
                                DesignationID = Convert.ToInt32(row["DesignationID"].ToString()),
                                DesignationName = row["Designation"].ToString(),


                            }

                            );
                    }
                }
                con.Close();
            }
            return View(designation);

        }

        public ActionResult CreateDesignation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateDesignation(Designation designation)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Designation WHERE Designation = @dname", con))
                {
                    cmd.Parameters.AddWithValue("@dname", designation.DesignationName);

                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();

                    int count = (int)cmd.ExecuteScalar();
                    con.Close();
                    if (count > 0)
                    {
                        TempData["SuccessMessage"] = "Designation already exists, Please Select Another";
                        return RedirectToAction("CreateDesignation");

                    }
                }
                con.Close();
            }
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("CreateDesignation", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Designation", designation.DesignationName);
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("Designation");
        }

        //Update the Designation


        public ActionResult EditDesignation(int id)
        {
            Designation designation = new Designation();

            DataTable dtdesignation = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "Select * from Designation where DesignationID = @ID";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@ID", id);
                sqlDA.Fill(dtdesignation);

            }
            if (dtdesignation.Rows.Count == 1)
            {
                designation.DesignationID = Convert.ToInt32(dtdesignation.Rows[0][0].ToString());
                designation.DesignationName = dtdesignation.Rows[0][1].ToString();

                return View(designation);
            }

            else


                return View("Designation");


        }


        [HttpPost]
        public ActionResult EditDesignation(int id, Designation dsg)
        {

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateDesignation", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@DesignationName", dsg.DesignationName);


                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("Designation");
        }



        //Delete the Subject
        public ActionResult DeleteDesignation(int id)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteDesignation", con))
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
            return RedirectToAction("Designation");
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

                                    SectionID = Convert.ToInt32(sdr["ID"].ToString()),
                                    Subjectname = sdr["Subject"].ToString()
                                }

                                );
                        }
                        con.Close();
                    }


                }
                con.Close();
                return subject;

            }
        }

        public ActionResult TimeTableClassLevel(string SortOrder, string SortBy, int PageNumber = 1)
        {
            List<ClassLevel> level = new List<ClassLevel>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetClassLevel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        level.Add(
                            new ClassLevel
                            {
                                LevelID = Convert.ToInt32(row["ID"].ToString()),
                                Levelname = row["ClassLevel"].ToString(),
                                Section = row["ClassType"].ToString()


                            }

                            );
                    }
                }
                con.Close();
                ViewBag.SortOrder = SortOrder;
                ViewBag.SortBy = SortBy;
            }

            ViewBag.TotalPages = Math.Ceiling(level.Count() / 5.0);
            ViewBag.PageNumber = PageNumber;
            level = level.Skip((PageNumber - 1) * 5).Take(5).ToList();
            return View(level);
        }

        public ActionResult CreateTimeTable(int id)
        {
            ViewBag.Subject = PopulateSubject();


            ClassTimeTable time = new ClassTimeTable();

            DataTable dtsession = new DataTable();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                con.Open();
                string query = "Select * from Classlevel where ID = @ID";
                SqlDataAdapter sqlDA = new SqlDataAdapter(query, con);
                sqlDA.SelectCommand.Parameters.AddWithValue("@ID", id);

                sqlDA.Fill(dtsession);
                con.Close();
            }

            if (dtsession.Rows.Count == 1)
            {
                time.ClassLevelID = Convert.ToInt32(dtsession.Rows[0][0].ToString());
                time.ClassLevelName = dtsession.Rows[0][1].ToString();
                return View(time);
            }

            else


                return View("TimeTableClassLevel");
        }

        [HttpPost]
        public ActionResult CreateTimeTable(ClassTimeTable time)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("CreateClassTimeTable", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LevelID", time.ClassLevelID);
                    cmd.Parameters.AddWithValue("@Days", time.Days);
                    if (time.Period1 != null)
                    {
                        cmd.Parameters.AddWithValue("@Period1", time.Period1);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Period1", "0");
                    }

                    if (time.Period2 != null)
                    {
                        cmd.Parameters.AddWithValue("@Period2", time.Period2);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Period2", "0");
                    }
                    if (time.Period3 != null)
                    {
                        cmd.Parameters.AddWithValue("@Period3", time.Period3);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Period3", "0");
                    }
                    if (time.Period4 != null)
                    {
                        cmd.Parameters.AddWithValue("@Period4", time.Period4);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Period4", "0");
                    }
                    if (time.Period5 != null)
                    {
                        cmd.Parameters.AddWithValue("@Period5", time.Period5);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Period5", "0");
                    }
                    if (time.Period6 != null)
                    {
                        cmd.Parameters.AddWithValue("@Period6", time.Period6);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Period6", "0");
                    }

                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
            TempData["SuccessMessage"] = "Record Saved Successfully";
            return RedirectToAction("TimeTableClassLevel");

        }

        public ActionResult ViewTimeTable(int id)
        {
            List<ClassTimeTable> time = new List<ClassTimeTable>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("GetClassTimeTable", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    if (con.State != System.Data.ConnectionState.Open)

                        con.Open();

                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtTable = new DataTable();

                    dtTable.Load(sdr);

                    foreach (DataRow row in dtTable.Rows)
                    {
                        time.Add(
                            new ClassTimeTable
                            {
                                ClassLevelName = row["ClassLevelName"].ToString(),
                                Days = row["Days"].ToString(),
                                Period1 = row["Period1"].ToString(),
                                Period2 = row["Period2"].ToString(),
                                Period3 = row["Period3"].ToString(),
                                Period4 = row["Period4"].ToString(),
                                Period5 = row["Period5"].ToString(),
                                Period6 = row["Period6"].ToString()
                            }

                            );
                    }
                }
                con.Close();
            }
            return View(time);
        }

    }
}
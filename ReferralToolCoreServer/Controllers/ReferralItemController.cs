using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Microsoft.AspNetCore.Mvc;
using ReferralToolCoreServer.Models;

namespace ReferralToolCoreServer.Controllers
{
    public class HistoryData
    {
        public string EditTime { get; set; }
        public string Name { get; set; }
        public string CAD { get; set; }
        public string Status { get; set; }
        public string DOD { get; set; }
        public string ReqTime { get; set; }
        public string Nature { get; set; }
        public string Provider { get; set; }
    }

    [Route("[controller]")]
    [ApiController]
    public class ReferralItemController : ControllerBase
    {
        private void UpdateMagick()
        {
            var connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                DataSource = "ReferralTool.db"
            };

            using var DBConnection = new SQLiteConnection(connectionStringBuilder.ConnectionString);
            DBConnection.Open();

            string DBCommand = "UPDATE Magick SET Number = @number WHERE ID = 1";
            using (var sqlCommand = new SQLiteCommand(DBCommand, DBConnection))
            {
                Random rnd = new Random();
                sqlCommand.Parameters.AddWithValue("@number", rnd.Next(1, 5000));

                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                catch
                {
                    System.Diagnostics.Trace.WriteLine("[STATIC UTILITY] UpdateMagick => DBUpdateMagick() => sqlCommand Fail");
                };
            }

            DBConnection.Close();
            System.Diagnostics.Trace.WriteLine("[STATIC UTILITY] UpdateMagick => DBUpdateMagick() => End");
        }

        private void AddHistoryItem(long? id, ReferralItem referralItem)
        {
            var connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                DataSource = "ReferralTool.db"
            };

            using var DBConnection = new SQLiteConnection(connectionStringBuilder.ConnectionString);
            DBConnection.Open();

            string DBCommand = "INSERT into History " +
                               "VALUES(@id, @edit_sig, @patient_name, @cad, @callstatus, @dod, @requestedtime, @nature, @provider)";
            using (var sqlCommand = new SQLiteCommand(DBCommand, DBConnection))
            {
                sqlCommand.Parameters.AddWithValue("@id", id);
                sqlCommand.Parameters.AddWithValue("@edit_sig", DateTime.Now.ToString("g") + $" by [{referralItem.CallTaker}]");
                //
                sqlCommand.Parameters.AddWithValue("@patient_name", referralItem.PatientName);
                sqlCommand.Parameters.AddWithValue("@cad", referralItem.CAD);
                sqlCommand.Parameters.AddWithValue("@callstatus", referralItem.CallStatus);
                sqlCommand.Parameters.AddWithValue("@dod", referralItem.DateOfDischarge);
                sqlCommand.Parameters.AddWithValue("@requestedtime", referralItem.RequestedTime);
                sqlCommand.Parameters.AddWithValue("@nature", referralItem.Nature);
                sqlCommand.Parameters.AddWithValue("@provider", referralItem.Provider);

                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("[STATIC UTILITY] ReferralItemController => AddHistoryItem() => sqlCommand Fail: " + ex.Message);
                };
            }

            DBConnection.Close();
            System.Diagnostics.Trace.WriteLine("[STATIC UTILITY] ReferralItemController => AddHistoryItem() => End");
        }

        // Add New Referral
        [HttpPost]
        public void Post([FromForm] ReferralItem referralItem)
        {
            long? id = null;

            var connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                DataSource = "ReferralTool.db"
            };

            using var DBConnection = new SQLiteConnection(connectionStringBuilder.ConnectionString);
            DBConnection.Open();

            string DBCommand = "INSERT INTO Referrals(PatientName, CAD, CallStatus, DateOfDischarge, " +
                               "RequestedTime, CallTaker, Nature, Provider, CreatedDate, CreatedTime) " +
                               "VALUES (@PatientName, @CAD, @CallStatus, @DateOfDischarge, " + 
                               "@RequestedTime, @CallTaker, @Nature, @Provider, @CreatedDate, @CreatedTime); " +
                               "SELECT last_insert_rowid();";

            using (var sqlCommand = new SQLiteCommand(DBCommand, DBConnection))
            {
                sqlCommand.Parameters.AddWithValue("@PatientName", referralItem.PatientName);
                sqlCommand.Parameters.AddWithValue("@CAD", referralItem.CAD);
                sqlCommand.Parameters.AddWithValue("@CallStatus", referralItem.CallStatus);
                sqlCommand.Parameters.AddWithValue("@DateOfDischarge", referralItem.DateOfDischarge);
                sqlCommand.Parameters.AddWithValue("@RequestedTime", referralItem.RequestedTime);
                sqlCommand.Parameters.AddWithValue("@CallTaker", referralItem.CallTaker);
                sqlCommand.Parameters.AddWithValue("@Nature", referralItem.Nature);
                sqlCommand.Parameters.AddWithValue("@Provider", referralItem.Provider);
                sqlCommand.Parameters.AddWithValue("@CreatedDate", referralItem.CreatedDate);
                sqlCommand.Parameters.AddWithValue("@CreatedTime", referralItem.CreatedTime);

                try
                {
                    id = (long)sqlCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("ReferralController => Post() => Exception: " + ex.Message);
                };
            }

            DBConnection.Close();
            UpdateMagick();
            System.Diagnostics.Trace.WriteLine("ReferralController => Post() => End");
            AddHistoryItem(id, referralItem);
        }

        // Edit/Update Referral
        [HttpPut]
        public void Put([FromForm] ReferralItem referralItem)
        {
            var connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                DataSource = "ReferralTool.db"
            };

            using var DBConnection = new SQLiteConnection(connectionStringBuilder.ConnectionString);
            DBConnection.Open();

            string DBCommand = "UPDATE Referrals SET PatientName = @PatientName, CAD = @CAD, " +
                               "CallStatus = @CallStatus, DateOfDischarge = @DateOfDischarge, " +
                               "RequestedTime = @RequestedTime, Nature = @Nature, Provider = @Provider " +
                               "WHERE ID = @ID";

            using (var sqlCommand = new SQLiteCommand(DBCommand, DBConnection))
            {
                sqlCommand.Parameters.AddWithValue("@PatientName", referralItem.PatientName);
                sqlCommand.Parameters.AddWithValue("@CAD", referralItem.CAD);
                sqlCommand.Parameters.AddWithValue("@CallStatus", referralItem.CallStatus);
                sqlCommand.Parameters.AddWithValue("@DateOfDischarge", referralItem.DateOfDischarge);
                sqlCommand.Parameters.AddWithValue("@RequestedTime", referralItem.RequestedTime);
                sqlCommand.Parameters.AddWithValue("@Nature", referralItem.Nature);
                sqlCommand.Parameters.AddWithValue("@Provider", referralItem.Provider);
                sqlCommand.Parameters.AddWithValue("@ID", referralItem.ID);

                try
                {
                    sqlCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("ReferralController => Put() => Exception: " + ex.Message);
                };
            }

            DBConnection.Close();
            UpdateMagick();
            System.Diagnostics.Trace.WriteLine("ReferralController => Put() => End");
            AddHistoryItem(Convert.ToUInt32(referralItem.ID), referralItem);
        }

        // Get Individual Referral Update History
        [HttpGet]
        public List<HistoryData> Get([FromQuery] string id)
        {
            List<HistoryData> historyList = new List<HistoryData>();
            var connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                DataSource = "ReferralTool.db"
            };

            using var DBConnection = new SQLiteConnection(connectionStringBuilder.ConnectionString);
            DBConnection.Open();

            string DBCommand = "SELECT * FROM History WHERE ID = @reqId";
            using (var sqlCommand = new SQLiteCommand(DBCommand, DBConnection))
            {
                sqlCommand.Parameters.AddWithValue("@reqId", id);
                try
                {
                    using SQLiteDataReader rdr = sqlCommand.ExecuteReader();
                    while (rdr.Read())
                    {
                        HistoryData historyItem = new HistoryData
                        {
                            EditTime = rdr.GetString(1),
                            Name = rdr.GetString(2),
                            CAD = rdr.GetString(3),
                            Status = rdr.GetString(4),
                            DOD = rdr.GetString(5),
                            ReqTime = rdr.GetString(6),
                            Nature = rdr.GetString(7),
                            Provider = rdr.GetString(8)
                        };
                        historyList.Add(historyItem);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("ReferralController => Get() => Exception: " + ex.Message);
                };
            }

            DBConnection.Close();
            System.Diagnostics.Trace.WriteLine("ReferralController => Get() => End");
            return historyList;
        }
    }
}
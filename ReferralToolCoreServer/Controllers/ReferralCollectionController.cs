using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Microsoft.AspNetCore.Mvc;
using ReferralToolCoreServer.Models;

namespace ReferralToolCoreServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReferralCollectionController : ControllerBase
    {
        private readonly List<ReferralItem> referralDatabase = new List<ReferralItem>();

        // Get 'Completed','Managed','Cancelled' Referrals
        [HttpGet("{current_date}")]
        public List<ReferralItem> Get(string current_date)
        {
            var connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                DataSource = "ReferralTool.db"
            };

            using var DBConnection = new SQLiteConnection(connectionStringBuilder.ConnectionString);
            DBConnection.Open();

            string DBCommand = "SELECT * FROM Referrals WHERE CreatedDate = @createdDate AND CallStatus in ('Completed','Managed','Cancelled')";
            using (var sqlCommand = new SQLiteCommand(DBCommand, DBConnection))
            {
                sqlCommand.Parameters.AddWithValue("@createdDate", current_date);

                try
                {
                    using SQLiteDataReader rdr = sqlCommand.ExecuteReader();
                    while (rdr.Read())
                    {
                        ReferralItem referralItem = new ReferralItem
                        {
                            ID = rdr.GetInt32(0).ToString(),
                            PatientName = rdr.GetString(1),
                            CAD = rdr.GetString(2),
                            CallStatus = rdr.GetString(3),
                            DateOfDischarge = rdr.GetString(4),
                            RequestedTime = rdr.GetString(5),
                            CallTaker = rdr.GetString(6),
                            Nature = rdr.GetString(7),
                            Provider = rdr.GetString(8),
                            CreatedDate = rdr.GetString(9),
                            CreatedTime = rdr.GetString(10)
                        };
                        referralDatabase.Add(referralItem);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("HistoryController => Get() => Exception: " + ex.Message);
                };
            }

            DBConnection.Close();

            System.Diagnostics.Trace.WriteLine("HistoryController => Get() => End");
            return referralDatabase;
        }

        // Get 'Active', 'Calling' Referrals
        [HttpGet]
        public List<ReferralItem> Get()
        {
            var connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                DataSource = "ReferralTool.db"
            };

            using var DBConnection = new SQLiteConnection(connectionStringBuilder.ConnectionString);
            DBConnection.Open();

            string DBCommand = "SELECT * FROM Referrals WHERE CallStatus in ('Active', 'Calling')";
            using (var sqlCommand = new SQLiteCommand(DBCommand, DBConnection))
            {
                try
                {
                    using SQLiteDataReader rdr = sqlCommand.ExecuteReader();
                    while (rdr.Read())
                    {
                        ReferralItem referralItem = new ReferralItem
                        {
                            ID = rdr.GetInt32(0).ToString(),
                            PatientName = rdr.GetString(1),
                            CAD = rdr.GetString(2),
                            CallStatus = rdr.GetString(3),
                            DateOfDischarge = rdr.GetString(4),
                            RequestedTime = rdr.GetString(5),
                            CallTaker = rdr.GetString(6),
                            Nature = rdr.GetString(7),
                            Provider = rdr.GetString(8),
                            CreatedDate = rdr.GetString(9),
                            CreatedTime = rdr.GetString(10)
                        };
                        referralDatabase.Add(referralItem);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("DatabaseController => Get() => Exception: " + ex.Message);
                };
            }

            DBConnection.Close();

            System.Diagnostics.Trace.WriteLine("DatabaseController => Get() => End");
            return referralDatabase;
        }
    }
}
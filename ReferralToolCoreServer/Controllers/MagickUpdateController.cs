using System;
using System.Data.SQLite;
using Microsoft.AspNetCore.Mvc;

namespace ReferralToolCoreServer.Controllers
{
    public class MagickUpdate
    {
        public int Magick { get; set; }
    }

    [Route("[controller]")]
    [ApiController]
    public class MagickUpdateController : ControllerBase
    {
        readonly MagickUpdate magick = new MagickUpdate();

        // Get Magick
        [HttpGet]
        public MagickUpdate Get()
        {
            var connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                DataSource = "ReferralTool.db"
            };

            using var DBConnection = new SQLiteConnection(connectionStringBuilder.ConnectionString);
            DBConnection.Open();

            string DBCommand = "SELECT Number FROM Magick WHERE ID = @id";
            using (var sqlCommand = new SQLiteCommand(DBCommand, DBConnection))
            {
                sqlCommand.Parameters.AddWithValue("@id", 1);
                try
                {
                    object dbVal = sqlCommand.ExecuteScalar();
                    magick.Magick = Convert.ToInt32(dbVal);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("MagickUpdateController => Get() => Exception: " + ex.Message);
                };
            }

            DBConnection.Close();

            System.Diagnostics.Trace.WriteLine("MagickUpdateController => Get() => End");
            return magick;
        }
    }
}

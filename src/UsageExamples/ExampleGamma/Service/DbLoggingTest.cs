using System;
using System.Data.SqlClient;
using WcfSoapLogger.FileWriting;

namespace Service
{
    // this object is created per each request-response pair

    public class DbLoggingTest
    {
      private const string ConnectionString = "Server=localhost; Database=LoggingTest; Trusted_Connection=True;";

      private int? insertedID;
      private string origin;
      private string locationPath;

      public DbLoggingTest(string origin, string locationPath)
      {
        this.origin = origin;
        this.locationPath = locationPath;
      }


      public void LogToDatabase(SoapMessage message, bool request)
      {
        string xml = message.GetIndentedXml();
        string operation = message.GetOperationName();

        if (request)
        {
          string location = message.GetNodeValue(locationPath);

          if (location == null)
          {
            return;
          }

          insertedID = LogRequest(this.origin, operation, xml, location);
        }
        else
        {

          if (insertedID == null)
          {
            return;
          }

          LogResponse(xml, insertedID.Value);
        }
      }



      private static int LogRequest(string origin, string operation, string request, string location)
      {
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
          connection.Open();

          using (var command = connection.CreateCommand())
          {
            command.CommandText = "insert into [Requests] (Origin, Operation, Location, Request) values (@origin, @operation, @location, @request); select @@IDENTITY;";
            command.Parameters.AddWithValue("origin", origin);
            command.Parameters.AddWithValue("operation", operation);
            command.Parameters.AddWithValue("location", location);
            command.Parameters.AddWithValue("request", request);
            var result = command.ExecuteScalar();
            return Convert.ToInt32(result);
          }
        }
      }


      private static void LogResponse(string response, int id)
      {
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
          connection.Open();

          using (var command = connection.CreateCommand())
          {
            command.CommandText = "update [Requests] set Response = @response where ID = @id";
            command.Parameters.AddWithValue("response", response);
            command.Parameters.AddWithValue("id", id);
            command.ExecuteNonQuery();
          }
        }
      }

      
      
/*
       
CREATE TABLE [dbo].[Requests](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Origin] [nvarchar](200) NULL,
	[Operation] [nvarchar](200) NULL,
	[Location] [nvarchar](200) NULL,
	[Request] [nvarchar](2000) NULL,
	[Response] [nvarchar](2000) NULL,
 CONSTRAINT [PK_Requests] PRIMARY KEY CLUSTERED ([ID] ASC)
) ON [PRIMARY]
GO
       
*/

    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml.Linq;
using WiseX.Data;
using WiseX.ViewModels.Home;

namespace WiseX.Services
{
    public class HomeService : DbContext
    {
        private readonly ApplicationDbContext _applicationDbContext;
        string connectionString = string.Empty;

        public HomeService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            //connectionString = _applicationDbContext.Database.GetDbConnection().ConnectionString;
            connectionString = DBConnection.ConnectionString;
        }


        public async Task<ChartProperties> GetBoxes()
        {
            ChartProperties lst = new ChartProperties();
            try
            { 
                _applicationDbContext.Database.SetCommandTimeout(300);
                lst = await _applicationDbContext.ChartProperties.FromSql("EXEC [Dashboard].[GetBoxes_Latest]").FirstOrDefaultAsync();                                                                                
               // lst = await _applicationDbContext.ChartProperties.FromSql("EXEC [Dashboard].[GetBoxes]").FirstOrDefaultAsync();
               // lst = await _applicationDbContext.ChartProperties.FromSql("EXEC [Dashboard].[GetBoxes_New]").FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return lst;
        }

        public async Task<List<ChartBoxProperties>> GetBoxesList(string BoxName, string UserID)
        {
            List<ChartBoxProperties> lst = new List<ChartBoxProperties>();
            var DashBoardName = new SqlParameter("@BoxName", BoxName);
           // var ParamUserID = new SqlParameter("@UserID", UserID);
            try
            {
                // lst = await _applicationDbContext.ChartBoxProperties.FromSql("EXEC [Dashboard].[GetBoxsDetails] @BoxName,@UserID", DashBoardName, ParamUserID).ToListAsync();
                lst = await _applicationDbContext.ChartBoxProperties.FromSql("EXEC [Dashboard].[GetBoxsDetails] @BoxName", DashBoardName).ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return lst;
        }

        public async Task<List<ChartBoxPropertiesLoad>> GetBoxesList1(int Start, int Length,string BoxName,string UserID)
        {
            List<ChartBoxPropertiesLoad> lst = new List<ChartBoxPropertiesLoad>();
            var DashBoardName = new SqlParameter("@BoxName", BoxName);
            var ParamStart = new SqlParameter("@Start", Start);
            var ParamLength = new SqlParameter("@Length", Length);
           // var ParamUserID = new SqlParameter("@UserID", UserID);
            try
            {
                // lst = await _applicationDbContext.ChartBoxPropertiesLoad.FromSql("EXEC [Dashboard].[GetBoxsDetailsLoad] @Start,@Length,@BoxName,@UserID", ParamStart, ParamLength, DashBoardName, ParamUserID).ToListAsync();
               lst = await _applicationDbContext.ChartBoxPropertiesLoad.FromSql("EXEC [Dashboard].[GetBoxsDetailsLoad] @Start,@Length,@BoxName", ParamStart, ParamLength, DashBoardName).ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return lst;
        }

        public async Task<List<ChartBoxFacilityPropertiesLoad>> GetBoxesFacilityList(int Start, int Length, string BoxName, string UserID)
        {
            List<ChartBoxFacilityPropertiesLoad> lst = new List<ChartBoxFacilityPropertiesLoad>();
            var DashBoardName = new SqlParameter("@BoxName", BoxName);
            var ParamStart = new SqlParameter("@Start", Start);
            var ParamLength = new SqlParameter("@Length", Length);
            try
            {
                lst = await _applicationDbContext.ChartBoxFacilityPropertiesLoad.FromSql("EXEC [Dashboard].[GetBoxsDetailsLoad] @Start,@Length,@BoxName", ParamStart, ParamLength, DashBoardName).ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return lst;
        }
        public async Task<List<NonEmergencyFacilityBalanceLoad>> GetBoxesNEFacilityList(int Start, int Length, string BoxName, string UserID)
        {
            List<NonEmergencyFacilityBalanceLoad> lst = new List<NonEmergencyFacilityBalanceLoad>();
            var DashBoardName = new SqlParameter("@BoxName", BoxName);
            var ParamStart = new SqlParameter("@Start", Start);
            var ParamLength = new SqlParameter("@Length", Length);
            try
            { 
                lst = await _applicationDbContext.NonEmergencyFacilityBalanceLoad.FromSql( "EXEC [Dashboard].[GetBoxsDetailsLoad] @Start,@Length,@BoxName", ParamStart, ParamLength, DashBoardName).ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return lst;
        }
        public async Task<List<Dictionary<string, object>>> GetBoxesForB5Data(int Start, int Length, string BoxName, string UserID)
        {
            var result = new List<Dictionary<string, object>>();

            using (var conn = _applicationDbContext.Database.GetDbConnection())
            {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[SP_getClientFacilityAgingwithNotesDataList]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@Start", Start));
                    //cmd.Parameters.Add(new SqlParameter("@Length", Length));
                    //cmd.Parameters.Add(new SqlParameter("@BoxName", BoxName));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader[i];
                            }

                            result.Add(row);
                        }
                    }
                }
            }

            return result;
        }
        public async Task<List<Dictionary<string, object>>> GetDashboardListCommonDetails(int Start, int Length, string BoxName, string UserID)
        {
            var result = new List<Dictionary<string, object>>();

            using (var conn = _applicationDbContext.Database.GetDbConnection())
            {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[Dashboard].[GetBoxsDetailsLoad]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Start", Start));
                    cmd.Parameters.Add(new SqlParameter("@Length", Length));
                    cmd.Parameters.Add(new SqlParameter("@BoxName", BoxName));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader[i];
                            }

                            result.Add(row);
                        }
                    }
                }
            }

            return result;
        }

        public async Task<List<NotesManagementFormDetails>> GetNotesManagementFormDetails(string facilityName, int Start, int Length, string Type )
        {
            List<NotesManagementFormDetails> lst = new List<NotesManagementFormDetails>();
            var ParamFormFacilityName = new SqlParameter("@facilityName", facilityName);
            var ParamStart = new SqlParameter("@Start", Start);
            var ParamLength = new SqlParameter("@Length", Length);
            var ParamType = new SqlParameter("@BoxName", Type);                       

            try
            {
                lst = await _applicationDbContext.NotesManagementFormDetails.FromSql("EXEC [Dashboard].[GetBoxsDetailsLoad] @Start,@Length,@BoxName,@facilityName", ParamStart, ParamLength, ParamType, ParamFormFacilityName).ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return lst;
        }
        public async Task<List<Dictionary<string, object>>> GetDashboardNoteManagementDetails(string facilityName)
        {
            var result = new List<Dictionary<string, object>>();

            using (var conn = _applicationDbContext.Database.GetDbConnection())
            {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[Dashboard].[NotesManagementFacilityDetailsList]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@facilityName", facilityName));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader[i];
                            }

                            result.Add(row);
                        }
                    }
                }
            }

            return result;
        }
    }
}
 
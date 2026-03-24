using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using WiseX.Data;
using WiseX.ViewModels.Home;
using System.Data;

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
                lst = await _applicationDbContext.ChartProperties.FromSql("EXEC [Dashboard].[GetBoxes]").FirstOrDefaultAsync();
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
    }
}

using Erp.Domain.Staff.Entities;
using Erp.Domain.Staff.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Erp.BackOffice.Controllers
{
    public class CheckInOutServiceController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetFPMachineList()
        {
            FPMachineRepository fPMachineRepository = new FPMachineRepository(new Domain.Staff.ErpStaffDbContext());
            var q = fPMachineRepository.GetAllFPMachine();
            var res = Request.CreateResponse(HttpStatusCode.OK, q.ToList());
            return res;
        }

        [HttpPost]
        public void Insert([FromBody]CheckInOut checkInOut)
        {
            CheckInOutRepository checkInOutRepository = new CheckInOutRepository(new Domain.Staff.ErpStaffDbContext());
            checkInOutRepository.InsertCheckInOut(checkInOut);
        }

        [HttpPost]
        public void InsertList([FromBody]List<CheckInOut> list)
        {
            CheckInOutRepository checkInOutRepository = new CheckInOutRepository(new Domain.Staff.ErpStaffDbContext());
            foreach (var item in list)
            {
                checkInOutRepository.InsertCheckInOut(item);
            }

            //Xóa trùng lặp
            string sqlQuery = " WITH CTE AS " +
                                 " ( SELECT 	TimeStr, UserEnrollNumber, ROW_NUMBER() OVER ( PARTITION BY TimeStr,UserEnrollNumber ORDER BY  TimeStr DESC,UserEnrollNumber) AS RowID " +
                                 " FROM [dbo].[CheckInOut] ) " +
                                 " DELETE FROM CTE " +
                                 " WHERE RowID > 1; ";

            Domain.Helper.SqlHelper.ExecuteSQL(sqlQuery);
        }
    }
}
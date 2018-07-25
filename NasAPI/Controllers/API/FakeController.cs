using NasAPI.Filters;
using NasAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace NasAPI.Controllers.API
{

   public class student
    {

        public string Id { get; set; }
        public string Name { get; set; }
    }

   [NasAuthorizationFilter]
   [EnableCors(origins: "*", headers: "*", methods: "*")]
   [ApiExplorerSettings(IgnoreApi = true)]

   public class FakeController : ApiController
    {
        public List<District> Get()
        {
            string sql = @"SELECT new_districtId districtId,new_name name,new_days days,new_shifts shifts,versionnumber from new_districtBase 
                        where new_days IS NOT NULL AND LEN(new_days) > 0 
                        AND new_shifts IS NOT NULL AND LEN(new_shifts) > 0 ";
            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
            List<District> List = new List<District>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                List.Add(new District()
                {
                    districtId = dt.Rows[i][0].ToString(),
                    name = dt.Rows[i][1].ToString(),
                    days = dt.Rows[i][2].ToString(),
                    shifts = dt.Rows[i][3].ToString()
                });
            }
            return List;

        }


        // GET: api/Fake/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Fake
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Fake/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Fake/5
        public void Delete(int id)
        {
        }
    }
}

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
      // [NasAuthorizationFilter]
       [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("{lang}/api/Car")] // en/api/Car 
    [ApiExplorerSettings(IgnoreApi = true)]

    public class CarController : ApiController
    {
        // GET api/default1



        // GET api/Car
        [Route("")]

        public List<Car> Get()
        {
            string sql = @"select car.new_carresourceId,car.new_name from new_carresourceBase car";

            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
            List<Car> List = new List<Car>();
            for (int i = 0; i < dt.Rows.Count; i++) List.Add(new Car()
            {
                Id = dt.Rows[i][0].ToString(),
                Name = dt.Rows[i][1].ToString(),
            });

            return List;
        }

        //public List<Car> Get(string username,string password)
        //{
        //    //string sql = @"select car.new_carresourceId,car.new_name from new_carresourceBase car";
        //    string sql = @"select car.new_carresourceId,car.new_name from new_carresourceBase car where car.new_username='" + username + "' and car.new_password='" + password + "'"; ;

        //    DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
        //    List<Car> List = new List<Car>();
        //    for (int i = 0; i < dt.Rows.Count; i++) List.Add(new Car()
        //    {
        //        Id = dt.Rows[i][0].ToString(),
        //        Name = dt.Rows[i][1].ToString(),
        //    });

        //    return List;
        //}




        // GET api/car/5
        [Route("{id}")]
        public List<District> Get(string id)
        {
            string sql = @"SELECT new_districtId districtId,new_name name,new_days days,new_shifts shifts,versionnumber from new_districtBase 
                        where new_cityid='@cityId' AND new_days IS NOT NULL AND LEN(new_days) > 0 
                        AND new_shifts IS NOT NULL AND LEN(new_shifts) > 0 ";
            sql = sql.Replace("@cityId", id);
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

        // POST api/city
        public void Post([FromBody]string value)
        {
        }

        // PUT api/city/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/city/5
        public void Delete(int id)
        {
        }
    }

    public class Car
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
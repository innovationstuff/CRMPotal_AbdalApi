using NasAPI.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

using NasAPI.Models;
using NasAPI.Managers;
using System.Web.Http.Description;

namespace NasAPI.Controllers.API
{
    // [NasAuthorizationFilter]

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("{lang}/api/city")] // en/api/city 
    //[RoutePrefix("{lang:alpha=en}/api/city")] // en/api/city 
    public class CityController : BaseApiController
    {
        CityManager Manager { get; set; }

        public CityController()
        {
            this.Manager = new CityManager();
        }

        [Route("")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<City> Get(int lang = 0)
        {
            string sql = @"SELECT distinct city.new_CityId CityId,city.new_name Name,city.versionnumber,new_englsihName from new_CityBase city,new_districtBase 
where city.new_CityId=new_districtBase.new_cityid and (new_districtBase.new_days IS NOT NULL AND LEN(new_districtBase.new_days) > 0 
                        AND new_districtBase.new_shifts IS NOT NULL AND LEN(new_districtBase.new_shifts) > 0 )  order by city.new_name ";

            System.Data.DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
            List<City> CityList = new List<City>();
            string name = "Name";

            if (lang == 1)
                name = "new_englsihName";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CityList.Add(new City() { CityId = dt.Rows[i][0].ToString(), Name = dt.Rows[i][name].ToString() });
            }
            return CityList;
        }

        [Route("GetAllCity")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<City> GetAllCity()
        {
            string sql = @"SELECT distinct city.new_CityId CityId,city.new_name Name  from new_CityBase city,new_districtBase 
where city.new_CityId=new_districtBase.new_cityid   order by city.new_name ";

            System.Data.DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
            List<City> CityList = new List<City>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CityList.Add(new City() { CityId = dt.Rows[i][0].ToString(), Name = dt.Rows[i][1].ToString() });
            }
            return CityList.GroupBy(x => x.Name)
                  .Select(group => group.First()).Where(a => a.Name != "الاحساء");

        }

        [Route("GetAllCityBase")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public List<City> GetAllCityBase(int lang = 0)
        {
            string sqlQuery = @" select
                                     city.new_CityId cityId,
                                     city.new_name cityNameAr,
                                     city.new_englsihName cityNameEn
                                     from new_City city order by city.new_name";


            DataTable dt = CRMAccessDB.SelectQ(sqlQuery).Tables[0];
            List<City> cities = new List<City>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (lang == 0)
                {
                    cities.Add(new City { CityId = dt.Rows[i]["cityId"].ToString(), Name = dt.Rows[i]["cityNameAr"].ToString() });

                }
                else
                    cities.Add(new City { CityId = dt.Rows[i]["cityId"].ToString(), Name = dt.Rows[i]["cityNameEn"].ToString() });

            }
            return cities;
        }

        [Route("QuickAll")]
        [ResponseType(typeof(List<BaseQuickLookup>))]
        public HttpResponseMessage GetQuickAll()
        {
            var result = Manager.GetAllForLookup(Language).ToList();
            return OkResponse<List<BaseQuickLookup>>(result);
        }

        [HttpGet]
        // GET Get All available District Of Specific City 
        [Route("Districts/{id}")]
        [ResponseType(typeof(List<District>))]
        public HttpResponseMessage Get(string id)
        {
            var result = Manager.GetDistrictsByCity(id).ToList();
            return OkResponse<List<District>>(result);
        }

        //[Route("GetLanguage")]
        //public string GetLanguage()
        //{
        //    return Language.ToString();
        //}

    }
}

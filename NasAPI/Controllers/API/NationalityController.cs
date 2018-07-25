using NasAPI.Filters;
using NasAPI.Managers;
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
    //   [NasAuthorizationFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("{lang}/api/Nationality")] // en/api/Nationality 
    public class NationalityController : BaseApiController
    {
        NationalityManager Manager { get; set; }

        public NationalityController()
        {
            this.Manager = new NationalityManager();

        }

        //Get api/Nationality
        [Route("")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<Nationality> Get(bool HasHourlyPricing = false)
        {
            string sql;
            if (!HasHourlyPricing)
                sql = @"SELECT new_CountryId CountryId , new_name name,new_code code,new_isocode isocode,
                        new_NameEnglish NameEnglish,new_axcode axcode ,versionnumber from new_CountryBase order by new_name";

            else
                sql = @"SELECT distinct country.new_CountryId CountryId ,country.new_name name,country.new_code code,country.new_isocode isocode,
                        country.new_NameEnglish NameEnglish,country.new_axcode axcode ,country.versionnumber  
                        from new_CountryBase country, new_hourlypricingBase hourPrice
                        where country.new_CountryId =hourPrice.new_nationality order by country.new_name ";

            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
            List<Nationality> List = new List<Nationality>();
            for (int i = 0; i < dt.Rows.Count; i++) List.Add(new Nationality()
            {
                CountryId = dt.Rows[i][0].ToString(),
                Name = dt.Rows[i][1].ToString(),
                Code = dt.Rows[i][2].ToString(),
                IsOCode = dt.Rows[i][3].ToString(),
                NameEnglish = dt.Rows[i][4].ToString(),
                AXCode = dt.Rows[i][5].ToString(),
                VersionNumber = dt.Rows[i][6].ToString()

            });
            return List;
        }

        //Get /api/Nationality/GetDistNats/34c98e70-37c0-e511-80fb-00505691216d
        [Route("GetDistNats/{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<Nationality> GetDistNats(string id)
        {
            string sql;

            sql = @"SELECT distinct country.new_CountryId CountryId ,country.new_name name,country.new_code code,country.new_isocode isocode,
                        country.new_NameEnglish NameEnglish,country.new_axcode axcode ,country.versionnumber  
                        from new_CountryBase country, new_hourlypricingBase hourPrice,new_carresource,new_district,new_district_carresource
                        where country.new_CountryId =hourPrice.new_nationality
						and new_carresource.new_carresourceId=new_district_carresource.new_carresourceid and new_district.new_districtId=new_district_carresource.new_districtId
						and new_district.new_districtId='@id'  order by country.new_name";
            sql = sql.Replace("@id", id);
            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
            List<Nationality> List = new List<Nationality>();
            for (int i = 0; i < dt.Rows.Count; i++) List.Add(new Nationality()
            {
                CountryId = dt.Rows[i][0].ToString(),
                Name = dt.Rows[i][1].ToString(),
                Code = dt.Rows[i][2].ToString(),
                IsOCode = dt.Rows[i][3].ToString(),
                NameEnglish = dt.Rows[i][4].ToString(),
                AXCode = dt.Rows[i][5].ToString(),
                VersionNumber = dt.Rows[i][6].ToString()

            });
            return List;
        }

        [HttpGet]
        [Route("GetAllNationlity")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public List<Nationality> GetAllNationlity(int lang = 0)
        {
            string sqlQuery = @"select
                                    country.new_CountryId CountryId,
                                    country.new_name CountryNameAr,
                                    country.new_NameEnglish CountryNameEn
                                    from new_CountryBase country
                                    order by country.new_name";

            DataTable dt = CRMAccessDB.SelectQ(sqlQuery).Tables[0];
            List<Nationality> nationlitys = new List<Nationality>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (lang == 0)
                {
                    nationlitys.Add(new Nationality { CountryId = dt.Rows[i]["CountryId"].ToString(), Name = dt.Rows[i]["CountryNameAr"].ToString() });

                }
                else
                    nationlitys.Add(new Nationality { CountryId = dt.Rows[i]["CountryId"].ToString(), Name = dt.Rows[i]["CountryNameEn"].ToString() });

            }
            return nationlitys;
        }


        [Route("QuickAll")]
        [ResponseType(typeof(List<BaseQuickLookup>))]
        public HttpResponseMessage GetQuickAll()
        {
            var result = Manager.GetAllForLookup(Language);
            result = result.OrderByDescending(i => i.Key.ToLower() == "1e0ff838-292f-e311-b3fd-00155d010303").ThenBy(i => i.Value);
            return OkResponse<List<BaseQuickLookup>>(result.ToList());
        }


        [Route("Individual")]
        [ResponseType(typeof(List<BaseQuickLookup>))]
        public HttpResponseMessage GetForIndividual()
        {
            var result = Manager.GetIndividualNationalities(Language);
            result = result.OrderByDescending(i => i.Key.ToLower() == "1e0ff838-292f-e311-b3fd-00155d010303").ThenBy(i => i.Value);
            return OkResponse<List<BaseQuickLookup>>(result.ToList());
        }

    }



    public class Nationality
    {

        public string CountryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string IsOCode { get; set; }
        public string NameEnglish { get; set; }
        public string AXCode { get; set; }
        public string VersionNumber { get; set; }

    }

}
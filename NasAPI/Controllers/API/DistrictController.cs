using NasAPI.Filters;
using NasAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    [RoutePrefix("{lang}/api/District")] // en/api/District 
    public class DistrictController : BaseApiController
    {
        // GET api/test
        [Route("")]
        [ApiExplorerSettings(IgnoreApi = true)]
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

        [HttpGet]
        // GET Get All available District Of Specific City  >> Transferred to City Controller
        [Route("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public List<District> Get(string id)
        {
            string sql = @"SELECT new_districtId districtId,new_name name,new_days days,new_shifts shifts,versionnumber from new_districtBase 
                        where new_cityid='@cityId' AND new_days IS NOT NULL AND LEN(new_days) > 0 
                        AND new_shifts IS NOT NULL AND LEN(new_shifts) > 0  order by new_name ";
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

        [Route("GetCityDistricts/{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public List<District> GetCityDistricts(string id)
        {
            string sql = @"SELECT new_districtId districtId,new_name name from new_districtBase 
                        where new_cityid='@cityId' order by new_name ";
            sql = sql.Replace("@cityId", id);
            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
            List<District> List = new List<District>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                List.Add(new District()
                {
                    districtId = dt.Rows[i][0].ToString(),
                    name = dt.Rows[i][1].ToString(),

                });
            }
            return List.GroupBy(x => x.name).Select(group => group.First()).Where(a => a.name != "الأسكان").ToList();

        }

        [HttpGet]
        //Get  District /api/district/GetCurrentDistrict?longitude=,latitude=
        [Route("DistrictDays/{id}")]
        [ResponseType(typeof(List<string>))]
        public HttpResponseMessage GetDistrictDays(string id)
        {
            List<string> result;

            if (ConfigurationManager.AppSettings["OpenDays"] == "true")
                result = Enum.GetNames(typeof(DayOfWeek)).ToList();

            string sql = @"SELECT new_days days from new_districtBase 
                        where new_districtId='@new_districtId' AND new_days IS NOT NULL AND LEN(new_days) > 0 
                        AND new_shifts IS NOT NULL AND LEN(new_shifts) > 0  ";
            sql = sql.Replace("@new_districtId", id);
            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];
            List<string> List = new List<string>();

            List = dt.Rows[0][0].ToString().Split(',').ToList();

            result = List;
            return OkResponse<List<string>>(result);
        }

        [Route("GetCurrentDistrict")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public area GetCurrentDistrict(string longitude, string latitude)
        {


            string sql = @"
select new_districtId,Area,distric.new_cityid,distric.new_name distname,city.new_name

 from   HourlyDB.dbo.new_districtBase distric left outer join HourlyDB.dbo.new_CityBase city on 
 distric.new_cityid=city.new_CityId
 where   CHARINDEX('@long @lat',convert(nvarchar(max),area,2)) > 0 
";

            sql = @"
select Area, new_districtId,Area,distric.new_cityid,distric.new_name,city.new_name,convert(nvarchar(max),area)

 from   HourlyDB.dbo.new_districtBase distric left outer join HourlyDB.dbo.new_CityBase city on 
 distric.new_cityid=city.new_CityId";


            // sql=sql.Replace("@long",longitude).Replace("@lat",latitude);


            DataTable dt = CRMAccessDB.SelectQ(sql).Tables[0];




            area areas = new area();
            if (dt.Rows.Count > 0)
                areas = new area()
                 {
                     district = new District()
                     {
                         name = dt.Rows[0]["distname"].ToString(),
                         districtId = dt.Rows[0]["new_districtId"].ToString(),
                     },
                     City = new Models.City()
                     {

                         Name = dt.Rows[0]["new_name"].ToString(),
                         CityId = dt.Rows[0]["new_cityid"].ToString(),

                     }



                 };

            return areas;

        }


        public static bool IsInPolygon(Point[] poly, Point point)
        {
            var coef = poly.Skip(1).Select((p, i) =>
                                            (point.X - poly[i].Y) * (p.X - poly[i].X)
                                          - (point.X - poly[i].X) * (p.Y - poly[i].Y))
                                    .ToList();

            if (coef.Any(p => p == 0))
                return true;

            for (int i = 1; i < coef.Count(); i++)
            {
                if (coef[i] * coef[i - 1] < 0)
                    return false;
            }
            return true;
        }

        //public static MapPolygon StringToMapPolygon(string polyString, MapPolygon mapPolygon = null)
        //{
        //    MapPolygon polygon = new MapPolygon { Locations = new LocationCollection() };

        //    var points = polyString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //    try
        //    {
        //        foreach (var point in points)
        //        {
        //            var pairs = point.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //            if (pairs[0].Contains("\n")) continue;
        //            if (pairs[1].Contains("\n")) continue;
        //            var lat = double.Parse(pairs[1]);
        //            var lon = double.Parse(pairs[0]);
        //            polygon.Locations.Add(new Location(lat, lon));
        //        }
        //        return polygon;
        //    }
        //    catch (Exception)
        //    {

        //        return polygon;
        //    }

        //}

    }


    //public class Districts
    //{
    //    //,new_days days,new_shifts shifts,versionnumber from new_districtBase 
    //    public string districtId { get; set; }
    //    public string name { get; set; }
    //    public string days { get; set; }
    //    public string shifts { get; set; }


    //}

    public class Point
    {

        public decimal X { get; set; }
        public decimal Y { get; set; }
    }
    public class area
    {

        public District district { get; set; }

        public NasAPI.Models.City City { get; set; }


    }

}
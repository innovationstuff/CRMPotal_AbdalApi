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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("{lang}/api/Professions")] // en/api/Professions 
    public class ProfessionsController : BaseApiController
    {
        ProfessionManager Manager { get; set; }

        public ProfessionsController()
        {
            this.Manager = new ProfessionManager();
        }

        [Route("QuickAll")]
        [ResponseType(typeof(List<BaseQuickLookup>))]
        public HttpResponseMessage GetQuickAll()
        {
            var result = Manager.GetAllForLookup(Language).ToList();
            return OkResponse<List<BaseQuickLookup>>(result);
        }


        [Route("Individual")]
        [ResponseType(typeof(List<BaseQuickLookup>))]
        public HttpResponseMessage GetForIndividual()
        {
            var result = Manager.GetIndividualProfessions(Language).ToList();
            return OkResponse<List<BaseQuickLookup>>(result);
        }


        #region deprecated
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public List<Profession> GetAllProfessions(int lang = 0)
        {

            string sqlQuery = @"select  
                                    distinct  
                                    profession.new_professionId  ProfessionId,
                                    profession.new_name  professionNameAr,
                                    profession.new_ProfessionEnglish professionNameEn   
                                    from new_profession profession
                                    order by profession.new_name ";
            DataTable dt = CRMAccessDB.SelectQ(sqlQuery).Tables[0];
            List<Profession> professions = new List<Profession>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (lang == 0)
                {
                    professions.Add(new Profession { ProfessionId = dt.Rows[i]["ProfessionId"].ToString(), ProfessionName = dt.Rows[i]["professionNameAr"].ToString() });

                }
                else
                    professions.Add(new Profession { ProfessionId = dt.Rows[i]["ProfessionId"].ToString(), ProfessionName = dt.Rows[i]["professionNameEn"].ToString() });

            }
            return professions;

        }
        #endregion


    }
    public class Profession
    {
        public string ProfessionId { get; set; }
        public string ProfessionName { get; set; }
    }
}

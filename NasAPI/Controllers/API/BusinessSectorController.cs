using NasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace NasAPI.Controllers.API
{
  [EnableCors(origins: "*", headers: "*", methods: "*")]
  [ApiExplorerSettings(IgnoreApi = true)]

    public class BusinessSectorController : ApiController
    {

        public NationalityController NationalityController { get; set; }
        public ProfessionsController ProfessionsController { get; set; }
        public CityController CityController { get; set; }
        public BusinessSector BusinessSector { get; set; }
        public OptionsController Sectors { get; set; }



        [HttpGet]

        public BusinessSector GetOnLoadData(int lang = 0)
        {

            NationalityController = new NationalityController();
            ProfessionsController = new ProfessionsController();
            CityController = new CityController();
            BusinessSector = new BusinessSector();
            Sectors = new OptionsController();


            BusinessSector.Nationality = NationalityController.GetAllNationlity(lang);
            BusinessSector.Profession = ProfessionsController.GetAllProfessions(lang);
            BusinessSector.sectors = Sectors.GetSectors(0);

            return BusinessSector;
        }



    }
    public class BusinessSector
    {
        public List<Nationality> Nationality { get; set; }

        public List<Profession> Profession { get; set; }

        public List<City> City { get; set; }
        public List<OptionList> sectors { get; set; }
    }
}
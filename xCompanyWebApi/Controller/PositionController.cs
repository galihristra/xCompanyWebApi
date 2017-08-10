using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using xCompanyWebApi.Models;
using xCompanyWebApi.Models.ViewModels;
using xCompanyWebApi.Database;

namespace xCompanyWebApi.Controller
{
    public class PositionController : ApiController
    {
        static readonly xcompanyEntities db = new xcompanyEntities();

        [HttpGet]
        public List<PositionVM> GetAllPositions()
        {
            List<PositionVM> listPos = new List<PositionVM>();

            foreach (var item in db.positions.OrderBy(x => x.posName))
            {
                PositionVM pos = new PositionVM();
                pos.posId = item.posId;
                pos.posName = item.posName;
                listPos.Add(pos);
            }

            return listPos;
        }
        
        [HttpGet]
        public PositionVM GetPositionByID(string param)
        {
            Guid raw = new Guid(param);
            position pos = (from s in db.positions
                              where s.posId == raw
                              select s).FirstOrDefault();

            PositionVM posResult = new PositionVM();
            posResult.posId = pos.posId;
            posResult.posName = pos.posName;

            return posResult;
        }

        [HttpGet]
        public PositionVM GetPositionByName(string param)
        {
            position pos = (from s in db.positions
                            where s.posName.ToUpper().Contains(param.ToUpper())
                            select s).FirstOrDefault();

            PositionVM posResult = new PositionVM();
            posResult.posId = pos.posId;
            posResult.posName = pos.posName;

            return posResult;
        }

        [HttpPost, HttpGet]
        public IHttpActionResult AddPosition(string param)
        {
            // if there's an error, will return the error message
            // if there's not, will return empty string
            string msg =  new PositionDBConnect().AddPosition(param);
            if (string.IsNullOrEmpty(msg))
            {
                return Ok();
            }
            else
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        public IHttpActionResult DeletePosition(string param)
        {
            if (string.IsNullOrEmpty(new PositionDBConnect().DeletePosition(param)))
            {
                return Ok();
            }
            else
            {
                return InternalServerError();
            }
        }
    }
}

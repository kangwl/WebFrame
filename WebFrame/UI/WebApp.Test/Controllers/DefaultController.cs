using System;
using System.Web.Mvc;
using Test.IService;

namespace WebApp.Test.Controllers
{
    public class DefaultController : Controller
    {
        public IUserServ UserServ { get; set; }
        // GET: Default
        public ActionResult Index()
        {

            var user = UserServ.GetOne(Guid.NewGuid());
            return View();
        }
    }
}
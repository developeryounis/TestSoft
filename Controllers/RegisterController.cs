using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Store.Domain;
using Store.Models;

namespace Store.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            if (Request.Cookies["Store"] == null)
            {
                return View(new Person());
            }
            else
                return RedirectToAction("Index", "Home");
        }
        private readonly PersonService _personService;
        public RegisterController() :this(new PersonService()){ }
        public RegisterController(PersonService personService)
        {
            _personService = personService;
        }

        [HttpPost]
        public ActionResult Add(Person person)
        {
            if (ModelState.IsValid)
            {
                Person Value = _personService.Add(person);
                UtilitiesModel.AddCookie("Store", new string[] {"PID", "Username"},
                    new string[] {Value.PersonID.ToString(), Value.Username}, true, Response);
                return RedirectToAction("AccountCreated",
                    new { id = "Thanks " + person.PersonName + " Your Account Created Successfuly" });
            }
            return View("Index", person);
        }

        public ActionResult AccountCreated(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
                return View("AccountCreated", new {message=id});
            return RedirectToAction("Index");
        }
    }
}
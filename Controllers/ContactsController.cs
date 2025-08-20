using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsWebsite.Data;
using NewsWebsite.Models;

namespace NewsWebsite.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }


        /// 
        /// 
        /// 
        //[Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(_context.Contacts.ToList());

        }



        //[ActionName("Contact")]
        //[Authorize(Roles = "Admin")]
        
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            return View(_context.Contacts.Find(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Contact contact)
        {
            _context.Update(contact);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {

                _context.Contacts.Add(contact);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            //same as return View() as long as action name same to view name
            return View("Create", contact);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            ;
            _context.Remove(_context.Contacts.Find(id));
            _context.SaveChanges();

            return RedirectToAction("Index");

        }
        ///         /// 


        //public IActionResult anotherIndex()
        //{
        //    return View(_context.Contacts.ToList());
        //}

        //[ActionName("Contact")]
        //public IActionResult ContactUs()
        //{
        //    return View();

        //}

        //[ValidateAntiForgeryToken]
        //[HttpPost]
        //public IActionResult ContactUs(Contact contact)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return View(contact);
        //    }
        //    else
        //    {
        //        _context.Contacts.Add(contact);
        //        _context.SaveChanges();

        //        return RedirectToAction("Index");

        //    }

        //}








    }
}

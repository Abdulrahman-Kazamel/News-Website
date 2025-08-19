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



        public IActionResult anotherIndex()
        {
            return View(_context.Contacts.ToList());
        }

        //[ActionName("Contact")]
        public IActionResult ContactUs()
        {
            return View();

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult ContactUs(Contact contact)
        {

            if (!ModelState.IsValid)
            {
                return View(contact);
            }
            else
            {
                _context.Contacts.Add(contact);
                _context.SaveChanges();

                return RedirectToAction("Index");

            }

        }








    }
}

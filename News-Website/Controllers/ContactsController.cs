using System.Threading.Tasks;
using NewsWebsite.Core.Interfaces;
using NewsWebsite.Core.Models;

namespace NewsWebsite.Controllers
{
    public class ContactsController : Controller
    {
        
        private readonly IRepository<Contact> _ContactsRepository;

        public ContactsController(IRepository<Contact> ContactsRepository)
        {
          
            _ContactsRepository = ContactsRepository;
        }


        /*        
                                                Index

        */


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _ContactsRepository.GetAllAsync());

        }


        /*        
                                                 Create
                                                 this create should not be there , for data integrity , as a moral bussiness should not create feedback or send messages to it self
                                                 in the feuture and for business logic this should be as ticketing system and add viewed by user on time DateTime
                                                 also add states as handled or not, or turned as real customer or not, and add converation rate on admin dashboard.
                                    

         */

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return  View();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                await _ContactsRepository.AddAsync(contact);
                await _ContactsRepository.SaveAsync();

                return RedirectToAction(nameof(Index));
            }
            //same as return View() as long as action name same to view name
            //just to try diffrent ways
            // I will update this to ViewModel later 
            return View("Create", contact);
        }




        /*        
                                             Edit

       */

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _ContactsRepository.GetByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Contact contact)
        {

            _ContactsRepository.Update(contact);
           await _ContactsRepository.SaveAsync();


            return RedirectToAction(nameof(Index));
        }


        /*        
                                            Details

       */

        public async Task<IActionResult> Details(int id)
        {
            var contact = await _ContactsRepository.GetByIdAsync(id);


            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }



    /*        
                                         Delete
                        later I will handle all delete with bootstrap modal pop up 

     */
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _ContactsRepository.GetByIdAsync(id);
             _ContactsRepository.Delete(contact);

            await _ContactsRepository.SaveAsync();

            return RedirectToAction("Index");

        }








    }
}

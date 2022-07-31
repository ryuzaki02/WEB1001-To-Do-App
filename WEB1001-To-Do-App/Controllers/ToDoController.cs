using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB1001_To_Do_App;

namespace WEB1001_To_Do_App.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoContext _context;

        // Constructor method which intialise local context object
        public ToDoController(ToDoContext context)
        {
            _context = context;
        }

        // GET: ToDo
        public async Task<IActionResult> Index()
        {
            // Gets data ToDo list from database and passed to actual view
            ViewData["ToDoList"] = await _context.ToDo.ToListAsync();
            return View();
        }

        // GET: ToDo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Method to create new ToDo, get user input data from binding variable and save to database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ToDoModelId,IsCompleted,CompletionDate,Description")] ToDoModel toDoModel)
        {
            // Checks if state is valid or not
            if (ModelState.IsValid)
            {
                // Saves actual Todo model to database
                _context.Add(toDoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: ToDo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // Method to updated IsCompleted state of particular Todo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] ToDoModel toDoModel)
        {
            //Just get all the objects from database
            // take todo model id
            // compare from db objects and updated isCompleted variable
            // save it again to database

            // Get list from database of Todos
            List<ToDoModel> models = await _context.ToDo.ToListAsync();
            // Get actual model from the list by comparing ids
            ToDoModel model = models.First(model => model.ToDoModelId == toDoModel.ToDoModelId);

            // Checks if model is null or not
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Changes the state of Todo IsCompleted
                model.IsCompleted = toDoModel.IsCompleted;
                try
                {
                    // This updates the model to Database
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoModelExists(toDoModel.ToDoModelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ToDo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // Method to delet particular Todo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, [FromForm] ToDoModel toDoModel)
        {
            // Get list from database of Todos
            List<ToDoModel> models = await _context.ToDo.ToListAsync();
            // Get actual model from the list by comparing ids
            ToDoModel model = models.First(model => model.ToDoModelId == toDoModel.ToDoModelId);
            try
            {
                // This removes the model to Database
                _context.ToDo.Remove(model);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // This method checks whether model exists or not with help of id comparison
        private bool ToDoModelExists(int id)
        {
            return _context.ToDo.Any(e => e.ToDoModelId == id);
        }





        // ---------------- EXTRA METHODS ----------
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ActionName("Index")]
        //public async Task<IActionResult> Index2(int id, [FromForm] ToDoModel toDoModel)
        //{
        //    if (id != toDoModel.ToDoModelId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(toDoModel);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ToDoModelExists(toDoModel.ToDoModelId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ToDoList"] = await _context.ToDo.ToListAsync();
        //    return View();
        //}

        //    // GET: ToDo/Delete/5
        //    public async Task<IActionResult> Delete(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var toDoModel = await _context.ToDo
        //            .FirstOrDefaultAsync(m => m.ToDoModelId == id);
        //        if (toDoModel == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(toDoModel);
        //    }

        //    // POST: ToDo/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> DeleteConfirmed(int id)
        //    {
        //        var toDoModel = await _context.ToDo.FindAsync(id);
        //        _context.ToDo.Remove(toDoModel);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //// GET: ToDo/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var toDoModel = await _context.ToDo
        //        .FirstOrDefaultAsync(m => m.ToDoModelId == id);
        //    if (toDoModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(toDoModel);
        //}
        // GET: ToDo/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var toDoModel = await _context.ToDo.FindAsync(id);
        //    if (toDoModel == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(toDoModel);
        //}
    }
}

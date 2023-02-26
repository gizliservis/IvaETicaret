using IvaETicaret.Data;
using Microsoft.AspNetCore.Mvc;

namespace IvaETicaret.ViewCompoments
{
    public class CategoryList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public CategoryList(ApplicationDbContext db)
        {
            _db = db;
        }
        public IViewComponentResult Invoke(int id)
        {
            var category=_db.Categories.Where(c=>c.DepartmentId==id).ToList();
            return View(category);
        }
    }
}

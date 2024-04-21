using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.ViewModel;

namespace WebApplication1.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentDbContext _db;
        private readonly IWebHostEnvironment _en;
        public StudentsController(StudentDbContext db, IWebHostEnvironment en)
        {
            _db = db;
            _en = en;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            IQueryable<Student> student = _db.Students.Include(x => x.StudentSkills).ThenInclude(x => x.Skill);

            ViewBag.search = searchString;
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                student = student.Where(e => e.StudentName.ToLower().Contains(searchString));
            }

            ViewBag.sortParam = string.IsNullOrEmpty(sortOrder) ? "desc_name" : "";
            switch (sortOrder)
            {
                case "desc_name":
                    student = student.OrderByDescending(e => e.StudentName);
                    break;
                default:
                    student = student.OrderBy(e => e.StudentName);
                    break;
            }
            return View(student);
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult AddNewSkills(int? id)
        {
            ViewBag.skill = new SelectList(_db.Skills, "SkillId", "SkillName", id.ToString() ?? "");
            return PartialView("_addNewSkills");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentVM studentVM, int[] skillId)
        {
            if (ModelState.IsValid)
            {
                Student student = new Student()
                {
                    StudentName = studentVM.StudentName,
                    DateOfBirth = studentVM.DateOfBirth,
                    Age = studentVM.Age,
                    MaritalStatus = studentVM.MaritalStatus
                };
                var file = studentVM.ImageFile;
                if (file != null)
                {
                    var webroot = _en.WebRootPath;
                    var folder = "Images";
                    var imgFileName = Path.GetFileName(studentVM.ImageFile.FileName);
                    var fileToSave = Path.Combine(webroot, folder, imgFileName);
                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        studentVM.ImageFile.CopyTo(stream);
                        student.Image = "/" + folder + "/" + imgFileName;
                    }
                }
                foreach (var item in skillId)
                {
                    StudentSkill studentSkill = new StudentSkill()
                    {
                        Student = student,
                        StudentId = student.StudentId,
                        SkillId = item
                    };
                    _db.StudentSkills.Add(studentSkill);
                }
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            var student = _db.Students.Find(id);
            StudentVM studentVM = new StudentVM()
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                Age = student.Age,
                DateOfBirth = student.DateOfBirth,
                MaritalStatus   = student.MaritalStatus,
                Image   = student.Image,
            };
            var existSkill = _db.StudentSkills.Where(x => x.StudentId == id).ToList();
            foreach (var item in existSkill)
            {
                studentVM.SkillList.Add(item.SkillId);
            }
            return View(studentVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(StudentVM studentVM, int[] skillId)
        {
            if (ModelState.IsValid)
            {
                Student student = new Student()
                {
                    StudentId = studentVM.StudentId,
                    StudentName = studentVM.StudentName,
                    DateOfBirth = studentVM.DateOfBirth,
                    Age = studentVM.Age,
                    MaritalStatus = studentVM.MaritalStatus,
                    Image = studentVM.Image
                };
                var file = studentVM.ImageFile;
                if (file != null)
                {
                    string webroot = _en.WebRootPath;
                    string folder = "Images";
                    string imgFileName = Path.GetFileName(studentVM.ImageFile.FileName);
                    string fileToSave = Path.Combine(webroot, folder, imgFileName);
                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        studentVM.ImageFile.CopyTo(stream);
                        student.Image = "/" + folder + "/" + imgFileName;
                    }
                }
                else
                {
                    student.Image = studentVM.Image;
                }

                var existSkill = _db.StudentSkills.Where(x => x.StudentId == student.StudentId).ToList();
                foreach (var item in existSkill)
                {
                    _db.StudentSkills.Remove(item);
                }
                foreach (var item in skillId)
                {
                    StudentSkill studentSkill = new StudentSkill()
                    {

                        StudentId = student.StudentId,
                        SkillId = item
                    };
                    _db.StudentSkills.Add(studentSkill);
                }
                _db.Update(student);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Delete(int ? id)
        {
            var student = _db.Students.Find(id);
            if (id != null)
            {
                var existSkill = _db.StudentSkills.Where(x => x.StudentId == id).ToList();
                _db.StudentSkills.RemoveRange(existSkill);
                _db.Students.Remove(student);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}

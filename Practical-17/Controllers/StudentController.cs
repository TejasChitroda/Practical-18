using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Practical_17.Models;
using Practical_17.Repository.StudentRepo;
//using Practical_17.ViewModels; // Make sure to import ViewModel namespace

namespace Practical_17.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly StudentRepository _studentRepository;
        private readonly IMapper mapper;

        public StudentController(ApplicationDbContext context, IMapper mapper)
        {
            _studentRepository = new StudentRepository(context);
            _context = context;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _studentRepository.GetStudentsAsync();
            var studentVMs = mapper.Map<List<StudentViewModel>>(students);
            return View(studentVMs);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _studentRepository.GetStudentAsync(id.Value);
            if (student == null)
                return NotFound();

            var viewModel = mapper.Map<StudentViewModel>(student);
            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var student = mapper.Map<Student>(viewModel);
            await _studentRepository.AddStudent(student);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _studentRepository.GetStudentAsync(id.Value);
            if (student == null)
                return NotFound();

            var viewModel = mapper.Map<StudentViewModel>(student);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentViewModel viewModel)
        {
            if (id != viewModel.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(viewModel);

            var student = await _studentRepository.GetStudentAsync(id);
            if (student == null)
                return NotFound();

            mapper.Map(viewModel, student);
            await _studentRepository.UpdateStudentAsync(student);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _studentRepository.GetStudentAsync(id.Value);
            if (student == null)
                return NotFound();

            var viewModel = mapper.Map<StudentViewModel>(student);
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _studentRepository.GetStudentAsync(id);
            if (student != null)
                _context.Students.Remove(student);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

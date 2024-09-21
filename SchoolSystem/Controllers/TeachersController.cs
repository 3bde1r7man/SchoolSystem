using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Data;
using SchoolSystem.Models;

namespace SchoolSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly SchoolSystemContext _context;

        public TeachersController(SchoolSystemContext context)
        {
            _context = context;
        }

        // GET: api/Teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeacher()
        {
            return await _context.Teacher.ToListAsync();
        }

        // GET: api/Teachers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(int? id)
        {
            var teacher = await _context.Teacher.FindAsync(id);

            if (teacher == null)
            {
                return NotFound();
            }

            return teacher;
        }

        // PUT: api/Teachers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int? id, Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return BadRequest();
            }

            _context.Entry(teacher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Teachers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Teacher>> PostTeacher(Teacher teacher)
        {
            _context.Teacher.Add(teacher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeacher", new { id = teacher.Id }, teacher);
        }

        // DELETE: api/Teachers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int? id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // GET: api/Teachers/5/Courses
        [HttpGet("{id}/Courses")]
        public async Task<ActionResult<IEnumerable<Course>>> GetTeacherCourses(int? id)
        {
            var teacherCourses = await _context.TeacherCourses
                .Where(tc => tc.TeacherId == id)
                .ToListAsync();

            List<Course> courses = new List<Course>();

            foreach (var teacherCourse in teacherCourses)
            {
                var course = await _context.Courses.FindAsync(teacherCourse.CourseId);
                courses.Add(course);
            }

            return courses;
        }

        // POST: api/Teachers/5/AssignCourse/1
        [HttpPost("{id}/AssignCourse/{courseId}")]
        public async Task<IActionResult> AssignCourse(int? id, int? courseId)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            var course = await _context.Courses.FindAsync(courseId);
            

            if (teacher == null || course == null)
            {
                return NotFound();
            }

            TeacherCourses teacherCourses = new TeacherCourses 
            { 
                TeacherId = id,
                CourseId = courseId
            };
            _context.TeacherCourses.Add(teacherCourses);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("GetTeacherCourses", new { id = id });

        }

        // DELETE: api/Teachers/5/UnassignCourse/1
        [HttpDelete("{id}/UnassignCourse/{courseId}")]
        public async Task<IActionResult> UnassignCourse(int? id, int? courseId)
        {
            var teacherCourse = await _context.TeacherCourses
                .Where(tc => tc.TeacherId == id && tc.CourseId == courseId)
                .FirstOrDefaultAsync();

            if (teacherCourse == null)
            {
                return NotFound();
            }

            _context.TeacherCourses.Remove(teacherCourse);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetTeacherCourses", new { id = id });
        }

        private bool TeacherExists(int? id)
        {
            return _context.Teacher.Any(e => e.Id == id);
        }
    }
}

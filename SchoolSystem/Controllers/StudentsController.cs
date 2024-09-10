using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Data;
using SchoolSystem.Models;
using SchoolSystem.Util;

namespace SchoolSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly SchoolSystemContext _context;

        public StudentsController(SchoolSystemContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int? id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int? id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("GetStudent", new { id = student.Id });
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            if (student.Grade < 1 || student.Grade > 12)
            {
                return BadRequest("Grade must be between 1 and 12");
            }
            
            string normalizedGender = student.Gender.ToUpper(); // MALE | FEMALE
            if (normalizedGender != "MALE" && normalizedGender != "FEMALE")
            {
                return BadRequest();
            }
           
            if (student.PhoneNumber != null)
            {
                PhoneValidation phoneValidation = new PhoneValidation();
                if (!phoneValidation.IsValid(student.PhoneNumber))
                {
                    return BadRequest("Invalid phone number");
                }
            }

            if (student.Email != null)
            {
                EmailValidation emailValidation = new EmailValidation();
                if (!emailValidation.IsValid(student.Email))
                {
                    return BadRequest("Invalid email");
                }
            }
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int? id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Students/5/courses
        // For a given student, return all courses He is registered in
        [HttpGet("{id}/courses")]
        public async Task<ActionResult<IEnumerable<Course>>> GetStudentCourses(int? id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var studentCourses = _context.StudentCourses.Where(sc => sc.StudentId == id).Select(sc => sc.Course).ToList();
            return studentCourses;
        }

        // GET: api/Students/5/avaliable-courses
        // For a given student, return all courses He is not registered in yet equal to the student's grade
        [HttpGet("{id}/avaliable-courses")]
        public async Task<ActionResult<IEnumerable<Course>>> GetStudentAvaliableCourses(int? id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var studentCourses = _context.StudentCourses.Where(sc => sc.StudentId == id).Select(sc => sc.Course).ToList();
            var studentGrade = student.Grade;
            var avaliableCourses = _context.Courses.Where(c => c.Grade == studentGrade && !studentCourses.Contains(c) && c.CurrentEnrollment < c.EnrollmentLimit).ToList();
            
            return avaliableCourses;
        }


        // POST: api/Students/5/register-course/1
        // For a given student, register him in a course
        [HttpPost("{studentId}/register-course/{courseId}")]
        public async Task<ActionResult<StudentCourses>> RegisterStudentInCourse(int? studentId, int? courseId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            course.CurrentEnrollment++;
            _context.Entry(course).State = EntityState.Modified;



            var studentCourse = new StudentCourses
            {
                StudentId = studentId,
                CourseId = courseId
            };

            _context.StudentCourses.Add(studentCourse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentCourses", new { id = studentId }, studentCourse);
        }



        [HttpPut("{studentId}/unregister-course/{courseId}")]
        public async Task<ActionResult<StudentCourses>> UnregisterStudentFromCourse(int? studentId, int? courseId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            course.CurrentEnrollment--;
            _context.Entry(course).State = EntityState.Modified;

            var studentCourse = await _context.StudentCourses.Where(sc => sc.StudentId == studentId && sc.CourseId == courseId).FirstOrDefaultAsync();
            if (studentCourse == null)
            {
                return NotFound();
            }

            _context.StudentCourses.Remove(studentCourse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentCourses", new { id = studentId }, studentCourse);
        }

        // update student score in a course
        [HttpPut("{studentId}/update-score/{courseId}")]
        public async Task<ActionResult<StudentCourses>> UpdateStudentScore(int? studentId, int? courseId, [FromBody] int score)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            var studentCourseToUpdate = await _context.StudentCourses.Where(sc => sc.StudentId == studentId && sc.CourseId == courseId).FirstOrDefaultAsync();
            if (studentCourseToUpdate == null)
            {
                return NotFound();
            }

            studentCourseToUpdate.Score = score;
            _context.Entry(studentCourseToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentCourses", new { id = studentId }, studentCourseToUpdate);
        }

        // get student score in a course
        [HttpGet("{studentId}/get-score/{courseId}")]
        public async Task<ActionResult<StudentCourses>> GetStudentScore(int? studentId, int? courseId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            var studentCourse = await _context.StudentCourses.Where(sc => sc.StudentId == studentId && sc.CourseId == courseId).FirstOrDefaultAsync();
            if (studentCourse == null)
            {
                return NotFound();
            }

            return studentCourse;
        }


        private bool StudentExists(int? id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}

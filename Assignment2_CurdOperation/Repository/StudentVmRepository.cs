using Assignment2_CurdOperation.Data;
using Assignment2_CurdOperation.Modals;
using Assignment2_CurdOperation.Repository.Interface;
using Assignment2_CurdOperation.ViewModals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using OfficeOpenXml;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.IO;

namespace Assignment2_CurdOperation.Repository
{
    public class StudentVmRepository : IStudentVmRepository
    {

        private readonly ApplicationDbContext _context;

        public StudentVmRepository(ApplicationDbContext context)
        {
            _context = context;
            
        }
        public async Task<IList<StudentViewModal>> GetStudentViewModals()
        {
            // return (Task<List<StudentViewModal>>)(IList<StudentViewModal>)result;
            //var students = await _context.Students.ToListAsync();
            // Student st = new Student();
            //var allstudent = await _context.Students.Include(nameof(StudentHobby)).Include(nameof(Hobby)).Include(x=>x.StudentHobby).Include(x=>x.StudentHobby)
            var data = (from c in _context.Students
                        join e in _context.StudentHobbies on c.Id equals e.StudentId
                        join t in _context.Hobbies on e.HobbyId equals t.Id
                        select new
                        {
                            id = c.Id,
                            name = c.Name,
                            address = c.Address,
                            age = c.Age,
                            email = c.Email,
                            salary = c.Salary,
                            bio = c.Bio,
                            Hobbies = t.Name
                        }).ToList();

            var result = (from Student in data
                          group Student by Student.id into grp
                          select new
                          {
                              Id = grp.Key,
                              Name = grp.First().name,
                              Address = grp.First().address,
                              Age = grp.First().age,
                              Email = grp.First().email,
                              Salary = grp.First().salary,
                              Bio = grp.First().bio,
                              hob = grp.Where(x => x.Hobbies != null).Select(x => x.Hobbies).ToArray()
                          }).Select(x => new StudentViewModal()
                          {
                              Id = x.Id,
                              Name = x.Name,
                              Address = x.Address,
                              Age = x.Age,
                              Email = x.Email,
                              Salary = x.Salary,
                              Bio = x.Bio,
                              hob = x.hob
                          }).ToList();


            return result;


        }
        public async Task<StudentViewModal> GetStudentById(int id)
        {
            var studentInDb = await _context.Students.Include(x => x.StudentHobby).Where(y => y.Id == id).FirstOrDefaultAsync();
            var selectId = studentInDb.StudentHobby.
                Select(x => x.HobbyId).ToList();
            var item = _context.Hobbies.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = selectId.Contains(x.Id)
            }).ToList();

            StudentViewModal vm = new StudentViewModal();
            vm.Id = studentInDb.Id;
            vm.Name = studentInDb.Name;
            vm.Address = studentInDb.Address;
            vm.Email = studentInDb.Email;
            vm.Age = studentInDb.Age;
            vm.Salary = studentInDb.Salary;
            vm.Bio = studentInDb.Bio;
            vm.hob = item.Select(x => x.Value).ToArray();
            if (vm == null) return null;
            return vm;

        }

        public async Task CreateStudentVM(StudentViewModal studentvm)
        {
            var student = new Student()
            {
                Name = studentvm.Name,
                Salary = studentvm.Salary,
                Bio = studentvm.Bio,
                Email = studentvm.Email,
                Address = studentvm.Address,
                Age = studentvm.Age
            };

            foreach (var item in studentvm.hob)
            {
                student.StudentHobby.Add(new StudentHobby()
                {
                    HobbyId = int.Parse(item)
                });
            }
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }


        public async Task<StudentViewModal> GetStudentDetails(int id)
        {
            var studentInDb = await _context.Students
               .Include(x => x.StudentHobby).ThenInclude(x => x.Hobby)
               .Where(y => y.Id == id).FirstOrDefaultAsync();
            if (studentInDb != null)
            {

                var selectId = studentInDb.StudentHobby.
                    Select(x => x.HobbyId).ToList();

                var item = _context.Hobbies.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = selectId.Contains(x.Id)
                }).Where(x => x.Selected == true).ToList();
                var item2 = item.Select(x => x.Value).ToList();
                StudentViewModal vm = new StudentViewModal();
                vm.Id = studentInDb.Id;
                vm.Name = studentInDb.Name;
                vm.Address = studentInDb.Address;
                vm.Email = studentInDb.Email;
                vm.Age = studentInDb.Age;
                vm.Salary = studentInDb.Salary;
                vm.Bio = studentInDb.Bio;

                vm.hob = item2.ToArray();
                return vm;
            }
            return null;
        }
        public async Task UpdateStudentVM(StudentViewModal studentvm)
        {
            var studentInDb = await _context.Students.FindAsync(studentvm.Id);
            studentInDb.Name = studentvm.Name;
            studentInDb.Address = studentvm.Address;
            studentInDb.Email = studentvm.Email;
            studentInDb.Age = studentvm.Age;
            studentInDb.Salary = studentvm.Salary;
            studentInDb.Bio = studentvm.Bio;
            studentInDb.Id = studentvm.Id;

            var studentById = await _context.Students.Include(x => x.StudentHobby).FirstOrDefaultAsync(x => x.Id == studentvm.Id);
            var exsistingHobbyId = studentById.StudentHobby.Select(x => x.HobbyId).ToList();

            var newSelectedHobbyId = studentvm.hob.Select(int.Parse).ToList();
            var toAdd = newSelectedHobbyId.Except(exsistingHobbyId);
            var toRemove = exsistingHobbyId.Except(newSelectedHobbyId);


            studentInDb.StudentHobby = studentInDb.StudentHobby.Where(x => !toRemove.Contains(x.HobbyId)).ToList();
            foreach (var item in toAdd)
            {
                studentInDb.StudentHobby.Add(new StudentHobby()
                {
                    HobbyId = item

                }); ;
            }
            _context.Students.Update(studentInDb);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteStudentVM(int id)
        {
            var studentInDb = await _context.Students.Include(x => x.StudentHobby).ThenInclude(x => x.Hobby)
                .Where(y => y.Id == id).FirstOrDefaultAsync();
            if (studentInDb != null)
            {
                _context.Students.Remove(studentInDb);
                await _context.SaveChangesAsync();
            }

        }

      
    }
}

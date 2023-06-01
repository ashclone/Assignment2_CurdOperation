using Assignment2_FrontEnd.Models;
using Assignment2_FrontEnd.Repository.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web.Helpers;

namespace Assignment2_FrontEnd.Controllers
{

    public class StudentVmController : Controller
    {
        private readonly IStudentVmRepo _vmRepo;
        private readonly IHobbiesRepo _hobbyRepo;
        private readonly T1Interface _t1obj;


        public StudentVmController(IStudentVmRepo vmRepo, IHobbiesRepo hobbyRepo, T1Interface t1obj)
        {
            _vmRepo = vmRepo;
            _hobbyRepo = hobbyRepo;
            _t1obj = t1obj;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DownloadExcel([FromBody] List<StudentVm> datastringfy)
        {

            try
            {

                var excel = datastringfy;
                //var excel = Json.Deserialize<StudentVm>(datastringfy);
                // var excel = await _vmRepo.GetAllAsync("https://localhost:7223/api/vm");


                //using (var workbook = new XLWorkbook())
                //{
                //    IXLWorksheet worksheet = workbook.Worksheets.Add("Students");
                //    worksheet.Cell(1, 1).Value = "Id";
                //    worksheet.Cell(1, 2).Value = "Name";
                //    worksheet.Cell(1, 3).Value = "Age";
                //    worksheet.Cell(1, 4).Value = "Address";
                //    worksheet.Cell(1, 5).Value = "Salary";
                //    worksheet.Cell(1, 6).Value = "Bio";
                //    worksheet.Cell(1, 7).Value = "Hobby";

                //    //IXLRange range = worksheet.Range(worksheet.Cell(1, 1).Address, worksheet.Cell(1, 4).Address);
                //    //range.Style.Fill.SetBackgroundColor(XLColor.Almond);

                //    int index = 1;

                //    foreach (var item in excel)
                //    {
                //        index++;

                //        worksheet.Cell(index, 1).Value = item.Id;
                //        worksheet.Cell(index, 2).Value = item.Name;
                //        worksheet.Cell(index, 3).Value = item.Age;
                //        worksheet.Cell(index, 4).Value = item.Address;
                //        worksheet.Cell(index, 5).Value = item.Salary;

                //        worksheet.Cell(index, 6).Value = Regex.Replace(item.Bio, @"<[^>]*>", "");
                //        string data = "";
                //        foreach (var hob in item.hob)
                //        {
                //            data += hob.ToString() + ", ";
                //        }
                //        worksheet.Cell(index, 7).Value = data;
                //    }

                //    using (var stream = new MemoryStream())
                //    {
                //        workbook.SaveAs(stream);
                //        var content = stream.ToArray();
                //        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                //        var strDate = Guid.NewGuid() + DateTime.Now.ToString("yyyyMMdd");
                //        string filename = string.Format($"Employees_{strDate}.xlsx");
                //        //Response.Headers.Add("Content-Disposition", $"attachment; filename={filename}");
                //        //return File(content, contentType, filename);
                //        //var file = File(content, contentType, filename);
                //        return Json(new { data = File(content, contentType, filename)});
                //    }
                // }
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }


        public async Task<JsonResult> GetAll()
        {
            return Json(new { data = await _vmRepo.GetAllAsync("https://localhost:7223/api/vm") });
        }
        public async Task<string?> GetToken()
        {
            try
            {
                var token = HttpContext.Session.GetString("Token");
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
                {
                    TempData["error"] = "Please Login first !!!!";
                    return null;
                }
                return token;
            }
            catch (Exception ex)
            {
                return ex.ToString();

            }

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                TempData["error"] = "Please Login first !!!!";
                return RedirectToAction("Login", "user");
            }

            if (await _vmRepo.DeleteAsync("https://localhost:7223/api/Vm/", id, token))
            {
                return Json(new { success = true, message = "data Deleted Successfully" });
            }
            return Json(new { success = false, message = "Data Not Found in Database!!" });
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                TempData["error"] = "Please Login first !!!!";
                return RedirectToAction("Login", "user");
            }

            StudentVm studentVm = new();
            var hobbies = await _hobbyRepo.GetAllAsync("https://localhost:7223/api/Hobby");
            if (id == null)
            {

                ViewBag.hobbies = hobbies.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),

                }).ToList();

                return View(studentVm);

            }
            studentVm = await _vmRepo.GetAsync("https://localhost:7223/api/Vm/Details/", id.GetValueOrDefault(), token);
            var selectedId = studentVm.hob.ToList();
            ViewBag.hobbies = hobbies.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = selectedId.Contains(x.Id.ToString())
            }).ToList();

            if (studentVm == null)
            {

                return NotFound();
            }
            return View(studentVm);

        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var obj= _t1obj.DeleteAsync();
            if (id != null)
            {
                var studentVm = await _vmRepo.GetAsync("https://localhost:7223/api/Vm/Details/", id);
                return View(studentVm);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Upsert(StudentVm studentVm)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                var token = HttpContext.Session.GetString("Token");

                if (ModelState.IsValid)
                {
                    if (studentVm.Id == 0)
                    {
                        if (await _vmRepo.CreateAsync("https://localhost:7223/api/Vm", studentVm, token))
                            TempData["success"] = "Employee Created Successfully";
                        else
                            TempData["error"] = "Error while Updating";
                    }
                    else
                    {
                        if (await _vmRepo.UpdateAsync("https://localhost:7223/api/Vm", studentVm, token))
                            TempData["success"] = "Employee Updated Successfully";
                        else
                            TempData["error"] = "Error while Updating";
                    }
                    return RedirectToAction("Index");
                }
                else
                    return View(studentVm);

            }
            else
            {
                TempData["error"] = "Please Login first !!!!";
                return RedirectToAction("Login", "user");
            }

        }




    }
}

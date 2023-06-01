using Assignment2_CurdOperation.Modals;
using Assignment2_CurdOperation.Repository.Interface;
using Assignment2_CurdOperation.ViewModals;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Interop;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Assignment2_CurdOperation.Controllers
{
    [Route("api/Vm")]
    [ApiController]
   
    public class VmController : ControllerBase
    {
        private readonly IStudentVmRepository _studentVmRepository;

        public VmController(IStudentVmRepository studentVmRepository)
        {
            _studentVmRepository = studentVmRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _studentVmRepository.GetStudentViewModals());

        }
        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> DownloadExcel()
        {
            try
            {

                var excel = await _studentVmRepository.GetStudentViewModals();

                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet = workbook.Worksheets.Add("Students");
                    worksheet.Cell(1, 1).Value = "Id";
                    worksheet.Cell(1, 2).Value = "Name";
                    worksheet.Cell(1, 3).Value = "Age";
                    worksheet.Cell(1, 4).Value = "Address";
                    worksheet.Cell(1, 5).Value = "Salary";
                    worksheet.Cell(1, 6).Value = "Bio";
                    worksheet.Cell(1, 7).Value = "Hobby";

                    //IXLRange range = worksheet.Range(worksheet.Cell(1, 1).Address, worksheet.Cell(1, 4).Address);
                    //range.Style.Fill.SetBackgroundColor(XLColor.Almond);

                    int index = 1;

                    foreach (var item in excel)
                    {
                        index++;

                        worksheet.Cell(index, 1).Value = item.Id;
                        worksheet.Cell(index, 2).Value = item.Name;
                        worksheet.Cell(index, 3).Value = item.Age;
                        worksheet.Cell(index, 4).Value = item.Address;
                        worksheet.Cell(index, 5).Value = item.Salary;
                        
                        worksheet.Cell(index, 6).Value = Regex.Replace(item.Bio, @"<[^>]*>", "");
                        string data ="";
                        foreach (var hob in item.hob)
                        {
                           data+= hob.ToString() + ", ";
                        }
                            worksheet.Cell(index, 7).Value = data  ;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                        var strDate =Guid.NewGuid()+DateTime.Now.ToString("yyyyMMdd");
                        string filename = string.Format($"Employees_{strDate}.xlsx");                        

                        return File(content, contentType, filename);
                    }
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpGet("{id}")]
        [Authorize]

        public async Task<IActionResult> Find(int id)
        {
            return Ok(await _studentVmRepository.GetStudentById(id));
        }

        [HttpGet("Details/{id}")]
        [Authorize]        
        public async Task<IActionResult> Details(int id)
        {
            return Ok( await _studentVmRepository.GetStudentDetails(id));
        }
        [HttpPost]
        //[Authorize]
        public async Task Post([FromBody] StudentViewModal studentvm)
        {
            await _studentVmRepository.CreateStudentVM(studentvm);

        }

        [HttpPut]
        [Authorize]
        public async Task Put([FromBody] StudentViewModal studentvm)
        {
            await _studentVmRepository.UpdateStudentVM(studentvm);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task Delete(int id)
        {
            await _studentVmRepository.DeleteStudentVM(id);
        }

       
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EasyUnitOfWork.Uow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestConnection.Models;
using TestConnection.Repositories;

namespace TestConnection.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IBookRepository _bookRepository;

        public HomeController(IUnitOfWorkManager unitOfWorkManager, IBookRepository bookRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _bookRepository = bookRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Test1()
        {
            await _bookRepository.InsertAsync(new Entitys.Book()
            {
                BookName = "测试1"
            });

            return View();
        }

        public async Task<IActionResult> Test2()
        {
            var result = await _bookRepository.InsertAndGetIdAsync(new Entitys.Book()
            {
                BookName = "测试1"
            });

            return View();
        }

        public async Task<IActionResult> Test3()
        {
            await _bookRepository.InsertAndGetIdAsync(new Entitys.Book()
            {
                BookName = "测试1"
            });
            throw new Exception("测试工作单元保存");
            return View();
        }

        public IActionResult Test4()
        {
            return View();
        }

        public IActionResult Test5()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

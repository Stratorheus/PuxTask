﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PuxTask.Abstract;
using PuxTask.WebClient.Models;

namespace PuxTask.WebClient.Controllers
{
    public class AnalysisController : Controller
    {
        private readonly IReportService _reportService;
        private readonly ILogger<AnalysisController> _logger;

        public AnalysisController(IReportService reportService, ILogger<AnalysisController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }
        public ActionResult Index()
        {
            return View(new AnalysisViewModel());
        }
        [HttpPost]
        public ActionResult Index(AnalysisViewModel vm) 
        {
            vm.ErrorMessage = string.Empty;
            var startTime = DateTime.Now;
            _logger.LogInformation("Analysis started. Time: "+startTime.ToShortTimeString());
            try
            {
                vm.Report = _reportService.GetReport(vm.analysedFolderPath);
                _logger.LogInformation($"Analysis finished. " +
                    $"Time: {DateTime.Now.ToShortTimeString()} " +
                    $"Duration: {(DateTime.Now - startTime).Milliseconds} ms " +
                    $"Files checked: {vm.Report.FileReports.Count}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"There was an error: {ex.Message}. \nStack trace: \n{ex.StackTrace}");
                vm.ErrorMessage = ex.Message;
            }
            return View(vm); 
        }
    }
}

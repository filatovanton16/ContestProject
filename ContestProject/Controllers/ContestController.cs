using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using ContestProject.Services;
using System.Text.RegularExpressions;

namespace ContestProject.Controllers
{
    [ApiController]
    [Route("api/contest")]
    public class ContestController : Controller
    {
        IDataService dataService;
        private IJDoodleService _JDoodleService;

        public ContestController(IDataService service, IJDoodleService jDoodleService)
        {
            dataService = service;
            _JDoodleService = jDoodleService;

            dataService.Initialize();
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return dataService.GetAllTasks().ToList();
        }

        [HttpGet("{taskName}")]
        public JsonResult Get(string taskName)
        {
            ContestTask contestTask = dataService.GetContestTask(taskName);
            return Json(contestTask.Description);
        }

        [HttpPost]
        public async Task<JsonResult> Post(UserTaskCode userTaskCode)
        {
            Tuple<bool, string> result = new Tuple<bool, string>(false, "");
            try
            {
                ContestTask contestTask = dataService.GetContestTask(userTaskCode.TaskName);
                dynamic response = await _JDoodleService.TryCompilateAsync(userTaskCode.Code, contestTask.InputParameter);
                if (response.statusCode == "200")
                {
                    result = TryCheckAnswer(response, contestTask.OutputParameter);
                    if (result.Item1)
                    {
                        dataService.SaveUserTask(userTaskCode, contestTask);
                    }
                }
            }
            catch
            {
                result = new Tuple<bool, string> (false, "Something went wrong. Please try again.");
            }
            return Json(result.Item2);
        }

        public Tuple<bool, string> TryCheckAnswer(dynamic response, int output)
        {
            if (response.statusCode == "200")
            {
                // Check if response has any letters
                if (!Regex.IsMatch(response.output.ToString(), "^[^x]+$"))
                {

                    return new Tuple<bool, string>(false, "There is an error in your code:\n " + response.output);
                }
                else
                {
                    //If not then comparing to the right answer
                    return Convert.ToInt32(response.output) == output ? 
                        new Tuple<bool, string>(true, "Success! Memory: " + response.memory + " CPU: " + response.cpuTime) :
                        new Tuple<bool, string>(false, "Wrong:(");
                }
            }
            else
            {
                return new Tuple<bool, string>(false, "Error: " + response.error);
            }
        }

    }
}
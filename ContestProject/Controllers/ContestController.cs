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
        DataService dataService;
        private IJDoodleService _JDoodleService;

        public ContestController(DataService service, IJDoodleService jDoodleService)
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
        public string Get(string taskName)
        {
            ContestTask contestTask = dataService.GetContestTask(taskName);
            return JsonConvert.SerializeObject(contestTask.Description);
        }

        [HttpPost]
        public async Task<string> Post(UserTaskCode userTaskCode)
        {
            string result;
            try
            {
                ContestTask contestTask = dataService.GetContestTask(userTaskCode.TaskName);
                dynamic response = await _JDoodleService.TryCompilate(userTaskCode.Code, contestTask.InputParameter);
                if (response.statusCode == "200") dataService.SaveUserTask(userTaskCode, contestTask);

                result = TryCheckAnswer(response, contestTask.OutputParameter);
            }
            catch
            {
                result = "Something went wrong. Please try again.";
            }
            return JsonConvert.SerializeObject(result);
        }

        public string TryCheckAnswer(dynamic response, int output)
        {
            if (response.statusCode == "200")
            {
                if (!Regex.IsMatch(response.output.ToString(), "^[^x]+$"))
                {

                    return "Error: " + response.output;
                }
                else
                {
                    return Convert.ToInt32(response.output) == output ?
                                            "Success! Memory: " + response.memory + " CPU: " + response.cpuTime :
                                            "Wrong:(";
                }
            }
            else
            {
                return "Error: " + response.error;
            }
        }

    }
}
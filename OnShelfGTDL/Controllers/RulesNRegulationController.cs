using Microsoft.AspNetCore.Mvc;
using OnShelfGTDL.Models;

namespace OnShelfGTDL.Controllers
{
    public class RulesNRegulationController : Controller
    {
        private readonly DatabaseHelper _dbHelper;
        public RulesNRegulationController(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public IActionResult Index()
        {
            //var rules = _dbHelper.GetAllRules();
            ViewData["ModuleName"] = "Rules and Regulations";
            return View();
        }
        [HttpGet]
        public IActionResult GetRules()
        {
            var rules = _dbHelper.GetAllRules();
            return Json(rules);
        }
        [HttpPost]
        public IActionResult AddRule([FromForm] RuleModel model)
        {
            if (string.IsNullOrWhiteSpace(model.RuleTitle))
                return BadRequest("Rule title is required.");

            _dbHelper.AddRule(model); // implement this method to insert into database
            return Ok();
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            bool result = _dbHelper.DeleteRule(id); // use your helper class here
            if (result)
                return RedirectToAction("Index"); // or wherever you want to go after delete
            else
                return BadRequest("Failed to delete rule.");
        }
        [HttpGet]
        public IActionResult GetRuleById(int id)
        {
            var rule = _dbHelper.GetRuleById(id); // or from EF context
            if (rule == null)
                return NotFound();

            return Json(rule);
        }
        [HttpPost]
        public IActionResult UpdateRule(RuleModel model)
        {
            if (ModelState.IsValid)
            {
                _dbHelper.UpdateRule(model);
                return Ok();
            }
            return BadRequest("Invalid data.");
        }

        public IActionResult Policy()
        {
            ViewData["ModuleName"] = "Rules and Regulations";
            var rules = _dbHelper.GetAllRulesNRegulation();
            return View(rules);
        }

    }
}

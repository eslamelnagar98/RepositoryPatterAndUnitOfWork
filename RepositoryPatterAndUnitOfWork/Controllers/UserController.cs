using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryPatterAndUnitOfWork.Core.IConfiguration;
using RepositoryPatterAndUnitOfWork.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryPatterAndUnitOfWork.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase,IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserController> _logger;

        public UserController(IUnitOfWork unitOfWork, ILogger<UserController> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
            => Ok(await _unitOfWork.UserRepository.GetAll(filter: q => EF.Functions.Like(q.LastName,"%s%"),
                                                          orderBy: q => q.OrderBy(s => s.LastName)));

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                user.Id = Guid.NewGuid();
                await _unitOfWork.UserRepository.Add(user);
                await _unitOfWork.CompleteAsync();
                return CreatedAtAction("GetItem", new { user.Id }, user);
            }

            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        [HttpGet("{id}", Name = "GetItem")]
        public async Task<IActionResult> GetItem(Guid id)
        {
            var user = await _unitOfWork.UserRepository.GetByID(id);
            if (user == null)
                return NotFound(); // 404 http status code 

            return Ok(user);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, User user)
        {
            if (id != user.Id)
                return BadRequest();

            await _unitOfWork.UserRepository.Upsert(user);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {

            var deletedUser = await _unitOfWork.UserRepository.Delete(id);
            if (!deletedUser) return BadRequest();
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        public void Dispose()
            => _unitOfWork.Dispose();

    }
}

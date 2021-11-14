using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryPatterAndUnitOfWork.Core.IConfiguration;
using RepositoryPatterAndUnitOfWork.Domain.Entities;
using RepositoryPatterAndUnitOfWork.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryPatterAndUnitOfWork.Controllers
{
    public class UserController : BaseApiController,IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork unitOfWork, ILogger<UserController> logger,IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); ;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
           var usersFromRepo= await _unitOfWork.UserRepository.GetAll(filter: q => EF.Functions.Like(q.LastName, "%s%"),
                                                          orderBy: q => q.OrderBy(s => s.LastName));
            if (usersFromRepo is null) return BadRequest();
            return Ok(_mapper.Map<UserDto>(usersFromRepo));
        }
       

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                user.Id = Guid.NewGuid();
                await _unitOfWork.UserRepository.Add(user);
                await _unitOfWork.CompleteAsync();
                var userToReturn=_mapper.Map<UserDto>(user);
                return CreatedAtAction("GetUser", new {id=user.Id }, userToReturn);
            }

            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var userFromRepo = await _unitOfWork.UserRepository.GetByID(id);
            if (userFromRepo == null)
                return NotFound(); // 404 http status code 

            return Ok(_mapper.Map<UserDto>(userFromRepo));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, UserDto user)
        {
            if (user is null) return BadRequest();
            var userFromRepo = await _unitOfWork.UserRepository.GetByID(id);
            if (userFromRepo is null) return NotFound();
            _mapper.Map(user, userFromRepo);
            //await _unitOfWork.UserRepository.Upsert(user);
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

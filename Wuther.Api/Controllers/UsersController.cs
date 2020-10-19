using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Wuther.Api.ActionConstraints;
using Wuther.Bussiness.Interface;
using Wuther.Bussiness.Service;
using Wuther.Entities.Models;
using Wuther.Util.DtoParameters;
using Wuther.Util.Enums;
using Wuther.Util.Models;
using Wuther.Util.PropertyMapping;

namespace Wuther.Api.Controllers
{
    [ApiController]
    [Route("api/Users")]
    [ResponseCache(CacheProfileName = "120sCacheProfile")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _propertyCheckerService;

        public UsersController(IUserRepository userRepository,
            IMapper mapper,
            IPropertyMappingService propertyMappingService,
            IPropertyCheckerService propertyCheckerService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _propertyMappingService = propertyMappingService;
            _propertyCheckerService = propertyCheckerService;
        }

        [Produces("application/json",
            "application/vnd.wuther.hateoas+json",
            "application/vnd.wuther.users.friendly+json",
            "application/vnd.wuther.users.friendly.hateoas+json",
            "application/vnd.wuther.users.full+json",
            "application/vnd.wuther.users.full.hateoas+json")]
        [HttpGet("{userId}", Name = nameof(GetUser))]
        public async Task<IActionResult> GetUser(int userId, string fields, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }
            if (!_propertyCheckerService.TypeHasProperties<UserDto>(fields))
            {
                return BadRequest();
            }
            var user = await _userRepository.GetUserAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            
            var includeLinks = parsedMediaType.SubTypeWithoutSuffix.
                EndsWith("hateoas", System.StringComparison.InvariantCultureIgnoreCase);
            IEnumerable<LinkDto> myLinks = new List<LinkDto>();
            if (includeLinks)
            {
                myLinks = CreateLinksForUsers(userId, fields);
            }

            var primaryMediaType = includeLinks
                ? parsedMediaType.SubTypeWithoutSuffix.Substring(0, parsedMediaType.SubTypeWithoutSuffix.Length - 8)
                : parsedMediaType.SubTypeWithoutSuffix;
            if(primaryMediaType == "vnd.wuther.users.full")
            {
                var full = _mapper.Map<Users, UserFullDto>(user).ShapeData(fields) as IDictionary<string, object>;
                if (includeLinks)
                {
                    full.Add("links", myLinks);
                }
                return Ok(full);
            }

            var friendly = _mapper.Map<UserDto>(user).ShapeData(fields) as IDictionary<string, object>;

            if (includeLinks)
            {
                friendly.Add("links", myLinks);
            }

            return Ok(friendly);
        }

        [HttpGet(Name = nameof(GetUsers))]
        [ResponseCache(Duration =60)]
        public async Task<IActionResult> GetUsers([FromQuery] DtoUserParameter parameter)
        {
            if (!_propertyCheckerService.TypeHasProperties<UserDto>(parameter.Fields))
            {
                return BadRequest();
            }
            if (!_propertyMappingService.ValidMappingExistsFor<UserDto, Users>(parameter.OrderBy))
            {
                return BadRequest();
            }
            var users = await _userRepository.GetUsersAsync(parameter);
            var paginationMetadata = new
            {
                totalCount = users.TotalCount,
                pageSize = users.PageSize,
                currentPage = users.CurrentPage,
                totalPages = users.TotalPages,
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));
            var userDtos = _mapper.Map<IList<UserDto>>(users);
            var shapedData = userDtos.shapeData(parameter.Fields);
            var links = CreateLinksForUsers(parameter, users.HasPrevious, users.HasNext);

            var shapedUsersWithLinks = shapedData.Select(c =>
            {
                var userDict = c as IDictionary<string, object>;
                var userLinks = CreateLinksForUsers((int)userDict["Id"], null);
                userDict.Add("links", userLinks);
                return userDict;
            });

            var linkedCollectionResource = new
            {
                value = shapedUsersWithLinks,
                links
            };

            return Ok(linkedCollectionResource);
        }

        [HttpPost(Name = nameof(CreateUserWithWrittenOffTime))]
        [RequestHeaderMatchesMediaType("Content-Type", "application/vnd.wuther.userforcreationwithwrittenofftime+json")]
        [Consumes("application/vnd.wuther.userforcreationwithwrittenofftime+json")]
        public async Task<ActionResult<Users>> CreateUserWithWrittenOffTime(UserAddWithWrittenOffTimeDto user)
        {
            var entity = _mapper.Map<Users>(user);
            var userAdd = await _userRepository.InsertAsync(entity);
            var returnDto = _mapper.Map<UserDto>(userAdd);
            var links = CreateLinksForUsers(returnDto.Id, null);
            var linkedDict = returnDto.ShapeData(null) as IDictionary<string, object>;
            linkedDict.Add("links", links);
            return CreatedAtRoute(nameof(GetUser), new { userId = linkedDict["Id"] }, linkedDict);
        }

        [HttpPost(Name = nameof(CreateUser))]
        [RequestHeaderMatchesMediaType("Content-Type", "application/json","application/vnd.wuther.userforcreation+json")]
        [Consumes("application/json", "application/vnd.wuther.userforcreation+json")]
        public async Task<ActionResult<Users>> CreateUser(UserAddDto user)
        {
            var entity = _mapper.Map<Users>(user);
            var userAdd = await _userRepository.InsertAsync(entity);
            var returnDto = _mapper.Map<UserDto>(userAdd);
            var links = CreateLinksForUsers(returnDto.Id, null);
            var linkedDict = returnDto.ShapeData(null) as IDictionary<string, object>;
            linkedDict.Add("links", links);
            return CreatedAtRoute(nameof(GetUser), new { userId = linkedDict["Id"] }, linkedDict);
        }

        

        [HttpPut("{userId}", Name = nameof(UpdateUser))]
        public async Task<ActionResult> UpdateUser(int userId, UserUpdateDto user)
        {
            var userEntity = await _userRepository.FindAsync(userId);
            if (userEntity == null)
            {
                return NotFound();
            }
            _mapper.Map(user, userEntity);

            await _userRepository.UpdateAsync(userEntity);
            return NoContent();
        }

        [HttpPatch("{userId}", Name = nameof(PartialUpdateUser))]
        public async Task<ActionResult> PartialUpdateUser(int userId, JsonPatchDocument<UserUpdateDto> patchDocument)
        {
            var userEntity = await _userRepository.FindAsync(userId);
            if (userEntity == null)
            {
                var userDto = new UserUpdateDto();
                patchDocument.ApplyTo(userDto, ModelState);
                if (!TryValidateModel(userDto))
                {
                    return ValidationProblem(ModelState);
                }
                var userToAdd = _mapper.Map<Users>(userDto);
                userToAdd.Id = userId;
                await _userRepository.InsertAsync(userToAdd);

                var toReturnDto = _mapper.Map<UserDto>(userToAdd);
                return CreatedAtRoute(nameof(GetUser), new { userId = userToAdd.Id }, toReturnDto);
            }
            var dtoToPatch = _mapper.Map<UserUpdateDto>(userEntity);
            patchDocument.ApplyTo(dtoToPatch, ModelState);
            if (!TryValidateModel(dtoToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(dtoToPatch, userEntity);
            await _userRepository.UpdateAsync(userEntity);
            return NoContent();
        }

        [HttpDelete("{userId}", Name = nameof(DeleteUser))]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            var userEntity = await _userRepository.FindAsync(userId);
            if (userEntity == null)
            {
                return NotFound();
            }
            await _userRepository.Delete(userEntity);
            return NoContent();
        }

        /// <summary>
        /// 将ValidationProblem返回 替换为startup里面配置的422状态码
        /// </summary>
        /// <param name="modelStateDictionary"></param>
        /// <returns></returns>
        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
            //return base.ValidationProblem(modelStateDictionary);
        }

        private string CreateUsersResourceUri(DtoUserParameter parameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetUsers), new
                    {
                        fields = parameters.Fields,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        username = parameters.Username,
                        account = parameters.Account,
                        orderBy = parameters.OrderBy
                    });

                case ResourceUriType.NextPage:

                    return Url.Link(nameof(GetUsers), new
                    {
                        fields = parameters.Fields,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        username = parameters.Username,
                        account = parameters.Account,
                        orderBy = parameters.OrderBy
                    });

                case ResourceUriType.CurrentPage:
                default:
                    return Url.Link(nameof(GetUsers), new
                    {
                        fields = parameters.Fields,
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        username = parameters.Username,
                        account = parameters.Account,
                        orderBy = parameters.OrderBy
                    });
            }
        }


        private IEnumerable<LinkDto> CreateLinksForUsers(int userId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(new LinkDto(Url.Link(nameof(GetUser), new { userId }),
                    "self",
                    "GET"));
            }
            else
            {
                links.Add(new LinkDto(Url.Link(nameof(GetUser), new { userId, fields }),
                    "self",
                    "GET"));
            }
            links.Add(new LinkDto(Url.Link(nameof(DeleteUser), new { userId }), "delete user", "DELETE"));

            links.Add(new LinkDto(Url.Link(nameof(CreateUser), new { }), "create user", "POST"));

            links.Add(new LinkDto(Url.Link(nameof(GetUsers), new { }), "get users", "GET"));
            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForUsers(DtoUserParameter parameters, bool hasPrevious, bool hasNext)
        {
            var links = new List<LinkDto>();


            links.Add(new LinkDto(CreateUsersResourceUri(parameters, ResourceUriType.CurrentPage),
                "self", "GET"));

            if (hasPrevious)
            {
                links.Add(new LinkDto(CreateUsersResourceUri(parameters, ResourceUriType.PreviousPage),
                    "previous_page", "GET"));
            }

            if (hasNext)
            {
                links.Add(new LinkDto(CreateUsersResourceUri(parameters, ResourceUriType.NextPage),
                    "next_page", "GET"));
            }

            return links;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wuther.Bussiness.Interface;
using Wuther.Util.PropertyMapping;

namespace Wuther.Api.Controllers
{
    [Route("api/[controller]")]
    public class MenusController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IMenuRepository _menuRepository;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _propertyCheckerService;

        public MenusController(IMapper mapper,
            IMenuRepository menuRepository,
            IPropertyMappingService propertyMappingService,
            IPropertyCheckerService propertyCheckerService)
        {
            _mapper = mapper;
            _menuRepository = menuRepository;
            _propertyMappingService = propertyMappingService;
            _propertyCheckerService = propertyCheckerService;
        }

        [Produces("application/json",
            "application/vnd.wuther.hateoas+json",
            "application/vnd.wuther.menus.friendly+json",
            "application/vnd.wuther.menus.friendly.hateoas+json",
            "application/vnd.wuther.menus.full+json",
            "application/vnd.wuther.menus.full.hateoas+json")]
        [HttpGet("{userId}", Name = nameof(GetMenu))]
        public async Task<IActionResult> GetMenu(int menuId, string fields, [FromHeader(Name = "Accept")] string mediaType)
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
            if (primaryMediaType == "vnd.wuther.users.full")
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
    }
}

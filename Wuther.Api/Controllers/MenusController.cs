using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Wuther.Api.ActionConstraints;
using Wuther.Bussiness.Interface;
using Wuther.Entities.Models;
using Wuther.Util.DtoParameters;
using Wuther.Util.Enums;
using Wuther.Util.Helper;
using Wuther.Util.Models;
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
        [HttpGet("{menuId}", Name = nameof(GetMenu))]
        public async Task<IActionResult> GetMenu(int menuId, string fields, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }
            if (!_propertyCheckerService.TypeHasProperties<MenuDto>(fields))
            {
                return BadRequest();
            }
            var menu = await _menuRepository.GetMenuAsync(menuId);
            if (menu == null)
            {
                return NotFound();
            }

            var includeLinks = parsedMediaType.SubTypeWithoutSuffix.
                EndsWith("hateoas", System.StringComparison.InvariantCultureIgnoreCase);
            IEnumerable<LinkDto> myLinks = new List<LinkDto>();
            if (includeLinks)
            {
                myLinks = CreateLinksForMenus(menuId, fields);
            }

            var primaryMediaType = includeLinks
                ? parsedMediaType.SubTypeWithoutSuffix.Substring(0, parsedMediaType.SubTypeWithoutSuffix.Length - 8)
                : parsedMediaType.SubTypeWithoutSuffix;
            if (primaryMediaType == "vnd.wuther.users.full")
            {
                var full = _mapper.Map<Menus, MenuFullDto>(menu).ShapeData(fields) as IDictionary<string, object>;
                if (includeLinks)
                {
                    full.Add("links", myLinks);
                }
                return Ok(full);
            }

            var friendly = _mapper.Map<MenuDto>(menu).ShapeData(fields) as IDictionary<string, object>;

            if (includeLinks)
            {
                friendly.Add("links", myLinks);
            }

            return Ok(friendly);
        }

        [HttpGet(Name = nameof(GetMenus))]
        public async Task<IActionResult> GetMenus([FromQuery] DtoMenuParameter parameter)
        {
            if (!_propertyCheckerService.TypeHasProperties<MenuDto>(parameter.Fields))
            {
                return BadRequest();
            }
            if (!_propertyMappingService.ValidMappingExistsFor<MenuDto, Menus>(parameter.OrderBy))
            {
                return BadRequest();
            }
            var menus = await _menuRepository.GetMenusAsync(parameter);
            var paginationMetadata = new
            {
                totalCount = menus.TotalCount,
                pageSize = menus.PageSize,
                currentPage = menus.CurrentPage,
                totalPages = menus.TotalPages,
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));
            var menuDtos = _mapper.Map<IList<MenuDto>>(menus);
            var menuGenerates = menuDtos.GenerateTree(c => c.Id, u => u.ParentId);

            var shapedData = menuGenerates.shapeData(parameter.Fields);
            var links = CreateLinksForMenus(parameter, menus.HasPrevious, menus.HasNext);

            //var shapedMenusWithLinks = shapedData.Select(c =>
            //{
            //    var menuDict = c as IDictionary<string, object>;
            //    var menusLinks = CreateLinksForMenus((int)menuDict["Id"], null);
            //    menuDict.Add("links", menusLinks);
            //    return menuDict;
            //});

            var linkedCollectionResource = new
            {
                value = shapedData,
                links
            };

            return Ok(linkedCollectionResource);
        }

        [HttpPost(Name = nameof(CreateMenu))]
        [RequestHeaderMatchesMediaType("Content-Type", "application/json", "application/vnd.wuther.menuforcreation+json")]
        [Consumes("application/json", "application/vnd.wuther.menuforcreation+json")]
        public async Task<ActionResult<Menus>> CreateMenu(MenuAddDto menu)
        {
            var entity = _mapper.Map<Menus>(menu);
            var userAdd = await _menuRepository.InsertAsync(entity);
            var returnDto = _mapper.Map<MenuDto>(userAdd);
            var links = CreateLinksForMenus(returnDto.Id, null);
            var linkedDict = returnDto.ShapeData(null) as IDictionary<string, object>;
            linkedDict.Add("links", links);
            return CreatedAtRoute(nameof(GetMenu), new { menuId = linkedDict["Id"] }, linkedDict);
        }



        [HttpPut("{menuId}", Name = nameof(UpdateMenu))]
        public async Task<ActionResult> UpdateMenu(int menuId, MenuUpdateDto menu)
        {
            var menuEntity = await _menuRepository.FindAsync(menuId);
            if (menuEntity == null)
            {
                return NotFound();
            }
            _mapper.Map(menu, menuEntity);

            await _menuRepository.UpdateAsync(menuEntity);
            return NoContent();
        }

        private string CreateUsersResourceUri(DtoMenuParameter parameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetMenus), new
                    {
                        fields = parameters.Fields,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        name = parameters.Name,
                        position = parameters.Position,
                        orderBy = parameters.OrderBy
                    });

                case ResourceUriType.NextPage:

                    return Url.Link(nameof(GetMenus), new
                    {
                        fields = parameters.Fields,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        name = parameters.Name,
                        position = parameters.Position,
                        orderBy = parameters.OrderBy
                    });

                case ResourceUriType.CurrentPage:
                default:
                    return Url.Link(nameof(GetMenus), new
                    {
                        fields = parameters.Fields,
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        name = parameters.Name,
                        position = parameters.Position,
                        orderBy = parameters.OrderBy
                    });
            }
        }

        private IEnumerable<LinkDto> CreateLinksForMenus(int menuId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(new LinkDto(Url.Link(nameof(GetMenu), new { menuId }),
                    "self",
                    "GET"));
            }
            else
            {
                links.Add(new LinkDto(Url.Link(nameof(GetMenu), new { menuId, fields }),
                    "self",
                    "GET"));
            }
            //links.Add(new LinkDto(Url.Link(nameof(DeleteMenu), new { userId }), "delete menu", "DELETE"));

            //links.Add(new LinkDto(Url.Link(nameof(CreateMenu), new { }), "create menu", "POST"));

            links.Add(new LinkDto(Url.Link(nameof(GetMenus), new { }), "get menus", "GET"));
            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForMenus(DtoMenuParameter parameters, bool hasPrevious, bool hasNext)
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

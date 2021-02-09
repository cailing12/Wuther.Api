using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
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
    public class BlogsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IBlogsRepository _blogsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _propertyCheckerService;

        public BlogsController(IMapper mapper,
            IBlogsRepository blogsRepository,
            IUserRepository userRepository,
            IMenuRepository menuRepository,
            IPropertyMappingService propertyMappingService,
            IPropertyCheckerService propertyCheckerService)
        {
            _mapper = mapper;
            _blogsRepository = blogsRepository;
            _userRepository = userRepository;
            _menuRepository = menuRepository;
            _propertyMappingService = propertyMappingService;
            _propertyCheckerService = propertyCheckerService;
        }
        [Produces("application/json",
            "application/vnd.wuther.hateoas+json",
            "application/vnd.wuther.blogs.friendly+json",
            "application/vnd.wuther.blogs.friendly.hateoas+json",
            "application/vnd.wuther.blogs.full+json",
            "application/vnd.wuther.blogs.full.hateoas+json")]
        [HttpGet("{blogId}", Name = nameof(GetBlog))]
        public async Task<IActionResult> GetBlog(int blogId, string fields, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }
            if (!_propertyCheckerService.TypeHasProperties<BlogDto>(fields))
            {
                return BadRequest();
            }
            

            var includeLinks = parsedMediaType.SubTypeWithoutSuffix.
                EndsWith("hateoas", System.StringComparison.InvariantCultureIgnoreCase);
            IEnumerable<LinkDto> myLinks = new List<LinkDto>();
            if (includeLinks)
            {
                myLinks = CreateLinksForBlogs(blogId, fields);
            }

            var primaryMediaType = includeLinks
                ? parsedMediaType.SubTypeWithoutSuffix.Substring(0, parsedMediaType.SubTypeWithoutSuffix.Length - 8)
                : parsedMediaType.SubTypeWithoutSuffix;
            if (primaryMediaType == "vnd.wuther.blogs.full")
            {
                var blog = await _blogsRepository.GetBlogAsync(blogId);
                if (blog == null)
                {
                    return NotFound();
                }
                var full = _mapper.Map<Blogs, BlogFullDto>(blog);
                var fullDic = full.ShapeData(fields) as IDictionary<string, object>;
                if (includeLinks)
                {
                    fullDic.Add("links", myLinks);
                }
                return Ok(full);
            }
            else
            {
                var blog = await _blogsRepository.FindAsync(blogId);
                if (blog == null)
                {
                    return NotFound();
                }
                var friendly = _mapper.Map<BlogDto>(blog).ShapeData(fields) as IDictionary<string, object>;

                if (includeLinks)
                {
                    friendly.Add("links", myLinks);
                }

                return Ok(friendly);
            }
        }

        [Produces("application/json",
            "application/vnd.wuther.hateoas+json",
            "application/vnd.wuther.blogs.friendly+json",
            "application/vnd.wuther.blogs.friendly.hateoas+json",
            "application/vnd.wuther.blogs.full+json",
            "application/vnd.wuther.blogs.full.hateoas+json")]
        [HttpGet(Name = nameof(GetBlogs))]
        public async Task<IActionResult> GetBlogs([FromQuery] DtoBlogParameter parameter, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }
            if (!_propertyCheckerService.TypeHasProperties<BlogDto>(parameter.Fields))
            {
                return BadRequest();
            }
            if (!_propertyMappingService.ValidMappingExistsFor<BlogDto, Blogs>(parameter.OrderBy))
            {
                return BadRequest();
            }
            var includeLinks = parsedMediaType.SubTypeWithoutSuffix.
                EndsWith("hateoas", System.StringComparison.InvariantCultureIgnoreCase);
            IEnumerable<LinkDto> myLinks = new List<LinkDto>();
            var primaryMediaType = includeLinks
                ? parsedMediaType.SubTypeWithoutSuffix.Substring(0, parsedMediaType.SubTypeWithoutSuffix.Length - 8)
                : parsedMediaType.SubTypeWithoutSuffix;


            if (primaryMediaType == "vnd.wuther.blogs.full")
            {
                var fullBlogs = await _blogsRepository.GetBlogsAsync(parameter, true);
                var fullPaginationMetadata = new
                {
                    totalCount = fullBlogs.TotalCount,
                    pageSize = fullBlogs.PageSize,
                    currentPage = fullBlogs.CurrentPage,
                    totalPages = fullBlogs.TotalPages,
                };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(fullPaginationMetadata, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }));
                var blogFullDtos = _mapper.Map<IList<BlogFullDto>>(fullBlogs);
                var shapedFullData = blogFullDtos.shapeData(parameter.Fields);
                var fullLinks = CreateLinksForMenus(parameter, fullBlogs.HasPrevious, fullBlogs.HasNext);
                var fullLinkedCollectionResource = new
                {
                    value = shapedFullData,
                    fullLinks
                };

                return Ok(fullLinkedCollectionResource);

            }
            var blogs = await _blogsRepository.GetBlogsAsync(parameter, false);
            var paginationMetadata = new
            {
                totalCount = blogs.TotalCount,
                pageSize = blogs.PageSize,
                currentPage = blogs.CurrentPage,
                totalPages = blogs.TotalPages,
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));

            var blogDtos = _mapper.Map<IList<BlogDto>>(blogs);
            var shapedData = blogDtos.shapeData(parameter.Fields);
            var links = CreateLinksForMenus(parameter, blogs.HasPrevious, blogs.HasNext);

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

        [HttpPost(Name = nameof(CreateBlog))]
        [RequestHeaderMatchesMediaType("Content-Type", "application/json", "application/vnd.wuther.blogforcreation+json")]
        [Consumes("application/json", "application/vnd.wuther.blogforcreation+json")]
        public async Task<ActionResult<Blogs>> CreateBlog(BlogAddDto blog)
        {
            //创建html文件
            //1.设置文件名
            var fileName = DateTimeHelper.GetTimestamp(DateTime.Now);
            //创建文件
            var htmlHead = System.IO.File.ReadAllText("../Wuther.Api/Resource/bloghead.txt");
            var htmlfoot = System.IO.File.ReadAllText("../Wuther.Api/Resource/blogfoot.txt");
            var htmlContent = $"{htmlHead}{blog.Content}{htmlfoot}";

            var buffer = Encoding.UTF8.GetBytes(htmlContent);
            FileHeper.CreateFile($"D:\\Project3\\wuther.ui\\src\\admin\\p\\{fileName}.html", buffer);

            var entity = _mapper.Map<Blogs>(blog);
            entity.CreateTime = DateTime.Now;
            entity.Path = $"\\admin\\p\\{fileName}.html";

            var blogAdd = await _blogsRepository.InsertAsync(entity);
            var returnDto = _mapper.Map<BlogDto>(blogAdd);
            var links = CreateLinksForBlogs(returnDto.Id, null);
            var linkedDict = returnDto.ShapeData(null) as IDictionary<string, object>;
            linkedDict.Add("links", links);
            return CreatedAtRoute(nameof(GetBlog), new { menuId = linkedDict["Id"] }, linkedDict);
        }

        private IEnumerable<LinkDto> CreateLinksForBlogs(int menuId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(new LinkDto(Url.Link(nameof(GetBlog), new { menuId }),
                    "self",
                    "GET"));
            }
            else
            {
                links.Add(new LinkDto(Url.Link(nameof(GetBlog), new { menuId, fields }),
                    "self",
                    "GET"));
            }
            //links.Add(new LinkDto(Url.Link(nameof(DeleteMenu), new { userId }), "delete menu", "DELETE"));

            //links.Add(new LinkDto(Url.Link(nameof(CreateMenu), new { }), "create menu", "POST"));

            links.Add(new LinkDto(Url.Link(nameof(GetBlogs), new { }), "get menus", "GET"));
            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForMenus(DtoBlogParameter parameters, bool hasPrevious, bool hasNext)
        {
            var links = new List<LinkDto>();


            links.Add(new LinkDto(CreateBlogsResourceUri(parameters, ResourceUriType.CurrentPage),
                "self", "GET"));

            if (hasPrevious)
            {
                links.Add(new LinkDto(CreateBlogsResourceUri(parameters, ResourceUriType.PreviousPage),
                    "previous_page", "GET"));
            }

            if (hasNext)
            {
                links.Add(new LinkDto(CreateBlogsResourceUri(parameters, ResourceUriType.NextPage),
                    "next_page", "GET"));
            }

            return links;
        }

        private string CreateBlogsResourceUri(DtoBlogParameter parameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetBlogs), new
                    {
                        fields = parameters.Fields,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        title = parameters.Title,
                        summary = parameters.Abstract,
                        orderBy = parameters.OrderBy
                    });

                case ResourceUriType.NextPage:

                    return Url.Link(nameof(GetBlogs), new
                    {
                        fields = parameters.Fields,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        title = parameters.Title,
                        summary = parameters.Abstract,
                        orderBy = parameters.OrderBy
                    });

                case ResourceUriType.CurrentPage:
                default:
                    return Url.Link(nameof(GetBlogs), new
                    {
                        fields = parameters.Fields,
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        title = parameters.Title,
                        summary = parameters.Abstract,
                        orderBy = parameters.OrderBy
                    });
            }
        }


    }
}
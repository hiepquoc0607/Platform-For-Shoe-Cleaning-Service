using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request;
using TP4SCS.Library.Models.Response;
using TP4SCS.Library.Utils.Mapper;
using TP4SCS.Services.Implements;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IServiceCategoryService _categoryService;
        private IMapper _mapper;

        public CategoryController(IServiceCategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories(string? keyword = null, int pageIndex = 1, int pageSize = 5, string orderBy = "Name")
        {
            var services = await _categoryService.GetServiceCategories(keyword, pageIndex, pageSize, orderBy);
            var allServices = await _categoryService.GetServiceCategories(keyword);
            var totalCount = services.Count();

            var pagedResponse = new PagedResponse<ServiceCategoryResponse>(
                services.Select(s => _mapper.Map<ServiceCategoryResponse>(s)),
                totalCount,
                pageIndex,
                pageSize
            );

            return Ok(new ResponseObject<PagedResponse<ServiceCategoryResponse>>("Fetch Category Success", pagedResponse));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetServiceCategoryById(id);
                if (category == null)
                {
                    Ok(new ResponseObject<ServiceCategoryResponse>($"Category with ID {id} not found.", null));
                }
                var response = _mapper.Map<ServiceCategoryResponse>(category);
                return Ok(new ResponseObject<ServiceCategoryResponse>("Fetch Category Success", response));
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseObject<string>(ex.Message));
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] ServiceCategoryRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ResponseObject<ServiceCategoryResponse>("Request body cannot be null."));
            }

            try
            {
                var category = _mapper.Map<ServiceCategory>(request);
                var response = _mapper.Map<ServiceCategoryResponse>(category);
                await _categoryService.AddServiceCategory(category);
                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id },
                    new ResponseObject<ServiceCategoryResponse>("Create Category Success", response));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ResponseObject<ServiceCategoryResponse>(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseObject<ServiceCategoryResponse>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<ServiceCategoryResponse>($"An error occurred: {ex.Message}"));
            }
        }

        [HttpPut("{existingCategoryId}")]
        public async Task<IActionResult> UpdateCategory(int existingCategoryId, [FromBody] ServiceCategoryRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ResponseObject<ServiceCategoryResponse>("Request body cannot be null."));
            }

            try
            {
                var categoryToUpdate = _mapper.Map<ServiceCategory>(request);

                await _categoryService.UpdateServiceCategory(categoryToUpdate, existingCategoryId);

                var updatedCategory = await _categoryService.GetServiceCategoryById(existingCategoryId);
                var response = _mapper.Map<ServiceCategoryResponse>(updatedCategory);

                return Ok(new ResponseObject<ServiceCategoryResponse>("Update Category Success", response));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseObject<ServiceCategoryResponse>(ex.Message));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ResponseObject<ServiceCategoryResponse>(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseObject<ServiceCategoryResponse>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<ServiceCategoryResponse>($"An error occurred: {ex.Message}"));
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteServiceCategory(id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseObject<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<string>($"An error occurred: {ex.Message}"));
            }
        }


    }
}

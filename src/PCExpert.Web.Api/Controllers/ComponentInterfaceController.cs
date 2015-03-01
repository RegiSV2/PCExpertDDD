using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using PCExpert.Core.Application;
using PCExpert.Core.Application.ViewObjects;
using PCExpert.Web.Model.Core;

namespace PCExpert.Web.Api.Controllers
{
	public class ComponentInterfaceController : ApiController
	{
		private readonly IComponentInterfaceService _componentInterfaceService;

		public ComponentInterfaceController(IComponentInterfaceService componentInterfaceService)
		{
			_componentInterfaceService = componentInterfaceService;
		}

		// GET: api/ComponentInterface
		public async Task<PagedResult<ComponentInterfaceModel>> Get(TableParameters parameters)
		{
			var results = await _componentInterfaceService.GetComponentInterfaces(parameters);
			return new PagedResult<ComponentInterfaceModel>(
				results.PagingParameters, results.CountTotal,
				Enumerable.ToList(results.Items.Select(Mapper.Map<ComponentInterfaceVO, ComponentInterfaceModel>)));
		}

		// GET: api/ComponentInterface/5
		public async Task<ComponentInterfaceVO> Get(Guid id)
		{
			var result = await _componentInterfaceService.GetComponentInterface(id);
			return Mapper.Map<ComponentInterfaceVO>(result);
		}

		// POST: api/ComponentInterface
		public void Post([FromBody] string value)
		{
		}

		// PUT: api/ComponentInterface/5
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE: api/ComponentInterface/5
		public void Delete(int id)
		{
		}
	}
}
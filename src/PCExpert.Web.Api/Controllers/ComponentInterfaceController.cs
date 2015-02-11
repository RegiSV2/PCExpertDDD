using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper;
using PCExpert.Core.Application;
using PCExpert.Core.Application.ViewObjects;
using PCExpert.Web.Api.Common;
using PCExpert.Web.Api.Common.WebModel;
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
			parameters = new TableParameters(new PagingParameters(0, 5), new OrderingParameters("Name", SortDirection.Ascending));
			var results = await _componentInterfaceService.GetComponentInterfaces(parameters);
			return new PagedResult<ComponentInterfaceModel>(
				results.PagingParameters, results.CountTotal,
				results.Items.Select(Mapper.Map<ComponentInterfaceVO, ComponentInterfaceModel>).ToList());
        }

        // GET: api/ComponentInterface/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ComponentInterface
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ComponentInterface/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ComponentInterface/5
        public void Delete(int id)
        {
        }
    }
}

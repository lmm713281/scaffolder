﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Scaffolder.API.Application;
using Scaffolder.Core.Engine;
using Scaffolder.Core.Meta;

namespace Scaffolder.API.Controllers
{
	[Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("api/[controller]")]
	public class DataController : Application.ControllerBase
	{
		public DataController(IOptions<AppSettings> settings)
			: base(settings)
		{
		}

		[HttpGet]
		public PagingInfo Get([ModelBinder(BinderType = typeof(FilterModelBinder))] Filter filter)
		{
			var engine = new Engine(ApplicationContext.Configuration.ConnectionString, ApplicationContext.Configuration.Engine);
			var table = ApplicationContext.Schema.GetTable(filter.TableName);
			var repository = engine.CreateRepository(table);

			var items = repository.Select(filter);
			var totalItemsCount = repository.GetRecordCount(filter);

			return new PagingInfo
			{
				CurrentPage = filter.CurrentPage,
				PageSize = filter.PageSize ?? totalItemsCount,
				Items = items,
				TotalItemsCount = totalItemsCount
			};
		}

		[HttpPost]
		public dynamic Post([FromBody] Payload payload)
		{
			var engine = new Engine(ApplicationContext.Configuration.ConnectionString, ApplicationContext.Configuration.Engine);
			var table = ApplicationContext.Schema.GetTable(payload.TableName);
			var repository = engine.CreateRepository(table);

			return repository.Insert(payload.Entity);
		}

		[HttpPut]
		public dynamic Put([FromBody] Payload payload)
		{
			var engine = new Engine(ApplicationContext.Configuration.ConnectionString, ApplicationContext.Configuration.Engine);
			var table = ApplicationContext.Schema.GetTable(payload.TableName);
			var repository = engine.CreateRepository(table);

			return repository.Update(payload.Entity);
		}

		[HttpDelete]
		public bool Delete([FromBody] Payload payload)
		{
			var engine = new Engine(ApplicationContext.Configuration.ConnectionString, ApplicationContext.Configuration.Engine);
			var table = ApplicationContext.Schema.GetTable(payload.TableName);
			var repository = engine.CreateRepository(table);

			var deletedOjbect = repository.Delete(payload.Entity);
			return deletedOjbect != null;
		}
	}
}
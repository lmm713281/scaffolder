﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Scaffolder.API.Application.Security;
using Scaffolder.Core.Meta;
using System.Security.Claims;

namespace Scaffolder.API.Application
{
    public class ControllerBase : Controller
    {
        private ApplicationContext _applicationContext;
	    
	    private static readonly Object _lock = new Object();

	    protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_applicationContext == null)
                {
                    lock (_lock)
                    {
                        if (_applicationContext == null)
                        {
                            var login = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                            var authorizationManager = new AuthorizationManager(Settings.WorkingDirectory);
                            var configuratoinLocation = authorizationManager.GetConfiguratoinLocationForUser(login);
                            _applicationContext = ApplicationContext.Load(configuratoinLocation);
                        }
                    }

                }

                return _applicationContext;
            }
        }

        protected AppSettings Settings { get; private set; }

        public ControllerBase(IOptions<AppSettings> settings)
        {
            Settings = settings.Value;
        }
    }
}
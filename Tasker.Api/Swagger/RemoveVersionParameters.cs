﻿using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tasker.Api.Swagger
{
    public class RemoveVersionParameters : IOperationFilter
    {
        public void Apply(
            Operation operation,
            OperationFilterContext context)
        {
            var versionParameter = operation.Parameters?.SingleOrDefault(p => p.Name == "version");
            if (versionParameter != null)
                operation.Parameters.Remove(versionParameter);
        }
    }

}

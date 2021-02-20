using System.Collections.Generic;
using Common.Model.Binders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Common.Model.Dto
{
    public class DtoFormFiles<TDto>
    {
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();

        [ModelBinder(BinderType = typeof(FormDataJsonBinder))]
        public TDto Data { get; set; }
    }
}
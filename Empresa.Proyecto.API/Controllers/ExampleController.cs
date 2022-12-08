using System.Net.Mime;
using Empresa.Proyecto.Models;
using Empresa.Proyecto.Models.Minion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Empresa.Proyecto.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ExampleController : ControllerBase
{
    //[Authorize]
    //[HttpGet]
    //[ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    //[ProducesDefaultResponseType]
    //public ActionResult<User> Get()
    //    => new User(User.Identity!.Name);

    private readonly AppSettings _appSettings;

    public ExampleController(IOptions<AppSettings> options)
    {        
        _appSettings = options.Value;
    }

    [Route("GetMinions")]
    [Authorize]
    [HttpPost]
    [ProducesDefaultResponseType]
    public ResponseGeneric<IEnumerable<Minion>> GetMinion(Minion model)
    {
        try
        {
            return new ResponseGeneric<IEnumerable<Minion>>(model.GetMinions(model, _appSettings));
        }
        catch (Exception ex)
        {
            return new ResponseGeneric<IEnumerable<Minion>>(ex);
        }         
    }


}

public record class User(string? UserName);
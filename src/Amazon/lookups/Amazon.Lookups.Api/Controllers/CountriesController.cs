using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Lookups.Api.Controllers;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<City> Cities { get; set; }
}

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
}


// should be a generic BC, not deserve dev time, but anyways
// this all is just a POC
public class CountriesController : ApiControllerBase
{
    private readonly List<Country> _countries;
    public CountriesController()
    {
        _countries = new() {
            new Country
            {
                Id = 1,
                Name = "Egypt",
                Cities = new List<City>
                {
                    new City { Id = 1, Name = "Sharqia"},
                    new City { Id = 2, Name = "10th Of Ramdan"},
                    new City { Id = 3, Name = "Cairo"},
                }
            },
            new Country
            {
                Id = 2,
                Name = "Lebanon",
                Cities = new List<City>
                {
                    new City { Id = 1, Name = "City1"},
                    new City { Id = 2, Name = "City2"},
                    new City { Id = 3, Name = "City3"},
                }
            }
        };
    }


    [HttpGet]
    public ActionResult<List<Country>> Get()
    {
        return Ok(_countries);
    }
}

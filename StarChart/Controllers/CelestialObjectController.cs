using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
            
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        [ActionName("GetById")]
        public IActionResult GetById(int id)
        {
            var res = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);

            if (res == null) return NotFound();

            var celestials = _context.CelestialObjects.Where(x => x.Id != id).ToList();
            res.Satellites = new List<Models.CelestialObject>();

            foreach (var celestial in celestials)
            {
                if (celestial.OrbitedObjectId == id)
                {
                    res.Satellites.Add(res);
                }
            }
            return Ok(res);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var res = _context.CelestialObjects.Where(x => x.Name == name).ToList();

            if (res.Count == 0) return NotFound();

            var celestials = _context.CelestialObjects.ToList();
            foreach(var celestial in res)
            {
                celestial.Satellites = new List<Models.CelestialObject>();
                foreach (var obj in celestials)
                {
                    if (celestial.Id == obj.Id) continue;
                    if (obj.OrbitedObjectId == celestial.Id)
                    {
                        celestial.Satellites.Add(obj);
                    }
                }
            }

            return Ok(res);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var res = _context.CelestialObjects.ToList();

            foreach (var celestial in res)
            {
                celestial.Satellites = new List<Models.CelestialObject>();
                foreach (var obj in res)
                {
                    if (celestial.Id == obj.Id) continue;
                    if (obj.OrbitedObjectId == celestial.Id)
                    {
                        celestial.Satellites.Add(obj);
                    }
                }
            }

            return Ok(res);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestial)
        {
            _context.CelestialObjects.Add(celestial);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new {id = celestial.Id}, celestial);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestial)
        {
            var res = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);

            if (res == null) return NotFound();

            res.Name = celestial.Name;
            res.OrbitalPeriod = celestial.OrbitalPeriod;
            res.OrbitedObjectId = celestial.OrbitedObjectId;

            _context.CelestialObjects.Update(res);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var res = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);

            if (res == null) return NotFound();

            res.Name = name;

            _context.CelestialObjects.Update(res);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var res = _context.CelestialObjects.Where(x => x.Id == id || x.OrbitedObjectId == id).ToList();

            if (res.Count == 0) return NotFound();

            _context.CelestialObjects.RemoveRange(res);
            _context.SaveChanges();

            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Pood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Pood.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoodidiController : ControllerBase
    {
        private static List<Poodi> _poed = new List<Poodi>
        {
            new Poodi("Maxima", new TimeSpan(10, 20, 20), new TimeSpan(20, 20, 20)),
            new Poodi("HLF", new TimeSpan(23, 20, 20), new TimeSpan(23, 50, 20)),
        };

        [HttpGet]
        public List<Poodi> Get()
        {
            return _poed;
        }

        [HttpPost("lisa")]
        public List<Poodi> LisaPood([FromBody] Poodi pood)
        {
            _poed.Add(pood);
            return _poed;
        }

        [HttpGet("lahtipood/{tund}/{minut}")]
        public IActionResult LahtiPood(int tund, int minut)
        {
            TimeSpan kellaaeg = new TimeSpan(tund, minut, 0);
            List<Poodi> lahtiPoed = _poed.Where(p => p.OnLahti(kellaaeg)).ToList();

            if (lahtiPoed.Count > 0)
            {
                List<string> poedNimed = lahtiPoed.Select(p => p.Nimi).ToList();
                return Ok(poedNimed);
            }
            else
            {
                return NotFound("Ühtegi poodi pole sel ajal lahti.");
            }
        }
        [HttpDelete("kustuta/{index}")]
        public List<Poodi> Delete(int index)
        {
            _poed.RemoveAt(index);
            return _poed;
        }

        [HttpPost("kylasta/{poodiNimi}")]
        public IActionResult kylasta([FromRoute] string poodiNimi)
        {
            var pood = _poed.FirstOrDefault(p => p.Nimi == poodiNimi);
            if (pood != null)
            {
                pood.KuulastusteArv += 1; // Увеличьте количество посетителей на 1
                return Ok(pood.KuulastusteArv); // Вернуть обновленное количество посетителей только для этого магазина
            }
            else
            {
                return NotFound($"Poodi nimega {poodiNimi} ei leitud.");
            }
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Controllers
{
    public class FilmBody
    {
        public string? tytul { get; set; }
        public string? rezyser { get; set; }
        public string? gatunek { get; set; }
        public int rok_wydania { get; set; }
    }

    public class Film : FilmBody
    {
        public int id { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class FilmController : ControllerBase
    {
        private static int idGen = 1;

        private static List<Film> lista = new List<Film>()
        {
            new Film{ id = idGen++, tytul = "Incepcja", rezyser = "Christopher Nolan", gatunek = "Sci-Fi", rok_wydania = 2010 },
            new Film{ id = idGen++, tytul = "Parasite", rezyser = "Bong Joon-ho", gatunek = "dramat", rok_wydania = 2019 },
            new Film{ id = idGen++, tytul = "Skazani na Shawshank", rezyser = "Frank Darabont", gatunek = "dramat", rok_wydania = 1994 },
            new Film{ id = idGen++, tytul = "Matrix", rezyser = "Lana i Lilly Wachowski", gatunek = "Sci-Fi", rok_wydania = 1999 },
            new Film{ id = idGen++, tytul = "Gladiator", rezyser = "Ridley Scott", gatunek = "historyczny", rok_wydania = 2000 }
        };

        private static readonly string[] validGenres = new[] { "Sci-Fi", "historyczny", "dramat" };

        [HttpGet]
        public ActionResult<IEnumerable<Film>> GetAll([FromQuery] string? titleFilter = null)
        {
            IEnumerable<Film> wynik = lista;

            if (!string.IsNullOrWhiteSpace(titleFilter))
            {
                wynik = wynik.Where(f => f.tytul != null && f.tytul.Contains(titleFilter, StringComparison.OrdinalIgnoreCase));
            }

            return Ok(wynik);
        }

        [HttpGet("{id}")]
        public ActionResult<Film> GetById(int id)
        {
            var film = lista.FirstOrDefault(f => f.id == id);
            if (film == null)
                return NotFound();

            return Ok(film);
        }

        [HttpPost]
        public ActionResult Post([FromBody] FilmBody filmBody)
        {
            var validationResult = ValidateFilmBody(filmBody);
            if (validationResult != null)
            {
                return BadRequest(validationResult);
            }

            var newFilm = new Film
            {
                id = idGen++,
                tytul = filmBody.tytul,
                rezyser = filmBody.rezyser,
                gatunek = filmBody.gatunek,
                rok_wydania = filmBody.rok_wydania
            };

            lista.Add(newFilm);

            return CreatedAtAction(nameof(GetById), new { id = newFilm.id }, newFilm);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] FilmBody filmBody)
        {
            var validationResult = ValidateFilmBody(filmBody);
            if (validationResult != null)
            {
                return BadRequest(validationResult);
            }

            var film = lista.FirstOrDefault(f => f.id == id);
            if (film == null)
            {
                return NotFound();
            }

            film.tytul = filmBody.tytul;
            film.rezyser = filmBody.rezyser;
            film.gatunek = filmBody.gatunek;
            film.rok_wydania = filmBody.rok_wydania;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var film = lista.FirstOrDefault(f => f.id == id);
            if (film == null)
            {
                return NotFound();
            }
            lista.Remove(film);
            return NoContent();
        }

        private string? ValidateFilmBody(FilmBody filmBody)
        {
            if (string.IsNullOrWhiteSpace(filmBody.tytul) || filmBody.tytul.Length > 100)
            {
                return "Tytuł jest wymagany i nie może przekraczać 100 znaków.";
            }

            if (string.IsNullOrWhiteSpace(filmBody.gatunek) ||
                !validGenres.Any(v => v.Equals(filmBody.gatunek, StringComparison.OrdinalIgnoreCase)))
            {
                return "Gatunek jest wymagany i musi być jednym z: Sci-Fi, historyczny, dramat.";
            }

            if (filmBody.rok_wydania > 2025)
            {
                return "Rok wydania nie może być większy niż 2025.";
            }

            return null;
        }
    }
}

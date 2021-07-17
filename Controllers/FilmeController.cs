using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.DTO;
using FilmesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public FilmeController(FilmeContext context, IMapper mapper)
        {
                _context = context;
                _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AdicionaFilme([FromBody] CreateFilmeDTO filmeDTO)
        {
            Filme filme = _mapper.Map<Filme>(filmeDTO);
           _context.Filmes.Add(filme); 
           _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaFilmesPorId), new { Id = filme.Id }, filme);
        }

        [HttpGet]
        public IEnumerable<Filme> RecuperaFilmes()
        {
            return _context.Filmes;
        }

        [HttpGet("{id}")]
        public IActionResult RecuperaFilmesPorId(int id)
        {
            Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if(filme != null)
            {
                ReadFilmeDTO read = _mapper.Map<ReadFilmeDTO>(filme);
                return Ok(read);
            }
            return NotFound();
        }

        [HttpPut("{id}")]
        public IActionResult AtualizaFilme([FromBody] UpdateFilmeDTO filmeDTO, int id){
            Filme filme = _context.Filmes.FirstOrDefault(f => f.Id == id);
            if(filme == null){
                return NotFound();
            } 
            _mapper.Map(filmeDTO, filme);
            
            _context.SaveChanges();
            return NoContent();            
        }

        [HttpDelete("{id}")]
        public IActionResult DeletaFilme(int id){
            Filme filme = _context.Filmes.FirstOrDefault(f => f.Id == id);
            if(filme != null){
                _context.Filmes.Remove(filme);
                _context.SaveChanges();
                return NoContent();
            } 
            return NotFound();
            
        }
    }
}

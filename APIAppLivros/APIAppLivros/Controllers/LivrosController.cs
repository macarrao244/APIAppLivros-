using APIAppLivros.Models;
using APIAppLivros.Repositores;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AlunosAPI.Controllers
{
    [Route("api/livros")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly LivrosRepository _livrosRepository;

        public LivrosController(LivrosRepository livrosRepository)
        {
            _livrosRepository = livrosRepository;
        }

        // GET: api/livros/listar
        [HttpGet("listar")]
        [SwaggerOperation(Summary = "Listar todos os Livros", Description = "Este endpoint retorna uma listagem de todos os Livros.")]
        public async Task<IEnumerable<Livro>> Listar([FromQuery] bool? ativo = null)
        {
            return await _livrosRepository.ListarTodosLivros(ativo);
        }

        // GET: api/livros/detalhes/5
        [HttpGet("detalhes/{id}")]
        [SwaggerOperation(Summary = "Obtém dados dos Livros pelo ID", Description = "Este endpoint retorna todos os dados de um Livro filtrando pelo ID.")]
        public async Task<ActionResult<Livro>> BuscarPorId(int id)
        {
            var livro = await _livrosRepository.BuscarPorId(id);
            if (livro == null)
            {
                return NotFound();
            }
            return Ok(livro);
        }

        // POST: api/livros
        [HttpPost]
        [SwaggerOperation(Summary = "Cadastrar um Livro", Description = "Este endpoint é responsável por cadastrar um novo Livro")]
        public async Task<IActionResult> Post([FromBody] Livro dados)
        {
            if (dados == null)
            {
                return BadRequest("Dados do livro são obrigatórios.");
            }

            await _livrosRepository.Salvar(dados);
            return CreatedAtAction(nameof(BuscarPorId), new { id = dados.Id }, dados);
        }

        // PUT: api/livros/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualizar os dados do Livro pelo ID", Description = "Este endpoint é responsável por atualizar os dados do Livro no banco")]
        public async Task<IActionResult> Put(int id, [FromBody] Livro dados)
        {
            if (dados == null || id != dados.Id)
            {
                return BadRequest("Dados inválidos.");
            }

            await _livrosRepository.Atualizar(dados);
            return NoContent();
        }

        // DELETE: api/livros/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remover um Livro filtrando pelo ID", Description = "Este endpoint é responsável por remover os dados de um Livro no banco")]
        public async Task<IActionResult> Delete(int id)
        {
            var livro = await _livrosRepository.BuscarPorId(id);
            if (livro == null)
            {
                return NotFound();
            }

            await _livrosRepository.DeletarPorId(id);
            return NoContent();
        }
    }
}

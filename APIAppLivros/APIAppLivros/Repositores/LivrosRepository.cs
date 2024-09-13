using APIAppLivros.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System.Data;

namespace APIAppLivros.Repositores
{
    public class LivrosRepository
    {
        private readonly string _connectionString;

        public LivrosRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection Connection =>
            new MySqlConnection(_connectionString);

        public async Task<IEnumerable<Livro>> ListarTodosLivros(bool? ativo = null)
        {
            using (var conn = Connection)
            {
                var sql = "SELECT * FROM tb_livro";
                return await conn.QueryAsync<Livro>(sql);
            }
        }

        public async Task<Livro> BuscarPorId(int id)
        {
            var sql = "SELECT * FROM tb_livro WHERE Id = @Id";

            using (var conn = Connection)
            {
                return await conn.QueryFirstOrDefaultAsync<Livro>(sql, new { Id = id });
            }
        }

        public async Task<int> DeletarPorId(int id)
        {
            var sql = "DELETE FROM tb_livro WHERE Id = @Id";

            using (var conn = Connection)
            {
                return await conn.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task<int> Salvar(Livro dados)
        {
            var sql = "INSERT INTO tb_livro (Titulo, Autor, AnoPublicacao, Genero, NumerodePagina) " +
                "VALUES (@Titulo, @Autor, @AnoPublicacao, @Genero, @NumerodePagina)";

            using (var conn = Connection)
            {
                return await conn.ExecuteAsync(sql, dados);
            }
        }

        public async Task<int> Atualizar(Livro dados)
        {
            var sql = "UPDATE tb_livro SET Titulo = @Titulo, Autor = @Autor, AnoPublicacao = @AnoPublicacao, Genero = @Genero, NumerodePagina = @NumerodePagina WHERE Id = @Id";

            using (var conn = Connection)
            {
                return await conn.ExecuteAsync(sql, dados);
            }
        }
    }
}

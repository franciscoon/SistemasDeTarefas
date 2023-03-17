using Microsoft.EntityFrameworkCore;
using SistemasDeTarefas.Data;
using SistemasDeTarefas.Models;
using SistemasDeTarefas.Repositorios.interfaces;

namespace SistemasDeTarefas.Repositorios
{
    public class TarefaRepositorio : ITarefaRepositorio
    {
        private readonly SistemaTarefasDBContext _dbcontext;
        public TarefaRepositorio(SistemaTarefasDBContext sistemaTarefasDBContext)
        {
            _dbcontext = sistemaTarefasDBContext;
        }
        public async Task<TarefaModel> BuscarPorId(int id)
        {
            return await _dbcontext.Tarefas.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<TarefaModel>> BuscarTodasTarefas()
        {
            return await _dbcontext.Tarefas.ToListAsync();
        }
        public async Task<TarefaModel> Adicionar(TarefaModel tarefa)
        {
            await _dbcontext.Tarefas.AddAsync(tarefa);
            await _dbcontext.SaveChangesAsync();

            return tarefa;
        }

        public async Task<bool> Apagar(int id)
        {
            TarefaModel tarefaPorId = await BuscarPorId(id);
            if(tarefaPorId == null)
            {
                throw new Exception($"Tarefa para o ID: {id} não foi encontrada no banco de dados");
            }

            _dbcontext.Tarefas.Remove(tarefaPorId);
            await _dbcontext.SaveChangesAsync();

            return true;
        }

        public async Task<TarefaModel> Atualizar(TarefaModel tarefa, int id)
        {
            TarefaModel tarefaPorId = await BuscarPorId(id);
            if(tarefaPorId == null)
            {
                throw new Exception($"Tarefa para o ID: {id} não foi encontrado no banco de dados");
            }

            tarefaPorId.Nome= tarefa.Nome;
            tarefaPorId.Descricao= tarefa.Descricao;
            tarefaPorId.Status = tarefa.Status;
            tarefaPorId.UsuarioId = tarefa.UsuarioId;

            _dbcontext.Tarefas.Update(tarefaPorId);
            await _dbcontext.SaveChangesAsync();

            return tarefaPorId;
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using ClinicaDentista.Data;
using ClinicaDentista.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ClinicaDentista.Controllers
{
    public class PacienteController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PacienteController> _logger;

        public PacienteController(AppDbContext context, ILogger<PacienteController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Paciente
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Acessando a lista de pacientes.");

            var pacientes = _context.Pacientes.ToList();

            _logger.LogInformation($"Total de pacientes encontrados: {pacientes.Count}");
            return View(pacientes);
        }

        // GET: Paciente/Create
        public IActionResult Create()
        {
            _logger.LogInformation("Acessando a tela de criação de paciente.");
            return View();
        }

        // POST: Paciente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,CPF,Telefone,DataNascimento")] Paciente paciente)
        {
            _logger.LogInformation("Iniciando criação de paciente.");

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Modelo válido. Tentando salvar o paciente.");
                try
                {
                    _context.Add(paciente);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Paciente criado com sucesso. Redirecionando para a lista de pacientes.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao tentar salvar o paciente.");
                    ModelState.AddModelError("", "Ocorreu um erro ao tentar salvar o paciente.");
                }
            }
            else
            {
                _logger.LogWarning("Modelo inválido ao tentar salvar o paciente.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Erro no ModelState: {ErrorMessage}", error.ErrorMessage);
                }
            }
            return View(paciente);
        }

        // GET: Paciente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogInformation("Acessando os detalhes do paciente com ID: {id}", id);

            if (id == null)
            {
                _logger.LogWarning("ID do paciente não fornecido.");
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                _logger.LogWarning("Paciente não encontrado para o ID: {id}", id);
                return NotFound();
            }

            _logger.LogInformation("Paciente encontrado: {Nome}, CPF: {CPF}", paciente.Nome, paciente.CPF);
            return View(paciente);
        }

        // GET: Paciente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogInformation("Acessando a tela de edição do paciente com ID: {id}", id);

            if (id == null)
            {
                _logger.LogWarning("ID do paciente não fornecido.");
                return NotFound();
            }

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                _logger.LogWarning("Paciente não encontrado para o ID: {id}", id);
                return NotFound();
            }

            _logger.LogInformation("Paciente encontrado para edição: {Nome}, CPF: {CPF}", paciente.Nome, paciente.CPF);
            return View(paciente);
        }

        // POST: Paciente/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CPF,Telefone,DataNascimento")] Paciente paciente)
        {
            _logger.LogInformation("Tentando editar paciente com ID: {id}", id);

            if (id != paciente.Id)
            {
                _logger.LogWarning("ID do paciente não corresponde ao ID fornecido.");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Paciente atualizado com sucesso. Redirecionando para a lista de pacientes.");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Erro de concorrência ao atualizar paciente.");
                    if (!_context.Pacientes.Any(e => e.Id == paciente.Id))
                    {
                        _logger.LogWarning("Paciente não encontrado para atualizar. ID: {id}", paciente.Id);
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Modelo inválido ao tentar editar o paciente.");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogWarning("Erro no ModelState: {ErrorMessage}", error.ErrorMessage);
            }

            return View(paciente);
        }

        // GET: Paciente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _logger.LogInformation("Acessando a tela de exclusão do paciente com ID: {id}", id);

            if (id == null)
            {
                _logger.LogWarning("ID do paciente não fornecido.");
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                _logger.LogWarning("Paciente não encontrado para o ID: {id}", id);
                return NotFound();
            }

            _logger.LogInformation("Paciente encontrado para exclusão: {Nome}, CPF: {CPF}", paciente.Nome, paciente.CPF);
            return View(paciente);
        }

        // POST: Paciente/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Confirmando exclusão do paciente com ID: {id}", id);

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Paciente excluído com sucesso.");
            }
            else
            {
                _logger.LogWarning("Paciente não encontrado para exclusão. ID: {id}", id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

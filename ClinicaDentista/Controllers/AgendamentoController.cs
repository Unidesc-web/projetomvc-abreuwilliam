using Microsoft.AspNetCore.Mvc;
using ClinicaDentista.Models;
using ClinicaDentista.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

public class AgendamentoController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<AgendamentoController> _logger;

    public AgendamentoController(AppDbContext context, ILogger<AgendamentoController> logger)
    {
        _context = context;
        _logger = logger;
    }

    private void CarregarPacientes()
    {
        var pacientes = _context.Pacientes?.ToList() ?? new List<Paciente>();
        ViewBag.Pacientes = new SelectList(pacientes, "Id", "Nome");
    }

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Buscando todos os agendamentos.");
        var agendamentos = await _context.Agendamentos
                                         .Include(a => a.Paciente)
                                         .ToListAsync();
        return View(agendamentos);
    }

    public IActionResult Create()
    {
        CarregarPacientes();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DataHora,PacienteId,Status")] Agendamento agendamento)
    {
        if (ModelState.IsValid)
        {
            if (agendamento.DataHora == default)
                ModelState.AddModelError("DataHora", "A data/hora fornecida é inválida.");

            if (!_context.Pacientes.Any(p => p.Id == agendamento.PacienteId))
                ModelState.AddModelError("PacienteId", "Paciente não encontrado.");

            if (string.IsNullOrWhiteSpace(agendamento.Status))
                ModelState.AddModelError("Status", "O status do agendamento é obrigatório.");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(agendamento);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao salvar o agendamento.");
                    ModelState.AddModelError("", "Erro ao salvar o agendamento.");
                }
            }
        }

        CarregarPacientes();
        return View(agendamento);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var agendamento = await _context.Agendamentos
                                        .Include(a => a.Paciente)
                                        .FirstOrDefaultAsync(m => m.Id == id);
        if (agendamento == null)
            return NotFound();

        return View(agendamento);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var agendamento = await _context.Agendamentos.FindAsync(id);
        if (agendamento == null)
            return NotFound();

        CarregarPacientes();
        return View(agendamento);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,DataHora,PacienteId,Status")] Agendamento agendamento)
    {
        if (id != agendamento.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(agendamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Agendamentos.Any(e => e.Id == agendamento.Id))
                    return NotFound();
                throw;
            }
        }

        CarregarPacientes();
        return View(agendamento);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var agendamento = await _context.Agendamentos
            .Include(a => a.Paciente)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (agendamento == null)
            return NotFound();

        return View(agendamento);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var agendamento = await _context.Agendamentos.FindAsync(id);
        if (agendamento != null)
        {
            _context.Agendamentos.Remove(agendamento);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}

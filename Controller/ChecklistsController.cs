using ChecklistApi.Models;
using ChecklistApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChecklistApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklistsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChecklistsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Checklists/cnpj/12345678000190
        [HttpGet("cnpj/{cnpj}")]
        public async Task<ActionResult<Checklist>> GetChecklistByCnpj(string cnpj)
        {
            Console.WriteLine($"Buscando CNPJ: {cnpj}");

            var checklist = await _context.Checklists
                .Include(c => c.Itens)
                .FirstOrDefaultAsync(c => c.Cnpj == cnpj);

            if (checklist == null)
            {
                Console.WriteLine("CNPJ não encontrado");
                return NotFound();
            }

            Console.WriteLine($"Checklist encontrado: {checklist.Id}");
            return Ok(checklist);
        }

        // POST: api/Checklists
        [HttpPost]
        public async Task<ActionResult<Checklist>> PostChecklist([FromBody] Checklist checklist)
        {
            Console.WriteLine($"Recebendo POST: {System.Text.Json.JsonSerializer.Serialize(checklist)}");

            // Adiciona os itens padrão se não vierem preenchidos
            if (checklist.Itens == null || checklist.Itens.Count == 0)
            {
                checklist.Itens = new List<ChecklistItem>();
                AdicionarItensPadrao(checklist);
            }

            _context.Checklists.Add(checklist);
            await _context.SaveChangesAsync();

            Console.WriteLine($"Checklist criado com ID: {checklist.Id}");
            return CreatedAtAction(nameof(GetChecklistByCnpj), new { cnpj = checklist.Cnpj }, checklist);
        }

        // PUT: api/Checklists/
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChecklist(int id, [FromBody] Checklist checklist)
        {
            Console.WriteLine($"Atualizando checklist ID: {id}");

            if (id != checklist.Id)
            {
                return BadRequest("ID do checklist não corresponde");
            }

            checklist.DataAtualizacao = DateTime.UtcNow;
            _context.Entry(checklist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChecklistExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Console.WriteLine("Checklist atualizado com sucesso");
            return NoContent();
        }

        private bool ChecklistExists(int id)
        {
            return _context.Checklists.Any(e => e.Id == id);
        }

        private void AdicionarItensPadrao(Checklist checklist)
        {
            // Itens de Implantação
            var itensImplantacao = new List<string>
            {
                "Adquirir informações da rotina do cliente",
                "Treinamento inicial de cadastros",
                "Treinamento entrada de notas",
                "Treinamento de ficha técnica/composição",
                "Treinamento de financeiro",
                "Treinamento de estoque",
                "Instalação de servidor + PDVs",
                "Solicitar Stone Code e solicitar credenciamento",
                "Treinamento de vendas",
                "Treinamento emissão de NFe saída",
                "Treinamento BMLINK",
                "Instalação integrador iFood",
                "Instalação integrador BMLINK",
                "Mostrar como gerar arquivo para etiquetas e configurar sistema para leitura",
                "Validar cadastros após conversão com cliente",
                "Gerar planilha de classificação fiscal",
                "Confirmar retorno da análise sobre a planilha de classificação fiscal",
                "Definir data para virada e abrir o chamado de virada",
                "Realizar verificação de pré-virada",
                "Cliente em produção",
                "Finalizar implantação"
            };

            // Itens de Pré-Virada
            var itensPreVirada = new List<string>
            {
                "Verificar comunicações com os terminais",
                "Verificar instalações do sistema",
                "Validar preços com o cliente",
                "Validar cadastros de produtos de balança e deixar sistema com a leitura correta",
                "Rodar validação de cadastros para emissão de NFCE"
            };

            int itemId = 1; // IDs começando em 1

            foreach (var item in itensImplantacao)
            {
                checklist.Itens.Add(new ChecklistItem
                {
                    Id = itemId++,
                    Descricao = item,
                    Concluido = false,
                    Tipo = "Implantacao"
                });
            }

            foreach (var item in itensPreVirada)
            {
                checklist.Itens.Add(new ChecklistItem
                {
                    Id = itemId++,
                    Descricao = item,
                    Concluido = false,
                    Tipo = "PreVirada"
                });
            }
        }
    }
}
using CadastroClientes.Data;
using CadastroClientes.Models;
using CadastroClientes.Services.SessaoService;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CadastroClientes.Controllers
{
    public class ClientesController : Controller
    {
        readonly private CadastroClientesDbContext _db;
        readonly private ISessaoInterface _sessaoInterface;
        public ClientesController(CadastroClientesDbContext db, ISessaoInterface sessaoInterface)
        {
            _db = db;
            _sessaoInterface = sessaoInterface;
        }

        public IActionResult Index()
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null) return RedirectToAction("Login", "Login");

            IEnumerable<ClientesModel> clientes = _db.Clientes;

            return View(clientes);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null) return RedirectToAction("Login", "Login");

            return View();
        }

        [HttpGet]
        public IActionResult Editar(int? id) 
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null) return RedirectToAction("Login", "Login");
            if (id == null || id == 0) return NotFound();
            
            ClientesModel cliente = _db.Clientes.FirstOrDefault(x => x.Id == id);

            if (cliente == null) return NotFound();

            return View(cliente);
        }

        [HttpGet]
        public IActionResult Excluir(int? id)
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null) return RedirectToAction("Login", "Login");

            if (id == null || id == 0) return NotFound();

            ClientesModel cliente = _db.Clientes.FirstOrDefault(x => x.Id == id);

            if (cliente == null) return NotFound();

            return View(cliente);
        }

        public IActionResult Exportar()
        {
            var dados = GetDados();

            using (XLWorkbook workbook = new XLWorkbook())
            {
                workbook.AddWorksheet(dados, "Dados Clientes");

                using (MemoryStream ms = new MemoryStream()) 
                {
                    workbook.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spredsheetml.sheet", "Clientes.xls");
                }
            }
        }

        private DataTable GetDados()
        {
            DataTable dt = new DataTable();
            dt.TableName = "Dados dos Clientes";
            dt.Columns.Add("Nome", typeof(string));
            dt.Columns.Add("Nascimento", typeof(DateTime));
            dt.Columns.Add("Telefone", typeof(string));
            dt.Columns.Add("CPF", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Rua", typeof(string));
            dt.Columns.Add("Bairro", typeof(string));
            dt.Columns.Add("Cidade", typeof(string));
            dt.Columns.Add("Estado", typeof(string));
            dt.Columns.Add("CEP", typeof(string));

            var dados = _db.Clientes.ToList();

            if (dados.Count > 0)
            {
                dados.ForEach(cliente =>
                {
                    dt.Rows.Add(cliente.Nome,
                        cliente.Nascimento,
                        cliente.Telefone,
                        cliente.CPF,
                        cliente.Email,
                        cliente.Rua,
                        cliente.Bairro,
                        cliente.Cidade,
                        cliente.Estado,
                        cliente.CEP);
                });
            } 

            return dt;
        }

        [HttpPost]
        public IActionResult Cadastrar(ClientesModel clientes)
        {
            if (ModelState.IsValid)
            {
                _db.Clientes.Add(clientes);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Cadastro realizado com sucesso!";

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Editar(ClientesModel cliente)
        {
            if (ModelState.IsValid)
            {
                _db.Clientes.Update(cliente);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Edição realizada com sucesso!";

                return RedirectToAction("Index");
            }


            return View(cliente);
        }

        [HttpPost]
        public IActionResult Excluir(ClientesModel cliente)
        {
            if (cliente == null) return NotFound();

            _db.Clientes.Remove(cliente);
            _db.SaveChanges();

            return RedirectToAction("Index");
            {
                
            }
        }
    }
}

using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            List<BeneficiarioModel> beneficiarios = new List<BeneficiarioModel>();
            ClienteModel cliente = new ClienteModel()
            {
                Beneficiarios = beneficiarios
            };
            return View(cliente);
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else if (bo.VerificarExistencia(model.CPF))
            {
                Response.StatusCode = 400;
                return Json("O CPF informado já existe no bando de dados");
            }
            else
            {
                
                
                model.Id = bo.Incluir(new Cliente()
                {                    
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

           
                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
       
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });
                               
                return Json(new { model.Id });
            }
        }

        

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                List<BeneficiarioModel> beneficiariosList = new List<BeneficiarioModel>();
                if (cliente.Beneficiarios.Count() > 0)
                {
                    foreach( var beneficiario in cliente.Beneficiarios)
                    {
                        beneficiariosList.Add(new BeneficiarioModel()
                        {
                            Id = beneficiario.Id,
                            IdCliente = beneficiario.IdCliente,
                            Nome = beneficiario.Nome,
                            CPF = beneficiario.CPF
                        });
                    }
                }
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF,
                    Beneficiarios = beneficiariosList
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult IncluirBeneficiario(List<BeneficiarioModel> model)
        {
            BoCliente bo = new BoCliente();

            BeneficiarioModel novoBeneficiario = model[0];

            model.RemoveAt(0);

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else if(model.Any(x => x.CPF == novoBeneficiario.CPF))
            {
                Response.StatusCode = 400;
                return Json("Já existe beneficiário com este CPF");
            }
            else
            {
                return Json(new { novoBeneficiario.Nome, novoBeneficiario.CPF });
            }
        }

        [HttpPost]
        public ActionResult InserirBeneficiarios(List<BeneficiarioModel> model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            if( model != null && model.Count > 0 )
            {
                List<Beneficiario> beneficiarios = new List<Beneficiario>();
                foreach( var beneficiario in model)
                {
                    beneficiarios.Add(new Beneficiario()
                    {
                        Nome = beneficiario.Nome,
                        CPF = beneficiario.CPF,
                        IdCliente = beneficiario.IdCliente,
                    });
                }

                bo.IncluirBeneficiarios(beneficiarios);
            }
            return Json("Cadastro realizado com sucesso!");

        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}
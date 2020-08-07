using EcommerceMVC.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EcommerceMVC.Controllers
{
    public class HomeController : Controller
    {
        EcommerceEntities db = new EcommerceEntities();
        public ActionResult Index()
        {
            return View(db.Produtos.ToList());
        }

        public ActionResult Carrinho()
        {
            return View();
        }

        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult Finalizado(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        public ActionResult CreateCliente(Cliente cliente)
        {
            Carrinho nCarrinho = new Carrinho();
            foreach(var cart in (List<Cart>)Session["Carrinho"])
            {
                nCarrinho.ProdutoCod = cart.Produto.Codigo;
                nCarrinho.Cliente = cliente;
                nCarrinho.ProdutoQuant = cart.ProdutoQuantidade;
                nCarrinho.CliendCod = cliente.ID;
            }
            if (ModelState.IsValid)
            {
                db.Carrinho.Add(nCarrinho);
                db.Cliente.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Finalizado/" + cliente.ID);
            }

            return View(cliente);
        }

        public ActionResult AdicionarQuantCarrinho(int produtoCod)
        {
            List<Cart> carrinho = (List<Cart>)Session["Carrinho"];
            foreach (var pProduto in carrinho)
            {
                if (pProduto.Produto.Codigo == produtoCod)
                {
                    pProduto.ProdutoQuantidade++;
                    break;
                }
            }
            Session["Carrinho"] = carrinho;

            return View("Carrinho");
        }

        public ActionResult RetirarQuantCarrinho(int produtoCod)
        {
            List<Cart> carrinho = (List<Cart>)Session["Carrinho"];
            foreach (var pProduto in carrinho)
            {
                if (pProduto.Produto.Codigo == produtoCod)
                {
                    if (pProduto.ProdutoQuantidade > 1)
                    {
                        pProduto.ProdutoQuantidade--;
                    }
                    else
                        ExcluirDoCarrinho(produtoCod);
                    break;
                }
            }
            if (carrinho.Count() == 0 || carrinho.Count().Equals(null))
                Session["Carrinho"] = null;
            else
                Session["Carrinho"] = carrinho;

            return View("Carrinho");
        }

        public ActionResult AdicionarAoCarrinho(int produtoCod)
        {
            if (Session["Carrinho"] == null)
            {
                var carrinho = new List<Cart>();
                var produto = db.Produtos.Find(produtoCod);
                carrinho.Add(new Cart()
                {
                    Produto = produto,
                    ProdutoQuantidade = 1
                });
                Session["Carrinho"] = carrinho;
            }
            else
            {
                List<Cart> carrinho = (List<Cart>)Session["Carrinho"];
                Session["Carrinho"] = null;
                var produto = db.Produtos.Find(produtoCod);
                foreach(var nProd in carrinho)
                {
                    if(nProd.Produto.Codigo == produtoCod)
                    {
                        nProd.ProdutoQuantidade++;
                    }
                    else
                    {
                        carrinho.Add(new Cart()
                        {
                            Produto = produto,
                            ProdutoQuantidade = 1
                        });
                        Session["Carrinho"] = carrinho;
                    }
                    break;
                }

            }

            return View("Carrinho");
        }

        public ActionResult ExcluirDoCarrinho(int produtoCod)
        {
            List<Cart> carrinho = (List<Cart>)Session["Carrinho"];
            foreach(var pExcluir in carrinho)
            {
                if(pExcluir.Produto.Codigo == produtoCod)
                {
                    carrinho.Remove(pExcluir);
                    break;
                }
            }

            if(carrinho.Count() == 0)   
                Session["Carrinho"] = null;
            else
                Session["Carrinho"] = carrinho;

            return View("Carrinho");
        }

        public ActionResult Detalhes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produtos produtos = db.Produtos.Find(id);
            if (produtos == null)
            {
                return HttpNotFound();
            }
            return View(produtos);
        }
    }
}
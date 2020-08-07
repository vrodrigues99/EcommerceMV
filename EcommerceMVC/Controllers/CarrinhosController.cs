using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EcommerceMVC.Models;

namespace EcommerceMVC.Controllers
{
    public class CarrinhosController : Controller
    {
        private EcommerceEntities db = new EcommerceEntities();

        // GET: Carrinhos
        public ActionResult Index()
        {
            var carrinho = db.Carrinho.Include(c => c.Cliente).Include(c => c.Produtos);
            return View(carrinho.ToList());
        }

        // GET: Carrinhos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carrinho carrinho = db.Carrinho.Find(id);
            if (carrinho == null)
            {
                return HttpNotFound();
            }
            return View(carrinho);
        }

        // GET: Carrinhos/Create
        public ActionResult Create()
        {
            ViewBag.CliendCod = new SelectList(db.Cliente, "ID", "Nome");
            ViewBag.ProdutoCod = new SelectList(db.Produtos, "Codigo", "Nome");
            return View();
        }

        // POST: Carrinhos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Codigo,ProdutoCod,ProdutoQuant,CliendCod")] Carrinho carrinho)
        {
            if (ModelState.IsValid)
            {
                db.Carrinho.Add(carrinho);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CliendCod = new SelectList(db.Cliente, "ID", "Nome", carrinho.CliendCod);
            ViewBag.ProdutoCod = new SelectList(db.Produtos, "Codigo", "Nome", carrinho.ProdutoCod);
            return View(carrinho);
        }

        // GET: Carrinhos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carrinho carrinho = db.Carrinho.Find(id);
            if (carrinho == null)
            {
                return HttpNotFound();
            }
            ViewBag.CliendCod = new SelectList(db.Cliente, "ID", "Nome", carrinho.CliendCod);
            ViewBag.ProdutoCod = new SelectList(db.Produtos, "Codigo", "Nome", carrinho.ProdutoCod);
            return View(carrinho);
        }

        // POST: Carrinhos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Codigo,ProdutoCod,ProdutoQuant,CliendCod")] Carrinho carrinho)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carrinho).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CliendCod = new SelectList(db.Cliente, "ID", "Nome", carrinho.CliendCod);
            ViewBag.ProdutoCod = new SelectList(db.Produtos, "Codigo", "Nome", carrinho.ProdutoCod);
            return View(carrinho);
        }

        // GET: Carrinhos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carrinho carrinho = db.Carrinho.Find(id);
            if (carrinho == null)
            {
                return HttpNotFound();
            }
            return View(carrinho);
        }

        // POST: Carrinhos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Carrinho carrinho = db.Carrinho.Find(id);
            db.Carrinho.Remove(carrinho);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

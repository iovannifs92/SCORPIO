using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SCORPIO.Data;
using SCORPIO.Models;
using SCORPIO.ModelsClass;

namespace SCORPIO.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private CategoriaModels categoriaModels;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
            categoriaModels = new CategoriaModels(_context);
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            return View();
        }

        //filtrarDatos
        public List<object[]> filtrarDatos(int numPagina, string valor, string order)
        {
            return categoriaModels.filtrarDatos(numPagina, valor, order);
        }

        //GET categorias
        public List<Categoria> getCategorias(int id) //el parametro debe ser igual al parametro de data en Categorias.js
        {
            return categoriaModels.getCategorias(id);
        }

        //Editar Categoria
        public List<IdentityError> editarCategoria(int id, string nombre, string descripcion, Boolean estado, int funcion)
        {
            return categoriaModels.editarCategoria(id, nombre, descripcion, estado, funcion);
        }

        //Guardar Categoria
        public List<IdentityError> guardarCategoria(string nombre, string descripcion, string estado)
        {
            return categoriaModels.guardarCategoria(nombre, descripcion, estado);
        }
    }
}

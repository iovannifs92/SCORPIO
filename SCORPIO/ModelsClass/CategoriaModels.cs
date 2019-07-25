using Microsoft.AspNetCore.Identity;
using SCORPIO.Data;
using SCORPIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCORPIO.ModelsClass
{
    public class CategoriaModels
    {
        private ApplicationDbContext context;
        private Boolean estados;
        public CategoriaModels(ApplicationDbContext context)
        {
            this.context = context;            
        }

        public List<IdentityError> guardarCategoria(string nombre, string descripcion, string estado)
        {
            var errorList = new List<IdentityError>();
            var categoria = new Categoria
            {
                Nombre = nombre,
                Descripcion = descripcion,
                Estado = Convert.ToBoolean(estado)
            };
            context.Add(categoria);
            context.SaveChanges();//SaveChangesAsync solo se ejecuta una vez cada vez que se invoca CategoriaModels

            errorList.Add(new IdentityError {
                Code = "Save",
                Description = "Save"
            });

            return errorList;
        }

        public List<object[]> filtrarDatos(int numPagina, string valor) {
            int count = 0, cant, numRegistros = 0, inicio = 0, reg_por_pagina = 10, 
                cant_paginas, pagina;
            string dataFilter = "", paginador = "", Estado = null;
            List<object[]> data = new List<object[]>();
            IEnumerable<Categoria> query;
            var categorias = context.Categoria.OrderBy(c => c.Nombre).ToList();
            numRegistros = categorias.Count;
            inicio = (numPagina - 1) * reg_por_pagina;
            cant_paginas = (numRegistros / reg_por_pagina);
            if (valor == "null")
            {
                query = categorias.Skip(inicio).Take(reg_por_pagina);
            }
            else
            {
                //query = categorias.Where(c => c.Nombre.StartsWith(valor) || c.Descripcion.StartsWith(valor)).Skip(inicio).Take(reg_por_pagina);
                query = categorias.Where(c => c.Nombre.StartsWith(valor) || c.Descripcion.StartsWith(valor)).Skip(inicio).Take(reg_por_pagina);
            }
            cant = query.Count();
            foreach (var item in query)
            {
                if (item.Estado == true)
                {
                    Estado = "<a data-toggle='modal' data-target='#ModalEstado' onclick='editarEstado(" 
                        + item.CategoriaID + ',' + 0 + ")' class='btn btn-success'>Activo</a>";
                }
                else
                {
                    Estado = "<a data-toggle='modal' data-target='#ModalEstado' onclick='editarEstado(" 
                        + item.CategoriaID + ',' + 0 + ")' class='btn btn-danger'>No activo</a>";
                }

                dataFilter += 
                    "<tr>" +
                        "<td>" + item.Nombre + "</td>" +
                        "<td>" + item.Descripcion + "</td>" +
                        "<td>" + Estado + "</td>" +
                        "<td>" +
                        "<a data-toggle='modal' data-target='#modalScorpion' onclick='editarEstado("
                        + item.CategoriaID + ',' + 1 + ")' class='btn btn-success'>Editar</a>" +
                        "</td>" +
                    "</tr>";
            }
            object[] dataObj = { dataFilter, paginador };
            data.Add(dataObj);
            return data;
        }

        public List<Categoria> getCategorias(int id)
        {
            return context.Categoria.Where(c => c.CategoriaID == id).ToList();
        }

        public List<IdentityError> editarCategoria(int idCategoria, string nombre, string descripcion, Boolean estado, int funcion)
        {
            var errorList = new List<IdentityError>();
            string code = "", des = "";
            switch (funcion)
            {
                case 0:
                    if (estado)
                    {
                        estados = false;
                    }
                    else
                    {
                        estados = true;
                    }
                    break;
                case 1:
                    estados = estado;
                    break;
            }

            var categoria = new Categoria()
            {
                CategoriaID = idCategoria,
                Nombre = nombre,
                Descripcion = descripcion,
                Estado = estados
            };
            try
            {
                context.Update(categoria);
                context.SaveChanges();
                code = "Save";
                des = "Save";
            }
            catch (Exception ex)
            {

                code = "error";
                des = ex.Message;
            }

            errorList.Add(new IdentityError
            {
                Code="1",
                Description="Save"
            });
            return errorList;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using masco.Models;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace masco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerroesController : ControllerBase
    {
        private readonly MASCOTASContext _context;

        public PerroesController(MASCOTASContext context)
        {
            _context = context;
        }

        // GET: api/Perroes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Perro>>> GetPerros()
        {
            //CONEXION A LA BDD Y CONFIGURACION CON EL SP
            SqlConnection conn = (SqlConnection)_context.Database.GetDbConnection(); 
            SqlCommand cn = conn.CreateCommand();
            cn.CommandText = "SP_BUSCADOR";
            cn.Parameters.Add("@identificador", System.Data.SqlDbType.Int, 20).Value = 1;
            cn.CommandType = System.Data.CommandType.StoredProcedure;
            cn.CommandTimeout = 0;
            cn.Connection=conn;
            conn.Open();
            // AGARRA LA INFORMACION DEL SP DE ARRIBA LA EJECUTA LA TRANSFORMA Y LA DEVUELVE
            var resul = cn.ExecuteReader(); //AGARRA
            var lista = DataReaderMapToList<Perro>(resul); //TRANSFORMA
            return lista; //DEVUELVE
            
        }
        //ESTA APRTE DEL CODIGO HAY EN INTERNET PERO HACE Q TRANSFORME Y DEVUELVA ARRIBA DONDE DICE 'TRANSFORMA' EN ESA FUNCION
        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        // GET: api/Perroes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Perro>> GetPerro(decimal id)
        {
          if (_context.Perros == null)
          {
              return NotFound();
          }
            var perro = await _context.Perros.FindAsync(id);

            if (perro == null)
            {
                return NotFound();
            }

            return perro;
        }

        // PUT: api/Perroes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerro(decimal id, Perro perro)
        {
            if (id != perro.IdMasc)
            {
                return BadRequest();
            }

            _context.Entry(perro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Perroes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Perro>> PostPerro(Perro perro)
        {
          if (_context.Perros == null)
          {
              return Problem("Entity set 'MASCOTASContext.Perros'  is null.");
          }

            
            SqlConnection conn = (SqlConnection)_context.Database.GetDbConnection(); //coneccion a la base de datos
            SqlCommand cn = conn.CreateCommand();
            conn.Open();
                cn.CommandType = System.Data.CommandType.StoredProcedure;
                cn.CommandText = "SP_BUSCADOR";
               // cn.Parameters.Add("@identificador", System.Data.SqlDbType.VarChar, 20).Value=perro.Raza;
                
                
                var resul= cn.ExecuteNonQuery();
                

            conn.Close();
            

            //_context.Perros.Add(perro);
            //await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerro", new { id = perro.IdMasc }, perro);
        }

        // DELETE: api/Perroes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerro(decimal id)
        {
            if (_context.Perros == null)
            {
                return NotFound();
            }
            var perro = await _context.Perros.FindAsync(id);
            if (perro == null)
            {
                return NotFound();
            }

            _context.Perros.Remove(perro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PerroExists(decimal id)
        {
            return (_context.Perros?.Any(e => e.IdMasc == id)).GetValueOrDefault();
        }
    }
}

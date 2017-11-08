using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ExtensionMethods;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace lesson3.Controllers
{
    //This is the default route of the API. 
    [Route("api/[controller]")]
    public class ActorController : Controller
    {
        private readonly MovieContext _context;

        public ActorController (MovieContext context) {
            _context = context;
        }

        // GET api/values
        [HttpGet("GetAll")]
        public IEnumerable<Actor> GetAll () {
            return _context.Actors.ToArray();
        }

        // GET api/values/5
        [HttpGet ("GetActor/{id}")]
        public IActionResult GetActor(int id) {
            var actor = _context.Actors.Where(a => a.Id == id).FirstOrDefault();
            if(actor == null) return NotFound();
            return Ok(actor);
        }

        [HttpGet ("GetActor/{name}")]
        public IActionResult GetActor(string name) {
            var actor = _context.Actors.Where(a => a.Name == name).FirstOrDefault();
            if(actor == null) return NotFound();
            return Ok(actor);
        } 

        [HttpGet ("GetActorPaged/{page_index}/{page_size}")]
        public IActionResult GetActorPaged(int page_index, int page_size) {
            var actors = _context.Actors.GetPage<Actor>(page_index,page_size, a => a.Id);
            if(actors == null) return NotFound();
            return Ok(actors);
        }

        [HttpPost ("GetActorFiltered")]
        public IActionResult GetActorFiltered([FromBody] Filter filter) {
            if(filter == null) return NotFound();
            Expression<Func<Actor,bool>> expr = FilterToExpr<Actor>(filter);
            var result = _context.Actors.Where(expr).ToArray(); 
            return Ok(result);
        } 

        [HttpPut]
        public IActionResult AddActor([FromBody] Actor newActor){
            Actor actorToAdd = new Actor(){Id = _context.Actors.Count() + 1, Name = newActor.Name};
            _context.Actors.Add(actorToAdd);
            _context.SaveChanges();
            return Ok();
        }


        public static Expression<Func<T, bool>> FilterToExpr<T>(Filter filter)
        {
            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");
            return FilterToExpr_AUX<T>(filter, parameter);
        }

        public static Expression<Func<T, bool>> FilterToExpr_AUX<T>(Filter filter, ParameterExpression parameter)
        {
            switch (filter.kind)
            {
                case 0:
                    {
                        var propertyReference = Expression.Property(parameter, filter.att);
                        var constantReference = Expression.Constant(filter.value);
                        return Expression.Lambda<Func<T, bool>>(Expression.Equal(propertyReference, constantReference), parameter);
                    }
                case 1:
                    {
                        Expression<Func<T, bool>> expr1 = FilterToExpr_AUX<T>(filter.a1, parameter);
                        Expression<Func<T, bool>> expr2 = FilterToExpr_AUX<T>(filter.a2, parameter);
                        var body = Expression.And(expr1.Body, expr2.Body);

                        var lambda = Expression.Lambda<Func<T, bool>>(body,parameter);
                        return lambda;
                    }
                default: return null;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace lesson3.Controllers
{
    //This is the default route of the API. 
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        private readonly MovieContext _context;

        public MovieController (MovieContext context) {
            _context = context;

            if(_context.Movies.Count() == 0){
                Movie m = new Movie(){
                    Title = "Skyfall",
                    Release = DateTime.Now,
                    Actors = new List<Actor>(){
                        new Actor(){ 
                            Name = "Daniel Craig",
                            Birth = DateTime.Now,
                            Gender =  "Male",
                        },
                        new Actor(){
                            Name = "Spiderman",
                            Birth = DateTime.Now,
                            Gender = "Male",
                        }
                    }
                };
                _context.Movies.Add(m);
                _context.SaveChanges();
            }
        }


        // GET api/values
        [HttpGet("GetAll")]
        public IEnumerable<Movie> GetAll () {
            var movies = from m in _context.Movies
                         let actors = _context.Actors.Where(a => a.MovieId == m.Id)
                         select new Movie(){Id=m.Id, Actors = actors.ToList(), Title = m.Title};
            return movies.ToArray();
        }

        // GET api/values/5
        [HttpGet ("GetMovie/{id}")]
        public IActionResult GetMovie(int id) {
            var movies = from m in _context.Movies
                         where m.Id == id
                         let actors = _context.Actors.Where(a => a.MovieId == m.Id)
                         select new Movie(){Id = m.Id, Actors = actors.ToList(), Title = m.Title};
            var movie = movies.FirstOrDefault();
            if(movie == null) return NotFound();
            return Ok(movie);
        }

                // GET api/values/5
        [HttpGet ("GetMovieByTitle/{title}")]
        public IActionResult GetMovieByTitle(string title) {
            var movies = from m in _context.Movies
                         where m.Title == title
                         let actors = _context.Actors.Where(a => a.MovieId == m.Id)
                         select new Movie(){Id = m.Id, Actors = actors.ToList(), Title = m.Title};
            var movie = movies.FirstOrDefault();
            if(movie == null) return NotFound();
            return Ok(movie);
        }

        [HttpPost ("GetMovieFiltered")]
        public IActionResult GetMovieFiltered([FromBody] Filter filter) {
            if(filter == null) return NotFound();
            Expression<Func<Movie,bool>> expr = FilterToExpr<Movie>(filter);
            var result = _context.Movies.Where(expr).ToArray(); 
            return Ok(result);
        } 


        [HttpPut]
        public IActionResult AddMovie([FromBody] Movie newMovie){
            var movieToAdd = new Movie(){Id = _context.Movies.Count() + 1, Title = newMovie.Title};
            _context.Movies.Add(movieToAdd);
            _context.SaveChanges();
            return Ok();
        }

        public IActionResult DeleteMovie(int id){
            var movieToDelete = _context.Movies.SingleOrDefault(m => m.Id == id);
            _context.Movies.Remove(movieToDelete);    
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
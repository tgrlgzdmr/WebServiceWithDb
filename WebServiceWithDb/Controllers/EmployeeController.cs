using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebServiceWithDb.Controllers
{
    public class EmployeeController : ApiController
    {
        EmployeeDbEntities db = new EmployeeDbEntities();
        public HttpResponseMessage Get(string gender = "all", int? top = 0)
        {
            IQueryable<Employee> query = db.Employees;
            gender = gender.ToLower();
            switch (gender)
            {
                case "all":
                    break;
                case "female":
                case "male":
                    query=query.Where(e => e.Gender.ToLower() == gender);
                    break ;
                default:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"{gender} is not a valiid gender. Please use all,male or female.");
            }

            if(top>0)
            {
                query=query.Take(top.Value);
            }
            return Request.CreateResponse(HttpStatusCode.OK, query.ToList());
        }

        public HttpResponseMessage Get(int id)
        {
            Employee employee = db.Employees.FirstOrDefault(x => x.Id == id);
            
            if(employee == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Id'si {id} olan çalışan bulunamadı");
            }
            return Request.CreateResponse(HttpStatusCode.OK, employee);

        }

        public HttpResponseMessage Post(Employee emp)
        {
            try
            {
                db.Employees.Add(emp);
                if (db.SaveChanges() > 0)
                {
                    HttpResponseMessage message = Request.CreateResponse(HttpStatusCode.Created, emp);
                    message.Headers.Location= new Uri(Request.RequestUri + "/" + emp.Id);
                    return message;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Veri Ekleme Yapılamadı");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }

        }

        //public HttpResponseMessage Put(Employee emp)
        //{
        //    try
        //    {
        //        Employee emp1 = db.Employees.FirstOrDefault(x => x.Id == emp.Id);

        //        if (emp1 == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "Employe Id:" + emp.Id);
        //        }
        //        else
        //        {
        //            emp1.Name = emp.Name;
        //            emp1.Surname = emp.Surname;
        //            emp1.Salary = emp.Salary;
        //            emp1.Gender = emp.Gender;

        //            if (db.SaveChanges() > 0)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, emp);

        //            }
        //            else
        //            {
        //                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Güncelleme Başarısız");
        //            }

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

        //    }

        //}

        //public HttpResponseMessage Put(int id, Employee emp)
        //{
        //    try
        //    {
        //        Employee emp1 = db.Employees.FirstOrDefault(x => x.Id == id);

        //        if (emp1 == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "Employe Id:" + emp.Id);
        //        }
        //        else
        //        {
        //            emp1.Name = emp.Name;
        //            emp1.Surname = emp.Surname;
        //            emp1.Salary = emp.Salary;
        //            emp1.Gender = emp.Gender;

        //            if (db.SaveChanges() > 0)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, emp);

        //            }
        //            else
        //            {
        //                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Güncelleme Başarısız");
        //            }

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

        //    }

        //}

        //public HttpResponseMessage Put([FromBody]int id, [FromUri]Employee emp)
        //{
        //    try
        //    {
        //        Employee emp1 = db.Employees.FirstOrDefault(x => x.Id == id);

        //        if (emp1 == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "Employe Id:" + emp.Id);
        //        }
        //        else
        //        {
        //            emp1.Name = emp.Name;
        //            emp1.Surname = emp.Surname;
        //            emp1.Salary = emp.Salary;
        //            emp1.Gender = emp.Gender;

        //            if (db.SaveChanges() > 0)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, emp);

        //            }
        //            else
        //            {
        //                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Güncelleme Başarısız");
        //            }

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

        //    }

        //}

        public HttpResponseMessage Put([FromBody] MyBodyType mytype, [FromUri] Employee emp)
        {
            try
            {
                
                Employee emp1 = db.Employees.FirstOrDefault(x => x.Id == mytype.id);

                if (emp1 == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Employe Id:" + emp.Id);
                }
                else
                {
                    emp1.Name = emp.Name;
                    emp1.Surname = emp.Surname;
                    emp1.Salary = emp.Salary;
                    emp1.Gender = emp.Gender;

                    if (db.SaveChanges() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, emp);

                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Güncelleme Başarısız");
                    }

                }


            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }

        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                Employee emp = db.Employees.FirstOrDefault(x => x.Id == id);
                if (emp == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Employe Id:" + id);
                }
                else
                {
                    db.Employees.Remove(emp);
                    if (db.SaveChanges() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "Employee Id:" + id);

                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Kayıt Silinemedi");
                    }
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

    }

    public class MyBodyType
    {
        public int id { get; set; }
        public string second { get; set; }
    }
}

using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class SinhvienService
    {
        public List<Sinhvien> GetAll()
        {
            StudentModel context = new StudentModel();
            return context.Sinhviens.ToList();
        }
        public Sinhvien FindById(string studentId)
        {
            StudentModel context = new StudentModel();
            return context.Sinhviens.FirstOrDefault(p => p.MaSV == studentId);
        }
        public void InsertUpdate(Sinhvien s)
        {
            StudentModel context = new StudentModel();
            context.Sinhviens.AddOrUpdate(s);
            context.SaveChanges();
        }
        /*public void DeleteById(string studentId)
        {
            StudentModel context = new StudentModel();
            Sinhvien student = FindById(studentId);
            context.Sinhviens.Remove(student);
            context.SaveChanges();
        }*/

        public void DeleteById(string id)
        {
            using (StudentModel e = new StudentModel())
            {
                var students =  e.Sinhviens.FirstOrDefault(p => p.MaSV == id);
                e.Sinhviens.Remove(students);
                e.SaveChanges();
            } 
        }
    }
}

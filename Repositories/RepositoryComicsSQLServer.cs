using Microsoft.AspNetCore.Http.HttpResults;
using SegundaPracticaNetCore.Models;
using System.Data;
using System.Data.SqlClient;

#region PROCEDIMIENTOS ALMACENADOS

//create procedure SP_INSERTCOMIC
//(@nombre nvarchar(50), @imagen nvarchar(150), @descripcion nvarchar(50))
//as
//	declare @nextId int
//	select @nextId = max(IDCOMIC) + 1 from COMICS
//	insert into COMICS VALUES (@nextId, @nombre, @imagen, @descripcion)
//go

#endregion

namespace SegundaPracticaNetCore.Repositories
{
    public class RepositoryComicsSQLServer : IRepositoryComics
    {
        private DataTable tablaComics;
        private SqlConnection cn;
        private SqlCommand cmd;

        public RepositoryComicsSQLServer()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS01;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.cmd = new SqlCommand();
            this.cmd.Connection = cn;
            this.tablaComics = new DataTable();
            string sql = "select * from COMICS";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            ad.Fill(this.tablaComics);
        }
        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;

            List<Comic> comics = new List<Comic>();
            foreach (var row in consulta)
            {
                Comic comic = new Comic
                {
                    Id = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")
                };
                comics.Add(comic);
            }

            return comics;
        
        }
        public Comic FindComic(int id)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == id
                           select datos;

            var row = consulta.First();

            Comic comic = new Comic
            {
                Id = row.Field<int>("IDCOMIC"),
                Nombre = row.Field<string>("NOMBRE"),
                Imagen = row.Field<string>("IMAGEN"),
                Descripcion = row.Field<string>("DESCRIPCION")
            };

            return comic;
        }

        public void InsertarComicProcedure(string nombre, string imagen, string descripcion)
        {
            this.cmd.Parameters.AddWithValue("@nombre", nombre);
            this.cmd.Parameters.AddWithValue("@imagen", imagen);
            this.cmd.Parameters.AddWithValue("@descripcion", descripcion);
            this.cmd.CommandType = CommandType.StoredProcedure;
            this.cmd.CommandText = "SP_INSERTCOMIC";
            this.cn.Open();
            this.cmd.ExecuteNonQuery();
            this.cn.Close();
            this.cmd.Parameters.Clear();
        }

        public void InsertarComicLambda(string nombre, string imagen, string descripcion)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;

            int nextId = consulta.Max(x => x.Field<int>("IDCOMIC")) + 1;

            string sql = "insert into COMICS values(@idcomic, @nombre, @imagen, @descripcion)";

            this.cmd.Parameters.AddWithValue("@idcomic", nextId);
            this.cmd.Parameters.AddWithValue("@nombre", nombre);
            this.cmd.Parameters.AddWithValue("@imagen", imagen);
            this.cmd.Parameters.AddWithValue("@descripcion", descripcion);
            this.cmd.CommandType = CommandType.Text;
            this.cmd.CommandText = sql;
            this.cn.Open();
            this.cmd.ExecuteNonQuery();
            this.cn.Close();
        }

        public void DeleteComic(int id)
        {
            string sql = "delete from COMICS where IDCOMIC=@idcomic";

            this.cmd.Parameters.AddWithValue("@idcomic", id);
            this.cmd.CommandType = CommandType.Text;
            this.cmd.CommandText = sql;
            this.cn.Open();
            this.cmd.ExecuteNonQuery();
            this.cn.Close();
            this.cmd.Parameters.Clear();
        }

    }
}

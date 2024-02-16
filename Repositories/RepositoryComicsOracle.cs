using Microsoft.AspNetCore.Http.HttpResults;
using Oracle.ManagedDataAccess.Client;
using SegundaPracticaNetCore.Models;
using System.Collections.Generic;
using System.Data;

#region PROCEDIMIENTOS ALMACENADOS

//CREATE OR REPLACE PROCEDURE SP_INSERTCOMIC (
//   p_nombre COMICS.NOMBRE%TYPE,
//   p_imagen COMICS.IMAGEN%TYPE,
//   p_descripcion COMICS.DESCRIPCION%TYPE
//)
//AS
//   nextId NUMBER;
//BEGIN
//   SELECT NVL(MAX(IDCOMIC), 0) + 1 INTO nextId FROM COMICS;

//INSERT INTO COMICS (IDCOMIC, NOMBRE, IMAGEN, DESCRIPCION)
//   VALUES (nextId, p_nombre, p_imagen, p_descripcion);

//COMMIT;
//END;


#endregion

namespace SegundaPracticaNetCore.Repositories
{
    public class RepositoryComicsOracle : IRepositoryComics
    {
        private DataTable tablaComics;
        private OracleConnection cn;
        private OracleCommand cmd;

        public RepositoryComicsOracle() 
        {
            string connectionString =
                @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True; User Id=SYSTEM; Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.cmd = new OracleCommand();
            this.cmd.Connection = cn;
            string sql = "select * from COMICS";
            OracleDataAdapter adapter = new OracleDataAdapter(sql, this.cn);
            this.tablaComics = new DataTable();
            adapter.Fill(this.tablaComics);
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
            this.cmd.Parameters.Add(new OracleParameter(":p_nombre", nombre));
            this.cmd.Parameters.Add(new OracleParameter(":p_imagen", imagen));
            this.cmd.Parameters.Add(new OracleParameter(":p_descripcion", descripcion));
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

            string sql = "insert into COMICS values(:p_idcomic, :p_nombre, :p_imagen, :p_descripcion)";

            this.cmd.Parameters.Add(new OracleParameter(":p_idcomic", nextId));
            this.cmd.Parameters.Add(new OracleParameter(":p_nombre", nombre));
            this.cmd.Parameters.Add(new OracleParameter(":p_imagen", imagen));
            this.cmd.Parameters.Add(new OracleParameter(":p_descripcion", descripcion));
            this.cmd.CommandType = CommandType.Text;
            this.cmd.CommandText = sql;
            this.cn.Open();
            this.cmd.ExecuteNonQuery();
            this.cn.Close();
            this.cmd.Parameters.Clear();
           
        }

        public void DeleteComic(int id)
        {
            string sql = "delete from COMICS where IDCOMIC=:p_idcomic";

            this.cmd.Parameters.Add(new OracleParameter(":p_idcomic", id));
            this.cmd.CommandType = CommandType.Text;
            this.cmd.CommandText = sql;
            this.cn.Open();
            this.cmd.ExecuteNonQuery();
            this.cn.Close();
            this.cmd.Parameters.Clear();
        }

    }
}

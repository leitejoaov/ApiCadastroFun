using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Any;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WebApiCadastroFuncionario.Models;
namespace WebApiFuncionarios.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebApiFuncionarios : ControllerBase
    {


        //[HttpGet(Name = "WebApiFuncionarios")]
        //public void Get()
        //{

        //    var client = new SqlConnection("Server=DIONNE\\SQLEXPRESS;Database=CadastroFuncionario;Trusted_Connection=True");

        //    client.Open();
        //}
        [HttpGet(Name = "BuscarCadastros")]
        public List<Funcionario> BuscarCadastros(int CPF)
        {
            var funcionarios = new List<Funcionario>();
            
            var client = new SqlConnection("Server=DIONNE\\SQLEXPRESS;Database=CadastroFuncionario;Trusted_Connection=True");
            var a = new List<DataColumn>();
            using (SqlCommand cmd = new SqlCommand("BuscarCadastros", client))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CPF", SqlDbType.Int).Value = CPF;
                client.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                //while (reader.HasRows)
                //{
                    while (reader.Read())
                    {
                        Funcionario newFuncionario = new Funcionario();
                        newFuncionario.CPF = reader.GetInt32(0);
                        newFuncionario.Nome = reader.GetString(1);
                        newFuncionario.TipoCliente = reader.GetInt32(2);
                        newFuncionario.Sexo = reader.GetString(3);
                        newFuncionario.SituacaoCliente = reader.GetInt32(4);
                        funcionarios.Add(newFuncionario);
                        reader.NextResult();
                    }


                //}
                client.Close();
                return funcionarios;

            }
            

            //return funcionarios;
        }
        [HttpPost(Name = "CriarCadastro")]
        public void CadastrarCliente(AnyType CPF, AnyType Nome, AnyType TipoCliente, AnyType Sexo, AnyType SituacaoCliente)
        {
            if (CPF != 0)
            {
                var client = new SqlConnection("Server=DIONNE\\SQLEXPRESS;Database=CadastroFuncionario;Trusted_Connection=True");
                using (SqlCommand cmd = new SqlCommand("CriarCadastro", client))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Nome", SqlDbType.VarChar).Value = Nome;
                    cmd.Parameters.Add("@CPF", SqlDbType.Int).Value = CPF;
                    cmd.Parameters.Add("@TipoCliente", SqlDbType.Int).Value = TipoCliente;
                    cmd.Parameters.Add("@Sexo", SqlDbType.VarChar).Value = Sexo;
                    cmd.Parameters.Add("@SituacaoCliente", SqlDbType.Int).Value = SituacaoCliente;
                    client.Open();
                    cmd.ExecuteNonQuery();
                    client.Close();
                }

            }
        }
        [HttpDelete(Name = "ExcluirFuncionario")]
        public void ExcluirFuncionario(int CPF)
        {
            var client = new SqlConnection("Server=DIONNE\\SQLEXPRESS;Database=CadastroFuncionario;Trusted_Connection=True");
            using (SqlCommand cmd = new SqlCommand("ExcluirFuncionario", client))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CPF", SqlDbType.Int).Value = CPF;
                client.Open();
                cmd.ExecuteNonQuery();
                client.Close();
            }
        }
        

    }
}
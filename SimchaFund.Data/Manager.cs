using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SimchaFund.Data
{
    public class Manager
    {
        private string _connectionString;
        public Manager(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Simcha> GetSimchos()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "Select*from Simchos";
            connection.Open();
            List<Simcha> simchos = new List<Simcha>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Simcha simcha = new Simcha
                {
                    Id = (int)reader["Id"],
                    SimchaName = (string)reader["SimchaName"],
                    Date = (DateTime)reader["Date"]


                };
                simchos.Add(simcha);
            }
            foreach (Simcha simcha in simchos)
            {
                ContributorCountAndSum(simcha);
            }
            return simchos;
        }
        private void ContributorCountAndSum(Simcha simcha)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "select COUNT(*) as 'Contributor Amount', ISNULL(SUM(amount) , 0)as 'Total'from SimchosContributors  where SimchaId=@id";
            cmd.Parameters.AddWithValue("@id", simcha.Id);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            simcha.TotalAmount = (decimal)reader["Total"];
            simcha.ContributorCount = (int)reader["Contributor Amount"];


        }
        public int GetContributorCount()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "select Count(*)from Contributors";
            connection.Open();
            return (int)cmd.ExecuteScalar();


        }
        public int AddSimcha(Simcha simcha)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Simchos Values(@simchaName, @date) select SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@simchaName", simcha.SimchaName);
            cmd.Parameters.AddWithValue("@date", simcha.Date);
            connection.Open();
            return (int)(decimal)cmd.ExecuteScalar();
        }
        public List<Contributor> GetContributors()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "Select*from Contributors";
            connection.Open();
            List<Contributor> contributors = new List<Contributor>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Contributor contributor = new Contributor
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    CellNumber = (Int64)reader["CellNumber"],
                    AlwaysInclude = (bool)reader["AlwaysInclude"]


                };
                contributors.Add(contributor);
            }
            foreach (Contributor contributor in contributors)
            {
                contributor.Balance = GetBalance(contributor.Id);
               
            }

            return contributors;
        }
      
        public decimal GetBalance(int contributorId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"Select ISNULL(Sum(DepositAmount), 0) from  Deposits where ContributorId=@id";
            cmd.Parameters.AddWithValue("@id", contributorId);
            List<Deposit> deposits = new List<Deposit>();
            connection.Open();
            decimal totalDeposits=(decimal)cmd.ExecuteScalar();
            decimal totalContributions = GetTotalContributions(contributorId);
            return totalDeposits - totalContributions;
            
          

        }
        private decimal GetTotalContributions(int contributorId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"select ISNULL(Sum(Amount), 0)  from SimchosContributors where ContributorId=@id ";
            cmd.Parameters.AddWithValue("@id", contributorId);
            connection.Open();
            return (decimal)cmd.ExecuteScalar();
        }
        public int AddContributor(Contributor contributor)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Contributors Values(@name, @cellNumber, @alwaysInclude)select SCOPE_IDENTITY() ";
            cmd.Parameters.AddWithValue("@name", contributor.Name);
            cmd.Parameters.AddWithValue("@cellNumber", contributor.CellNumber);
            cmd.Parameters.AddWithValue("@alwaysInclude", contributor.AlwaysInclude);
            connection.Open();
            return (int)(decimal)cmd.ExecuteScalar();
        }
        public void EditContributor(Contributor contributor)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Contributors SET Name=@name, CellNumber=@cellNumber, AlwaysInclude=@alwaysInclude where id=@id";
            cmd.Parameters.AddWithValue("@id", contributor.Id);
            cmd.Parameters.AddWithValue("@name", contributor.Name);
            cmd.Parameters.AddWithValue("@cellNumber", contributor.CellNumber);
            cmd.Parameters.AddWithValue("@alwaysInclude", contributor.AlwaysInclude);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public void Deposit(Deposit deposit)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Deposits Values (@depositAmount, @date, @contributorId)";

            cmd.Parameters.AddWithValue("@depositAmount", deposit.DepositAmount);
            cmd.Parameters.AddWithValue("@date", deposit.Date);
            cmd.Parameters.AddWithValue("@contributorId", deposit.ContributorId);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
        public string GetSimchaName(int simchaId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "Select SimchaName from Simchos WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", simchaId);

            connection.Open();
            return (string)cmd.ExecuteScalar();
        }
        public string GetContributorName(int contributorId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "Select Name from Contributors WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", contributorId);

            connection.Open();
            return (string)cmd.ExecuteScalar();
        }
        public void UpdateContributions(int simchaId, int contributorId, decimal amountWishesToGive)
        {
            
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = " INSERT INTO SimchosContributors Values (@simchaId, @contributorId, @amount, @date)";

           
            cmd.Parameters.AddWithValue("@contributorId", contributorId);
            cmd.Parameters.AddWithValue("@simchaId", simchaId);
            cmd.Parameters.AddWithValue("@amount", amountWishesToGive);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            connection.Open();
            cmd.ExecuteNonQuery();

        }
        public void DeleteFromSimchosContributors(int simchaId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "Delete From SimchosContributors WHERE SimchaId=@id";
            cmd.Parameters.AddWithValue("@id", simchaId);

            connection.Open();
            cmd.ExecuteNonQuery();
        }
        public List<History> GetHistory(int contributorId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "Select DepositAmount, DepositDate from Deposits  WHERE ContributorId=@id";
            cmd.Parameters.AddWithValue("@id", contributorId);
            connection.Open();
            List<History> histories = new List<History>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                History history = new History
                {
                    Action = "Deposit",
                    Date = (DateTime)reader["DepositDate"],
                    Amount = (decimal)reader["DepositAmount"],
                    IsContribution = false
                };
                histories.Add(history);
            }
            histories.AddRange(  GetContributions(contributorId));
            
            return histories;


        }
        private List<History> GetContributions(int contributorId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "Select SimchaId, Amount, Date from SimchosContributors  WHERE ContributorId=@id";
            cmd.Parameters.AddWithValue("@id", contributorId);
            connection.Open();
            List<History> histories = new List<History>();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                History history = new History
                {

                    Action = $"Contributed to the {GetSimchaName((int)reader["SimchaId"])} ",
                    Date = (DateTime)reader["Date"],
                    Amount = (decimal)reader["Amount"],
                    IsContribution = true
                };
                histories.Add(history);
            }
            return histories;
        }
        public bool AlreadyIncluded(int simchaId, int contributorId)
        {

            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "select*from SimchosContributors where Simchaid=@simchaId And ContributorId=@conId";
            cmd.Parameters.AddWithValue("@simchaId", simchaId);
            cmd.Parameters.AddWithValue("@conId", contributorId);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return false;
            }
            
            return true;
        }

    }



    public class Simcha
    {
        public int Id { get; set; }
        public string SimchaName { get; set; }
        public DateTime Date { get; set; }
        public int ContributorCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class Contributor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Int64 CellNumber { get; set; }
        public bool AlwaysInclude { get; set; }
        public decimal Balance { get; set; }
        public bool Include { get; set; }
        public decimal AmountWishesToGive { get; set; }
        public bool AlreadyIncluded { get; set; }
    }
    public class Deposit
    {
        public decimal DepositAmount { get; set; }
        public DateTime Date { get; set; }
        public int ContributorId { get; set; }
    }
    public class Contribution
    {
        public int SimchaId { get; set; }
        public decimal Amount { get; set; }
        public string SimchaName { get; set; }
        public DateTime Date { get; set; }
    }
    public class History
    {
        
        public string Action { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public bool IsContribution { get; set; }
        


    }
}
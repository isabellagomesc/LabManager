using LabManager.Database;
using LabManager.Models;
using Microsoft.Data.Sqlite;
using Dapper;

namespace LabManager.Repositories;

class ComputerRepository
{
    private readonly DatabaseConfig _databaseConfig;

    public ComputerRepository(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    public IEnumerable<Computer> GetAll()
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

        var computers = connection.Query<Computer>("SELECT * FROM Computers");

        return computers;
    }


    public Computer Save(Computer computer)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

        connection.Execute("INSERT INTO Computers VALUES(@Id, @Ram, @Processor)", computer);

        return computer;
    }


    public Computer Update(Computer computer)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

        connection.Execute("UPDATE Computers SET ram = (@Ram), processor = (@Processor) WHERE id = (@Id)", computer);

        return computer;
    }

     public Computer GetById(int id)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

        var computer = connection.QuerySingle<Computer>("SELECT * FROM Computers WHERE id = (@Id)", new {Id = id});

        return computer;
    }

    public void Delete(int id)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

        connection.Execute("DELETE FROM Computers WHERE id = (@Id)", new {Id = id});

    }

    public bool ExistsById(int id)
    {
        using (var connection = new SqliteConnection(_databaseConfig.ConnectionString))
        {
            var sql = "SELECT count(id) FROM Computers WHERE id = (@Id)";
            var count = connection.ExecuteScalar<Boolean>(sql, new {Id = id});
            return count;
        }
    }

    private Computer ReaderToComputer(SqliteDataReader reader)
    {
        var computer = new Computer(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));

        return computer;
    }
}
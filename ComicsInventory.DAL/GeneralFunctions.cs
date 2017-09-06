using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ComicsInventory.DAL.DTOs;
using Dapper;

namespace ComicsInventory.DAL
{
    public class GeneralFunctions
    {
        private readonly string _con = ConfigurationManager.ConnectionStrings["Inventory"].ConnectionString;
        private IDbConnection _dapper;

        // public GeneralFunctions()
        //{
        //     //_dapper = new SqlConnection(_con);
        //}

        public IEnumerable<StockTableDto> StockTable()
        {
//Used on Stock Manage INdex AJAX return using DataTable.js
            using (_dapper = new SqlConnection(_con))
            {
                const string sql = "SELECT p.Publisher_ID AS PublisherId," +
                                   "p.Pub_Name AS Publisher," +
                                   "s.Series_ID AS SeriesId," +
                                   "s.Series_Name AS SeriesName," +
                                   "i.Issue_ID AS IssueId," +
                                   "i.Box_ID AS BoxId," +
                                   "i.Number AS IssueNumber," +
                                   "i.publication_date AS PublishDate, " +
                                   "i.barcode AS Barcode " +
                                   "FROM Publisher p " +
                                   "INNER JOIN Series s ON p.Publisher_ID = s.Publisher_ID " +
                                   "INNER JOIN Issue i ON s.Series_ID = i.Series_ID " +
                                   "ORDER BY s.Series_Name, cast(i.Number AS int) ASC;";
                var data = _dapper.Query<StockTableDto>(sql);
                return data;
            }
        }
    }
}
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using ComicsInventory.DAL.DTOs;
using Dapper;
using MySql.Data.MySqlClient;

namespace ComicsInventory.DAL
{
    /// <summary>
    ///     This repository utilises Dapper to query the GCD database
    /// </summary>
    public class ComicDataRepo
    {
        private readonly IDbConnection _db;

        public ComicDataRepo()
        {
            _db = new MySqlConnection(ConfigurationManager.ConnectionStrings["GrandComicsDatabase"].ConnectionString);
        }


        //Searching of ComicData done here
        /// <summary>
        ///     Searches the GCD using a given barcode
        /// </summary>
        /// <param name="scan">Barcode Given</param>
        /// <returns>Ienumerable</returns>
        public IEnumerable<GcdDataDto> SearchBy(string scan)
        {
            using (_db)
            {
                const string sql =
                    "SELECT pub.id as PublisherId, pub.name as Publisher, pub.year_began as PublisherStartYear, pub.url as PubUrl," +
                    "s.id AS SeriesId, s.name as SeriesName, s.year_began as SeriesBeginYear, s.dimensions as Dimensions, s.paper_stock as PaperStock," +
                    "i.id as IssueId,i.number as IssueNumber,i.publication_date as PublishDate,i.page_count as PageCount,i.indicia_frequency as Frequency," +
                    "i.editing as Editor, i.valid_isbn as Isbn, i.barcode as Barcode " +
                    "FROM gcd_series s " +
                    "INNER JOIN gcd_issue i ON s.id = i.series_id " +
                    "INNER JOIN gcd_publisher pub ON s.publisher_id = pub.id " +
                    "WHERE i.barcode = @barcode OR i.valid_isbn = @isbn LIMIT 1";

                var output = _db.Query<GcdDataDto>(sql, new {barcode = scan, isbn = scan});
                return output;
            }
        }

        /// <summary>
        ///     Searches the GCD using Series Name and Issue Number.
        /// </summary>
        /// <param name="seriesName">Name of the series.</param>
        /// <param name="issueNumber">The issue number</param>
        /// <returns>Ienumerable of data</returns>
        public IEnumerable<GcdDataDto> SearchBy(string seriesName, string issueNumber)
        {
            using (_db)
            {
                const string sql =
                    "SELECT pub.id as PublisherId, pub.name as Publisher, pub.year_began as PublisherStartYear, pub.url as PubUrl," +
                    "s.id AS SeriesId, s.name as SeriesName, s.year_began as SeriesBeginYear, s.dimensions as Dimensions, s.paper_stock as PaperStock," +
                    "i.id as IssueId,i.number as IssueNumber,i.publication_date as PublishDate,i.page_count as PageCount,i.indicia_frequency as Frequency," +
                    "i.editing as Editor, i.valid_isbn as Isbn, i.barcode as Barcode " +
                    "FROM gcd_series s " +
                    "INNER JOIN gcd_issue i ON s.id = i.series_id " +
                    "INNER JOIN gcd_publisher pub ON s.publisher_id = pub.id " +
                    "WHERE s.name = @seriesName AND i.number = @issueNumber LIMIT 1";

                var output = _db.Query<GcdDataDto>(sql, new {seriesName, issueNumber});
                return output;
            }
        }

        /// <summary>
        ///     Gets the publisher details from GCD DB using Id #
        /// </summary>
        /// <param name="pubId">The publisher ID #</param>
        /// <returns></returns>
        public IEnumerable<GcdPublisherDto> GetPublisherDetails(int pubId)
        {
            using (_db)
            {
                const string sql =
                    "SELECT pub.name as Publisher, pub.year_began as PublisherStartYear, pub.url as PubUrl " +
                    "FROM gcd_publisher pub WHERE pub.id = @pubId;";
                var exit = _db.Query<GcdPublisherDto>(sql, new {pubId});
                return exit;
            }
        }

        /// <summary>
        ///     Gets the issue details using GCD DB Id #.
        /// </summary>
        /// <param name="issueId">The issue identifier.</param>
        /// <returns></returns>
        public IEnumerable<GcdIssueDto> GetIssueDetails(int issueId)
        {
            using (_db)
            {
                const string sql =
                    "SELECT i.number as IssueNumber, i.publication_date as PublishDate, i.page_count as PageCount, i.indicia_frequency as Frequency, " +
                    "i.editing as Editor, i.valid_isbn as Isbn, i.barcode as Barcode " +
                    "FROM gcd_issue i WHERE i.id = @issueId";
                var result = _db.Query<GcdIssueDto>(sql, new {issueId});
                return result;
            }
        }

        /// <summary>
        ///     Gets the series details using GCD DB Id #
        /// </summary>
        /// <param name="seriesId">The series identifier</param>
        /// <returns></returns>
        public IEnumerable<GcdSeriesDto> GetSeriesDetails(int seriesId)
        {
            using (_db)
            {
                const string sql =
                    "SELECT s.name AS SeriesName, s.year_began AS SeriesBeginYear, s.year_ended AS YearEnd, " +
                    "s.dimensions AS Dimensions, s.paper_stock AS PaperStock " +
                    "FROM gcd_series s WHERE s.id = @seriesId";
                var rslt = _db.Query<GcdSeriesDto>(sql, new {seriesId});

                return rslt;
            }
        }
    }
}
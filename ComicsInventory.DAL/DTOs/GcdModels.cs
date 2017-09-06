namespace ComicsInventory.DAL.DTOs
{
    /// <summary>
    ///     Data Transfer Objects for Dapper to utilise on query of the GCD Database
    /// </summary>
    public class GcdDataDto
    {
        /// <summary>
        ///     DTO For when Dapper queries the GCD MySql Database, for the addition of comics by barcode, or name and number
        /// </summary>
        public int PublisherId { get; set; }

        public string Publisher { get; set; }
        public int PublisherStartYear { get; set; }
        public string PubUrl { get; set; }
        public int SeriesId { get; set; }
        public string SeriesName { get; set; }
        public int SeriesBeginYear { get; set; }
        public string Dimensions { get; set; }
        public string PaperStock { get; set; }
        public int IssueId { get; set; }
        public string IssueNumber { get; set; }
        public string PublishDate { get; set; }
        public double PageCount { get; set; }
        public string Frequency { get; set; }
        public string Editor { get; set; }
        public string Isbn { get; set; }
        public string Barcode { get; set; }
    }

    /// <summary>
    ///     Filled by Dapper query on GCD searched by IssueId #
    /// </summary>
    public class GcdIssueDto
    {
        public string IssueNumber { get; set; }
        public string PublishDate { get; set; }
        public double PageCount { get; set; }
        public string Frequency { get; set; }
        public string Editor { get; set; }
        public string Isbn { get; set; }
        public string Barcode { get; set; }
    }

    /// <summary>
    ///     Utilised by Dapper on search of GCD Database by Series ID #
    /// </summary>
    public class GcdSeriesDto
    {
        public string SeriesName { get; set; }
        public int SeriesBeginYear { get; set; }
        public int YearEnd { get; set; }
        public string Dimensions { get; set; }
        public string PaperStock { get; set; }
    }

    /// <summary>
    ///     Used by Dapper on search of GCD by Publisher ID #
    /// </summary>
    public class GcdPublisherDto
    {
        public string Publisher { get; set; }

        public int PublisherStartYear { get; set; }
        public string PubUrl { get; set; }
    }
}
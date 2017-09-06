using System;
using System.ComponentModel.DataAnnotations;

namespace ComicsInventory.DAL.DTOs
{
    //FOR USE with Dapper.NET on custom search Criteria
    public class StockTableDto
    {
//Stock Manage initial load results into this model

        public int PublisherId { get; set; }
        public string Publisher { get; set; }
        public string PublishDate { get; set; }
        public int SeriesId { get; set; }
        public string SeriesName { get; set; }
        public int IssueId { get; set; }
        public string IssueNumber { get; set; }
        public string Barcode { get; set; }
        public int BoxId { get; set; }
    }

    public class IssueConDto
    {
        public int Id { get; set; }
        public int IssueId { get; set; }
        public byte GradeId { get; set; }
        public int Quantity { get; set; }
        public string GradeName { get; set; }
    }

    public class BoxContentDto
    {
        public int IssueId { get; set; }
        public string Publisher { get; set; }
        public string SeriesName { get; set; }
        public string IssueNumber { get; set; }
        public int Qty { get; set; }
        public string Condition { get; set; }
        public int GradeId { get; set; }
    }

    public class IssuesWithoutBoxesDto
    {
        public int IssueId { get; set; }
        public string Publisher { get; set; }
        public string SeriesName { get; set; }
        public string IssueNumber { get; set; }
        public string Barcode { get; set; }
    }

    public class LatestAdditionsDto
    {
        //Used for Dapper to return the TOP 3 latest additions to the inventory

        public int IssueId { get; set; }
        public int BoxId { get; set; }
        public string Publisher { get; set; }
        public string SeriesName { get; set; }
        public string IssueNumber { get; set; }
        public string PublishedDate { get; set; }
        public decimal? PageCount { get; set; }
        public string Frequency { get; set; }
        public int GcdId { get; set; }

        [DisplayFormat(DataFormatString = "G")]
        public DateTime? AddedOn { get; set; }
    }
}
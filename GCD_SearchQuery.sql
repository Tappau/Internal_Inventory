/*Search by Isbn OR Barcode*/
SELECT   
	pub.id as PublisherId, 
	pub.name as Publisher,
    pub.year_began as PublisherStartYear,
    pub.url as PubUrl,
    s.id AS SeriesId,
    s.name as SeriesName,
    s.year_began as SeriesBeginYear,
    s.dimensions as Dimensions,
    s.paper_stock as PaperStock,
    i.id as IssueId,
    i.number as IssueNumber,
    i.publication_date as PublishDate,
    i.page_count as PageCount,
    i.indicia_frequency as Frequency,
    i.editing as Editor,
    i.valid_isbn as Isbn,
    i.barcode as Barcode
FROM
    gcd_series s
        INNER JOIN
    gcd_issue i ON s.id = i.series_id
        INNER JOIN
    gcd_publisher pub ON s.publisher_id = pub.id
WHERE /* barcode is, ISBN is, */
    i.barcode = 0714860241508;
	
-- Search by Series name & Issue NUMBER

SELECT 
   	pub.id as PublisherId, 
	pub.name as Publisher,
    pub.year_began as PublisherStartYear,
    pub.url as PubUrl,
    s.id AS SeriesId,
    s.name as SeriesName,
    s.year_began as SeriesBeginYear,
    s.dimensions as Dimensions,
    s.paper_stock as PaperStock,
    i.id as IssueId,
    i.number as IssueNumber,
    i.publication_date as PublishDate,
    i.page_count as PageCount,
    i.indicia_frequency as Frequency,
    i.editing as Editor,
    i.valid_isbn as Isbn,
    i.barcode as Barcode
FROM
    gcd_series s
        INNER JOIN
    gcd_issue i ON s.id = i.series_id
        INNER JOIN
    gcd_publisher pub ON s.publisher_id = pub.id
WHERE
    s.name = 'The amazing Spider-man'
        AND i.number = '252';	
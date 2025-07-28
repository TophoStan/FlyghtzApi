using DataExtractor.Wikipedia;
using Domain;

namespace DataExtractor.Wikipedia;


public interface IScraper
{
    public Task<List<WikiPediaAirportData>> Scrape(string letter);
}
namespace ASAP_Clients.PolygonEntities
{
    public class PreviousClose
    {
        public required string ticker { get; set; }
        public int queryCount { get; set; }
        public int resultsCount { get; set; }
        public bool adjusted { get; set; }
        public required List<PreviousCloseResult> results { get; set; }
        public required string status { get; set; }
        public required string request_id { get; set; }
        public int count { get; set; }
    }
}
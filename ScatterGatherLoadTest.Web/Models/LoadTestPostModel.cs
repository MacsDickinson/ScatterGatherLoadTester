namespace ScatterGatherLoadTest.Web.Models
{
    public class LoadTestPostModel
    {
        public string Domain { get; set; }
        public string Resource { get; set; }
        public int Requests { get; set; }
    }
}
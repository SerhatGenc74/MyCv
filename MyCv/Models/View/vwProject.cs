namespace MyCv.Models.View
{
    public class vwProject
    {

        public int ProjectId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string coverImgUrl { get; set; }
        public string tags { get; set; }
        public List<ProjectDetail> Details { get; set; }
    }
}

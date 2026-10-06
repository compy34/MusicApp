namespace MusicApp.Models
{
    public class Song
    {
        public int Id { get; set; } 
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int Duration { get; set; }
        public long FileSize { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string S3Key { get; set; } = string.Empty;
        public string S3Url { get; set; } = string.Empty;
        public DateTime UploadAt { get; set; } 

    }
}

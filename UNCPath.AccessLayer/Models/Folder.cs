namespace UNCPath.AccessLayer.Models
{
    public class Folder : UNCPathContainer
    {
        //mark RootFolder for simplicity
        public bool RootFolder { get; set; }
        public virtual ICollection<Folder> Folders { get; set;} = new List<Folder>();
        public virtual ICollection<File> Files { get; set; } = new List<File>();

    }
}

using System.ComponentModel.DataAnnotations;

namespace UNCPath.AccessLayer.Models
{
    public abstract class UNCPathContainer : Entity
    {
        public long Size { get; set; }
        public DateTime CreationDate { get; set; }
        //windows max allowed path length
        [StringLength(260)]
        public string Path { get; set; }

        //windows max allowed path length
        [StringLength(260)]
        public string Name { get; set; }
    }
}
